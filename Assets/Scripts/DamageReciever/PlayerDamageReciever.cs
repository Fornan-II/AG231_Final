using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PlayerDamageReciever : DamageReciever
{
    public PostProcessProfile DeathProfile;
    public PostProcessVolume PPVolume;

    public override void Die()
    {
        base.Die();

        PPVolume.profile = DeathProfile;
        StartCoroutine(WaitToReloadScene(5.0f));
    }

    protected virtual IEnumerator WaitToReloadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Valve.VR.SteamVR_LoadLevel.Begin(SceneManager.GetActiveScene().name);
    }
}