using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts {
    public class SendChosenHeroUpdate : MonoBehaviour {
 
        private Dropdown _dropdown;
        private Dictionary<int, GameObject> _models;
        private GameObject _footmanModel;
        private GameObject _lichModel;
        private GameObject _gruntModel;
        private GameObject _golemModel;

        void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            //Add listener for when the value of the Dropdown changes, to take action
            _dropdown.onValueChanged.AddListener(delegate {
                DropdownValueChanged(_dropdown);
            });
            _footmanModel = GameObject.Find("FootmanModel");
            _lichModel = GameObject.Find("LichModel");
            _gruntModel = GameObject.Find("GruntModel");
            _golemModel = GameObject.Find("GolemModel");

            _models = new Dictionary<int, GameObject>();
            _models.Add(0, _footmanModel);
            _models.Add(1, _lichModel);
            _models.Add(2, _gruntModel);
            _models.Add(3, _golemModel);

            foreach (var keyValuePair in _models) {
                keyValuePair.Value.SetActive(false);
            }
            _models[0].SetActive(true);
        }

        //Ouput the new value of the Dropdown into Text
        void DropdownValueChanged(Dropdown change)
        {
            PickHero(change.value);
            foreach (var keyValuePair in _models) {
                keyValuePair.Value.SetActive(false);
            }
            _models[change.value].SetActive(true);
        }

        public void PickHero(int heroIndex)
        {
            NetworkManager.singleton.GetComponent<MyNetworkManager>().LocalChosenCharacter = heroIndex;
        }
    }
}
