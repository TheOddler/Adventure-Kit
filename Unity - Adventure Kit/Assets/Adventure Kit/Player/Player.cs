using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	[SerializeField]
	float _walkingSpeed = 5f;

	CharacterController _controller;

	void Awake() {
		_controller = GetComponent<CharacterController>();
	}

	void Start () {
		LockCursor();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.L))
		{
			LockCursor();
		}

		Vector3 move = new Vector3(
			Input.GetAxisRaw("Horizontal"), 
			0, 
			Input.GetAxisRaw("Vertical"));
		
		_controller.SimpleMove(move * _walkingSpeed);
	}

	void LockCursor() {
		Cursor.lockState = CursorLockMode.Locked;
	}
}
