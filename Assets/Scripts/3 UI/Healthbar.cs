using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public RectTransform rectTransform;

    Transform _target;
    Vector3 _lastTargetPosition;
    Vector2 _position;

    private float _yOffset;

    void Update()
    {
        if (!_target || _lastTargetPosition != _target.position)
            return;

        SetPosition();
    }

    public void Initialize(Transform target, float yOffset)
    {
        _target = target;
        _yOffset = yOffset;
    }

    public void SetPosition()
    {
        if (!_target)
        {
            return;
        }
            
        _position = Camera.main.WorldToScreenPoint(_target.position);
        _position.y += _yOffset;
        rectTransform.anchoredPosition = _position;
        _lastTargetPosition = _target.position;
    }
}
