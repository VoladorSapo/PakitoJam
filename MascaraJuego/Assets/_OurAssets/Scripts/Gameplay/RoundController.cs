using System;
using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Timeline;

public class RoundController : MonoBehaviour
{
    [Inject] UIEvents uiEvents;
    [Inject] GameEvents gameEvents;
    [Inject] PauseService pauseService;
    [Inject] MusicService musicService;

    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] private bool debugStartCutscene;
    [SerializeField] TimelineAsset startCutscene;
    
    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] private float endDelay = 1;
    [SerializeField] private bool debugEndCutscene;
    [SerializeField] TimelineAsset endCutscene;
    
    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] SceneServiceClient sceneServiceClient;
    [SerializeField, Scene] private string CreditsScene;
    [SerializeField, Scene] private string MenuScene;
    
    bool roundStarted;
    
    void Start()
    {
        
        if (startCutscene != null || debugStartCutscene)
        {
            PlayOpeningCutscene();
        }
        gameEvents.OnRoundStarted += StartRound;
        gameEvents.OnRoundEnded += CheckEnd;
    }

    private void OnDestroy()
    {
        gameEvents.OnRoundStarted -= StartRound;
        gameEvents.OnRoundEnded -= CheckEnd;
    }

    void AwakeRound()
    {
        gameEvents.NotifyRoundAwakened();
    }
    void StartRound()
    {
        roundStarted = true;
        musicService.PlayMusic("Batalla", 0);
    }

    public void Update()
    {
        if(!roundStarted) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseService.Pause(PauseLevel.UI);
        }
    }

    void CheckEnd(bool hasWon)
    {
        if(hasWon) WinRound();
        else LoseRound();
    }
    public void WinRound()
    {
        if(!roundStarted) return;
        roundStarted = false;
        
        gameEvents.NotifyRoundEnd(true);

        if (endCutscene != null || debugEndCutscene)
        {
            PlayEndingCutscene();
        }
            
    }
    public void LoseRound()
    {
        if(!roundStarted) return;
        roundStarted = false;
        
        gameEvents.NotifyRoundEnd(false);
        this.InvokeDelayed(endDelay, () => gameEvents.NotifyResultScreenCalled());
    }

    void PlayOpeningCutscene()
    {
        pauseService.Pause(PauseLevel.Dialog);
            
        Debug.Log("startCutscene.name");
        this.InvokeDelayed(2, AwakeRound);
        pauseService.Resume(PauseLevel.Dialog);
    }

    void PlayEndingCutscene()
    {
        pauseService.Pause(PauseLevel.Dialog);
            
        Debug.Log("endCutscene.name");
        this.InvokeDelayed(2 + endDelay, () => gameEvents.NotifyResultScreenCalled());
        pauseService.Resume(PauseLevel.Dialog);
    }

    public void LeaveGame()
    {
        uiEvents.DisableButtonGroup(1);
        MoveToMenu();
        this.InvokeDelayed(0.75f, () => pauseService.Resume(PauseLevel.UI));
    }
    public void MoveToMenu()
    {
        sceneServiceClient.ChangeScene(MenuScene);
        musicService.StopMusic(0, 1);
    }
    public void MoveToCredits()
    {
        sceneServiceClient.ChangeScene(CreditsScene);
        musicService.StopMusic(0, 1);
    }
}
