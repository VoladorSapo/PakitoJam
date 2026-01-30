using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class PauseServiceClient : MonoBehaviour
{
    [Inject] PauseService pauseService;
    
    [SerializeField] MonoBehaviour pausable;
    public IPausable Pausable => pausable as IPausable;
    
    [HorizontalLine(color: EColor.White)]
    [ReadOnly, SerializeField] bool isPaused;
    [SerializeField] private PauseLevel minimalPauseReason;
    
    void Awake()
    {
        pauseService.OnPauseCalled += OnPause;
        pauseService.OnResumeCalled += OnResume;
    }

    void OnPause(PauseLevel reasonToPause)
    {
        if(isPaused || minimalPauseReason == PauseLevel.None) return;
        if(reasonToPause >= minimalPauseReason)
        {
            isPaused = true;
            Pausable?.Pause();
        }
    }

    void OnResume(PauseLevel reasonToResume)
    {
        if(!isPaused || minimalPauseReason == PauseLevel.None) return;
        if (reasonToResume == minimalPauseReason)
        {
            isPaused = false;
            Pausable?.Resume();
        }
    }

    /*void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)) pauseService.Pause(PauseLevel.Dialog);
        if(Input.GetKeyDown(KeyCode.X)) pauseService.Pause(PauseLevel.UI);
        
        if(Input.GetKeyDown(KeyCode.C)) pauseService.Resume(PauseLevel.Dialog);
        if(Input.GetKeyDown(KeyCode.V)) pauseService.Resume(PauseLevel.UI);
    }*/
}