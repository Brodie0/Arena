using UnityEngine;

namespace Assets.Scripts {
    public class OnHitBySpellScript : MonoBehaviour {
        public AudioSource _src;
        public AudioClip _explosion;
        void OnParticleCollision(GameObject other){
            _src.PlayOneShot(_explosion, 1f);
        }
    }
}
