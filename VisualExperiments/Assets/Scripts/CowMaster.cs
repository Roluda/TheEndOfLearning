using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class CowMaster : MonoBehaviour
{
    [SerializeField]
    Volume volume;
    [SerializeField]
    SoundCapture capture;
    [SerializeField]
    GameObject cow;
    [SerializeField]
    BPMMeasure bpmMeasure;


    public void SetMaxFrequency(float value)
    {
        capture.maxFreq = (int)value;
    }

    public void SetBloomScatter(float value)
    {
        if(volume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.scatter.value = value;
        }
    }

    public void SetBloomIntensity(float value)
    {
        if (volume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.intensity.value = value;
        }
    }

    Vector3 reference;
}
