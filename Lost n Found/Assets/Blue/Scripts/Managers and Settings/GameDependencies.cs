using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameDependencies : MonoBehaviour
{
    // Class that holds all Managers and Dependancies for easy accessibility
    // This class should ONLY hold dependencys for other classes, and not contain any functionality

    public static GameDependencies Singleton;

    public void Awake()
    {
        Singleton = this;
    }
}