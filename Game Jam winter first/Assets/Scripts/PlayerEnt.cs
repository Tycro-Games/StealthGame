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
    public static bool Imobile = false;
    public static bool InStrom = true;
    // Start is called before the first frame update
    void Start()
    {
        playerShooting = GetComponent<PlayerShooting>();
        playerController = GetComponent<PlayerController>();
        character = GetComponent<ThirdPersonCharacter>();
        StartCoroutine(CheckTemperature());
    }


    IEnumerator CheckTemperature()
    {

        while (Temperature > minTemp)
        {
            if (InStrom)
                Temperature -= RateDrop;
            else
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
        Imobile = true;
        playerController.StopDestination();
        playerShooting.StopAllCoroutines();
    }
    public void Die()
    {
        Deactivate();
        Debug.Log("Player dies"); //animation coroutine
        if (onDead != null)
            onDead();


    }


}
