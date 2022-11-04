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
}
