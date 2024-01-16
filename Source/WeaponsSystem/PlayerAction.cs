using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector gunSelector;

    /// <summary>
    /// Reference to the Player Input component attached to the same GameObject (Player) (This is using the new Unity Input System)
    /// </summary>
    private PlayerInput _playerInput;

    [Tooltip("Whether the gun should auto reload on magazine empty, provided there's ammo available")]
    [SerializeField] bool AutoReload = true;

    

    /// <summary>
    /// Animator required to play Reloading and Shooting animations
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Boolean flag used to determine if active gun's reloading animation is playing or not
    /// </summary>
    private bool IsReloading;
 


    private void Awake()
    {
        //Initialize PlayerInput gameObject component
        _playerInput = GetComponent<PlayerInput>();

        //Initializing animator component 
        _animator = GetComponent<Animator>();   

    }

    /// <summary>
    /// Bind callback functions to Player input Actions
    /// </summary>
    private void OnEnable()
    {
        _playerInput.actions["Fire"].performed += PlayerShoot;
        _playerInput.actions["Reload"].performed += AttemptToReload;

        //Start listening to reloading animations completed
        GameEventsManager.Instance.PlayerEvents.onReloadingAnimationCompleted += OnReloadingAnimationFinished;

    }

    /// <summary>
    /// Unbind callback functions to Player input Actions
    /// </summary>
    private void OnDisable()
    {
        _playerInput.actions["Fire"].performed -= PlayerShoot;
        _playerInput.actions["Reload"].performed -= AttemptToReload;

    }

    /// <summary>
    /// Tries to reload the player's Active gun
    /// </summary>
    /// <param name="context">Class reference that holds relevant Input data</param>
    private void AttemptToReload(InputAction.CallbackContext context)
    {
        if (ShouldManualReload())
        {
            IsReloading = true;
            gunSelector.ActiveGun.GetAnimator().SetTrigger("IsReloading");
            //Start listening to reloading animations completed
            GameEventsManager.Instance.PlayerEvents.onReloadingAnimationCompleted += OnReloadingAnimationFinished;


        }
    }



    /// <summary>
    /// This is invoked whenever the Shoot action is performed
    /// </summary>
    /// <param name="context">Class reference that holds relevant Input data</param>
    private void PlayerShoot(InputAction.CallbackContext context)
    {
        if (ShouldAutoReload())
        {
            IsReloading = true;
            gunSelector.ActiveGun.GetAnimator().SetTrigger("IsReloading");
            //Start listening to reloading animations completed
            GameEventsManager.Instance.PlayerEvents.onReloadingAnimationCompleted += OnReloadingAnimationFinished;
        }
        else
            gunSelector?.ActiveGun.Shoot();
    }



    /// <summary>
    /// Return whether the gun should Auto-Reload based on AutoReload flag, amount of ammo in current clip, total 
    /// available ammo and if we're not reloading.
    /// </summary>
    private bool ShouldAutoReload()
    {
        return !IsReloading 
            && AutoReload 
            && gunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo == 0 
            && gunSelector.ActiveGun.CanReload();
    }


    /// <summary>
    /// Performs check to see if player is able to reload by calling the active gun's CanReload function and whether or not they're in the process 
    /// of reloading or not
    /// </summary>
    /// <returns>true or false dependning on whether the player is allowed to reload in this frame or not</returns>
    private bool ShouldManualReload()
    {
        return !IsReloading && gunSelector.ActiveGun.CanReload();
    }


    /// <summary>
    /// Updates ammo values once reloading animation has ended
    /// </summary>
    private void OnReloadingAnimationFinished()
    {
        gunSelector.ActiveGun.OnReloadingAnimationFinished();
        IsReloading = false;
        //Stop listening to reloading animations completed
        GameEventsManager.Instance.PlayerEvents.onReloadingAnimationCompleted -= OnReloadingAnimationFinished;


    }


    /// <summary>
    /// Reset ammo count when game is closed
    /// </summary>
    private void OnApplicationQuit()
    {
        if (gunSelector.ActiveGun != null)
            gunSelector.ActiveGun.ResetAmmo();
    }


}
