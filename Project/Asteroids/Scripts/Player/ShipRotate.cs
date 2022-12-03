using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotate : MonoBehaviour
{

    [SerializeField]
    private float RotationSpeed;

    private ShipInput _playerInput;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<ShipInput>();
    }

    private void OnEnable()
    {
        if (_playerInput == null) return;

        _playerInput.OnRotateLeftKeyHeld += () => { Rotate(1); };
        _playerInput.OnRotateRightKeyHeld += () => { Rotate(-1); };
    }

    private void OnDisable()
    {
        if (_playerInput == null) return;

        _playerInput.OnRotateLeftKeyHeld += () => { Rotate(0); };
        _playerInput.OnRotateRightKeyHeld += () => { Rotate(0); };
    }

    private void Rotate(float dir)
    {
        _rb.AddTorque(dir * RotationSpeed * Time.deltaTime * 100f);
    }
}