using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject MenuMain;
    public GameObject MenuLevel;

    private bool ClickOnce;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && !ClickOnce)
        {
            ClickOnce = true;
            OnClick();
        }
    }

    public void OnClick()
    {
        MenuMain.SetActive(false);
        MenuLevel.SetActive(true);
    }
}
