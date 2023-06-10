using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControlAxis : MonoBehaviour
{
    [SerializeField]
    public float minValue;
    [SerializeField]
    public float maxValue;
    [SerializeField]
    public float initialValue;
    [SerializeField]
    public string inputAxis;
    [SerializeField]
    InputActionReference controlAxis;

    float m_currentValue;
    float currentValue
    {
        get => m_currentValue;
        set
        {
            value = Mathf.Clamp(value, minValue, maxValue);
            onValueChanged?.Invoke(value);
        }
    }

    public UnityEvent<float> onValueChanged;

    public void Update()
    {
        float input = controlAxis.action.ReadValue<float>();
        if (!Hold.isHolding)
        {
            currentValue = Mathf.Lerp(minValue, maxValue, input);
        }
    }
}
