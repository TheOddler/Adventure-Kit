using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Text))]
public class DebugUIClicker : UIBehaviour, IPointerClickHandler
{
    Text _text;
	int _count = 0;
	const string PREFIX = "Click me - ";

    override protected void Awake()
    {
        _text = GetComponent<Text>();
        _text.text = PREFIX + "...";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _count++;
        _text.text = PREFIX + _count;
    }
}
