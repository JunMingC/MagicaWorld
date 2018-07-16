﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footman : Minion
{
    public override void Run()
    {
        if (Health <= HealthMax - 1 && !IsRun)
        {
            Animator.SetTrigger("Run");
            StartCoroutine(MiniStun());
            IsRun = true;
        }
    }

    public IEnumerator MiniStun()
    {
        Speed = 0;
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        Speed = RunSpeed;
    }
}
