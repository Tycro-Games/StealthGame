using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitch : MonoBehaviour
{
    private AudioSource Audio = null;

    [SerializeField]
    private float minS = 1;

    [SerializeField]
    private float maxS = 1;

    private void OnEnable()
    {
        Audio = GetComponent<AudioSource>();
        ChangePitch();
    }

    public void ChangePitch()
    {
        Audio.pitch = Random.Range(minS, maxS);
    }
}