﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

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
                Anim.SetTrigger (Random.Range (0, 2) == 0 ? attack01Hash : attack02Hash);
                AudioSource.PlayOneShot (whack, 1f);
            }
        }
    }
}
