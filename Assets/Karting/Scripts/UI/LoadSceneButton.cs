using UnityEngine;
using UnityEngine.SceneManagement;

namespace KartGame.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        [Tooltip("What is the name of the scene we want to load when clicking the button?")]
        public string SceneName;

        public int Fuzzy;
        public int Defuzz;
        public int RuleSet;

        public void LoadTargetScene() 
        {
            PlayerPrefs.SetInt("Fuzzy", Fuzzy);
            PlayerPrefs.SetInt("Defuzz", Defuzz);
            PlayerPrefs.SetInt("Rule", RuleSet);
            SceneManager.LoadSceneAsync(SceneName);
        }
    }
}
