using System.Collections;
using UnityEngine;
using System;

public class Sworder : Character
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private GameObject _buffSkillAnimation;
    [SerializeField] private float _timeBuffSkill;
    [SerializeField] private float _numberToHealHitpoints;
    [SerializeField] private float _timeToUseBuffSkill;
    [SerializeField] private float _addSpeedByBuff;
    [SerializeField] private float _addCriticalDamageChanse;
    [SerializeField] private float _addProtectionByBuff;
    [SerializeField] private float _addAttackSpeed;
    [SerializeField] private float _cooldownDashSkill;
    [SerializeField] private float _timeDashStun;
    [SerializeField] private Transform _dashAnimationSpawnPoint;
    [SerializeField] private GameObject _dashAnimation;

    private string targetTag = "Player";
    private GameObject attackTarget;
    private float maxHp;
    private bool isUsedBuffSkill = false;
    private float speedForAnimation = 0.5f;
    private bool _isDashSkillReadyToUse = false;
    private bool isDashActivated = false;

    private void Start()
    {
        float _secondsToUseDashSkillAfterStart = 5f;
        attackTarget = GameObject.FindGameObjectWithTag(targetTag);
        maxHp = GiveHpInfo();
        StartCoroutine(ReloadDashSkill(_secondsToUseDashSkillAfterStart));
    }

    private IEnumerator ReloadDashSkill(float seconds)
    {
        _isDashSkillReadyToUse = false;
        yield return new WaitForSeconds(seconds);
        _isDashSkillReadyToUse = true;
    }

    private void FixedUpdate()
    {
        Vector3 direction = attackTarget.transform.position - transform.position;
        float distance = direction.magnitude;
        transform.LookAt(attackTarget.transform);
        if (IsAttackingdOrStuned() == false)
        {
            if (UseBuffSkill()) return;
            UseDashSkill(distance);
            if (distance <= _attackDistance)
            {
                Attack();
            }
            else
            {
                direction.Normalize();
                transform.position += direction * _moveSpeed * Time.deltaTime;
                _animator.SetFloat("Speed", speedForAnimation);
            }
        }
    }

    private bool UseBuffSkill()
    {
        bool isUsed = false;
        float percentHpToUseBuff = 30;
        if (GiveHpInfo() / maxHp * 100f <= percentHpToUseBuff)
        {
            if (isUsedBuffSkill == false)
            {
                isUsedBuffSkill = true;
                StopCharacter(false, _timeToUseBuffSkill);
                MakeBuffAnimation();
                HealCharacter(_numberToHealHitpoints);
                isUsed = true;
                AddStrenght();
            }
        }
        return isUsed;
    }

    private void UseDashSkill(float distanse)
    {
        if (_isDashSkillReadyToUse)
        {
            float numberOfUpMoveSpeed = 30f;
            if (distanse >= 3f)
            {
                if (isDashActivated == false)
                {
                    UpMoveSpeed(numberOfUpMoveSpeed);
                    isDashActivated = true;
                }
            }
            else
            {
                if (distanse <= _attackDistance)
                {
                    if (isDashActivated)
                    {
                        MakeDashAnimation();
                        RemoveMoveSpeed(numberOfUpMoveSpeed);
                        StunTarget();
                        isDashActivated = false;
                        StartCoroutine(ReloadDashSkill(_cooldownDashSkill));
                    }
                }
            }
        }
    }

    private void MakeDashAnimation()
    {
        Instantiate(_dashAnimation, _dashAnimationSpawnPoint.transform.position, Quaternion.identity);
    }

    private void StunTarget()
    {
        ICharacter stunTarget = attackTarget.GetComponent<ICharacter>();
        stunTarget.StopCharacter(true, _timeDashStun);
    }

    private void UpMoveSpeed(float speed)
    {
        _moveSpeed += speed;
    }

    private void RemoveMoveSpeed(float speed)
    {
        _moveSpeed -= speed;
    }

    private void AddStrenght()
    {
        MakeImmunityToStun(_timeBuffSkill);
        AddMoveSpeed(_addSpeedByBuff, _timeBuffSkill);
        AddCriticalDamageChance(_addCriticalDamageChanse, _timeBuffSkill);
        AddProtection(_addProtectionByBuff, _timeBuffSkill);
        AddAttackSpeed(_addAttackSpeed, _timeBuffSkill);
    }

    public void AddMoveSpeed(float addedSpeed, float seconds)
    {
        _moveSpeed += addedSpeed;
        StartCoroutine(RemoveMoveSpeed(addedSpeed, seconds));
    }

    private IEnumerator RemoveMoveSpeed(float addedSpeed, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _moveSpeed -= addedSpeed;
        speedForAnimation = 0.5f;
    }

    private void MakeBuffAnimation()
    {
        _animator.SetTrigger("BuffSkill");
        _animator.SetFloat("AttackSpeed", 1);
        StartCoroutine(RemoveAttackSpeedAnimation());
        GameObject buffAnimation = Instantiate(_buffSkillAnimation, transform.position, Quaternion.identity);
        AuraAnimation animationControler = buffAnimation.GetComponent<AuraAnimation>();
        animationControler.GetTargetAndTimeToDeath(transform, _timeBuffSkill);
    }

    private IEnumerator RemoveAttackSpeedAnimation()
    {
        yield return new WaitForSeconds(_timeBuffSkill);
        _animator.SetFloat("AttackSpeed", 0);
    }

    private void Attack()
    {
        StopCharacter(false, GetCurrentAttackSpeed());
        MakeDamage();
        _animator.SetTrigger("Attack");
    }

    private void MakeDamage()
    {
        ICharacter enemy = attackTarget.GetComponent<ICharacter>();
        enemy.TakeDamage(GiveDamage(), GivePenetration());
    }
}
