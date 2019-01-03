using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts {
    public class OnHitByCycloneScript : NetworkBehaviour {

        void OnParticleCollision(GameObject other){
            var rb = other.GetComponent<Rigidbody> ();
            if(rb){
                rb.AddTorque(0, 20, 0, ForceMode.Impulse);
            }
        }
    }
}
