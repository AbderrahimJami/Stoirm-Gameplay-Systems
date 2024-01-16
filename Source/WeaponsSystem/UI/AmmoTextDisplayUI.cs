using FPSRetroKit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class AmmoTextDisplayUI : MonoBehaviour
{
    [Tooltip("Player Gun Selector component, used to get the active gun's ammo stats")]
    [SerializeField] private PlayerGunSelector _gunSelector;
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _woodText;




    // Update is called once per frame
    void Update()
    {
        if (_gunSelector.ActiveGun != null)
            _ammoText.text = $"{_gunSelector.ActiveGun.AmmoConfig.CurrentClipAmmo} / {_gunSelector.ActiveGun.AmmoConfig.CurrentAmmo}";

        _woodText.text = _playerHealth.Wood.ToString();
    }
}
