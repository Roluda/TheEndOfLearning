using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    PlayerInput playerInput;
    // Start is called before the first frame update

    private void OnValidate()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }
    void Start()
    {
        playerInput.actions["Quit"].performed += Quit;
        playerInput.actions["Fullscreen"].performed += Fullscreen;
    }

    private void Fullscreen(InputAction.CallbackContext obj)
    {
        if (Screen.fullScreenMode != FullScreenMode.Windowed)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            return;
        }
        if (Screen.fullScreenMode != FullScreenMode.MaximizedWindow)
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            return;
        }
    }

    private void Quit(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}
