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
    [Inject] private SingletonLocator singletonLocator;
    [Inject] private MusicService musicService;
    
    [SerializeField, ReadOnly] ManuCanvasState currentMenuState;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject creditsMenu;

    private void Awake()
    {
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
    
    public void PlayGame()
    {
        musicService.StopMusic(0, 1);
        GetComponent<SceneServiceClient>().ChangeScene("RaulTest");
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
