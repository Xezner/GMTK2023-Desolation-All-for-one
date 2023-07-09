using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataHolder : ManagerBehaviour
{
    [SerializeField] private CharacterData _characterData;
    [SerializeField] private Animator _bloodAnimator;
    [SerializeField] private Animator _tendrilsAnimator;
    [SerializeField] private Animator _characterAnimator;
    public bool IsStartDegen = false;
    public bool IsKilled = false;
    private float timer = 0f;
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
        Movespeed = characterData.Movespeed - 1f;
        TurnRate = characterData.TurnRate;
        HealthDegen = characterData.HealthDegen;
        KnockBack = characterData.KnockBack;
        CharacterType = characterData.CharacterType;

        ResetAnimator();
        _characterAnimator.SetBool("IsMoving", false);
        _characterAnimator.SetFloat("MoveX", 1);
        GetComponentInChildren<WeaponController>().InitData();
    }

    public void Start()
    {
        _healthOverlay = GameObject.FindGameObjectWithTag("HealthOverlay");
    }

    public void Update()
    {
        if(IsKilled)
        {
            _healthOverlay.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }

        if(GameManager.IsPossessed && HP > 0 && !GameManager.IsGamePaused && !GameManager.IsWaitingForFirstPossession)
        {
            timer += Time.deltaTime;
            if(timer >= 1f)
            {
                HP -= HealthDegen;
                //Enable the Screen Effect
                _healthOverlay.transform.GetChild(0).gameObject.SetActive(true);

          
                IsKilled = false;
                timer = 0;
            }
        }
    }
    public void BloodVFX()
    {
        _bloodAnimator.gameObject.SetActive(false);
        _bloodAnimator.gameObject.SetActive(true);
    }
    public void TendrilsVFX()
    {
        _tendrilsAnimator.gameObject.SetActive(false);
        _tendrilsAnimator.gameObject.SetActive(true);
    }

    public void AnimateRun(bool isRunning)
    {
        if (isRunning)
        {
            ResetAnimator();
            _characterAnimator.SetBool("IsMoving", isRunning);
            _characterAnimator.SetFloat("MoveX", 1);
        }
        else
        {
            _characterAnimator.SetBool("IsMoving", isRunning);
        }
    }


    public void AnimateDeath()
    {
        _characterAnimator.SetFloat("MoveX", 0);
        _characterAnimator.SetTrigger("IsDead");
    }


    public void ResetAnimator()
    {
        _characterAnimator.ResetTrigger("IsDead");
    }
}
