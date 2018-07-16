using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [HideInInspector]
    public bool IsDied;

    public GameObject HitParticle;
    public Image HealthFill;
    public Transform BarrelTransform;
    public GameObject BulletPrefab;
    public Animator Animator;
    public Renderer Renderer;
    public int Health;
    public int Damage;
    public float Speed;
    public int Cooldown;
    public float Delay;

    private Collider Collider;
    private GameObject Object;
    private float NextAttack;
    private int HealthMax;
    private bool StopUpdate;
    private AudioSource AudioSourceBoss;

    public void Start()
    {
        AudioSourceBoss = GetComponent<AudioSource>();
        Collider = GetComponent<Collider>();
        NextAttack = Time.time + 1.5f;
        HealthMax = Health;
        HitParticle.transform.position = new Vector3(0, 0, 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Health -= other.GetComponent<PlayerBullet>().GetDamage();
            HealthFill.fillAmount = (float)Health / HealthMax;
        }
    }

    public void Update()
    {
        if (Renderer.enabled && !IsDied && !StopUpdate)
        {
            LookAt();
            Attack();
            Die();
        }
    }

    public void LookAt()
    {
        Vector3 lookPos = Camera.main.transform.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
    }

    public void Attack()
    {
        if (Time.time > NextAttack)
        {
            Animator.SetTrigger("Attack");
            Invoke("Shoot", Delay);
            NextAttack = Time.time + Cooldown;
        }
    }

    public void Shoot()
    {
        Object = Instantiate(BulletPrefab, BarrelTransform.position, Quaternion.identity);
        Object.GetComponent<BossBullet>().Setup(this);
        AudioSourceBoss.PlayOneShot(AudioSourceBoss.clip);
    }

    public void GetHit()
    {
        Animator.SetTrigger("GetHit");
    }

    public void Die()
    {
        if (Health <= 0 && !IsDied)
        {
            Animator.SetTrigger("Die");
            StartCoroutine(DeactiveBoss());
        }
    }

    public IEnumerator DeactiveBoss()
    {
        StopUpdate = true;
        Collider.enabled = false;
        HealthFill.transform.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        IsDied = true;
        gameObject.SetActive(false);
    }
}
