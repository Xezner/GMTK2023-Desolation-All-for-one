using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : ManagerBehaviour
{
    [SerializeField] private float turnRate = 5f;
    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            //mouse position on screen
            Vector3 screenMousePosition = Input.mousePosition;

            //mouse position on world
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(screenMousePosition);
            worldMousePosition.z = 0f;

            //normalize the direction to get a value between 0 and 1
            Vector3 direction = worldMousePosition - transform.position;
            direction.Normalize();

            //angle of rotation - 90f to get the top part of the screen as the initial rotation
            float angleRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;


            Quaternion targetRotation = Quaternion.AngleAxis(angleRotation, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnRate * Time.deltaTime);
        }
    }
}
