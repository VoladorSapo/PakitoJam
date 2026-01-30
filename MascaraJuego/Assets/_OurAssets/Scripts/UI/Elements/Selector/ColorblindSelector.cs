using System;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorblindSelector : UISelector
{
    [Inject] GameSettings settings;

    private void OnEnable()
    {
        int storedColorblindMode = settings.CurrentColorblindMode;
        ChangeValue(storedColorblindMode, false);
    }

    public void SetLUT()
    {
        settings.SetColorblindMode(CurrentIndex);
    }
}