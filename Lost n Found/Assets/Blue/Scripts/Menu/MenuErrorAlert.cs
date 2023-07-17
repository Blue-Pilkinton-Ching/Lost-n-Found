using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;

public class MenuErrorAlert : MonoBehaviour
{
    NetworkHelper networkConnectionData;
    public TextMeshProUGUI ErrorText;
    public GameObject ErrorObject;

    [Inject]
    private void Construct(NetworkHelper networkConnectionData){
        this.networkConnectionData = networkConnectionData;
    }

    private void Awake() {
        networkConnectionData.OnConnectionError += OnConnectionError;
    }

    private void OnConnectionError(System.Exception ex){
        ErrorText.text = ex.Message;

        ErrorObject.gameObject.SetActive(true);
    }
}
