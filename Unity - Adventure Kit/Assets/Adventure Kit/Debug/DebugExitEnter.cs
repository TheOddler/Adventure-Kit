﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Text))]
public class DebugExitEnter : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	Text _text;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
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
