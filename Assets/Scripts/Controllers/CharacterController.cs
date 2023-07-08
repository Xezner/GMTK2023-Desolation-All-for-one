using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : ManagerBehaviour
{
    [SerializeField] private float _turnRate = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _possessionRadius = 3f;
    [SerializeField] private GameObject _character;
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private PossessionRadiusDrawer _radiusDrawer;
    public float PossessionRadius { get { return _possessionRadius; } }
    public GameObject Character { get { return _character; } }

    private void Start()
    {
        TempInit();
    }

    private void TempInit()
    {
        CooldownBarUI.Instance.TargetObject = _character.transform;
        _rigidBody = _character.GetComponent<Rigidbody2D>();
        _character.transform.GetChild(0).GetComponent<WeaponController>().IsPlayer = true;
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {   
            if (Input.GetMouseButtonDown(1))
            {
                CheckPossessionRadius();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsGamePaused)
        {
            RotateCharacterOnMousePosition();
            MovementControl();
        }
    }

    private void RotateCharacterOnMousePosition()
    {
        //mouse position on screen
        Vector3 screenMousePosition = Input.mousePosition;

        //mouse position on world
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(screenMousePosition);
        worldMousePosition.z = 0f;

        //normalize the direction to get a value between 0 and 1
        Vector3 direction = worldMousePosition - _character.transform.position;
        direction.Normalize();

        //angle of rotation - 90f to get the top part of the screen as the initial rotation
        float angleRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;


        Quaternion targetRotation = Quaternion.AngleAxis(angleRotation, Vector3.forward);
        _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, targetRotation, _turnRate * Time.deltaTime);
    }

    private void MovementControl()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Debug.Log($"{moveHorizontal}, {moveVertical}");
        Vector2 movement = new(moveHorizontal, moveVertical);

        movement = movement.normalized;

        _rigidBody.velocity = movement * _moveSpeed;
    }

    private void CheckPossessionRadius()
    {
        _radiusDrawer.StartDrawing();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            CharacterDataHolder characterData = hit.collider.GetComponent<CharacterDataHolder>();
            float distance = Vector3.Distance(characterData.transform.position, _character.transform.position);
            if (characterData != null && distance <= _possessionRadius)
            {
                Debug.Log(characterData.name);
                CharacterSwitch(characterData.gameObject);
            }
            else
            {
                Debug.Log(distance);
            }
        }
    }

    private void CharacterSwitch(GameObject characterData)
    {
        //reset curret weapon controller before switching up
        _character.GetComponentInChildren<WeaponController>().IsPlayer = false;
        _rigidBody.velocity = Vector2.zero;
        
        //switches current character to the new character and reassign all the values
        _character = characterData;
        _rigidBody = _character.GetComponent<Rigidbody2D>();
        _character.GetComponentInChildren<WeaponController>().IsPlayer = true;

        //Update cooldown bar UI Tracker
        CooldownBarUI.Instance.TargetObject = _character.transform;

        //Update Camera control tracker
        GetComponent<CameraController>().Character = _character;

        //Update all enemy tracking cjaracter
        EnemyController[] enemyControllerList = FindObjectsOfType<EnemyController>();
        foreach(EnemyController enemyController in enemyControllerList)
        {
            enemyController.TargetPosition = _character.transform;
        }
    }
}
