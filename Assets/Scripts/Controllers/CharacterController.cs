using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : ManagerBehaviour
{
    [Header("Character Data")]
    [SerializeField] private float _turnRate = 5f;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _possessionRadius = 3f;
    [SerializeField] private Rigidbody2D _rigidBody;

    [Header("Game Objects")]
    [SerializeField] private PossessionRadiusDrawer _radiusDrawer;
    [SerializeField] private GameObject _character;
    [SerializeField] private GameObject _symbiote;
    [SerializeField] private GameObject _ftuePrompt;

    private string _playerLayer = "Player";
    private string _enemyLayer = "Enemy";
    public float PossessionRadius { get { return _possessionRadius; } }
    public GameObject Character { get { return _character; } }

    private void Start()
    {
        Debug.Log(gameObject.name);
        Init();
    }

    private void Init()
    {
        if (_character != null)
        {
            CooldownBarUI.Instance.TargetObject = _character.transform;
            _rigidBody = _character.GetComponent<Rigidbody2D>();
            _character.transform.GetChild(0).GetComponent<WeaponController>().IsPlayer = true;
        }
        UpdateEnemyControllers();
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused)
        {
            bool RadiusIsOn = true;
            if (Input.GetMouseButtonDown(1))
            {
                _radiusDrawer.StartDrawing();
                RadiusIsOn = true;
            }
            
           

            if (Input.GetMouseButtonUp(1)&& RadiusIsOn==true)
            {
                CheckPossessionRadius();
            }
            else
            {
                RadiusIsOn = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsGamePaused && _character != null)
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

        Vector2 movement = new(moveHorizontal, moveVertical);

        movement = movement.normalized;

        _rigidBody.velocity = movement * _moveSpeed;
    }

    private void CheckPossessionRadius()
    {
        //_radiusDrawer.StartDrawing();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Character") && hit.collider.gameObject != _character)
        {
            CharacterDataHolder characterData = hit.collider.GetComponent<CharacterDataHolder>();
            float distance = Vector3.Distance(characterData.transform.position, 
                _character ? _character.transform.position: transform.position);
            if (characterData != null && distance <= _possessionRadius)
            {
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
        //Play possession here
        SymbiotePossessionAnimation();

        //reset curret weapon controller before switching up
        ResetCurrentCharacter();

        //switches current character to the new character and reassign all the values
        SwitchToTargetCharacter(characterData);

        //Update cooldown bar UI Tracker
        CooldownBarUI.Instance.TargetObject = _character.transform;

        //Update Camera control tracker
        GetComponent<CameraController>().Character = _character;

        //Update all enemy tracking cjaracter
        UpdateEnemyControllers();
    }

    private void SymbiotePossessionAnimation()
    {
        //Play possession here
        if (_ftuePrompt)
        {
            _ftuePrompt.SetActive(false);
            GameManager.IsWaitingForFirstPossession = false;
        }
        _symbiote.SetActive(false);
    }

    private void ResetCurrentCharacter()
    {
        //reset curret weapon controller before switching up
        if (_character != null)
        {
            _character.GetComponentInChildren<WeaponController>().IsPlayer = false;
            _rigidBody.velocity = Vector2.zero;
            CooldownBarUI.Instance.ResetCoolDownBarUI();
            Destroy(_character);
        }
    }

    private void SwitchToTargetCharacter(GameObject characterData)
    {
        //switches current character to the new character and reassign all the values
        _character = characterData;
        _rigidBody = _character.GetComponent<Rigidbody2D>();
        _character.GetComponentInChildren<WeaponController>().IsPlayer = true;
    }

    private void UpdateEnemyControllers()
    {
        EnemyController[] enemyControllerList = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemyController in enemyControllerList)
        {
            if (_character != null)
            {
                if (enemyController.WeaponController.IsPlayer)
                {
                    enemyController.gameObject.layer = LayerMask.NameToLayer(_playerLayer);
                    enemyController.WeaponController.InitData();
                    Destroy(enemyController);
                    continue;
                }
            }
            enemyController.gameObject.layer = LayerMask.NameToLayer(_enemyLayer);
            enemyController.TargetPosition = _character ? _character.transform : transform;
        }
    }
}
