using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdventureInputModule : BaseInputModule
{
	//
	// Settings
	// ---

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

		// Handle enter and exit events on the GUI controlls that are hit
		base.HandlePointerExitAndEnter(_pointerData, _pointerData.pointerCurrentRaycast.gameObject);
    }

	void UpdatePointerDataForMouse()
	{
		if (_pointerData == null) _pointerData = new PointerEventData(eventSystem);
		else _pointerData.Reset();

		_pointerData.delta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //so it works when mouse is locked
		_pointerData.position = Input.mousePosition;
		_pointerData.scrollDelta = Input.mouseScrollDelta;
	}

	/// <summary>
	/// OnGUI is called for rendering and handling GUI events.
	/// This function can be called multiple times per frame (one call per event).
	/// </summary>
	void OnGUI()
	{
		GUILayout.Label("-- Debug --");
		GUILayout.Label("Raycast count: " + m_RaycastResultCache.Count);
		GUILayout.Label("Current object: " + _pointerData.pointerCurrentRaycast.gameObject);
	}
}
