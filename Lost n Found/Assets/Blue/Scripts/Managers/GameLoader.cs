using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    int synchronizedClientCallsCount = 0;
    private void Awake() {
        DependencyHolder.Singleton.NetworkManager.OnServerStarted += OnServerStarted;
    }
    private void OnServerStarted() 
    {
        NetworkManager.Singleton.SceneManager.OnSynchronize += OnSynchronizeComplete;
    }
    public void StartGame() 
    {
        DOTween.KillAll();
        NetworkManager.Singleton.SceneManager.LoadScene(DependencyHolder.Singleton.SharedKeys.OrphangeSceneName, LoadSceneMode.Single);
    }
    private void OnSynchronizeComplete(ulong id)  
    {
        synchronizedClientCallsCount++;

        if (synchronizedClientCallsCount == 3) 
        {
            Debug.Log("Start Game");
        }
    }
}
