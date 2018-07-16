using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector]
    public Vector3 Direction;

    public Transform BarrelTransform;
    public GameObject BulletPrefab;
    public GameObject HitParticle;
    public int Damage;
    public float Speed;
    public float Range;
    public float Cooldown;
    public float Duration;

    private Vector3 CameraCenter;
    private Vector3 CameraForward;
    private GameObject Object;
    private float NextFire;
    private SceneControl SceneControl;
    private AudioSource AudioSourceBarrel;

    public void Start()
    {
        AudioSourceBarrel = BarrelTransform.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (Input.GetButton("Fire1") && NextFire < Time.unscaledTime)
        {
            CameraCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            CameraForward = Camera.main.transform.forward;
            Direction = CameraCenter + CameraForward * Range;
            Object = Instantiate(BulletPrefab, BarrelTransform.position, Quaternion.identity);
            Object.GetComponent<PlayerBullet>().Setup(this);
            AudioSourceBarrel.PlayOneShot(AudioSourceBarrel.clip);
            NextFire = Time.unscaledTime + Cooldown;
        }

        /*
                if ((Input.GetButtonDown("Fire2")))
                {
                    SceneControl = GameObject.FindWithTag("SceneControl").GetComponent<SceneControl>();
                    int SceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

                    if (SceneIndex >= 4)
                    {
                        SceneIndex = 0;
                    }

                    SceneControl.LoadScene(SceneIndex);
                }
                */
    }
}
