using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TemperatureCheck : MonoBehaviour
{
    PlayerEnt playerTemp;
    [SerializeField]
    Image thermo = null;
    bool stop = false;
    private void OnEnable()
    {
        PlayerEnt.onDead += Stop;
    }
    private void OnDisable()
    {
        PlayerEnt.onDead -= Stop;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerTemp = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEnt>();
        StartCoroutine(UpdateTemp());
    }

    // Update is called once per frame
    IEnumerator UpdateTemp()
    {
        while (!stop)
        {
            thermo.fillAmount = playerTemp.TemperatureTo01();
            yield return null;
        }
    }
    void Stop()
    {
        stop = true;
    }
}
