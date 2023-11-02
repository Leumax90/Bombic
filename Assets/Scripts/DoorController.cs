using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    public float openDistance = 5f; // ����������, ��� ������� ����� �����������
    public Animator doorAnimator; 
    public GameObject ball;
    private bool isOpen = false; // �������� ��� ������������ ��������� �����

    private void Update()
    {
        // ������� ���������� ����� ������� � ������
        float distanceToPlayer = Vector3.Distance(transform.position, ball.transform.position);

        // ���� ����� ��������� �� ����������, ������� ��� ������ openDistance � ����� �������
        if (distanceToPlayer <= openDistance && !isOpen)
        {
            // �������� �������� �������� ��� �������� �����
            doorAnimator.SetBool("IsOpen", true);
            isOpen = true;
        }
        // ���� ����� ��������� �� ���������� ������ openDistance � ����� �������
        else if (distanceToPlayer > openDistance && isOpen)
        {
            // �������� �������� �������� ��� �������� �����
            doorAnimator.SetBool("IsOpen", false);
            isOpen = false;
        }
    }
}
