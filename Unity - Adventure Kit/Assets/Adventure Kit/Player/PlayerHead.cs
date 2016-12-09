using UnityEngine;

public class PlayerHead : MonoBehaviour {

	[SerializeField]
	bool _invertX = false;

	[SerializeField]
	bool _invertY = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float mx = Input.GetAxisRaw("Mouse X");
		float my = Input.GetAxisRaw("Mouse Y");

		mx *= _invertX ? -1 : 1;
		my *= _invertY ? -1 : 1;

		Vector3 rot = transform.localEulerAngles + new Vector3(my, mx, 0);
		transform.localRotation = Quaternion.Euler(rot);
	}
}
