using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandDestroy : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(DestroyAfterDelay());
    }
    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
