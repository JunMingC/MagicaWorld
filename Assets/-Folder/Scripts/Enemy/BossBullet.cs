using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Renderer Renderer;
    private Rigidbody RgBody;
    private int Damage;
    private float Speed;
    private bool Ready;
    private GameObject CameraAR;
    private GameObject HitParticle;

    public void Setup(Boss param)
    {
        CameraAR = Camera.main.gameObject;
        RgBody = GetComponent<Rigidbody>();
        Damage = param.Damage;
        Speed = param.Speed;
        HitParticle = param.HitParticle;
        transform.LookAt(CameraAR.transform.position);
        Ready = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HitParticle.GetComponent<ParticleSystem>().Play();
            other.GetComponentInChildren<PlayerHit>().GetDamage(Damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }

    public void FixedUpdate()
    {
        if (Ready && Renderer.enabled)
        {
            Move();
        }
    }

    private void Move()
    {
        RgBody.AddForce(transform.forward * Time.deltaTime * Speed * 100f, ForceMode.VelocityChange);

        if (transform.position.z < CameraAR.transform.position.z + (-100f))
        {
            Destroy(gameObject);
        }
    }
}
