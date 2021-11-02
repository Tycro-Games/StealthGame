using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bogadanul.Assets.Scripts.Utility
{
    public class SceneChange : MonoBehaviour
    {
        private List<string> levels = new List<string>();

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadSceneSi(string name)
        {
            if (levels.Count == 0)
                GetSceneNames();
            if (name.Length > 0 && levels.Contains(name))
                SceneManager.LoadScene(name);
        }

        private void GetSceneNames()
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string name = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

                levels.Add(name);
            }
        }

        public void LoadSceneAd(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }

        public void UnloadScene(string name)
        {
            SceneManager.UnloadSceneAsync(name);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}