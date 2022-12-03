using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Collider2D))]
public class AdjustColliderToRectSize : MonoBehaviour
{

    private RectTransform _rect;
    private List<Collider2D> _colliders = new();

    [Button]
    private void Adjust()
    {
        if (_rect == null)
            _rect = GetComponent<RectTransform>();

        if (_colliders.Count <= 0)
            _colliders.AddRange(GetComponents<Collider2D>());

        foreach (Collider2D collider in _colliders)
        {
            var boxColider = collider as BoxCollider2D;
            var circleColider = collider as CircleCollider2D;

            if (boxColider != null)
            {
                Vector2 size = new(_rect.rect.width, _rect.rect.height);
                boxColider.offset = Vector2.zero;
                boxColider.size = size;
            }

            if (circleColider != null)
            {
                circleColider.offset = Vector2.zero;
                circleColider.radius = _rect.rect.width / 2;
            }
        }
    }
}