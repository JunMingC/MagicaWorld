using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public LevelControl LevelControl;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        LevelControl.ResumeGame();
    }
}
