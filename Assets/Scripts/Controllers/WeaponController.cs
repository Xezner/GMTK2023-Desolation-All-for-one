using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : ManagerBehaviour
{
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private Image _cooldownBar;
    [SerializeField] private WeaponCollisionDetection _weaponCollisionDetection;
    [SerializeField] private ProjectileUtil _projectile;
    [SerializeField] private Transform _spawnPoint;
    public bool CanAttack = true;
    public bool IsRanged = false;
    private float _attackRateHolder = 1f;
    private const string NORMAL_ATTACK_TRIGGER = "NormalAttack";
    
    //Player Check
    public bool IsPlayer = false;

    //BattleData
    private float _attackRate = 1f;
    private float _attackDamage;
    private float _attackRange;
    private float _projectileDuration;
    private float _projectileSpeed;
    private CharacterType _characterType;

    //PhysicsData
    private Collider2D _weaponCollider;

    //CharacterData
    private CharacterDataHolder _characterData;
    public CharacterDataHolder CharacterData { get { return _characterData; } }
    private void Start()
    {
    }

    private void Update()
    {
        if (GameManager.IsWaitingForFirstPossession)
        {
            return;
        }

        if(_characterData != null && _characterData.HP <= 0)
        {
            return;
        }    

        if (!GameManager.IsGamePaused && GameManager.IsPossessed)
        {
            //Check if cooldown is over
            if (!CanAttack)
            {
                _attackRateHolder -= Time.deltaTime;

                if (IsPlayer)
                {
                    _cooldownBar.fillAmount = _attackRateHolder / _attackRate;
                }
                
                //Reset Attack if CD is over
                if (_attackRateHolder <= 0f)
                {
                    CanAttack = true;
                    _attackRateHolder = _attackRate;

                    if (IsPlayer)
                    {
                        _cooldownBar.fillAmount = 0f;
                    }
                }
            }
            //Can attack now
            else
            {
                //Attack for Left Click
                if (Input.GetMouseButtonDown(0) || !IsPlayer)
                {
                    _weaponAnimator.SetTrigger(NORMAL_ATTACK_TRIGGER);
                    CanAttack = false;
                }
            }
        }
    }

    public void InitData()
    {
        _characterData = GetComponentInParent<CharacterDataHolder>();
        _attackRate = _characterData.AtkRate;
        _projectileDuration = _characterData.ProjectileDuration;
        _projectileSpeed = _characterData.ProjectileSpeed;
        _cooldownBar = CooldownBarUI.Instance.CooldownBar;
        CanAttack = true;
        _attackRateHolder = _attackRate;
        _cooldownBar.fillAmount = 0f;
    }


    public void ShootProjectile()
    {
        var spawnPos = _spawnPoint ? _spawnPoint.position : transform.position;
        var projectile = Instantiate(_projectile, spawnPos, Quaternion.identity, SpawnManager.Instance.transform);
        if (IsPlayer)
        {
            Vector3 launchDirection = FindObjectOfType<CharacterController>().MouseDirection.normalized;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg - 90f);
            projectile.WeaponController = this;
            projectile.StartProjectileCoroutine(projectile.gameObject, launchDirection, _projectileDuration, _projectileSpeed);
        }
        else
        {
            var character = FindAnyObjectByType<CharacterController>().Character;
            Vector3 launchDirection = (character? character.transform.position  : Vector2.zero) - transform.parent.position;
            launchDirection = launchDirection.normalized;
            projectile.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(launchDirection.y, launchDirection.x) * Mathf.Rad2Deg - 90f);
            projectile.WeaponController = this;
            projectile.StartProjectileCoroutine(projectile.gameObject, launchDirection, _projectileDuration, _projectileSpeed);
        }
    }
    public void DamagePlayer(float damage)
    {
        if (_characterData.HP > 0)
        {
            _characterData.HP -= (int)damage;
            _characterData.IsKilled = false;
        }

        if(_characterData.HP <= 0 && !IsPlayer)
        {
            Debug.Log("HELLO");
            _characterData.IsKilled = true;
        }

        Debug.Log($"Isplayer: {IsPlayer}, Is killed: {_characterData.IsKilled}, HP {_characterData.HP}");
    }
}