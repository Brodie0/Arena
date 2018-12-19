using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendChosenHeroUpdate : MonoBehaviour {
 
    private Dropdown _dropdown;
    private Dictionary<int, GameObject> _models;
    private GameObject _footmanModel;
    private GameObject _lichModel;

    void Start()
    {
        _dropdown = GetComponent<Dropdown>();
        //Add listener for when the value of the Dropdown changes, to take action
        _dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(_dropdown);
        });
        _footmanModel = GameObject.Find("FootmanModel");
        _lichModel = GameObject.Find("LichModel");

        _models = new Dictionary<int, GameObject>();
        _models.Add(0, _footmanModel);
        _models.Add(1, _lichModel);

        foreach (var keyValuePair in _models) {
            keyValuePair.Value.SetActive(false);
        }
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

    public void PickHero(int hero)
    {
        NetworkManager.singleton.GetComponent<MyNetworkManager>().ChosenCharacter = hero;
    }
}
