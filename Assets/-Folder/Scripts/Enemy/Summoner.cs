using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : MonoBehaviour
{
    public LevelControl LevelControl;
    public float MinInterval;
    public float MaxInterval;

    private float Interval;
    private float Cooldown;
    private Renderer Renderer;
    private bool IsBlocked;
    private RaycastHit Hit;

    public void Start()
    {
        Renderer = GetComponent<Renderer>();
        Interval = Time.time + Random.Range(MinInterval, MaxInterval);
    }

    public void FixedUpdate()
    {
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 50), transform.forward, out Hit, 100f))
        {
            if (Hit.collider.CompareTag("Enemy"))
            {
                IsBlocked = true;
            }
        }
        else
        {
            IsBlocked = false;
        }
    }

    public void Update()
    {
        if (Renderer.enabled)
        {
            if (!IsBlocked)
            {
                if (LevelControl.TotalSummonCount > LevelControl.MinionList.Count)
                {
                    if (Time.time >= Interval)
                    {
                        GameObject Minion = Instantiate(LevelControl.MinionPrefab, transform.position, transform.rotation, LevelControl.gameObject.transform);
                        LevelControl.MinionList.Add(Minion);
                        Cooldown = Random.Range(MinInterval, MaxInterval);
                        Interval = Time.time + Cooldown;
                    }
                }
            }
        }
    }

    public void SpawnNow()
    {
        Interval = Time.time;
    }

    public void SpawnFaster(float time)
    {
        Interval -= time;
    }

    public float GetIntervalLeft()
    {
        return Mathf.Abs(Time.time - Interval);
    }
}
