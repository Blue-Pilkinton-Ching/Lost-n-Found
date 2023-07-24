using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void StartGame() {

        DOTween.KillAll();
        NetworkManager.Singleton.SceneManager.LoadScene(DependencyHolder.Singleton.SharedKeys.OrphangeSceneName, LoadSceneMode.Single);
    }
}
