using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

[AddComponentMenu("VFX/Property Binders/Torus")]
[VFXBinder("Input/Torus")]
public class TorusVFX : VFXBinderBase
{
    public string AxisProperty { get { return (string)m_AxisProperty; } set { m_AxisProperty = value; } }

    [VFXPropertyBinding("UnityEngine.Vector2"), SerializeField]
    protected ExposedProperty m_AxisProperty = "Axis";


    [SerializeField]
    float gain;
    [SerializeField]
    float minTorusRadius;
    [SerializeField]
    float exponent;

    public InputActionReference axisBinding;

    Vector2 storedValue;

    public override bool IsValid(VisualEffect component)
    {
        return component.HasVector2(m_AxisProperty);
    }

    public override void UpdateBinding(VisualEffect component)
    {
        var valueRaw = axisBinding.action.ReadValue<Vector2>();
        var add = valueRaw * gain * Time.deltaTime;
        storedValue = Vector2.ClampMagnitude(storedValue + add, 1);
        var calulatedValue = CalculateTorusValue(storedValue);
        if (!Hold.isHolding)
        {
            component.SetVector2(m_AxisProperty, calulatedValue);
        }
    }

    public override string ToString()
    {
        return string.Format("Input Axis: '{0}' -> {1}", m_AxisProperty, axisBinding.name.ToString());
    }

    Vector2 CalculateTorusValue(Vector2 accumulatedInput)
    {
        if (accumulatedInput.x == 0 || accumulatedInput.y == 0)
        {
            accumulatedInput += Vector2.one / 1000f;
        }
        float signX = Mathf.Sign(accumulatedInput.x);
        float signY = Mathf.Sign(accumulatedInput.y);
        float x = minTorusRadius / Mathf.Pow(Mathf.Abs(accumulatedInput.x), exponent);
        float y = minTorusRadius / Mathf.Pow(Mathf.Abs(accumulatedInput.y), exponent);
        var vector = new Vector2(signX * x, signY * y);

        return Vector2.ClampMagnitude(vector, 100000);
    }
}
