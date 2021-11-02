using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utility
{
    public class LoadScenes : MonoBehaviour
    {
        [SerializeField]
        private float transTime = 1f;

        public event Action OnSceneLoad;

        public void LoadScene(string name)
        {
            StartCoroutine(LoadLevel(0));
        }

        public void NextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        private IEnumerator LoadLevel(int levelIndex)
        {
            OnSceneLoad?.Invoke();
            yield return new WaitForSecondsRealtime(transTime);

            SceneManager.LoadScene(levelIndex);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}