using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class LichScript : MonoBehaviour {

        public Animator anim;
        public Text countText;
        public Text winText;
        public int id;
        public AudioClip whack;
        public AudioClip hit;
        public AudioClip fireball;
        public AudioClip cycloneaudio;
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
            if (!isDead) {
                float moveHorizontal = Input.GetAxis ("Horizontal" + id);
                float moveVertical = Input.GetAxis ("Vertical" + id);
                Vector3 movement = new Vector3 (moveHorizontal, 0f, moveVertical);
                rb.AddForce (movement, ForceMode.Impulse);
                Rotate (movement);
                if (rb.velocity.magnitude > maxSpeed)
                    rb.velocity = rb.velocity.normalized * maxSpeed;

                anim.SetFloat ("Speed", rb.velocity.magnitude);

                if (Input.GetKeyDown (id == 0 ? KeyCode.T : KeyCode.Backslash)) {
                    anim.SetTrigger (Random.Range (0, 2) == 0 ? attack01Hash : attack02Hash);
                    adsrc.PlayOneShot (whack, 1f);
                }
                else if (Input.GetKeyDown (KeyCode.U) && id == 1) {
                    StartCoroutine (CastSpellDelayed (fireBall, fireball, 1f));
                }
                else if (Input.GetKeyDown (KeyCode.I) && id == 1) {
                    StartCoroutine (CastSpellDelayed (cyclone, cycloneaudio, 1f));
                }
            }
        }

        void Rotate(Vector3 dest){
            float step = 10f * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, dest, step, 0f);
            //TODO nie uzywaj transforma jesli masz rigidbody!!!
            //TODO przy kolzji graczy wyglada jakby to te meshcollidery z shadow kolidowały a nie kapsuły :/
            rb.MoveRotation(Quaternion.LookRotation(newDir));
        }

        void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag ("PickUp")) {
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

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag ("Ball")) {
                DecreaseHp (2);
                anim.SetTrigger (hitHash);
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
            if (hp >= 0 + count) {
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

        IEnumerator CastSpellDelayed(Transform spell, AudioClip audio, float timeToWait){
            anim.SetTrigger (attack02Hash);
            yield return new WaitForSeconds(timeToWait);
            adsrc.PlayOneShot (audio, 1f);
            Instantiate (spell, new Vector3 (rb.position.x, rb.position.y + 0.5f, rb.position.z) + rb.rotation * new Vector3 (0f, 0f, 3f), rb.rotation);
        }
    }
}
