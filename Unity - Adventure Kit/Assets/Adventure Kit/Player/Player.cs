using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LockCursor();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.L))
		{
			LockCursor();
		}
	}

	void LockCursor() {
		Cursor.lockState = CursorLockMode.Locked;
	}
}
