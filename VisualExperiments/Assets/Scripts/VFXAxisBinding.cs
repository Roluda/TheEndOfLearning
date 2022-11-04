using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float AccumulateSpeed = 1.0f;
    public bool Accumulate = true;
    public float minValue;
    public float maxValue;

    public override bool IsValid(VisualEffect component)
    {
        return component.HasFloat(m_AxisProperty);
    }

    public override void UpdateBinding(VisualEffect component)
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        float axis = Input.GetAxisRaw(AxisName);

        if (Accumulate)
        {
            float value = component.GetFloat(m_AxisProperty);
            component.SetFloat(m_AxisProperty, Mathf.Clamp(value + (AccumulateSpeed * axis * Time.deltaTime), minValue, maxValue));
        }
        else
            component.SetFloat(m_AxisProperty, axis);
#endif
    }

    public override string ToString()
    {
        return string.Format("Input Axis: '{0}' -> {1}", m_AxisProperty, AxisName.ToString());
    }
}
