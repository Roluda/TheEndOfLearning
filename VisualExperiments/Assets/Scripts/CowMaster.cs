using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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
    SpriteRenderer cowHead;
    [SerializeField]
    SpriteRenderer cowTorso;
    [SerializeField]
    BPMMeasure bpmMeasure;
    [SerializeField]
    Vector3 headbangTranslation;
    [SerializeField]
    Vector3 torsoBangTranslation;
    [SerializeField]
    PlayerInput input;
    [SerializeField]
    string headBangToggle = "VFXToggle5";

    private Texture2D textureCache;
    public uint samples => (uint)capture.numBars;
    Color[] colorCache;
    bool cowHeadbang = true;
    Vector3 basePositionHead;
    Vector3 basePositionTorso;

    Vector3 targetPositionHead;
    Vector3 targetPositionTorso;
    float distance => headbangTranslation.magnitude;
    float timerHead;
    float timerTorso;

    private void Start()
    {
        basePositionHead = cowHead.transform.position;
        basePositionTorso = cowHead.transform.position;
        input.actions[headBangToggle].performed += (context) => ActivateCowHeadbang(!cowHeadbang);
    }

    public void ActivateCowHeadbang(bool value)
    {
        cowHeadbang = value;
    }

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

    void UpdateRenderer(SpriteRenderer renderer, Vector3 basePosition, Vector3 translation, ref Vector3 targetPosition, ref float timer)
    {
        int bpm = bpmMeasure.bpm;
        float loopDuration = 60f / bpm;
        timer += Time.deltaTime;
        if (timer > loopDuration && cowHeadbang)
        {
            targetPosition = basePosition + translation + (Vector3)Random.insideUnitCircle * 0.1f;

            timer = 0;
        }
        float baseDistance = Vector3.Distance(renderer.transform.position, basePosition);
        if (timer > loopDuration / 2 && baseDistance > 0.01f)
        {
            renderer.transform.Translate((basePosition - renderer.transform.position).normalized * (distance / loopDuration * 2) * Time.deltaTime);
        }
        float targetDistance = Vector3.Distance(renderer.transform.position, targetPosition);
        if (timer < loopDuration / 2 && targetDistance > 0.01f)
        {
            renderer.transform.Translate((targetPosition - renderer.transform.position).normalized * (distance / loopDuration * 2) * Time.deltaTime);
        }
    }

    private void Update()
    {

        UpdateRenderer(cowHead, basePositionHead, headbangTranslation, ref targetPositionHead, ref timerHead);
        UpdateRenderer(cowTorso, basePositionTorso, torsoBangTranslation, ref targetPositionTorso, ref timerTorso);
    }

    public void SetBloomIntensity(float value)
    {
        if (volume.profile.TryGet<Bloom>(out var bloom))
        {
            bloom.intensity.value = value;
        }
    }

    private void UpdateTexture()
    {
        if (textureCache == null || textureCache.width != samples)
        {
            textureCache = new Texture2D((int)samples, 1, TextureFormat.RFloat, false);
            colorCache = new Color[samples];
        }

        for (int i = 0; i < samples; i++)
        {
            colorCache[i] = new Color(capture.barData[i], 0, 0, 0);
        }

        textureCache.SetPixels(colorCache);
        textureCache.name = "AudioData" + samples;
        textureCache.Apply();
    }

    Vector3 reference;
}
