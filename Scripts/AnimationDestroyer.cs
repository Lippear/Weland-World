using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroyer : MonoBehaviour
{
    [SerializeField] private float _secondsToDestroyAnimaton;

    private void Start()
    {
        StartCoroutine(DestroyAnimation());
    }

    private IEnumerator DestroyAnimation()
    {
        yield return new WaitForSeconds(_secondsToDestroyAnimaton);
        Destroy(gameObject);
    }
}
