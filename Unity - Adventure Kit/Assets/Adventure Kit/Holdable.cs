
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Holdable : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
	[SerializeField]
	float _strength = 5;

	bool _dragging;
	float _distance;
	Vector3 _dragPoint = Vector3.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragging = true;
		_distance = eventData.pointerCurrentRaycast.distance;
		UpdateDragPoint(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
		UpdateDragPoint(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _dragging = false;
    }

	void UpdateDragPoint(PointerEventData eventData)
	{
		Vector2 screenPos = eventData.position;
		Ray ray = eventData.pressEventCamera.ScreenPointToRay(screenPos);
		_dragPoint = ray.origin + _distance * ray.direction;
	}

	void FixedUpdate()
	{
		if (_dragging)
		{
			Rigidbody body = GetComponent<Rigidbody>();
			body.velocity = (_dragPoint - body.position) * _strength;
		}
	}
}
