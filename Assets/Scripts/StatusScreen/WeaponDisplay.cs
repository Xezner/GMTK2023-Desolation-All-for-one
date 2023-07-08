using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponDisplay : MonoBehaviour
{
    [SerializeField] private WeaponInfo _weaponInfo;

    [SerializeField] private TextMeshProUGUI _weaponName, _weaponType, _weaponRange, _weaponPower, _weaponRate, _hostName;

    [SerializeField] private Image _weaponImage;

    // Start is called before the first frame update
    void Start()
    {
        _weaponName.text = _weaponInfo.WeaponName;
        _weaponType.text = _weaponInfo.WeaponType;
        _weaponRange.text = _weaponInfo.Range;
        _weaponPower.text = _weaponInfo.Power.ToString();
        _weaponRate.text = _weaponInfo.Rate.ToString() + "%";
        _weaponImage.sprite = _weaponInfo.WeaponImage;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
