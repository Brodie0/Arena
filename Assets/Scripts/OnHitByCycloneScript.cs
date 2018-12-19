using UnityEngine;

namespace Assets.Scripts {
    public class OnHitByCycloneScript : MonoBehaviour {

        void OnParticleCollision(GameObject other){
            Rigidbody rb = other.GetComponent<Rigidbody> ();
            if(rb){
                rb.AddForce (0f, 10f, 0f, ForceMode.Impulse);
            }
        }
    }
}
