using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public PlayerInput playerInput;

    bool toogle;

    public override bool IsValid(VisualEffect component)
    {
        return component.HasBool(m_ButtonProperty) && playerInput;
    }

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetBool(m_ButtonProperty, toogle);
    }

    private void OnValidate()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    public override string ToString()
    {
        return string.Format("Input Button: '{0}' -> {1}", m_ButtonProperty, ButtonName.ToString());
    }

    private void Start()
    {
        playerInput.actions[ButtonName].performed += PressButton;
    }

    private void PressButton(InputAction.CallbackContext obj)
    {
        toogle = !toogle;
    }
}
