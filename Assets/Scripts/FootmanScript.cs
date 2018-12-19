using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class FootmanScript : PlayerScript {

        public AudioClip whack;
        int attack01Hash = Animator.StringToHash("Attack01");
        int attack02Hash = Animator.StringToHash("Attack02");

        void Start () {
            MaxHp = 20;
            Hp = MaxHp;
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
}
