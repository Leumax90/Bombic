using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform ballTransform;
    public Transform cameraPointFinish;
    public Transform cameraPointPlayer;
    private float Speed = 2.5f; // Исходное значение скорости изменения масштаба камеры
    public bool stopMove = false;

    void Start()
    {
        StartCoroutine(ChangeSpeed());
    }

    void Update()
    {
        if (!stopMove)
        {
            Vector3 targetPosition = cameraPointPlayer.position;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / Speed);

            // Плавное вращение камеры
            Quaternion targetRotation = cameraPointPlayer.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / Speed);

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, cameraPointFinish.transform.position, Time.deltaTime);
        }
    }

    private IEnumerator ChangeSpeed()
    {
        yield return new WaitForSeconds(5.0f);
        Speed = 0.1f; 
    }
}
