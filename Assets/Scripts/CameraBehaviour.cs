using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts {
    public class CameraBehaviour : NetworkBehaviour {
        private Camera _camera;

        void Awake() {
            _camera = GetComponentInChildren<Camera>();
            _camera.enabled = false;
        }

        public override void OnStartLocalPlayer()
        {
            _camera.enabled = true;
        }
    }
}
