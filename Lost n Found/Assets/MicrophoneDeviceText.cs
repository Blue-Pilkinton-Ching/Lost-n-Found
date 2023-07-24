using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class MicrophoneDeviceText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        DependencyHolder.Singleton.AudioDeviceManager.OnCurrentMicChanged += CurrentMicChanged;
    }

    private void CurrentMicChanged() 
    {
        text.text = DependencyHolder.Singleton.AudioDeviceManager.CurrentMicName;
    }
}
