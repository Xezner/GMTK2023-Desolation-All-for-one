using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollisionDetection : MonoBehaviour
{
    [SerializeField] private WeaponController _weaponController;
    [SerializeField] private ProjectileUtil _projectileUtil;
    private void Start()
    {
        if (_projectileUtil)
        {
            _weaponController = _projectileUtil.WeaponController;
        }
        else
        {
            _weaponController = GetComponentInParent<WeaponController>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            var otherWeaponController = collision.GetComponentInChildren<WeaponController>();
            if (otherWeaponController == null)
            {
                Debug.Log("NULL");
                return;
            }

            if (_weaponController.IsPlayer && !otherWeaponController.IsPlayer)
            {
                otherWeaponController.DamagePlayer(_weaponController.CharacterData.AtkDamage);

                otherWeaponController.GetComponentInParent<EnemyController>().ApplyKnockBackForce(FindAnyObjectByType<CharacterController>().MouseDirection, _weaponController.CharacterData.KnockBack);
                DestroyProjectile();
            }
            else if (!_weaponController.IsPlayer && otherWeaponController.IsPlayer)
            {
                otherWeaponController.DamagePlayer(_weaponController.CharacterData.AtkDamage);
                Vector2 playerPosition = transform.position;
                Vector2 enemyPosition = collision.transform.position;

                Vector2 hitDirection = enemyPosition - playerPosition;

                FindAnyObjectByType<CharacterController>().ApplyKnockBack(hitDirection, _weaponController.CharacterData.KnockBack);
                DestroyProjectile();
            }
            else
            {
                //Debug.Log("NO TO FRIENDLY DAMAGE");
            }
        }
    }

    private void DestroyProjectile()
    {
        if (_projectileUtil)
        {
            Destroy(_projectileUtil.gameObject);
        }
    }
}
