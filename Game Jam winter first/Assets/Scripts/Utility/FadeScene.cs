using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class FadeScene : MonoBehaviour
    {
        [SerializeField]
        private Animator anim = null;

        private LoadScenes load = null;

        private void OnEnable()
        {
            load = GetComponent<LoadScenes>();
            load.OnSceneLoad += OnSceneLoad;
        }

        private void OnDisable()
        {
            load.OnSceneLoad -= OnSceneLoad;
        }

        public void OnSceneLoad()
        {
            anim.SetTrigger("Fade");
        }
    }
}