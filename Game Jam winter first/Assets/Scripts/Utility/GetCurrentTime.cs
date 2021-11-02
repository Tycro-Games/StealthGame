using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Utility
{
    public class GetCurrentTime : MonoBehaviour
    {
        private TextMeshProUGUI text = null;

        public void ChangeText(float val)
        {
            text.text = val.ToString();
        }

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
            TimeController.OnTimeChange += ChangeText;
        }

        private void OnDisable()
        {
            TimeController.OnTimeChange -= ChangeText;
        }
    }
}