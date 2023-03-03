using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class VirtualPosition : InputProcessor<float>
{
#if UNITY_EDITOR
    static VirtualPosition()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<VirtualPosition>();
    }

    public float gain = 1;

    float currentValue;

    public override float Process(float value, InputControl control)
    {
        currentValue = Mathf.Clamp(currentValue + Time.deltaTime * value * gain, 0, 1);

        return currentValue;
    }
}
