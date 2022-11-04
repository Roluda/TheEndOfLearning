using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlAxis : MonoBehaviour
{
    [SerializeField]
    public float minValue;
    [SerializeField]
    public float maxValue;
    [SerializeField]
    public float gain;
    [SerializeField]
    public float initialValue;
    [SerializeField]
    public string inputAxis;
    [SerializeField]
    public bool loop;

    float m_currentValue;
    float currentValue
    {
        get => m_currentValue;
        set
        {
            if (!loop)
            {
                value = Mathf.Clamp(value, minValue, maxValue);
            }
            else
            {
                value = Loop(value);
            }
            if(value != m_currentValue)
            {
                onValueChanged?.Invoke(value);
            }
            m_currentValue = value;
        }
    }

    public UnityEvent<float> onValueChanged;

    private void Start()
    {
        currentValue = initialValue;
    }

    public void Update()
    {
        float input = Input.GetAxis(inputAxis);
        currentValue += input * gain * Time.deltaTime;
    }

    float Loop(float value)
    {
        if(value < minValue)
        {
            float excess = value - minValue;
            return maxValue + excess;
        }
        if(value > maxValue)
        {
            float excess = value - maxValue;
            return minValue + excess;
        }
        return value;
    }
}
