using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class Slot : MonoBehaviour
{
    public List<GameObject> ValidCards;
    public Transform CardSlot;

    protected bool _hasValidCard = false;
    public bool HasValidCard { get { return _hasValidCard; } }

    protected virtual void OnTriggerStay(Collider other)
    {
        if(_hasValidCard) { return; }

        Debug.Log("Attempting to validate: " + other.name);

        if (ValidCards.Contains(other.gameObject))
        {
            CardValidateSuccess(other.gameObject);
        }
        else
        {
            CardValidateFailure();
        }
    }

    protected virtual void CardValidateSuccess(GameObject card)
    {
        _hasValidCard = true;
        Debug.Log("Success with " + card.name);

        StartCoroutine(MoveCardToCardSlot(card.transform, 2.0f));
    }

    protected virtual void CardValidateFailure()
    {

    }

    protected virtual IEnumerator MoveCardToCardSlot(Transform cardTransform, float duration)
    {
        Throwable throwable = cardTransform.GetComponent<Throwable>();
        if(throwable)
        {
            throwable.onDetachFromHand.Invoke();
            if(throwable.interactable.attachedToHand)
            {
                throwable.interactable.attachedToHand.Show();
            }

            throwable.interactable.highlightOnHover = false;
            throwable.interactable.enabled = false;
            Destroy(throwable);
        }

        Rigidbody rb = cardTransform.GetComponent<Rigidbody>();
        if(rb)
        {
            rb.isKinematic = true;
        }

        cardTransform.parent = CardSlot;

        Vector3 startingPosition = cardTransform.localPosition;
        Quaternion startingRotation = cardTransform.localRotation;

        for(float timer = 0.0f; timer < duration; timer += Time.deltaTime)
        {
            float lerpFactor = timer / duration;

            cardTransform.localPosition = Vector3.Lerp(startingPosition, Vector3.zero, lerpFactor);
            cardTransform.localRotation = Quaternion.Lerp(startingRotation, Quaternion.identity, lerpFactor);

            yield return null;
        }

        cardTransform.localPosition = Vector3.zero;
        cardTransform.localRotation = Quaternion.identity;
    }
}
