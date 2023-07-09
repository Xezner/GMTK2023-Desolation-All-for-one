using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public GameObject Character;
    public string Name;
    public int HP;
    public float AtkDamage;
    public float AtkRate;
    public float AtkRange;
    public float ProjectileSpeed;
    public float ProjectileDuration;
    public float Movespeed;
    public float TurnRate;
    public int HealthDegen;
    public float KnockBack;
    public CharacterType CharacterType;
}

public enum CharacterType
{
    Melee,
    Ranged
}

