using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private float explosionDelay = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(ExplodeWithDelay());
        }
    }

    private IEnumerator ExplodeWithDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Destroy(gameObject);
    }
}
