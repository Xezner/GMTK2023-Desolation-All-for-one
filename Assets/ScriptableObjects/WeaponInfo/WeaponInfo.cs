using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "ScriptableObjects/WeaponInfo")]

public class WeaponInfo : ScriptableObject
{
    public string WeaponName;
    public string WeaponType;
    public string Range;
    public float Power;
    public float Rate;
    public Sprite WeaponImage;
}
