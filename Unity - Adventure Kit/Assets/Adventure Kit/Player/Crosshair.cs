using UnityEngine;

public class Crosshair : MonoBehaviour
{
	[SerializeField]
	float _size;

	[SerializeField]
	Texture2D _texture;
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		float x = Input.mousePosition.x - _size / 2f;
		float y = Screen.height - Input.mousePosition.y - _size / 2f; //fixed mouse coords flip
		GUI.DrawTexture(new Rect(x, y, _size, _size), _texture);

		//GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 10, 10), Texture2D.whiteTexture);
	}
}
