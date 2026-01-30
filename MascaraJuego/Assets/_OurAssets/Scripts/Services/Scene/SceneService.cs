using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService
{
    private string currentSceneName;

    public Action<string> OnSceneLoadCall;
    public Action<string> OnSceneLoadComplete;
    public Action<TransitionParameters> OnTransitionRequested;
    
    bool changingScene = false;
    public async Task ChangeScene(string sceneName, Action openOperationCompleted = null, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if(changingScene) return;
        Debug.Log($"Changing scene to {sceneName}");
        currentSceneName = sceneName;
        
        changingScene = true;
        bool success = await LoadSceneAsync(sceneName, openOperationCompleted, mode);
        if(!success)
        {
            Debug.LogError($"Scene not found: {sceneName}");
        }
        
        changingScene = false;
    }
    async Task<bool> LoadSceneAsync(string sceneName, Action openOperationCompleted = null, LoadSceneMode mode = LoadSceneMode.Single)
    {
        try
        {
            OnSceneLoadCall?.Invoke(sceneName);
            
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName, mode);
            if (loadSceneAsync != null)
            {
                loadSceneAsync.completed += (operation) =>
                {
                    OnSceneLoadComplete?.Invoke(sceneName);
                };
                await loadSceneAsync;
                
                openOperationCompleted?.Invoke();
                return true;
            }
            openOperationCompleted?.Invoke();
            return false;
            
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            openOperationCompleted?.Invoke();
            return false;
        }
    }
}