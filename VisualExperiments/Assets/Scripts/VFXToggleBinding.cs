using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Input Toggle Binder")]
[VFXBinder("Input/Toggle")]
class VFXToggleBinding : VFXBinderBase
{
    public string ButtonProperty { get { return (string)m_ButtonProperty; } set { m_ButtonProperty = value; } }

    [VFXPropertyBinding("System.Boolean"), SerializeField, UnityEngine.Serialization.FormerlySerializedAs("m_ButtonParameter")]
    protected ExposedProperty m_ButtonProperty = "ButtonDown";

    public string ButtonName = "Action";

#if ENABLE_LEGACY_INPUT_MANAGER
    float m_CachedSmoothValue = 0.0f;
#endif

    public override bool IsValid(VisualEffect component)
    {
        return component.HasBool(m_ButtonProperty);
    }

    public override void UpdateBinding(VisualEffect component)
    {
#if ENABLE_LEGACY_INPUT_MANAGER
        bool press = Input.GetButtonDown(ButtonName);
        if (press)
        {
            bool toggle = !component.GetBool(m_ButtonProperty);
            component.SetBool(m_ButtonProperty, toggle);
        }
#endif
    }

    public override string ToString()
    {
        return string.Format("Input Button: '{0}' -> {1}", m_ButtonProperty, ButtonName.ToString());
    }
}
