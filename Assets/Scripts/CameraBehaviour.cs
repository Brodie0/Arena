using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
