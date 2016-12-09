using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AdventureInputModule : BaseInputModule
{
	List<RaycastResult> _raycastResultCache = new List<RaycastResult>();

	PointerEventData _pointerData;

    public override void Process()
    {
		UpdatePointerData();

        eventSystem.RaycastAll(_pointerData, _raycastResultCache); //also clears cache

		Debug.Log(_raycastResultCache.Count);
    }

	void UpdatePointerData()
	{
		if (_pointerData == null) 
		{
			_pointerData = new PointerEventData(eventSystem);
		}
		_pointerData.position = Input.mousePosition;
	}
}
