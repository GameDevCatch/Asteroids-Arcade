using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipThrust : MonoBehaviour
{

    [SerializeField]
    private float ThrustForce;
    [SerializeField]
    private float MaxSpeed;
    [SerializeField]
    private float Friction;

    private Rigidbody2D _rb;
    private ShipInput _playerInput;

    private bool _thrusting;

    private void Awake()
    {
        _playerInput = GetComponent<ShipInput>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (_playerInput != null)
            _playerInput.OnThrustKeyHeld += () => { _thrusting = true; };
    }

    private void OnDisable()
    {
        if (_playerInput != null)
            _playerInput.OnThrustKeyHeld -= () => { _thrusting = false; };
    }

    private void FixedUpdate()
    {
        if (_thrusting)
            Thrust();

        _thrusting = false;
    }

    private void Thrust()
    {
        if (_rb.velocity.magnitude < MaxSpeed)
            _rb.AddForce(transform.up * ThrustForce * Time.deltaTime);
    }
}