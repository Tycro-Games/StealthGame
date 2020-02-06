using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;
public class PlayerEnt : MonoBehaviour, ILiving
{

    [SerializeField]
    const float minTemp = 0;
    [SerializeField]
    const float maxTemp = 100;
    [Range(minTemp, maxTemp)]
    [SerializeField]
    private float Temperature = 0;
    [SerializeField]
    private float TimeBetweenTemperatureChecks = .1f;
    [SerializeField]
    private float RateDrop = 5.0f;
    [SerializeField]
    private float RateRise = 10.0f;
    PlayerController playerController;
    PlayerShooting playerShooting;
    ThirdPersonCharacter character;
    public delegate void OnDead();
    public static event OnDead onDead;

    ShelterCheck shelter;

    public static bool Dead = false;
    public static bool InStrom = true;
    // Start is called before the first frame update
    void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        playerController = GetComponent<PlayerController>();
        character = GetComponent<ThirdPersonCharacter>();
        shelter = GetComponent<ShelterCheck>();
        StartCoroutine(CheckTemperature());
    }


    IEnumerator CheckTemperature()
    {

        while (Temperature > minTemp)
        {
            if (InStrom)
                Temperature -= RateDrop;
            else if (Temperature < maxTemp)
            {
                Temperature += RateRise;
            }
            yield return new WaitForSeconds(TimeBetweenTemperatureChecks);

            yield return null;
        }
        Debug.Log("Player freezes");
        character.Die();


    }
    public float TemperatureTo01()
    {
        return Temperature / 100;
    }
    public void Deactivate()
    {
        Dead = true;
        playerController.StopDestination();
        playerController.enabled = false;
        playerShooting.enabled = false;

    }
    public void Die()
    {
        StopCoroutine(CheckTemperature());
        if (!Dead)
            Deactivate();
        Debug.Log("Player dies"); //animation coroutine
        if (onDead != null)
        {
            onDead();

        }


    }


}
