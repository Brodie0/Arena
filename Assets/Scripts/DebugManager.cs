using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class DebugManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private VisualElement _root;

        public GameObject[] SpawnObjects;

        void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            var container = _root.Q<VisualElement>("container");

            foreach (var spawnObject in SpawnObjects)
            {
                var button = new Button(() => Spawn(spawnObject));
                button.text = spawnObject.name;
                container.Add(button);
            }

            _root.style.display = DisplayStyle.None;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12))
            {
                _root.style.display = _root.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public void Spawn(GameObject spawnObject)
        {
            var playerPos = GameObject.FindObjectOfType<PlayerScript>().gameObject.transform;
            Vector3 pos = playerPos.position + playerPos.forward * 3f;
            Instantiate(spawnObject, pos, Quaternion.identity);
        }
    }
}
