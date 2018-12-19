using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class FootmanScript : NetworkBehaviour {

        public Animator anim;
        int hp;
        int attack01Hash = Animator.StringToHash("Attack01");
        int attack02Hash = Animator.StringToHash("Attack02");
        int hitHash = Animator.StringToHash("GetHit");
        public Text countText;
//        public Text winText;
        AudioSource adsrc;
        public AudioClip whack;
        public AudioClip hit;
        private bool isDead = false;

        // Use this for initialization
        void Start () {
            anim.GetComponent<Animator>();
            hp = 10;
//            winText.text = "";
            SetTexts ();
            adsrc = GetComponent<AudioSource> ();
        }
	
        // Update is called once per frame
        void FixedUpdate () {
            if (!isLocalPlayer)
            {
                return;
            }
            if (!isDead) {
                if (Input.GetKeyDown (KeyCode.Q)) {
                    anim.SetTrigger (Random.Range (0, 2) == 0 ? attack01Hash : attack02Hash);
                    adsrc.PlayOneShot (whack, 1f);
                }
            }
        }

        void OnTriggerEnter(Collider other) {
            if (!isLocalPlayer)
            {
                return;
            }
            if (other.gameObject.CompareTag ("LifePotion")) {
                other.gameObject.SetActive (false);
                IncreaseHp (1);
            } 
            else if (other.gameObject.CompareTag ("Weapon")) {
                DecreaseHp (1);
                anim.SetTrigger (hitHash);
                adsrc.PlayOneShot (hit, 1f);
            } 
            SetTexts ();
        }

        void OnParticleCollision(GameObject other){
            if (!isLocalPlayer)
            {
                return;
            }
            if (other.gameObject.CompareTag ("FireSpell")) {
                DecreaseHp (3);
                anim.SetTrigger (hitHash);
            }
            SetTexts ();
        }

        void SetTexts() {
//            countText.text = "Player" + id + " HP: " + hp;
//            if (hp <= 0) {
//                winText.text = "Player" + (id == 0 ? 1 : 0) + " WINS!";
//                isDead = true;
//            }
        }

        void DecreaseHp(int count){
            if (hp - count >= 0) {
                hp -= count;
                anim.SetInteger ("HP", hp);
            }
        }

        void IncreaseHp(int count){
            if (hp > 0) {
                hp += count;
                anim.SetInteger ("HP", hp);
            }
        }
    }
}
