using UnityEngine;
using System.Collections.Generic;


public class Player : Character
{
    private readonly float m_interpolation = 10;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private float _moveSpeed;

    private float m_currentV;
    private float m_currentH;
    private Vector3 m_currentDirection;

    private void FixedUpdate()
    {
        if (IsAttackingdOrStuned() == false)
        {
            float v = _joystick.Vertical;
            float h = _joystick.Horizontal;
            Transform camera = Camera.main.transform;
            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);
            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;
            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;
            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * _moveSpeed * Time.deltaTime;

                _animator.SetFloat("Speed", direction.magnitude);
            }
        }
    }

    protected Collider DesignateTarget(float attackRadius,LayerMask enemyMask)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, enemyMask);
        Collider closestEnemy = null;
        List<Collider> enemies = FindEnemies(colliders);
        if (enemies.Count > 0)
        {
            closestEnemy = FindClosestEnemy(enemies);
        }
        return closestEnemy;
    }

    private List<Collider> FindEnemies(Collider[] colliders)
    {
        List<Collider> enemies = new List<Collider>();
        foreach (Collider collider in colliders)
        {
            Collider enemy = collider.GetComponent<Collider>();
            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }

        return enemies;
    }

    private Collider FindClosestEnemy(List<Collider> enemies)
    {
        Collider closestEnemy = enemies[0];
        float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
        foreach (Collider enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }
        return closestEnemy;
    }
}

