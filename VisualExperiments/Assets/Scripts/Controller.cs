using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Controller : MonoBehaviour
{
    [SerializeField]
    VisualEffect visualEffect;
    [SerializeField]
    SoundCapture capture;
    [SerializeField]
    VisualEffect stars;

    [SerializeField]
    float colorChangeSpeed = 0.01f;
    [SerializeField]
    float lowPassChangeSpeed = 0.1f;

    [SerializeField]
    string highColorRotateAxis;
    [SerializeField]
    string lowColorRotateAxis;
    [SerializeField]
    string startStopCapture;
    [SerializeField]
    string lowPassAxis;
    [SerializeField]
    string frequencyDouble;
    [SerializeField]
    string frequencyHalf;
    [SerializeField]
    string toggleStars;

    bool starsOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        capture.gameObject.SetActive(!Input.GetButton(startStopCapture));

        float low = Input.GetAxis(lowColorRotateAxis);
        float high = Input.GetAxis(highColorRotateAxis);
        float lowPass = Input.GetAxis(lowPassAxis);

        if (Input.GetButtonDown(frequencyDouble))
        {
            capture.maxFreq = capture.maxFreq * 2;
        }
        if (Input.GetButtonDown(frequencyHalf))
        {
            capture.maxFreq = capture.maxFreq / 2;
        }

        if (Input.GetButtonDown(toggleStars))
        {
            stars.gameObject.SetActive(!stars.gameObject.activeInHierarchy);
        }

        if (low != 0)
        {
            Color.RGBToHSV(visualEffect.GetVector4("LowColor"), out float h, out float s, out float v);
            h = (h + Time.deltaTime * colorChangeSpeed * low) % 1;
            visualEffect.SetVector4("LowColor", Color.HSVToRGB(h, s, v));
        }

        if (high != 0)
        {
            Color.RGBToHSV(visualEffect.GetVector4("HighColor"), out float h, out float s, out float v);
            h = (h + Time.deltaTime * colorChangeSpeed * high) % 1;
            visualEffect.SetVector4("HighColor", Color.HSVToRGB(h, s, v));
        }

        if (lowPass != 0)
        {
            float current = visualEffect.GetFloat("ColorStep");
            current = Mathf.Clamp01(current + Time.deltaTime * lowPass * lowPassChangeSpeed);
            visualEffect.SetFloat("ColorStep", current);
        }


        Color highColor = visualEffect.GetVector4("HighColor");

    }
}
