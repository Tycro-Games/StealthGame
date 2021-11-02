using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Utility
{
    public class DisplayScene : MonoBehaviour
    {
        private TextMeshProUGUI text = null;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            text.text = SceneManager.GetActiveScene().name;
        }
    }
}