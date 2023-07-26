using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ToggleButtonScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image buttonImage;
    public Sprite normalSprite1;
    public Sprite hoverSprite1;
    public Sprite pressedSprite1;

    public Sprite normalSprite2;
    public Sprite hoverSprite2;
    public Sprite pressedSprite2;

    public UnityEvent OnClickEvent;

    private bool isState1 = true; // Initial state
    private bool isPointerOver = false;

    public void SetState(bool isState1) {
        this.isState1 = isState1;
        UpdateButtonImage(false);
    }

    private void Start() {
        UpdateButtonImage(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        isState1 = !isState1;
        UpdateButtonImage(false);
        OnClickEvent.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isPointerOver = true;
        UpdateButtonImage(false);
    }

    public void OnPointerExit(PointerEventData eventData) {
        isPointerOver = false;
        UpdateButtonImage(false);
    }

    public void OnPointerDown(PointerEventData eventData) {
        UpdateButtonImage(true);
    }

    public void OnPointerUp(PointerEventData eventData) {
        UpdateButtonImage(false);
    }

    private void UpdateButtonImage(bool isPressed) {

        if (isPointerOver) {
            if (isState1) {
                buttonImage.sprite = isPressed ? pressedSprite1 : hoverSprite1;
            } else {
                buttonImage.sprite = isPressed ? pressedSprite2 : hoverSprite2;
            }
        } else {
            if (isState1) {
                buttonImage.sprite = normalSprite1;
            } else {
                buttonImage.sprite = normalSprite2;
            }
        }
    }
}