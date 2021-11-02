using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class EndLevel : MonoBehaviour
    {
        public static event Action EndGame;

        public static event Action BeforeCoolDown;

        [SerializeField]
        private float time = 2.0f;

        private IEnumerator OnMouseDown()
        {
            BeforeCoolDown?.Invoke();
            yield return new WaitForSecondsRealtime(time);
            EndGame?.Invoke();
        }
    }
}