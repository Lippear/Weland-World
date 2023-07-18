using System.Collections;
using UnityEngine;

public class MagicBallHitAnimation : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyAnimation());
    }

    private IEnumerator DestroyAnimation()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
}
