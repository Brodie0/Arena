using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

namespace Assets.Scripts {
    public class Move : NetworkBehaviour {
        private Animator _anim;
        private NavMeshAgent _navMeshAgent;
        private Camera _camera;
        private Terrain _terrain;
        private Vector3 _target;
        private const float RotationSpeed = 2.5f;

        private Vector3 _previousPosition;
        private float _curSpeed;

        void Start() {
            _anim = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _camera = GetComponentInChildren<Camera>();
            _terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
            _navMeshAgent.Warp(transform.position);
        }
        
        void Update() {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Input.GetMouseButtonDown(1)) {
                RaycastHit hit;
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                var terrainCollider = _terrain.GetComponent<Collider>();
                if (terrainCollider.Raycast(ray, out hit, Mathf.Infinity)) {
                    _target = hit.point;
                    _navMeshAgent.destination = _target;
                }
            }
            CalculateSpeed();
            _anim.SetFloat ("Speed", _curSpeed);
            Rotate(_navMeshAgent.steeringTarget);
        }

        private void CalculateSpeed() {
            var curMove = transform.position - _previousPosition;
            _curSpeed = curMove.magnitude / Time.deltaTime;
            _previousPosition = transform.position;
        }

        private void Rotate(Vector3 dest){
            var targetDir = dest - transform.position;

            // The step size is equal to speed times frame time.
            float step = RotationSpeed * Time.deltaTime;

            var newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);
            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }
}
