using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharpenText : MonoBehaviour
{
    private Text text;
    private float RefreshCount;

    public void Start()
    {
        text = GetComponent<Text>();
    }

    public void Update()
    {
        if(RefreshCount > Time.time)
        {
            text.text = text.text;
        }
    }

    public void OnBecameVisible()
    {
        text.text = text.text;
        RefreshCount = Time.time + 1.5f;
    }
}
