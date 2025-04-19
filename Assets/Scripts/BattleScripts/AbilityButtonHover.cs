using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AbilityButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI hoverText;
    public string hoverTextLabel;

    public void OnPointerEnter(PointerEventData eventData) => hoverText.text = hoverTextLabel;
    public void OnPointerExit(PointerEventData eventData) => hoverText.text = "";

    public void SetHoverText(string text) => hoverTextLabel = text;
}
