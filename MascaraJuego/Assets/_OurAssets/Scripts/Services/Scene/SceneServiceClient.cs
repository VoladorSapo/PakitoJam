using System;
using Reflex.Attributes;
using Reflex.Extensions;
using UltEvents;
using UnityEngine;

public class SceneServiceClient : MonoBehaviour
{
    [SerializeField] TransitionParameters closingParameters;
    [SerializeField] TransitionParameters openingParameters;
    
    [Inject] SceneService sceneService;
    [Inject] SingletonLocator singletonLocator;

    private void Awake()
    {
        openingParameters.ClosingDirection = false;
    }

    public void ResetScene()
    {
        ChangeScene(gameObject.scene.name);
    }
    public void ChangeScene(string sceneName)
    {
        var transitionController = singletonLocator.TransitionController;
        closingParameters.OnCompleteAction = () =>
        {
            _ = sceneService.ChangeScene(sceneName, 
                () => transitionController.DoTransition(openingParameters));
        };
        
        transitionController.DoTransition(closingParameters);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) ResetScene();
        
        if(Input.GetKeyDown(KeyCode.R)) ChangeScene("RaulTest2");
    }
}
