using TMPro;
using UltEvents;
using UnityEngine;

[System.Serializable]
public class UISelectionElement
{
    public string Name;
    [SerializeField] UltEvent OnSelected;
    
    public void InvokeSelection() => OnSelected?.Invoke();
}
public class UISelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selectionText;
    [SerializeField] private UISelectionElement[] elements;
    protected int CurrentIndex;

    public void MoveToNextValue(bool notify = true)
    {
        if(!HasValidSelection()) return;
        
        int newIndex = (CurrentIndex + 1) % elements.Length;
        ChangeValue(newIndex, notify);
    }
    public void MoveToPreviousValue(bool notify = true)
    {
        if(!HasValidSelection()) return;
        
        int newIndex = (CurrentIndex - 1 + elements.Length) % elements.Length;
        ChangeValue(newIndex, notify);
    }
    public void ChangeValue(int index, bool notify = true)
    {
        if(!HasValidSelection() || !IsIndexValid(index)) return;
        
        CurrentIndex = index;
        selectionText.text = elements[CurrentIndex].Name;
        
        if(notify)
            elements[CurrentIndex].InvokeSelection();
    }

    public UISelectionElement GetCurrentSelection()
    {
        return HasValidSelection() ? elements[CurrentIndex] : null;
    }
    
    bool HasValidSelection() => elements != null && elements.Length > 0;
    bool IsIndexValid(int index) => index >= 0 && index < elements.Length;
}