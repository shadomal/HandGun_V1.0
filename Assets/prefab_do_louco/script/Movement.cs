using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody rig;
    public Animator anim;
    private int s;

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        s = 20;
    }

    public void move(float moveH, float moveV)
    {
        if (moveV == 0 && moveH == 0)
        {
            return;
        }

        rig.velocity = transform.forward * moveV * s;
        transform.Rotate(0, moveH * s, 0);
        anim.SetFloat("Blend X", moveH);
        anim.SetFloat("Blend Y", moveV);
    }
}