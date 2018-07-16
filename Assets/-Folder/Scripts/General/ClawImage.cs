using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClawImage : MonoBehaviour
{
    private Image Image;
    private Color Color;
    private float Duration;

    private float Alpha;
    private float Lerp;
    private float Timer;

    public void Start()
    {
        Image = GetComponent<Image>();
        Color = Image.color;
        Duration = 3f;
    }

    public void PlayEffect()
    {
        Timer = Time.time + Duration;
        Image.enabled = true;
        Lerp = 0;
    }

    public void Update()
    {
        if (Time.time > Timer)
        {
            Image.enabled = false;
        }
        else
        {
            Alpha = Mathf.Lerp(1, 0, Lerp);
            Color.a = Alpha;
            Image.color = Color;
            Lerp += Time.deltaTime / Duration;
        }
    }
}
