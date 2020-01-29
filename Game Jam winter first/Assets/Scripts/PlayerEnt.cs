using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerEnt : MonoBehaviour
{
    public const float minTemp = -30;
    public const float maxTemp = 2;
    [Range(minTemp, maxTemp)]
    public float Temperature = 2;
    public float TimeBetweenTemperatureChecks = .5f;
    PlayerController playerController;
    PlayerShooting playerShooting;
    public static bool InStrom = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckTemperature());
        playerShooting = GetComponent<PlayerShooting>();
        playerController = GetComponent<PlayerController>();
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
    public void Deactivate()
    {
        playerShooting.enabled = false;
        playerController.enabled = false;
    }
    void Die()
    {
        Deactivate();
        Debug.Log("Player dies"); //animation coroutine
        SceneManager.LoadScene(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shelter")
        {
            InStrom = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shelter")
        {
            InStrom = true;
        }
    }

}
