using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    public SceneControl Scene;
    public string NextSceneName;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        Scene.LoadScene(NextSceneName);
    }
}
