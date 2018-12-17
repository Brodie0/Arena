using UnityEngine;

namespace Assets.Scripts {
    public class OnHitBySpellScript : MonoBehaviour {
        private AudioSource _src;
        private AudioClip _explosion;
        void OnParticleCollision(GameObject other){
            _src.PlayOneShot(_explosion, 1f);
        }
    }
}
