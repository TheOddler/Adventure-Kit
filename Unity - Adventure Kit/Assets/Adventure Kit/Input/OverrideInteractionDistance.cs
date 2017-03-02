using UnityEngine;

public class OverrideInteractionDistance : MonoBehaviour
{
	[SerializeField]
	float _maximumInteractionDistance = 10;
	public float MaximumInteractionDistance {
		get { return _maximumInteractionDistance; }
		set { _maximumInteractionDistance = value; }
	}
}
