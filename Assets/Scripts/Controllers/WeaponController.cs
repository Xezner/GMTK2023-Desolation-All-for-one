using UnityEngine;
using UnityEngine.UI;

public class WeaponController : ManagerBehaviour
{
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private Image _cooldownBar;
    private bool _canAttack = true;
    private float _attackRateHolder = 1f;
    private const string NORMAL_ATTACK_TRIGGER = "NormalAttack";


    public bool IsPlayer = false;

    private void Start()
    {
        _attackRateHolder = _attackRate;
        _cooldownBar.fillAmount = 0f;
    }

    private void Update()
    {
        if (!GameManager.IsGamePaused && IsPlayer)
        {
            if (!_canAttack)
            {
                _attackRateHolder -= Time.deltaTime;

                _cooldownBar.fillAmount = _attackRateHolder / _attackRate;

                if (_attackRateHolder <= 0f)
                {

                    _canAttack = true;
                    _attackRateHolder = _attackRate;
                    _cooldownBar.fillAmount = 0f;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _weaponAnimator.SetTrigger(NORMAL_ATTACK_TRIGGER);
                    _canAttack = false;
                }
            }
        }
    }

}