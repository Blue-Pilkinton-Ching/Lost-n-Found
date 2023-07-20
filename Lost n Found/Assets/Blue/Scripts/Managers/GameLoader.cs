using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void StartGame() {
        NetworkManager.Singleton.SceneManager.LoadScene(DependencyHolder.Singleton.SharedKeys.OrphangeSceneName, LoadSceneMode.Single);
    }
}
