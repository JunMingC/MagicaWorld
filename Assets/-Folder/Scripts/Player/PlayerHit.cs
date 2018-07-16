using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHit : MonoBehaviour
{
    public Text HealthText;
    public Image HealthFill;
    public int HealthMax;
    public int HealthNow;

    private int HealthPrev = -1;
    private Color DarkRedColor;
    private Color SkyboxColor;
    private Color LerpColor;
    private AudioSource AudioSourcePlayerHit;

    public void Start()
    {
        HealthMax = HealthNow;
        ColorUtility.TryParseHtmlString("#800000FF", out DarkRedColor);
        ColorUtility.TryParseHtmlString("#80808080", out SkyboxColor);
        AudioSourcePlayerHit = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (HealthNow != HealthPrev)
        {
            HealthPrev = HealthNow;
            HealthText.text = HealthNow + "/" + HealthMax;
            HealthFill.fillAmount = (float)HealthNow / HealthMax;
            LerpSkybox();
        }
    }

    public void GetDamage(int Damage)
    {
        HealthNow -= Damage;
        AudioSourcePlayerHit.PlayOneShot(AudioSourcePlayerHit.clip);
    }

    public void LerpSkybox()
    {
        if (RenderSettings.skybox.HasProperty("_Tint"))
        {
            LerpColor = Color.Lerp(DarkRedColor, SkyboxColor, (float)HealthNow / HealthMax);
            RenderSettings.skybox.SetColor("_Tint", LerpColor);
        }
    }
}
