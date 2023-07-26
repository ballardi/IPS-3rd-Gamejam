using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Assertions;

public class ButtonHoverEventExposer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textToChange;
    public Color normalColor;
    public Color hoverColor;

    private void Awake() {
        Assert.IsNotNull(textToChange);
    }

    private void Start() {
        GameStateManager.instance.OnTitleScreenStartEvent.AddListener(ResetToNormalColor);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        textToChange.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        textToChange.color = normalColor;
    }

    public void ResetToNormalColor() {
        textToChange.color = normalColor;
    }

}
