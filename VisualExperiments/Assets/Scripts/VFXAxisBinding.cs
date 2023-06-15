using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Input Axis Binder Ranged")]
[VFXBinder("Input/Axis Range")]
class VFXAxisBinding : VFXBinderBase
{
    public string AxisProperty { get { return (string)m_AxisProperty; } set { m_AxisProperty = value; } }

    [VFXPropertyBinding("System.Single"), SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_AxisParameter")]
    protected ExposedProperty m_AxisProperty = "Axis";

    public string AxisName = "Horizontal";
    public float minValue;
    public float maxValue;

    public bool accumulate = false;
    public InputActionReference axisBinding;
    float accumulatedValue = 0.5f;
    public float gain = 1;

    public override bool IsValid(VisualEffect component)
    {
        return component.HasFloat(m_AxisProperty);
    }

    public override void UpdateBinding(VisualEffect component)
    {
        float valueRaw = axisBinding.action.ReadValue<float>();
        if (accumulate)
        {
            accumulatedValue += valueRaw * Time.deltaTime * gain;
            accumulatedValue = Mathf.Clamp(accumulatedValue, 0, 1f);
            valueRaw = accumulatedValue;
        }
        if (!Hold.isHolding)
        {
            component.SetFloat(m_AxisProperty, Mathf.Lerp(minValue, maxValue, valueRaw));
        }
    }

    public override string ToString()
    {
        return string.Format("Input Axis: '{0}' -> {1}", m_AxisProperty, AxisName.ToString());
    }
}
