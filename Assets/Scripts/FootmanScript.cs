﻿using UnityEngine;

namespace Assets.Scripts {
    public class FootmanScript : PlayerScript {

        public AudioClip whack;

        void Start () {
            if (!isLocalPlayer)
            {
                return;
            }
            MaxHp = 20;
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
                    Anim.SetTrigger (Attack01Hash);
                    AudioSource.PlayOneShot (whack, 1f);
                }
                if (Input.GetKeyDown (KeyCode.W)) {
                    Anim.SetTrigger (Attack02Hash);
                    AudioSource.PlayOneShot (whack, 1f);
                }
            }
        }
    }
}
