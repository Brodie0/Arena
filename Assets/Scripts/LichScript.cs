﻿using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class LichScript : PlayerScript {

        public AudioClip whackAudio;
        public AudioClip fireballAudio;
        public AudioClip cycloneAudio;
        public GameObject fireBall;
        public Transform cyclone;

        // Use this for initialization
        void Start()  {
            if (!isLocalPlayer)
            {
                return;
            }
            MaxHp = 10;
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
                    Anim.SetTrigger (attack02Hash);
                    AudioSource.PlayOneShot (fireballAudio, 1f);
                    CmdSpawnFireball(new Vector3 (Rb.position.x, Rb.position.y + 1f, Rb.position.z) + Rb.rotation * new Vector3 (0f, 0f, 3f), Rb.rotation);
                }
                else if (Input.GetKeyDown (KeyCode.W)) {
                    //todo ms obiekt w networkmanager
                    // komenda do serwera
                    // musi byc networkidentity
                    StartCoroutine (CastSpellDelayed (cyclone, cycloneAudio, 1f));
                }
            }
        }

        private IEnumerator CastSpellDelayed(Transform spell, AudioClip audioClip, float timeToWait){
            Anim.SetTrigger(attack02Hash);
            yield return new WaitForSeconds(timeToWait);
            AudioSource.PlayOneShot (audioClip, 1f);
            Instantiate (spell, new Vector3 (Rb.position.x, Rb.position.y + 1f, Rb.position.z) + Rb.rotation * new Vector3 (0f, 0f, 3f), Rb.rotation);
        }

        [Command]
        private void CmdSpawnFireball(Vector3 position, Quaternion rotation) {
            var instantiate = Instantiate(fireBall, position, rotation);
            NetworkServer.Spawn(instantiate);
        }
    }
}
