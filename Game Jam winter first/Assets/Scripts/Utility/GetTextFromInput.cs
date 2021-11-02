using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Bogadanul
{
    [System.Serializable]
    public class UnityEventString : UnityEvent<string>
    {
    }

    [System.Serializable]
    public class GetTextFromInput : MonoBehaviour
    {
        private TMP_InputField input = null;

        [SerializeField]
        private UnityEventString unityEvent = null;

        private void Start()
        {
            input = GetComponent<TMP_InputField>();
        }

        public void GoToScene()
        {
            unityEvent?.Invoke(input.text);
        }
    }
}