using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Minion : MonoBehaviour
{
    public GameObject HitParticle;
    public Transform ParticleArea;
    public Image HealthFill;
    public Renderer Renderer;
    public int Health;
    public int Damage;
    public float WalkSpeed;
    public float RunSpeed;
    public float Range;
    public float Cooldown;
    public float AttackAnimationDelay;

    protected PlayerHit PlayerHit;
    protected LevelControl LevelControl;
    protected Animator Animator;
    protected Rigidbody RigidBody;
    protected Collider Collider;
    protected float Speed;
    protected float PrevSpeed;
    protected int HealthMax;
    protected float NextAttack;
    protected bool IsRun;
    protected bool IsDead;
    protected bool StopMove;
    protected GameObject ParticleInstance;
    protected RaycastHit Hit;
    protected AudioSource AudioSourceMinion;

    public void Start()
    {
        PlayerHit = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerHit>();
        LevelControl = GameObject.FindWithTag("LevelControl").GetComponent<LevelControl>();
        Animator = GetComponent<Animator>();
        RigidBody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        AudioSourceMinion = GetComponent<AudioSource>();
        Speed = WalkSpeed;
        HealthMax = Health;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Damaged(other.GetComponent<PlayerBullet>().GetDamage());
        }
    }

    public void FixedUpdate()
    {
        if (Renderer.enabled && !IsDead)
        {
            Raycast();

            if (!StopMove)
            {
                Move();
            }
        }
    }

    public void Update()
    {
        if (Renderer.enabled && !IsDead)
        {
            if (transform.localPosition.z > Range)
            {
                Run();
            }
            Attack();
            Die();
        }
    }

    public void Raycast()
    {
        if (Physics.Raycast(transform.position, transform.forward, out Hit, 50f))
        {
            if (Hit.collider.CompareTag("Enemy"))
            {
                StopMove = true;
            }
        }
        else if (StopMove)
        {
            StopMove = false;
        }
    }

    public abstract void Run();

    public void Damaged(int TempDamage)
    {
        Health -= TempDamage;
        HealthFill.fillAmount = (float)Health / HealthMax;
    }

    public void Move()
    {
        RigidBody.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }

    public void Attack()
    {
        if (transform.localPosition.z <= Range)
        {
            Speed = 0;
            StopMove = true;

            if (Time.time >= NextAttack)
            {
                StartCoroutine(InflictDamage());
                Animator.SetTrigger("Attack");
                AudioSourceMinion.PlayOneShot(AudioSourceMinion.clip);

                if (!ParticleInstance)
                {
                    ParticleInstance = Instantiate(HitParticle, transform);
                    ParticleInstance.transform.position = new Vector3(ParticleArea.position.x, ParticleArea.position.y, 0);
                }
                else
                {
                    ParticleInstance.SetActive(true);
                }

                NextAttack = Time.time + Cooldown;
            }
        }
    }

    public IEnumerator InflictDamage()
    {
        yield return new WaitForSeconds(AttackAnimationDelay);
        if(!IsDead)
        {
            PlayerHit.GetDamage(Damage);
        }
    }

    public void Die()
    {
        if (Health <= 0 && !IsDead)
        {
            Animator.SetTrigger("Die");
            LevelControl.MinionKilled();
            Speed = 0;
            IsDead = true;
            StopMove = true;
            Collider.enabled = false;
            HealthFill.transform.parent.gameObject.SetActive(false);
            StartCoroutine(DeactiveMinion());
        }
    }

    public IEnumerator DeactiveMinion()
    {
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        gameObject.SetActive(false);
    }
}
