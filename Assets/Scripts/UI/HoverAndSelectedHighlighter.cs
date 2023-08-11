using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverAndSelectedHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isSelected;
    private bool isHovered;
    private bool isEnabled;

    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnselectedColor;
    [SerializeField] private Color HoverColor;
    [SerializeField] private Color PressedColor;
    [SerializeField] private Color DisabledColor;

    [SerializeField] private Image imageToChange;

    void Awake()
    {
        isSelected = false;
        isHovered = false;
        isEnabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        UpdateColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        UpdateColor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        imageToChange.color = PressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateColor();
    }

    public void OnSelected(Toggle _change)
    {
        isSelected = _change.isOn;

        UpdateColor();
    }

    public void SetEnabled(bool _enabled = true)
    {
        isEnabled = _enabled;

        UpdateColor();
    }

    private void UpdateColor()
    {
        imageToChange.color = isEnabled ? (isHovered ? HoverColor : (isSelected ? SelectedColor : UnselectedColor)) : DisabledColor;
    }
}
