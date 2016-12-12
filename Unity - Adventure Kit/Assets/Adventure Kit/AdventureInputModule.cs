using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class AdventureInputModule : BaseInputModule
{
	//
	// Settings
	// ---
	[System.Serializable]
	class AutoDeselectSettings
	{
		public bool enabled = true; // Should an element be automatically deselected when looking away?
		public bool onlyWhenNotClicking = true;
		public bool evenWhenHoveringOverSameObject = false;
	}

	[SerializeField]
	AutoDeselectSettings _autoDeselect = new AutoDeselectSettings();

	//
	// Caches
	// ---
	PointerEventData _pointerData;

	//
	// Code
	// ---
    public override void Process()
    {
		// Update data to use the mouse
		UpdatePointerDataForMouse();

		// Trigger raycast
        eventSystem.RaycastAll(_pointerData, m_RaycastResultCache); //also clears cache
		_pointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

		// Some temp variables
		var currentOverGO = _pointerData.pointerCurrentRaycast.gameObject;

		// Handle enter and exit events on the GUI controlls that are hit
		base.HandlePointerExitAndEnter(_pointerData, currentOverGO);

		if (Input.GetMouseButtonDown(0))
		{
			ClearSelection();

			// Some bookkeeping
			_pointerData.pressPosition = _pointerData.position;
			_pointerData.pointerPressRaycast = _pointerData.pointerCurrentRaycast;
			_pointerData.pointerPress = null;
			
			if (currentOverGO != null)
			{
				GameObject newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGO, _pointerData, ExecuteEvents.pointerDownHandler);

				if (newPressed == null) // Some UI elements might only have click handler and not pointer down handler
				{
					newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGO);
				}
				
				if (newPressed != null)
				{
					_pointerData.pointerPress = newPressed;
					Select(newPressed);
				}

				// TODO Smarter dragging? Rather than just always dragging
				ExecuteEvents.Execute(_pointerData.pointerPress, _pointerData, ExecuteEvents.initializePotentialDrag);
                ExecuteEvents.Execute(_pointerData.pointerPress, _pointerData, ExecuteEvents.beginDragHandler);
                _pointerData.pointerDrag = _pointerData.pointerPress;
			}
		}

		if (Input.GetMouseButtonUp(0))
		{
			if (_pointerData.pointerDrag != null)
			{
                ExecuteEvents.Execute(_pointerData.pointerDrag, _pointerData, ExecuteEvents.endDragHandler);
				
				if(currentOverGO != null)
				{
					ExecuteEvents.ExecuteHierarchy(currentOverGO, _pointerData, ExecuteEvents.dropHandler);
				}
				
                _pointerData.pointerDrag = null;
			}

			ExecuteEvents.ExecuteHierarchy(currentOverGO, _pointerData, ExecuteEvents.pointerUpHandler);
			
			// check if the pointer is over the same object as press for click event
            GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGO);
			if (_pointerData.pointerPress != null && _pointerData.pointerPress == pointerUpHandler)
			{
				ExecuteEvents.Execute(pointerUpHandler, _pointerData, ExecuteEvents.pointerClickHandler);
			}

			_pointerData.rawPointerPress = null;
			_pointerData.pointerPress = null;
		}

		if(_pointerData.pointerDrag != null)
		{
        	ExecuteEvents.Execute(_pointerData.pointerDrag, _pointerData, ExecuteEvents.dragHandler);
		}

		if (_autoDeselect.enabled)
		{
			if (_pointerData.pointerPress == null) // We're NOT clicking
			{
				if (_autoDeselect.evenWhenHoveringOverSameObject)
				{
					ClearSelection();
				}
				else if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGO) != base.eventSystem.currentSelectedGameObject)
				{
					ClearSelection();
				}
			}
			else // We ARE clicking
			{
				if (_autoDeselect.onlyWhenNotClicking)
				{
					// do nothing because we ARE clicking
				}
				else
				{
					if (_autoDeselect.evenWhenHoveringOverSameObject)
					{
						ClearSelection();
					}
					else if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGO) != base.eventSystem.currentSelectedGameObject)
					{
						ClearSelection();
					}
				}
			}
		}
		/*if (_autoDeselect.enabled && ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGO) != base.eventSystem.currentSelectedGameObject)
		{
			if (!_autoDeselect.onlyWhenNotClicking)
			{
				ClearSelection();
			}
			else if(_pointerData.pointerPress == null)
			{
				ClearSelection();
			}
		}*/

		if(base.eventSystem.currentSelectedGameObject != null)
		{
        	ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(), ExecuteEvents.updateSelectedHandler);
		}

		// TODO IScrollHandler
    }

	void UpdatePointerDataForMouse()
	{
		if (_pointerData == null) _pointerData = new PointerEventData(eventSystem);
		else _pointerData.Reset();

		_pointerData.delta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //so it works when mouse is locked
		_pointerData.position = Input.mousePosition;
		_pointerData.scrollDelta = Input.mouseScrollDelta;
	}

	// clear the current selection
	public void ClearSelection()
	{
		if(base.eventSystem.currentSelectedGameObject)
		{
			base.eventSystem.SetSelectedGameObject(null);
		}
	}

	// select a game object
	private void Select(GameObject go)
	{
		ClearSelection();

		//if(ExecuteEvents.GetEventHandler<ISelectHandler>(go))
		//{
			base.eventSystem.SetSelectedGameObject(go);
		//}
	}


	void OnGUI()
	{
		GUILayout.Label("-- Debug --");
		GUILayout.Label("Raycast count: " + m_RaycastResultCache.Count);
		GUILayout.Label("Current raycast: " + _pointerData.pointerCurrentRaycast.gameObject);
		GUILayout.Label("Current selected: " + _pointerData.selectedObject);
	}
}
