using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraAnimation : MonoBehaviour
{
    private float timeToDeath;
    private Transform targetPosition;

    public void GetTargetAndTimeToDeath(Transform targetPosition, float timeToDeath)
    {
        this.targetPosition = targetPosition;
        this.timeToDeath = timeToDeath;
    }

    private void Start()
    {
        StartCoroutine(DestroyAnimation());
    }

    private IEnumerator DestroyAnimation()
    {
        yield return new WaitForSeconds(timeToDeath);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        transform.position = targetPosition.transform.position;
    }
}
