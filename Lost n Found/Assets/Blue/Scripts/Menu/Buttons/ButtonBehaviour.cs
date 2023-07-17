using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(Button))]
public abstract class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    // Any button class that inherits from this, can set 'shouldFreezeButtons' inside the Awake method, to freeze the buttons until ButtonsFrozen is set to false


    public static bool ButtonsFrozen {get; protected set;} = false;

    protected bool shouldFreezeButtons = false;

    Button button;
    ButtonSettingsInstaller buttonSettings;
    protected virtual void Awake(){
        button = GetComponent<Button>();
    }

    [Inject]
    private void Construct(ButtonSettingsInstaller buttonSettings){
        this.buttonSettings = buttonSettings;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!ButtonsFrozen)
        {
            button.image.DOColor(buttonSettings.HoverColor, buttonSettings.FadeDuration);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!ButtonsFrozen)
        {
            button.image.DOColor(buttonSettings.ClickColor, buttonSettings.FadeDuration).OnComplete(() =>
                button.image.DOColor(buttonSettings.NormalColor, buttonSettings.FadeDuration));

            if (shouldFreezeButtons)
            {
                ButtonsFrozen = true;
            }

            Invoke("OnClick", buttonSettings.ClickDelay);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!ButtonsFrozen)
        {
            button.image.DOColor(buttonSettings.NormalColor, buttonSettings.FadeDuration);
        }
    }
    
    protected abstract void OnClick();
}
