using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FullscreenToggleController : MonoBehaviour
{
    public Toggle fullscreenToggle;

    void Start()
    {
        // Baslangicta gecerli ekran modunu toggle'a yansit
        fullscreenToggle.isOn = Screen.fullScreen;

        // Toggle degistiginde FullscreenToggleChanged metodunu cagir
        fullscreenToggle.onValueChanged.AddListener(FullscreenToggleChanged);
    }

    public void FullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}