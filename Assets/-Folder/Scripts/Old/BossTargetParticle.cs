using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossTargetParticle : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> others = new List<Transform>();

    public ParticleSystem particle;
    public bool autoRemove;

    private ParticleSystem.Particle[] particles;
    private List<GameObject> go = new List<GameObject>();

    public void Start()
    {
        particles = new ParticleSystem.Particle[particle.main.maxParticles];
        if (autoRemove)
            StartCoroutine(AutoRemove());
        if (particle == null)
            GetComponent<ParticleSystem>();
        if (particle == null)
            particle = gameObject.AddComponent<ParticleSystem>();
    }

    public void LateUpdate()
    {
        int length = particle.GetParticles(particles);
        int diff = others.Count - length;
        for (int j = 0; j < diff; j++)
        {
            particle.Emit(1);
        }
        length = particle.GetParticles(particles);
        for (int i = 0; i < others.Count; i++)
        {
            particles[i].position = others[i].position;
        }
        particle.SetParticles(particles, length);
    }

    public void Add(Transform tr)
    {
        if (!others.Contains(tr))
        {
            others.Add(tr);
            go.Add(tr.gameObject);
        }
    }

    public void Remove(Transform tr)
    {
        if (others.Contains(tr))
        {
            others.Remove(tr);
            go.Remove(tr.gameObject);
        }
    }

    IEnumerator AutoRemove()
    {
        while (true)
        {
            for (int k = 0; k < others.Count; k++)
            {
                if (!go[k].activeSelf)
                {
                    Remove(others[k]);
                }
            }
            yield return null;
        }
    }
}
