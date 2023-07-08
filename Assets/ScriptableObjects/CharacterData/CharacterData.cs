using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public GameObject Character;
    public string Name;
    public float HP;
    public float AtkDamage;
    public float AtkRate;
    public float AtkRange;
    public float Movespeed;
    public float TurnRate;
    public float HealthDegen;
}

public enum CharacterType
{
    Melee,
    Ranged
}

