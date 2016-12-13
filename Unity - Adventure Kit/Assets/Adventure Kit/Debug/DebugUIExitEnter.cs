using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Text))]
public class DebugUIExitEnter : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Text _text;

    override protected void Awake()
    {
        _text = GetComponent<Text>();
        _text.text = "...";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.text = "Inside";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.text = "Outside";
    }
}
