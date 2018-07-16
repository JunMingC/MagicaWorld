using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDefeat : MonoBehaviour
{
    public Animator Animator;

    public void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        Animator.Play("VictoryDefeat");
    }
}
