using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float moveSpeed = 0;
    public float explosionDelay = 0.5f; 
    public GameObject explosionEffect; 
    public GameObject bomb;
    private GameObject player; 
    private Vector3 previousPosition;
    private Collider selfColider;
    private AudioSource explosionSound;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        selfColider = GetComponent<SphereCollider>();
        explosionSound = GetComponent<AudioSource>();
        previousPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(5, 10, 0) * Time.deltaTime);

        Vector3 newPosition = transform.position + Vector3.forward * moveSpeed * Time.deltaTime;

        if (newPosition.z < previousPosition.z)
        {
            newPosition.z = previousPosition.z;
        }

        previousPosition = newPosition;
        transform.position = newPosition;

        transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Obstacle"))
                {
                    StartCoroutine(ExplodeObstacle(hitCollider.gameObject));
                }
            }
        }

        if (collision.gameObject.CompareTag("FrontLimit"))
        {
            ExplodeProjectile();
        }
    }

    private IEnumerator ExplodeObstacle(GameObject hitObstacle)
    {
        yield return new WaitForSeconds(explosionDelay);
        moveSpeed = 0;
        explosionEffect.SetActive(true);
        bomb.SetActive(false);

        explosionSound.Play();

        yield return new WaitForSeconds(explosionDelay);
        Destroy(hitObstacle);
        selfColider.enabled = false;

        // we wait until the Animation is completely finished
        yield return new WaitForSeconds(explosionDelay*2);
        ExplodeProjectile();
    }

    private void ExplodeProjectile()
    {
        Destroy(gameObject);
    }
}
