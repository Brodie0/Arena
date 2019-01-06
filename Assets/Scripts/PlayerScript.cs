using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class PlayerScript : NetworkBehaviour {

        public bool IsDead;
        protected int Hp;
        protected int MaxHp;
        protected Animator Anim;
        protected NetworkAnimator NetAnim;
        protected Rigidbody Rb;
        protected AudioSource AudioSource;
        protected readonly int GetHitHash = Animator.StringToHash("GetHit");
        protected readonly int Attack01Hash = Animator.StringToHash("Attack01");
        protected readonly int Attack02Hash = Animator.StringToHash("Attack02");
        protected readonly int IsDeadHash = Animator.StringToHash("IsDead");
        protected AudioClip Hit;
        private Text _health;

        protected void Start () {
            var playerCollider = GetComponent<Collider>();
            var terrainCollider = GameObject.Find("Terrain").GetComponent<TerrainCollider>();
            Physics.IgnoreCollision(playerCollider, terrainCollider);
            Anim = GetComponent<Animator>();
            NetAnim = GetComponent<NetworkAnimator>();
            AudioSource = GetComponent<AudioSource>();
            if (!isLocalPlayer)
            {
                return;
            }
            Rb = GetComponent<Rigidbody>();
            _health = GetComponentInChildren<Text>();
            UpdateUI();
        }

        protected void FixedUpdate() {
            if (!isLocalPlayer)
            {
                return;
            }
            Anim.SetFloat ("Speed", Rb.velocity.magnitude);
        }

        protected void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag ("LifePotion")) {
                other.gameObject.SetActive (false);
                IncreaseHp (1);
            } 
            else if (other.gameObject.CompareTag ("Weapon")) {
                DecreaseHp (1);
                NetAnim.SetTrigger(GetHitHash);
                AudioSource.PlayOneShot (Hit, 1f);
            } 
        }

        void OnParticleCollision(GameObject other){
            if (other.gameObject.CompareTag ("FireSpell")) {
                DecreaseHp (2);
                //necessary for second animation layer synchronization (Torso), i dont know why first layer works without this tho...
                NetAnim.SetTrigger(GetHitHash);
            }
        }

        protected void DecreaseHp(int count){
            if (!isLocalPlayer)
            {
                return;
            }
            if (Hp - count > 0) {
                Hp -= count;
            }
            else {
                Hp = 0;
                IsDead = true;
                NetAnim.SetTrigger (IsDeadHash);
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
            }
            UpdateUI();
        }

        protected void UpdateUI() {
            _health.text = "Health: " + Hp;
        }
    }
}
