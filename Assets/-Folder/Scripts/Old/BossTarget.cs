using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTarget: MonoBehaviour
{
    public BossTargetParticle BossTargetParticle;

    public void Start()
    {
        BossTargetParticle.Add(transform);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            GetComponentInParent<Boss>().Health--;
            GetComponentInParent<Boss>().GetHit();

            if (GetComponentInParent<Boss>().Health <= 0)
            {
                BossTargetParticle.Remove(transform);
                Destroy(gameObject);
            }
        }
    }
}
