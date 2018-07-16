using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float Delay = 2;

    public void Start()
    {
        Destroy(gameObject, Delay);
    }
}
