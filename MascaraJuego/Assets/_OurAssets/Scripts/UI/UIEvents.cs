using System;

public class UIEvents
{
    public event Action<UIButton> OnButtonSelected;
    public event Action<UIButton> OnButtonUnselected;
        
    public void SelectButton(UIButton interactiveButton)
    {
        OnButtonSelected?.Invoke(interactiveButton);
    }
    public void UnselectButton(UIButton interactiveButton)
    {
        OnButtonUnselected?.Invoke(interactiveButton);
    }
        
    public event Action<int> OnButtonGroupSelected;
    public event Action<int> OnButtonGroupUnselected;
    public void SelectButtonGroup(int group)
    {
        OnButtonGroupSelected?.Invoke(group);
    }
    public void UnselectButtonGroup(int group)
    {
        OnButtonGroupUnselected?.Invoke(group);
    }
        
    public void UnselectAllButtons()
    {
        OnButtonSelected?.Invoke(null);
    }

    public event Action<int> OnButtonGroupEnabled;
    public event Action<int> OnButtonGroupDisabled;

    public void EnableButtonGroup(int group)
    {
        OnButtonGroupEnabled?.Invoke(group);
    }
    public void DisableButtonGroup(int group)
    {
        OnButtonGroupDisabled?.Invoke(group);
    }

    public event Action OnOptionsChanged;
    public void NotifyOptionsChanged() => OnOptionsChanged?.Invoke();
}
