using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Player
{
    [SerializeField] private float _attackRadius;
    [SerializeField] private GameObject _stunPrefab;
    [SerializeField] private LayerMask _enemyMask;
    [SerializeField] private Transform _magicBallSpawnPoint;
    [SerializeField] private GameObject _magicBall;
    [SerializeField] private float _timeStunSkill;
    [SerializeField] private float _timeToUseStunSkill;
    [SerializeField] private float _secondsToReloadStun;
    [SerializeField] private float _ticksNumberOfHeal;
    [SerializeField] private float _secondsToReloadHealSkill;
    [SerializeField] private float _healOfOneTick;
    [SerializeField] private GameObject _healAnimation;

    public static event Action<GameObject> MakeMenu;

    private bool isStunReadyToUse = true;
    private bool isHealSkillReadyToUse = true;

    public void Attacking()
    {
        if (IsAttackingdOrStuned() == false)
        {
            Collider attackTarget = DesignateTarget(_attackRadius, _enemyMask);
            if (attackTarget != null)
            {
                StopCharacter(false, GetCurrentAttackSpeed());
                LookAtEnemy(attackTarget.transform.position);
                _animator.SetTrigger("Attack");
                CreateMagicBall(attackTarget.transform);
            }
        }
    }

    public float SecondsToReloadHealSkill()
    {
        return _secondsToReloadHealSkill + _ticksNumberOfHeal;
    }

    private void CreateMagicBall(Transform attackTarget)
    {
        GameObject magicBall = Instantiate(_magicBall, _magicBallSpawnPoint.transform.position, Quaternion.identity);
        MagicBall magicBall1Controler = magicBall.GetComponent<MagicBall>();
        magicBall1Controler.GetPoints(attackTarget, _magicBallSpawnPoint, GiveDamage(), GivePenetration());
    }

    public bool Stuning()
    {
        bool stunUsed = false;
        if (isStunReadyToUse)
        {
            if (IsAttackingdOrStuned() == false)
            {
                Collider stunTarget = DesignateTarget(_attackRadius, _enemyMask);
                if (stunTarget != null)
                {
                    isStunReadyToUse = false;
                    StopCharacter(false, _timeToUseStunSkill);
                    LookAtEnemy(stunTarget.transform.position);
                    StunEnemy(stunTarget);
                    CreateStunAnimations(stunTarget.transform.position);
                    StartCoroutine(ReloadingStun());
                    stunUsed = true;
                }
            }
        }
        return stunUsed;
    }

    public bool Healing()
    {
        bool isHealUsed = false;
        if (IsAttackingdOrStuned() == false)
        {
            if (isHealSkillReadyToUse)
            {
                isHealSkillReadyToUse = false;
                StartCoroutine(UseTicksOfHeal());
                isHealUsed = true;
                StartCoroutine(ReloadHealSkill());
                MakeHealAnimation();
            }
        }
        return isHealUsed;
    }

    private void MakeHealAnimation()
    {
        GameObject healAnimation = Instantiate(_healAnimation, transform.position, Quaternion.identity);
        AuraAnimation animationControler = healAnimation.GetComponent<AuraAnimation>();
        animationControler.GetTargetAndTimeToDeath(transform, _ticksNumberOfHeal);
    }

    private IEnumerator ReloadHealSkill()
    {
        yield return new WaitForSeconds(_ticksNumberOfHeal + _secondsToReloadHealSkill);
        isHealSkillReadyToUse = true;
    }

    private IEnumerator UseTicksOfHeal()
    {
        float ticksNumberOfHeal = _ticksNumberOfHeal;
        do
        {
            HealCharacter(_healOfOneTick);
            ticksNumberOfHeal--;
            yield return new WaitForSeconds(1);
        }
        while (ticksNumberOfHeal > 0);
    }

    private IEnumerator ReloadingStun()
    {
        yield return new WaitForSeconds(_secondsToReloadStun);
        isStunReadyToUse = true;
    }

    public float TimeReloadingStun()
    {
        return _secondsToReloadStun;
    }

    private void CreateStunAnimations(Vector3 enemyPosition)
    {
        GameObject stunPrefab = Instantiate(_stunPrefab, enemyPosition, Quaternion.identity);
        StartCoroutine(DestroyStunAnimation(stunPrefab));
        _animator.SetTrigger("Stun");
    }

    private IEnumerator DestroyStunAnimation(GameObject stunPrefab)
    {
        yield return new WaitForSeconds(_timeStunSkill);
        Destroy(stunPrefab);
    }

    private void StunEnemy(Collider stunTarget)
    {
        ICharacter enemy = stunTarget.GetComponent<ICharacter>();
        enemy.StopCharacter(true, _timeStunSkill);
    }

    private void LookAtEnemy(Vector3 enemyPosition)
    {
        transform.LookAt(enemyPosition);
    }
}

