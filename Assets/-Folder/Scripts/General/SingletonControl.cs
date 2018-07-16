using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonControl : MonoBehaviour
{
    public static bool FirstRun = true;

    public GameObject Camera;
    public GameObject Environment;

    void Awake()
    {
        if (FirstRun)
        {
            Camera.transform.root.gameObject.SetActive(true);
            Environment.SetActive(true);
            DontDestroyOnLoad(Camera.transform.root.gameObject);
            DontDestroyOnLoad(Environment);
            DontDestroyOnLoad(gameObject);
            FirstRun = false;
        }
        else
        {
            Destroy(Camera.transform.root.gameObject);
            Destroy(Environment);
            Destroy(gameObject);
        }
    }
}
