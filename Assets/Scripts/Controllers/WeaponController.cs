using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Animator _weaponAnimator;


    private const string WEAPON_NORMAL_ATTACK = "WeaponNormalAttack";

    private const string NORMAL_ATTACK_TRIGGER = "NormalAttack";

    private void Update()
    {
        if(IsAnimationPlaying(WEAPON_NORMAL_ATTACK))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _weaponAnimator.SetTrigger(NORMAL_ATTACK_TRIGGER);
        }

        if(Input.GetMouseButtonDown(1))
        {
            Debug.LogError("RIGHT CLICK");
        }
    }

    private bool IsAnimationPlaying(string animationName)
    {
        AnimatorStateInfo stateInfo = _weaponAnimator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime < 1f;
    }
}