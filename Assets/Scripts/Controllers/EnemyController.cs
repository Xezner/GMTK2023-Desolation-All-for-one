using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : ManagerBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    private WeaponController _weaponController;
    private CharacterData _characterData;
    private Transform _targetPosition;
    public Transform TargetPosition { set { _targetPosition = value; } }

    // Start is called before the first frame update
    void Start()
    {
        _weaponController = GetComponentInChildren<WeaponController>();
        _characterData = GetComponentInChildren<CharacterDataHolder>().CharacterData;
        _targetPosition = FindFirstObjectByType<CharacterController>().Character.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_weaponController.IsPlayer && _targetPosition != null)
        {
            Debug.Log(gameObject.name + "Is Enemy");
           
            Vector3 direction = (_targetPosition.position - transform.position).normalized;
            
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            _rigidbody2D.SetRotation(targetAngle);

            Vector2 desiredVelocity = direction * _characterData.Movespeed;

            // Apply the desired velocity
            _rigidbody2D.velocity = desiredVelocity;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rigidbody2D.velocity = Vector2.zero;
    }
}
