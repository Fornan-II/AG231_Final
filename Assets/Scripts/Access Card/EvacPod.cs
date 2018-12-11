using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EvacPod : Slot
{
    public Animator myAnim;

    protected override IEnumerator MoveCardToCardSlot(Transform cardTransform, float duration)
    {
        yield return base.MoveCardToCardSlot(cardTransform, duration);

        if (myAnim)
        {
            myAnim.SetTrigger("Insert");
        }
    }

    public virtual void OnInsertEnd()
    {
        //Trigger Game End
        Debug.Log("Escape!");
        StartCoroutine(LoadSceneAfter(5.0f));
    }

    protected virtual IEnumerator LoadSceneAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
