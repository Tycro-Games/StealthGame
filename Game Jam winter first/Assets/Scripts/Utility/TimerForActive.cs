using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Utility
{
    public class TimerForActive : MonoBehaviour
    {
        [SerializeField]
        private float TimeToWait = 0;

        [SerializeField]
        private bool loop = false;

        [SerializeField]
        private UnityEvent OnTimerFinished = null;

        private void OnEnable()
        {
            StartCoroutine(TimerLoop());
        }

        private void OnDisable()
        {
            StopCoroutine(TimerLoop());
        }

        private IEnumerator TimerLoop()
        {
            yield return new WaitForSeconds(TimeToWait);
            OnTimerFinished?.Invoke();

            if (gameObject.activeInHierarchy && loop)
                StartCoroutine(TimerLoop());
        }
    }
}