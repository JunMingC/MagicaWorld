using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet: MonoBehaviour
{
    public Renderer Renderer;
    private Rigidbody RgBody;
    private GameObject HitParticle;
    private int Damage;
    private float Speed;
    private Vector3 Direction;
    private float Duration;
    private bool Ready;
    private float TimeSinceStartup;

    public void Setup(PlayerInput param)
    {
        RgBody = GetComponent<Rigidbody>();
        HitParticle = param.HitParticle;
        Damage = param.Damage;
        Speed = param.Speed;
        Direction = param.Direction;
        Duration = param.Duration;
        transform.LookAt(Direction);
        Destroy(gameObject, Duration);
        Ready = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(HitParticle, transform.position, Quaternion.identity);
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
        if (Time.timeScale < 1)
        {
            RgBody.AddForce(transform.forward * Time.unscaledDeltaTime * Speed * 100f * 200f, ForceMode.VelocityChange);
        }
        else
        {
            RgBody.AddForce(transform.forward * Time.deltaTime * Speed * 100f, ForceMode.VelocityChange);
        }
    }

    public int GetDamage()
    {
        if (Time.timeScale < 1)
        {
            return 0;
        }
        else
        {
            return Damage;
        }
    }
}
