using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControl : MonoBehaviour
{
    public Rigidbody RgBody;
    public GameObject source;
    public GameObject target;
    public int damage;
    public float speed;
    public Vector3 tempDirection;
    public bool isInitialized;

    public void FixedUpdate()
    {
        if (!isInitialized) return;
        Move();
    }

    public void Initialize(GameObject TempSource, GameObject TempTarget, int TempDamage, float TempSpeed, Vector3 TempDirection)
    {
        source = TempSource;
        target = TempTarget;
        damage = TempDamage;
        speed = TempSpeed;
        tempDirection = TempDirection;
        
        transform.LookAt(tempDirection);
        isInitialized = true;
    }

    public void Move()
    {

        RgBody.AddForce(transform.forward * speed);

        if (source.CompareTag("Player"))
        {
            if (transform.position.z > 500)
            {
               Destroy(gameObject);
            }
        }
        else if(source.CompareTag("Enemy"))
        {
            if (transform.position.z < target.transform.position.z)
            {
              Destroy(gameObject);
            }
        }
    }
}
