using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrapper : MonoBehaviour
{

    private RectTransform _canvas;
    private RectTransform _rect;

    private Vector2 _canvasSizeHalfed;

    private void Awake()
    {
        _rect = transform.GetComponent<RectTransform>();
    }

    private void Start()
    {
        _canvas = gameObject.FindParentCanvas().GetComponent<RectTransform>();
        _canvasSizeHalfed = _canvas.rect.size / 2;
    }

    private void Update()
    {
        if (_rect.anchoredPosition.y > _canvasSizeHalfed.y + _rect.localScale.y)
            _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, (-_canvasSizeHalfed.y) - _rect.localScale.y);
        else if (_rect.anchoredPosition.y < (-_canvasSizeHalfed.y) - _rect.localScale.y)
           _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _canvasSizeHalfed.y + _rect.localScale.y);

        else if (_rect.anchoredPosition.x > _canvasSizeHalfed.x + _rect.localScale.x)
            _rect.anchoredPosition = new Vector2((-_canvasSizeHalfed.x) - _rect.localScale.x, _rect.anchoredPosition.y);
        else if (_rect.anchoredPosition.x < (-_canvasSizeHalfed.x) - _rect.localScale.x)
            _rect.anchoredPosition = new Vector2(_canvasSizeHalfed.x + _rect.localScale.x, _rect.anchoredPosition.y);
    }
}