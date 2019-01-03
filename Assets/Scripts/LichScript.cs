using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class LichScript : PlayerScript {

        public AudioClip whackAudio;
        public AudioClip fireballAudio;
        public AudioClip cycloneAudio;
        public GameObject fireBall;
        public GameObject cyclone;

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
                    Anim.SetTrigger (attack01Hash);
                    AudioSource.PlayOneShot (cycloneAudio, 1f);
                    CmdSpawnCyclone(new Vector3 (Rb.position.x, Rb.position.y + 1f, Rb.position.z) + Rb.rotation * new Vector3 (0f, 0f, 9f), Rb.rotation);
                }
            }
        }

        [Command]
        private void CmdSpawnFireball(Vector3 position, Quaternion rotation) {
            var instantiate = Instantiate(fireBall, position, rotation);
            NetworkServer.Spawn(instantiate);
        }

        [Command]
        private void CmdSpawnCyclone(Vector3 position, Quaternion rotation) {
            var instantiate = Instantiate(cyclone, position, rotation);
            NetworkServer.Spawn(instantiate);
        }
    }
}
