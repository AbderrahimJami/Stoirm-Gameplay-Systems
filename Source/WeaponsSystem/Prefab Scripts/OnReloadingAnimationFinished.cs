using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReloadingAnimationFinished : MonoBehaviour
{
    public void OnReloadingAnimFinished()
    {
        GameEventsManager.Instance.PlayerEvents.ReloadingAnimationCompleted();
    }
}
