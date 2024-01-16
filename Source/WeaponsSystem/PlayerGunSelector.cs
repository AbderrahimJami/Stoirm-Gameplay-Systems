using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField] private GunType _gun;

    [SerializeField] private Transform _gunParent;
    [SerializeField] private Camera _camera;

    [SerializeField] private List<GunSO> _guns;

    [SerializeField] private PlayerInput _playerInput;


    [Space]
    [Header("Runtime Filled")]
    public GunSO ActiveGun;

    private void OnEnable()
    {
        GameEventsManager.Instance.PlayerEvents.onWeaponUnlocked += SetupPlayerGun;



    }

    private void OnDisable()
    {
        GameEventsManager.Instance.PlayerEvents.onWeaponUnlocked -= SetupPlayerGun;

    }

    private void Start()
    {
        //SetupPlayerGun(_gun);

    }

    private void SetupPlayerGun(GunType gunType)
    {
        GunSO gun = _guns.Find(gun => gun.Type == gunType);

        if (gun == null)
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");

        ActiveGun = gun;
        ActiveGun.Spawn(_gunParent, this, _camera);

        //Add weapon action map to the input to enable shooting input detection 
        if (_playerInput.actions.FindActionMap("Shooting").enabled != true)
            _playerInput.actions.FindActionMap("Shooting").Enable();
    }
}
