using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    public float openDistance = 5f; // –ассто€ние, при котором дверь открываетс€
    public Animator doorAnimator; 
    public GameObject ball;
    private bool isOpen = false; // ѕараметр дл€ отслеживани€ состо€ни€ двери

    private void Update()
    {
        // Ќаходим рассто€ние между игроком и дверью
        float distanceToPlayer = Vector3.Distance(transform.position, ball.transform.position);

        // ≈сли игрок находитс€ на рассто€нии, меньшем или равном openDistance и дверь закрыта
        if (distanceToPlayer <= openDistance && !isOpen)
        {
            // ¬ключаем параметр анимации дл€ открыти€ двери
            doorAnimator.SetBool("IsOpen", true);
            isOpen = true;
        }
        // ≈сли игрок находитс€ на рассто€нии больше openDistance и дверь открыта
        else if (distanceToPlayer > openDistance && isOpen)
        {
            // ¬ключаем параметр анимации дл€ закрыти€ двери
            doorAnimator.SetBool("IsOpen", false);
            isOpen = false;
        }
    }
}
