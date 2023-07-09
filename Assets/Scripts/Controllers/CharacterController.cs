using System;
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
    [SerializeField]private CharacterDataHolder _characterDataHolder;
    [Header("Game Objects")]
    [SerializeField] private PossessionRadiusDrawer _radiusDrawer;
    [SerializeField] private GameObject _character;
    [SerializeField] private GameObject _symbiote;
    [SerializeField] private GameObject _ftuePrompt;

    private string _playerLayer = "Player";
    private string _enemyLayer = "Enemy";
    public Rigidbody2D RigidBody { get { return _rigidBody; } }
    public float PossessionRadius { get { return _possessionRadius; } }
    public GameObject Character { get { return _character; } }
    public Vector2 MouseDirection = Vector2.zero;


    public bool Test = false;
    //Knowckback
    private float _knockbackDuration = 0.2f;
    public bool IsKnockedBack = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _ftuePrompt = GameManager.FTUEPrompt;
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
        if (!GameManager.IsGamePaused && GameManager.IsControllable)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _radiusDrawer.StartDrawing();
            }
            if (Input.GetMouseButtonUp(1)&& _radiusDrawer.IsRadiusOn)
            {
                CheckPossessionRadius();
            }

            if (_character != null)
            {
                CheckHP();
            }
        }
    }

    private void CheckHP()
    {
        if(_characterDataHolder.HP <= 0)
        {
            Debug.Log("DEAD");
            GameManager.GameOverScreen();
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsGamePaused && _character != null)
        {

            RotateCharacterOnMousePosition();
            if(GameManager.IsControllable)
                MovementControl();
        }
    }

    private void RotateCharacterOnMousePosition()
    {
        //mouse position on screen
        Vector3 screenMousePosition = Input.mousePosition;

        //mouse position on world
        Vector3 worldMousePosition = GameManager.Camera.ScreenToWorldPoint(screenMousePosition);
        worldMousePosition.z = 0f;

        //normalize the direction to get a value between 0 and 1
        Vector3 direction = worldMousePosition - _character.transform.position;
        direction.Normalize();
        MouseDirection = direction;
        //angle of rotation - 90f to get the top part of the screen as the initial rotation
        float angleRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;


        Quaternion targetRotation = Quaternion.AngleAxis(angleRotation, Vector3.forward);
        _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, targetRotation, _turnRate * Time.deltaTime);
    }

    private void MovementControl()
    {
        if (IsKnockedBack)
        {
            _knockbackDuration -= Time.deltaTime;
            if (_knockbackDuration <= 0f)
            {
                IsKnockedBack = false;
                Debug.Log("Done Knockback");
            }
            return;
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new(moveHorizontal, moveVertical);

        movement = movement.normalized;

        _rigidBody.velocity = movement * _moveSpeed;
    }

    public void ApplyKnockBack(Vector2 direction, float knockbackMagnitude)
    {
        IsKnockedBack = true;
        _knockbackDuration = 0.2f;
        _rigidBody.AddForce(direction.normalized * knockbackMagnitude, ForceMode2D.Impulse);
    }

    private void CheckPossessionRadius()
    {
        //_radiusDrawer.StartDrawing();
        Vector2 mousePosition = GameManager.Camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Character") && hit.collider.gameObject != _character)
        {
            CharacterDataHolder characterData = hit.collider.GetComponent<CharacterDataHolder>();
            float distance = Vector3.Distance(characterData.transform.position, 
                _character ? _character.transform.position: transform.position);
            if (characterData != null && distance <= _possessionRadius && characterData.HP > 0)
            {
                CharacterSwitch(characterData.gameObject);
            }
            else
            {
                //Debug.Log(distance);
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
        if (Test)
        {
            Debug.Log("FTUE");
            _ftuePrompt.SetActive(false);
            GameManager.IsWaitingForFirstPossession = false;
            _symbiote.SetActive(false);
        }
        else
        {
            GameManager.IsControllable = false;
            Debug.Log("NO FTUE");
            GameManager.IsWaitingForFirstPossession = true;
            _symbiote.SetActive(true);
            _symbiote.GetComponent<SymbioteController>().AnimatePossession();
        }

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
        _character.name = "Player";
        _rigidBody = _character.GetComponent<Rigidbody2D>();
        _character.GetComponentInChildren<WeaponController>().IsPlayer = true;
        _characterDataHolder = _character.GetComponent<CharacterDataHolder>();
        _characterDataHolder.HP += 10;
        _characterDataHolder.AtkRate *= 0.7f;
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
                    Destroy(enemyController.CollisionDetector);
                    Destroy(enemyController);
                    continue;
                }
            }
            enemyController.gameObject.layer = LayerMask.NameToLayer(_enemyLayer);
            enemyController.TargetPosition = _character ? _character.transform : transform;
        }
    }
}
