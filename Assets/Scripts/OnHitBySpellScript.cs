using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts {
    public class OnHitBySpellScript : NetworkBehaviour {
        public AudioClip explosion;
        void OnParticleCollision(GameObject other) {
            Invoke("RemoveOnServer", 2);
            var src = GetComponent<AudioSource>();
            src.PlayOneShot(explosion, 1f);
        }

        private void RemoveOnServer() {
            NetworkServer.Destroy(gameObject);
        }
    }
}
