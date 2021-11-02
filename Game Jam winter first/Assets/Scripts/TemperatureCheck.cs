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
    [SerializeField]
    private Color blue; 
    [SerializeField]
    private Color red;

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
            if (PlayerEnt.InStrom)
                thermo.color = blue;
            else
                thermo.color = red;
            thermo.fillAmount = playerTemp.TemperatureTo01();
            yield return null;
        }
    }
    void Stop()
    {
        stop = true;
    }
}
