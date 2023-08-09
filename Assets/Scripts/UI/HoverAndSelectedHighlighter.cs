using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverAndSelectedHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isSelected;
    private bool isHovered;

    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnselectedColor;
    [SerializeField] private Color HoverColor;
    [SerializeField] private Color PressedColor;

    [SerializeField] private Image imageToChange;

    void Awake()
    {
        isSelected = false;
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

    private void UpdateColor()
    {
        imageToChange.color = isHovered ? HoverColor : (isSelected ? SelectedColor : UnselectedColor);
    }
}
