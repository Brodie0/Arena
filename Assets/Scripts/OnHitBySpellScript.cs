using UnityEngine;

namespace Assets.Scripts {
    public class OnHitBySpellScript : MonoBehaviour {
        public AudioClip explosion;
        void OnParticleCollision(GameObject other){
            var src = GetComponent<AudioSource>();
            src.PlayOneShot(explosion, 1f);
        }
    }
}
