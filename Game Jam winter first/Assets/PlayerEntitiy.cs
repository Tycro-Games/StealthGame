using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntitiy : MonoBehaviour
{
    public const float minTemp = -30;
    public const float maxTemp = 2;
    [Range(minTemp, maxTemp)]
    public float Temperature = 2;
    public float TimeBetweenTemperatureChecks = .5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckTemperature());

    }
    private void OnEnable()
    {
        Guard.OnGuardHasSpottedPlayer += Die;
    }
    private void OnDisable()
    {
        Guard.OnGuardHasSpottedPlayer -= Die;
    }
    IEnumerator CheckTemperature()
    {
        while (Temperature > minTemp)
        {
            yield return new WaitForSeconds(TimeBetweenTemperatureChecks);
            yield return null;
        }

    }
    void Die()
    {
        Debug.Log("Player dies"); //animation coroutine
        Application.Quit();
    }

}
