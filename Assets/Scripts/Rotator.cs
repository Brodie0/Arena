using UnityEngine;

namespace Assets.Scripts {
    public class Rotator : MonoBehaviour {
	
        // Update is called once per frame
        void Update () {
            transform.Rotate(new Vector3(14, 60, 14) * Time.deltaTime);
        }
    }
}
