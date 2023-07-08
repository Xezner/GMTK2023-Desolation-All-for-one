using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ManagerBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _obstacleDetectionRadius = 2f;
    private WeaponController _weaponController;
    private CharacterData _characterData;
    private Transform _targetPosition;
    public Transform TargetPosition { set { _targetPosition = value; } }
    public WeaponController WeaponController { get { return _weaponController; } }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
        InitSpawn();
    }

    public void InitSpawn()
    {
        _weaponController = GetComponentInChildren<WeaponController>();
        _characterData = GetComponentInChildren<CharacterDataHolder>().CharacterData;
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
            return;
        }

        if (!_weaponController.IsPlayer && _targetPosition != null)
        {
            Vector3 direction = (_targetPosition.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float currentAngle = Mathf.LerpAngle(_rigidbody2D.rotation, targetAngle, _characterData.TurnRate * Time.deltaTime);
            _rigidbody2D.SetRotation(currentAngle);



            //Vector2 obstacleDetectionForce = ObstacleDetectionForce();
            //Vector2 enemyDetectionForce = EnemyDetectionForce();

            Vector2 desiredVelocity = direction * _characterData.Movespeed;
            //desiredVelocity += obstacleDetectionForce + enemyDetectionForce;

            _rigidbody2D.velocity = desiredVelocity;
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2D.velocity = Vector2.zero;
    }
}
