using System.Collections;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    [SerializeField] private float _secondsForRelise;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _destroyAnimation;

    private Transform _enemy;
    private Transform _spawnPoint;
    private bool isRelised = false;
    private float damage;
    private float penetration;

    public void GetPoints(Transform enemy, Transform spawnPoint, float damage, float penetration)
    {
        _enemy = enemy;
        _spawnPoint = spawnPoint;
        this.damage = damage;
        this.penetration = penetration;
    }

    private void Awake()
    {
        StartCoroutine(ReliseMagicBalll());
        EnableCollider();
    }

    private void EnableCollider()
    {
        Collider collider = GetComponent<Collider>();
        StartCoroutine(TimeToEnableCollider(collider));
    }

    private IEnumerator TimeToEnableCollider(Collider collider)
    {
        yield return new WaitForSeconds(_secondsForRelise);
        collider.enabled = true;
    }

    private void FixedUpdate()
    {
        if (isRelised == false)
        {
            transform.position = _spawnPoint.transform.position;
        }
        else
        {
            Vector3 direction = _enemy.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            transform.position = transform.position + direction * _speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MakeDamage(other);
        Destroy(gameObject);
        MakeAnimation();
    }

    private void MakeAnimation()
    {
        GameObject animation = Instantiate(_destroyAnimation, transform.position, Quaternion.identity);
    }

    private void MakeDamage(Collider attackTarget)
    {
        ICharacter enemy = attackTarget.GetComponent<ICharacter>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage, penetration);
        }
    }

    private IEnumerator ReliseMagicBalll()
    {
        yield return new WaitForSeconds(_secondsForRelise);
        isRelised = true;
    }
}
