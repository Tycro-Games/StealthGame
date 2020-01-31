using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerEnt : MonoBehaviour
{
    [SerializeField]
    const float minTemp = -30;
    [SerializeField]
    const float maxTemp = 2;
    [Range(minTemp, maxTemp)]
    [SerializeField]
    private float Temperature = 2;
    [SerializeField]
    private float TimeBetweenTemperatureChecks = .1f;
    [SerializeField]
    private float RateDrop = 0.2f;
    [SerializeField]
    private float RateRise = 0.2f;
    PlayerController playerController;
    PlayerShooting playerShooting;
    ShelterCheck shelter;
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
    private void Update()
    {

    }
    IEnumerator CheckTemperature()
    {

        while (Temperature > minTemp)
        {
            if (InStrom)
            {
                Temperature -= RateDrop;
            }
            else
            {
                if (Temperature < maxTemp)
                    Temperature += RateRise;
            }
            yield return new WaitForSeconds(TimeBetweenTemperatureChecks);

            yield return null;
        }
        Debug.Log("Player freezes");
        Die();


    }
    public void Deactivate()
    {
        playerShooting.enabled = false;
        playerController.StopDestination();
    }
    void Die()
    {
        Deactivate();
        Debug.Log("Player dies"); //animation coroutine
        SceneManager.LoadScene(0);
    }


}
