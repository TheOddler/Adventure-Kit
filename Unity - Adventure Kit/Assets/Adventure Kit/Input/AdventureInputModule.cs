using UnityEngine;
using UnityEngine.EventSystems;

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

	override protected void Awake()
	{
		base.Awake();
		_pointerData = new PointerEventData(eventSystem);
	}

	public override void Process()
	{
		// Update pointer data;
		_pointerData.Reset();
		_pointerData.delta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //so it works when mouse is locked
		_pointerData.position = Input.mousePosition;
		_pointerData.scrollDelta = Input.mouseScrollDelta;

		// Raycast everything (via eventSystem)
		eventSystem.RaycastAll(_pointerData, m_RaycastResultCache); //fills cache with sorted raycasts, based on distance
		_pointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache); //first result with non-null gameobject

		// Some useful variables
		GameObject currentGO = _pointerData.pointerCurrentRaycast.gameObject;

		// Handle the exit/enter for the current pointer data
		HandlePointerExitAndEnter(_pointerData, currentGO);

		if (Input.GetMouseButtonDown(0))
		{
			UpdateSelection(null);

			// Bookkeeping for press
			_pointerData.pressPosition = _pointerData.position;
			_pointerData.pointerPressRaycast = _pointerData.pointerCurrentRaycast;
			_pointerData.rawPointerPress = currentGO;
			_pointerData.pointerPress = null; //default

			if (currentGO != null)
			{
				GameObject pointerDownGO = 
					ExecuteEvents.ExecuteHierarchy(currentGO, _pointerData, ExecuteEvents.pointerDownHandler)
					// There might be elements with only the on-click handler, no pointer-down
					// If so, treat that one as if it did get the pointer-down message
					?? ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGO);

				if (pointerDownGO != null)
				{
					_pointerData.pointerPress = pointerDownGO;
					UpdateSelection(pointerDownGO);
				}

				// Simple dragging
				GameObject pointerGO = pointerDownGO ?? currentGO;
				GameObject draggingGO = 
					ExecuteEvents.GetEventHandler<IDragHandler>(pointerGO)
					?? ExecuteEvents.GetEventHandler<IBeginDragHandler>(pointerGO)
					?? ExecuteEvents.GetEventHandler<IEndDragHandler>(pointerGO)
					?? ExecuteEvents.GetEventHandler<IInitializePotentialDragHandler>(pointerGO);
				
				ExecuteEvents.Execute(draggingGO, _pointerData, ExecuteEvents.initializePotentialDrag);
				ExecuteEvents.Execute(draggingGO, _pointerData, ExecuteEvents.beginDragHandler);
				_pointerData.pointerDrag = draggingGO;
			}	
		}

		if (Input.GetMouseButtonUp(0))
		{
			// We were dragging some object
			if (_pointerData.pointerDrag != null)
			{
				// Tell it we aren't anymore
				ExecuteEvents.Execute(_pointerData.pointerDrag, _pointerData, ExecuteEvents.endDragHandler);

				// Also, if we are now over some other object, tell it we dropped this object there
				if (currentGO != null)
				{
					ExecuteEvents.Execute(currentGO, _pointerData, ExecuteEvents.dropHandler);
				}

				_pointerData.pointerDrag = null;
			}

			// Tell current object we did pointer-up
			ExecuteEvents.ExecuteHierarchy(currentGO, _pointerData, ExecuteEvents.pointerUpHandler);

			// Also give a pointer-up to the object that got the pointer-down, but only if we haven't already in the previous line
			if (currentGO != _pointerData.pointerPress)
			{
				ExecuteEvents.ExecuteHierarchy(_pointerData.pointerPress, _pointerData, ExecuteEvents.pointerUpHandler);
			}

			// Do a click if the down and up are the same
			GameObject clickHandlerGO = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGO);
			if (_pointerData.pointerPress == clickHandlerGO)
			{
				ExecuteEvents.Execute(clickHandlerGO, _pointerData, ExecuteEvents.pointerClickHandler);
			}

			_pointerData.rawPointerPress = null;
			_pointerData.pointerPress = null;
		}

		if (_pointerData.pointerDrag != null)
		{
			ExecuteEvents.Execute(_pointerData.pointerDrag, _pointerData, ExecuteEvents.dragHandler);
		}

		AutoDeselect();

		if (eventSystem.currentSelectedGameObject != null)
		{
			ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, _pointerData, ExecuteEvents.updateSelectedHandler);
		}
	}

	void UpdateSelection(GameObject go)
	{
		if (go != eventSystem.currentSelectedGameObject)
		{
			// Stop dragging if we were
			if (_pointerData.pointerDrag)
			{
				ExecuteEvents.Execute(_pointerData.pointerDrag, _pointerData, ExecuteEvents.endDragHandler);
				_pointerData.pointerDrag = null;
			}
			// Change selection (will send de/select events)
			eventSystem.SetSelectedGameObject(go);
		}
	}

	void AutoDeselect()
	{
		var currentOverGO = _pointerData.pointerCurrentRaycast.gameObject;

		bool shouldDeselect = false;
		if (_autoDeselect.enabled)
		{
			if (_pointerData.pointerPress == null) // We're NOT clicking
			{
				if (_autoDeselect.evenWhenHoveringOverSameObject)
				{
					shouldDeselect = true;
				}
				// Else check if we are hovering over same object
				// if not, should deselect
				else if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGO) != eventSystem.currentSelectedGameObject)
				{
					shouldDeselect = true;
				}
			}
			else // We ARE clicking
			{
				if (_autoDeselect.onlyWhenNotClicking)
				{
					// do nothing because we ARE clicking
					//shouldDeselect = false; //default
				}
				else
				{
					if (_autoDeselect.evenWhenHoveringOverSameObject)
					{
						shouldDeselect = true;
					}
					else if (ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGO) != base.eventSystem.currentSelectedGameObject)
					{
						shouldDeselect = true;
					}
				}
			}
		}

		if (shouldDeselect)
		{
			UpdateSelection(null);
		}
	}
}