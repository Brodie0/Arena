﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking;

public class GolemScript : PlayerScript {

    public AudioClip whack;

    void Start () {
        if (!isLocalPlayer)
        {
            return;
        }
        MaxHp = 50;
        Hp = MaxHp;
        base.Start();
    }
	
    // Update is called once per frame
    void FixedUpdate () {
        if (!isLocalPlayer)
        {
            return;
        }
        if (!IsDead) {
            if (Input.GetKeyDown (KeyCode.Q)) {
                NetAnim.SetTrigger(Attack01Hash);
                AudioSource.PlayOneShot (whack, 1f);
            }
            if (Input.GetKeyDown (KeyCode.W)) {
                NetAnim.SetTrigger(Attack02Hash);
                AudioSource.PlayOneShot (whack, 1f);
            }
        }
    }
}
