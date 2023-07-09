using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUtil : MonoBehaviour
{
    [SerializeField] Collider2D _collider;
    public WeaponController WeaponController;

    float enemyDuration = 0.5f;
    float enemySpeed = 10f;

    float playerDuration = 0.5f;
    float playerSpeed = 12f;
    public void StartProjectileCoroutine(GameObject projectile, Vector3 launchDirection)
    {
        _collider.enabled = true;
        StartCoroutine(MoveProjectile(projectile, launchDirection));    
    }


    private IEnumerator MoveProjectile(GameObject projectile, Vector3 launchDirection)
    {
        float elapsedTime = 0f;
        var duration = WeaponController.IsPlayer ? playerDuration : enemyDuration;
        var speed = WeaponController.IsPlayer ? playerSpeed : enemySpeed;
        while (elapsedTime < duration)
        {
            // Calculate the distance to move the projectile
            float currentSpeed = Mathf.Lerp(speed, speed, elapsedTime / duration);
            float distanceToMove = currentSpeed * Time.deltaTime;

            // Move the projectile along the launch direction
            projectile.transform.position += launchDirection * distanceToMove;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Destroy the projectile after the duration has elapsed
        Destroy(gameObject);
    }
}
