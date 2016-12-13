using UnityEngine;
using UnityEngine.EventSystems;

public class DebugClicker : MonoBehaviour, IPointerClickHandler
{
	int _count = 0;
	const string PREFIX = "Click me - ";

    public void OnPointerClick(PointerEventData eventData)
    {
        _count++;
    }

	void OnGUI()
	{
		var screenpos = Camera.main.WorldToScreenPoint(transform.position);
		screenpos.y = Screen.height - screenpos.y;
		GUI.TextField(new Rect (screenpos.x - 45, screenpos.y - 10, 90, 20), PREFIX + _count);
	}
}
