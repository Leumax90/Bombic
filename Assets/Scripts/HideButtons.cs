using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideButtons : MonoBehaviour 
{
    public void HideButtonss()
    {
        Image[] imagesToDisable = GetComponentsInChildren<Image>();
        foreach (Image image in imagesToDisable)
        {
            image.enabled = false;
        }
    }

    public void ShowButtons()
    {
        Image[] imagesToEnable = GetComponentsInChildren<Image>();
        foreach (Image image in imagesToEnable)
        {
            image.enabled = true;
        }
    }
}