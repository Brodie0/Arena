using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class PlayerScript : NetworkBehaviour {

        public bool IsDead;
        protected int Hp;
        protected int MaxHp;
        protected Animator Anim;
        protected Rigidbody Rb;
        protected AudioSource AudioSource;
        protected readonly int HitHash = Animator.StringToHash("GetHit");
        protected readonly int Attack01Hash = Animator.StringToHash("Attack01");
        protected readonly int Attack02Hash = Animator.StringToHash("Attack02");
        protected readonly int DieHash = Animator.StringToHash("Death");
        protected AudioClip Hit;
        private Text _health;

        protected void Start () {
            var playerCollider = GetComponent<Collider>();
            var terrainCollider = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
            Physics.IgnoreCollision(playerCollider, terrainCollider);
            Anim = GetComponent<Animator>();
            AudioSource = GetComponent<AudioSource>();
            if (!isLocalPlayer)
            {
                return;
            }
            Rb = GetComponent<Rigidbody>();
            _health = GetComponentInChildren<Text>();
            Anim.SetInteger("HP", Hp);
            UpdateUI();
        }

        protected void FixedUpdate() {
            Anim.SetFloat ("Speed", Rb.velocity.magnitude);
        }

        protected void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag ("LifePotion")) {
                other.gameObject.SetActive (false);
                IncreaseHp (1);
            } 
            else if (other.gameObject.CompareTag ("Weapon")) {
                DecreaseHp (1);
                Anim.SetTrigger (HitHash);
                AudioSource.PlayOneShot (Hit, 1f);
            } 
        }

        void OnParticleCollision(GameObject other){
            if (other.gameObject.CompareTag ("FireSpell")) {
                DecreaseHp (2);
                Anim.SetTrigger (HitHash);
            }
        }

        protected void DecreaseHp(int count){
            if (!isLocalPlayer)
            {
                return;
            }
            if (Hp - count >= 0) {
                Hp -= count;
                Anim.SetInteger("HP", Hp);
            }
            else {
                IsDead = true;
                Anim.SetTrigger (DieHash);
            }
            UpdateUI();
        }

        protected void IncreaseHp(int count){
            if (!isLocalPlayer)
            {
                return;
            }
            if (Hp > 0) {
                Hp += count;
                Anim.SetInteger("HP", Hp);
            }
            UpdateUI();
        }

        protected void UpdateUI() {
            _health.text = "Health: " + Hp;
        }
    }
}
