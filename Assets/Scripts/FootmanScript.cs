using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
                    Anim.SetTrigger (Random.Range (0, 2) == 0 ? Attack01Hash : Attack02Hash);
                    AudioSource.PlayOneShot (whack, 1f);
                }
            }
        }
    }
}
