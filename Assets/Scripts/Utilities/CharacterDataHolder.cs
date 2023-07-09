using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHolder : ManagerBehaviour
{
    [SerializeField] private CharacterData _characterData;
    [SerializeField] private Animator _animator;
    public CharacterData CharacterData { get { return _characterData; } }

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
    public void AssignStats(CharacterData characterData, string name)
    {
        //Name = name;
        Name = characterData.Name;
        HP = characterData.HP;
        AtkDamage = characterData.AtkDamage;
        AtkRate = characterData.AtkRate;
        AtkRange = characterData.AtkRange;
        ProjectileSpeed = characterData.ProjectileSpeed;
        ProjectileDuration = characterData.ProjectileDuration;
        Movespeed = characterData.Movespeed;
        TurnRate = characterData.TurnRate;
        HealthDegen = characterData.HealthDegen;
        KnockBack = characterData.KnockBack;
        CharacterType = characterData.CharacterType;

        GetComponentInChildren<WeaponController>().InitData();
    }
}
