using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Capture Spectrum Binder")]
[VFXBinder("Audio/Capture Spectrum to AttributeMap")]
public class SpectrumCaptureBinder : VFXBinderBase {

    public string countProperty { get { return (string)m_CountProperty; } set { m_CountProperty = value; } }
    [VFXPropertyBinding("System.UInt32"), SerializeField]
    protected ExposedProperty m_CountProperty = "Count";

    public string audioOutputProperty { get { return (string)m_audioOutputProperty; } set { m_audioOutputProperty = value; } }
    [VFXPropertyBinding("UnityEngine.Texture2D"), SerializeField]
    protected ExposedProperty m_audioOutputProperty = "AudioTexture";

    public uint samples => (uint)soundCapture.numBars;
    public SoundCapture soundCapture = null;

    private Texture2D textureCache;
    private Color[] colorCache;

    public override bool IsValid(VisualEffect component) {
        bool reference = soundCapture != null;
        bool texture = component.HasTexture(audioOutputProperty);
        bool count = component.HasUInt(countProperty);

        return reference && texture && count;
    }

    public override void UpdateBinding(VisualEffect component) {
        UpdateTexture();
        component.SetTexture(audioOutputProperty, textureCache);
        component.SetUInt(countProperty, samples);
    }

    private void UpdateTexture() {
        if (textureCache == null || textureCache.width != samples) {
            textureCache = new Texture2D((int)samples, 1, TextureFormat.RFloat, false);
            colorCache = new Color[samples];
        }

        for (int i = 0; i < samples; i++) {
            colorCache[i] = new Color(soundCapture.barData[i], 0, 0, 0);
        }

        textureCache.SetPixels(colorCache);
        textureCache.name = "AudioData" + samples;
        textureCache.Apply();
    }
}
