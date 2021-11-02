using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class TimeController : MonoBehaviour
    {
        public static event Action<float> OnTimeChange;

        public void ChangeTime(float number)
        {
            float time = Time.timeScale;
            time += number;
            if (time >= 0)
            {
                Time.timeScale = time;
                OnTimeChange?.Invoke(time);
            }
        }

        public static void SetTime(float number)
        {
            Time.timeScale = number;
            OnTimeChange?.Invoke(Time.timeScale);
        }

        private void Start()
        {
            OnTimeChange?.Invoke(Time.timeScale);
        }
    }
}