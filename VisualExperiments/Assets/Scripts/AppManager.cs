using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AppManager : MonoBehaviour
{
    [SerializeField]
    InputActionReference quitAction;
    [SerializeField]
    InputActionReference fullscreenAction;
    // Start is called before the first frame update

    void Start()
    {
        quitAction.action.performed += Quit;
        fullscreenAction.action.performed += Fullscreen;
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
