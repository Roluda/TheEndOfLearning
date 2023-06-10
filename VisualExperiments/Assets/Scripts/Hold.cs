using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Hold : MonoBehaviour
{
    public static bool isHolding;
    [SerializeField]
    InputActionReference holdInput;

    private void Awake()
    {
        holdInput.action.performed += Toggle;
    }

    void Toggle(CallbackContext context)
    {
        isHolding = !isHolding;
    }
}
