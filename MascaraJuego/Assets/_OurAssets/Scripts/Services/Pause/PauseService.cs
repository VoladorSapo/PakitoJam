using System;
using System.Collections;
using System.Collections.Generic;

public enum PauseLevel
{
    None = -1,
    Dialog = 0,
    UI = 1,
}
public class PauseService
{
    public event Action<PauseLevel> OnPauseCalled;
    public event Action<PauseLevel> OnResumeCalled;

    Stack<PauseLevel> pauseStack = new Stack<PauseLevel>();

    public void Pause(PauseLevel reasonToPause)
    {
        if (pauseStack.Count == 0 || reasonToPause > pauseStack.Peek())
        {
            pauseStack.Push(reasonToPause);
            OnPauseCalled?.Invoke(reasonToPause);
        }
    }

    public void Resume(PauseLevel reasonToResume)
    {
        if (pauseStack.Count > 0 && pauseStack.Peek() == reasonToResume)
        {
            pauseStack.Pop();
            OnResumeCalled?.Invoke(reasonToResume);
        }
    }

    public bool IsPaused => pauseStack.Count > 0;
    public PauseLevel CurrentLevel => pauseStack.Count > 0 ? pauseStack.Peek() : PauseLevel.None;
}
