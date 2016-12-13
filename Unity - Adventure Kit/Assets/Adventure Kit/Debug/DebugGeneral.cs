using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class DebugGeneral : MonoBehaviour,
	IBeginDragHandler,
    ICancelHandler,
    IDeselectHandler,
    IDragHandler,
    IDropHandler,
    IEndDragHandler,
    IInitializePotentialDragHandler,
    IMoveHandler,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerUpHandler,
    IScrollHandler,
    ISelectHandler,
    ISubmitHandler,
    IUpdateSelectedHandler
{
	// Settings
	const int SHOWN_EVENTS_COUNT = 5;

	// Data to catch events
	List<string> _eventStack = new List<string>();
	bool _dragged = false;
	bool _moved = false;
	float _scoll = 0;
	bool _updatingSelected = false;

	void Update()
	{
		StartCoroutine(CleanUp());
	}

	IEnumerator CleanUp()
	{
		yield return new WaitForEndOfFrame();
		_dragged = false;
		_moved = false;
		_updatingSelected = false;
	}

	void OnGUI()
	{
		var screenpos = Camera.main.WorldToScreenPoint(transform.position);
		screenpos.y = Screen.height - screenpos.y;

		var sb = new StringBuilder();

		sb.Append("Dragged: "); sb.Append(_dragged); 			sb.AppendLine();
		sb.Append("Moved: "); 	sb.Append(_moved); 				sb.AppendLine();
		sb.Append("Upd Sel: "); sb.Append(_updatingSelected); 	sb.AppendLine();
		sb.Append("Scoll: "); 	sb.Append(_scoll); 				sb.AppendLine();
		
		sb.AppendLine("--- oldest ---");

		int lines = Mathf.Min(SHOWN_EVENTS_COUNT, _eventStack.Count);
		for (int i = 0; i < lines; ++i)
		{
			sb.AppendLine(_eventStack[_eventStack.Count - lines + i]);
		}

		sb.Append("--- NEWEST ---");

		var content = new GUIContent(sb.ToString());
		var style = new GUIStyle(GUI.skin.textField);
		style.alignment = TextAnchor.MiddleCenter;
		var size = style.CalcSize(content);
		size.x = 100; //fix width for nicer result
		GUI.Label(new Rect (screenpos.x - size.x/2, screenpos.y - size.y/2, size.x, size.y), content, style);
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        _eventStack.Add("Begin Drag");
    }

    public void OnCancel(BaseEventData eventData)
    {
        _eventStack.Add("Cancel");
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _eventStack.Add("Deselect");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _dragged = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        _eventStack.Add("Drop");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _eventStack.Add("End Drag");
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        _eventStack.Add("Init Pot Drag");
    }

    public void OnMove(AxisEventData eventData)
    {
		_moved = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _eventStack.Add("Pointer Click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _eventStack.Add("Pointer Down");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _eventStack.Add("Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _eventStack.Add("Pointer Exit");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _eventStack.Add("Pointer Up");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _eventStack.Add("Submit");
    }

    public void OnScroll(PointerEventData eventData)
    {
        _scoll += eventData.scrollDelta.y + eventData.scrollDelta.x;
    }

    public void OnSelect(BaseEventData eventData)
    {
        _eventStack.Add("Select");
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        _updatingSelected = true;
    }
}
