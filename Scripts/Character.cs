using System.Collections;
using System;
using UnityEngine;
using TMPro;

public abstract class Character : MonoBehaviour, ICharacter
{
    [SerializeField] private float _startHp;
    [SerializeField] private float _damage;
    [SerializeField] private float _penetration;
    [SerializeField] private float _protection;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _criticalDamageChanse;
    [SerializeField] protected Animator _animator;
    [SerializeField] private TMP_Text _hitPointString;

    public static event Action<string> CharacterDead;

    private const float CriticalDamageRatio = 2f;
    private bool isStunedOrAttacking = false;
    private int numberOfStunsAndAttacks = 0;
    private float hp;
    private bool ImmunityToStun = false;

    private void Awake()
    {
        hp = _startHp;
        UpdateHealthBar();
    }
    public void HealCharacter(float numberToHealHitpoints)
    {
        hp += numberToHealHitpoints;
        UpdateHealthBar();
    }

    public void MakeImmunityToStun(float seconds)
    {
        ImmunityToStun = true;
        StartCoroutine(RemoveImmunity(seconds));
    }

    private void UpdateHealthBar()
    {
        if (hp <= 0)
        {
            hp = 0;
        }
        int roundedHP = Mathf.RoundToInt(hp);
        _hitPointString.text = roundedHP.ToString();
    }

    private IEnumerator RemoveImmunity(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ImmunityToStun = false;
    }

    public void TakeDamage(float damage, float penetration)
    {
        float coefficient = 100;
        if (_protection - penetration > 0)
        {
            hp -= damage * (1f - ((_protection - penetration) / coefficient));
        }
        else
        {
            hp -= damage;
        }
        UpdateHealthBar();
        DeathCkeck();
    }
    private void DeathCkeck()
    {
        if (hp <= 0)
        {
            CharacterDead?.Invoke(gameObject.tag);
            Destroy(gameObject);
        }
    }

    public void AddAttackSpeed(float numberToUpAttackSpeed,float seconds)
    {
        _attackSpeed -= numberToUpAttackSpeed;
        StartCoroutine(RemoveAttackSpeed(numberToUpAttackSpeed, seconds));
    }

    private IEnumerator RemoveAttackSpeed(float numberToRemoveAttackSpeed,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _attackSpeed += numberToRemoveAttackSpeed;
    }

    public void StopCharacter(bool isStuned ,float seconds)
    {
        if (isStuned)
        {
            if(ImmunityToStun==false)
            {
                StopTarget(seconds);
            }
        }
        else
        {
            StopTarget(seconds);
        }
    }

    private void StopTarget(float seconds)
    {
        isStunedOrAttacking = true;
        numberOfStunsAndAttacks++;
        StartCoroutine(LetToMove(seconds));
        _animator.SetFloat("Speed", 0);
    }

    protected float GiveHpInfo()
    {
        return hp;
    }

    public float GetCurrentAttackSpeed()
    {
        return _attackSpeed;
    }

    public bool IsAttackingdOrStuned()
    {
        return isStunedOrAttacking;
    }

    protected float GiveDamage()
    {
        int randomNumber = UnityEngine.Random.Range(1, 101);
        if (randomNumber <= _criticalDamageChanse)
        {
            return _damage * CriticalDamageRatio;
        }
        else
        {
            return _damage;
        }
    }

    protected float GivePenetration()
    {
        return _penetration;
    }

    private IEnumerator LetToMove(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        numberOfStunsAndAttacks--;
        CheckNumberOfStuns();
    }

    private void CheckNumberOfStuns()
    {
        if (numberOfStunsAndAttacks == 0)
        {
            isStunedOrAttacking = false;
        }
    }

    public void AddProtection(float addProtection, float seconds)
    {
        _protection += addProtection;
        StartCoroutine(RemoveProtection(addProtection,seconds));
    }

    private IEnumerator RemoveProtection(float removeProtection,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _protection -= removeProtection;
    }

    public void AddCriticalDamageChance(float addChance,float seconds)
    { 
        _criticalDamageChanse += addChance;
        StartCoroutine(RemoveCriticalDamageChance(addChance, seconds));
    }

    private IEnumerator RemoveCriticalDamageChance(float removeChance,float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _criticalDamageChanse -= removeChance;
    }
}