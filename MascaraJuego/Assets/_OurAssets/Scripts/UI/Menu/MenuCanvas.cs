using System;
using NaughtyAttributes;
using Reflex.Attributes;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    public enum ManuCanvasState
    {
        None,
        MainMenu,
        OptionsMenu,
        CreditsMenu,
    }
    [Inject] private GameSettings gameSettings;
    [Inject] private SingletonLocator singletonLocator;
    [Inject] private MusicService musicService;
    
    [SerializeField, ReadOnly] ManuCanvasState currentMenuState;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject creditsMenu;

    private void Awake()
    {
        musicService.PlayMusic("main menu", 0);
        ChangeMenuState(ManuCanvasState.MainMenu);
    }

    public void ChangeMenuState(ManuCanvasState newState)
    {
        if(currentMenuState == newState) return;
        
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        
        currentMenuState = newState;
        switch (newState)
        {
            case ManuCanvasState.MainMenu: mainMenu.SetActive(true); break;
            case ManuCanvasState.OptionsMenu: optionsMenu.SetActive(true); break;
            case ManuCanvasState.CreditsMenu: creditsMenu.SetActive(true); break;
        }
    }
    
    public void PlayNormalGame()
    {
        gameSettings.InfiniteMode = false;
        musicService.StopMusic(0, 1);
        GetComponent<SceneServiceClient>().ChangeScene("RingScene");
    }

    public void PlayInfiniteGame()
    {
        gameSettings.InfiniteMode = true;
        musicService.StopMusic(0, 1);
        GetComponent<SceneServiceClient>().ChangeScene("RingScene");
    }

    public void SaveConfiguration()
    {
        singletonLocator.SaveClient?.Save();
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
