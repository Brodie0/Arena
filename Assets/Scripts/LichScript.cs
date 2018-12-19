using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class LichScript : MonoBehaviour {

        public Animator anim;
        public Text countText;
        public Text winText;
        public AudioClip whackAudio;
        public AudioClip hitAudio;
        public AudioClip fireballAudio;
        public AudioClip cycloneAudio;
        public Transform fireBall;
        public Transform cyclone;

        int hp;
        int attack01Hash = Animator.StringToHash("Attack01");
        int attack02Hash = Animator.StringToHash("Attack02");
        int hitHash = Animator.StringToHash("GetHit");
        Rigidbody rb;
        float maxSpeed = 100f;//Replace with your max speed
        AudioSource adsrc;
        private bool isDead = false;

        // Use this for initialization
        void Start () {
            anim.GetComponent<Animator>();
            hp = 10;
//		winText.text = "";
            SetTexts ();
            rb = GetComponent<Rigidbody> ();
            adsrc = GetComponent<AudioSource> ();
        }

        // Update is called once per frame
        void FixedUpdate () {
//            if (!isDead) {
//                float moveHorizontal = Input.GetAxis ("Horizontal");
//                float moveVertical = Input.GetAxis ("Vertical");
//                Vector3 movement = new Vector3 (moveHorizontal, 0f, moveVertical);
//                rb.AddForce (movement, ForceMode.Impulse);
//                Rotate (movement);
//                if (rb.velocity.magnitude > maxSpeed)
//                    rb.velocity = rb.velocity.normalized * maxSpeed;
//
//                anim.SetFloat ("Speed", rb.velocity.magnitude);
//
//                if (Input.GetKeyDown (KeyCode.Q)) {
//                    StartCoroutine (CastSpellDelayed (fireBall, fireballAudio, 1f));
//                }
//                else if (Input.GetKeyDown (KeyCode.W)) {
//                    StartCoroutine (CastSpellDelayed (cyclone, cycloneAudio, 1f));
//                }
//            }
        }

        void Rotate(Vector3 dest){
            float step = 10f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, dest, step, 0f);
            //TODO nie uzywaj transforma jesli masz rigidbody!!!
            //TODO przy kolzji graczy wyglada jakby to te meshcollidery z shadow kolidowały a nie kapsuły :/
            rb.MoveRotation(Quaternion.LookRotation(newDir));
        }

        void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag ("LifePotion")) {
                other.gameObject.SetActive (false);
                IncreaseHp (1);
            } 
            else if (other.gameObject.CompareTag ("Weapon")) {
                DecreaseHp (1);
                anim.SetTrigger (hitHash);
                adsrc.PlayOneShot (hitAudio, 1f);
            } 
            SetTexts ();
        }

        void SetTexts() {
//		countText.text = "Player" + id + " HP: " + hp;
//		if (hp <= 0) {
//			winText.text = "Player" + (id == 0 ? 1 : 0) + " WINS!";
//			isDead = true;
//		}
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

        private IEnumerator CastSpellDelayed(Transform spell, AudioClip audioClip, float timeToWait){
            anim.SetTrigger (attack02Hash);
            yield return new WaitForSeconds(timeToWait);
            adsrc.PlayOneShot (audioClip, 1f);
            Instantiate (spell, new Vector3 (rb.position.x, rb.position.y + 0.5f, rb.position.z) + rb.rotation * new Vector3 (0f, 0f, 3f), rb.rotation);
        }
    }
}
