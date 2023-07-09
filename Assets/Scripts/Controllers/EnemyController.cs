using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ManagerBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _obstacleDetectionRadius = 2f;
    [SerializeField] private GameObject _collisionDetector;
    private WeaponController _weaponController;
    private CharacterDataHolder _characterData;
    private Transform _targetPosition;
    public Transform TargetPosition { set { _targetPosition = value; } }
    public WeaponController WeaponController { get { return _weaponController; } }
    public GameObject CollisionDetector { get { return _collisionDetector; } }

    //Knowckback
    private float _knockbackDuration = 0.2f;
    public bool IsKnockedBack = false;
    public bool IsDead = false;
    void Start()
    {
        InitSpawn();
    }

    public void InitSpawn()
    {
        _weaponController = GetComponentInChildren<WeaponController>();
        _characterData = GetComponentInChildren<CharacterDataHolder>();
        var characterController = FindFirstObjectByType<CharacterController>();
        _targetPosition = characterController.Character ? characterController.Character.transform : characterController.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.IsWaitingForFirstPossession)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }

        if(GameManager.IsGamePaused)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }

        if (_weaponController.IsPlayer)
        {
            Destroy(_collisionDetector);
            return;
        }

        BasicEnemyNavigation();

        CheckHP();
    }

    private void BasicEnemyNavigation()
    {
        if(IsKnockedBack)
        {
            _knockbackDuration -= Time.deltaTime;
            if(_knockbackDuration <= 0f)
            {
                if (IsDead)
                {
                    return;
                }
                IsKnockedBack = false;
                _collisionDetector.SetActive(true);
                Debug.Log("Done Knockback");
            }
            return;
        }

        if (!_weaponController.IsPlayer && _targetPosition != null)
        {
            Vector3 direction = (_targetPosition.position - transform.position);
            float distance = direction.magnitude;
            direction = direction.normalized;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = Mathf.LerpAngle(_rigidbody2D.rotation, targetAngle, _characterData.TurnRate * Time.deltaTime);
            _rigidbody2D.SetRotation(currentAngle);

            //Vector2 obstacleDetectionForce = ObstacleDetectionForce();
            //Vector2 enemyDetectionForce = EnemyDetectionForce();

            Vector2 desiredVelocity = direction * _characterData.Movespeed;
            //desiredVelocity += obstacleDetectionForce + enemyDetectionForce;

            _weaponController.IsInRanged = distance <= _characterData.AtkRange;
            _weaponController.CharacterData.AnimateRun(!_weaponController.IsInRanged);
            if (distance <= _characterData.AtkRange)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody2D.velocity = desiredVelocity;
            }
        }
    }


    private Vector2 ObstacleDetectionForce()
    {
        Vector2 detectionForce = Vector2.zero;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, _obstacleDetectionRadius, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Obstacles"))
            {
                Vector2 obstacleDirection = new Vector2(transform.position.x, transform.position.y) - hit.point;
                detectionForce += obstacleDirection.normalized / obstacleDirection.magnitude;
            }
        }

        return detectionForce;
    }

    private Vector2 EnemyDetectionForce()
    {
        Vector2 detectionForce = Vector2.zero;
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        foreach (EnemyController enemyController in enemies)
        {
            if(enemyController.WeaponController.IsPlayer)
            {
                continue;
            }

            if (enemyController != this && Vector2.Distance(transform.position, enemyController.transform.position) < _obstacleDetectionRadius)
            {
                Vector2 separationDirection = transform.position - enemyController.transform.position;
                detectionForce += separationDirection.normalized / separationDirection.magnitude;
            }
        }

        return detectionForce;
    }

    private void CheckHP()
    {
        if (_characterData.HP <= 0 && !IsDead)
        {
            Debug.Log($"ENEMY DEAD: {gameObject.name}, Iskilled: {_characterData.IsKilled}");
            if (GameManager.PossessionGauge < 5 && _characterData.IsKilled)
            {
                GameManager.PossessionGauge++;
                _characterData.IsKilled = false;
            }
            IsDead = true;
            StartCoroutine(WaitTillDead(gameObject));
        }
        else if (_characterData.HP > 0)
        {
            IsDead = false;
        }
    }

    public void ApplyKnockBackForce(Vector2 direction, float knockbackMagnitude)
    {
        _collisionDetector.SetActive(false);
        IsKnockedBack = true;
        _knockbackDuration = 0.2f;
        _rigidbody2D.AddForce(direction.normalized * knockbackMagnitude, ForceMode2D.Impulse);
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    _rigidbody2D.velocity = Vector2.zero;         
        
    //    if(collision.gameObject.CompareTag("Weapon"))
    //    {
    //        Debug.Log("weapon hit");
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Weapon"))
    //    {
    //        Debug.Log("weapon hit");
    //    }
    //}


    IEnumerator WaitTillDead(GameObject character)
    {
        _characterData.AnimateDeath();
        _weaponController.AnimationIsDead();
        yield return new WaitForSeconds(1f);
        Destroy(character);
    }
}
