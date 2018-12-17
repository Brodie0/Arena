using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts {
    public class PausePanelNewGameButton : MonoBehaviour {
        private GameObject _pausePanel;
        private GameObject _optionsTint;

        public void Start () {
            _pausePanel.SetActive (false);
            _optionsTint.SetActive(false);
            SceneManager.LoadScene ("MainGame");
        }
    }
}
