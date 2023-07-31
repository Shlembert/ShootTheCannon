using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MenuController menuController;
    [SerializeField] private ButtonType buttonType;
    [SerializeField] private float distance = 0.75f;
    [SerializeField] private Transform button, shadowButton;
    [SerializeField] private Image image;
    [SerializeField] private Color highlightColor, pressColor, relaxColor;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip highlightSound, clickSound;

    private bool _upButton = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = pressColor;
        
        AnimationButton(-1f);
        RaycastEnable(false);
        if (buttonType == ButtonType.bgButton) return;
        audioSource.PlayOneShot(clickSound);
    }

    public async void OnPointerUp(PointerEventData eventData)
    {
        if (_upButton) return;
        _upButton = true;
        AnimationButton(1f);
        await UniTask.Delay(200);
        image.color = relaxColor;
        menuController.CheckButton(buttonType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = highlightColor;
        if (buttonType == ButtonType.bgButton) return;
        audioSource.PlayOneShot(highlightSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!image.raycastTarget|| buttonType == ButtonType.bgButton) return;
        image.color = relaxColor;
    }

    private void AnimationButton(float direction)
    {
        transform.DOMoveY(transform.position.y + distance * direction, 0.1f, false);
        transform.DOMoveX(transform.position.x - distance * direction, 0.1f, false);
        shadowButton.DOMoveX(shadowButton.position.x + distance * direction, 0.1f, false);
    }

    public void RaycastEnable(bool value)
    {
        image.raycastTarget = value;
    }

    public void UpButtonFalse()
    {
        _upButton = false;
    }
}

