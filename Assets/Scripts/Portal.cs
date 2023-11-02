using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public float portalRotationSpeed = 30f;
    public MainCamera mainCamera;
    public PlayerController playerController;
    public GameController gameController;

    private void Update()
    {
        transform.Rotate(new Vector3(0, -1, 0) * portalRotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCamera.stopMove = true;
            playerController.playerTuchPortal = true;
            gameController.StartCoroutine("YouWin");
        }
    }
}
