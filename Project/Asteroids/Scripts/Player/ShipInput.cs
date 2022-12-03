using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipInput : MonoBehaviour
{

    [SerializeField]
    private KeyCode ShootKey;
    [SerializeField]
    private KeyCode WarpKey;
    [SerializeField]
    private KeyCode ThrustKey;
    [SerializeField]
    private KeyCode RotateLeftKey;
    [SerializeField]
    private KeyCode RotateRightKey;

    public event Action OnThrustKeyHeld;
    public event Action OnRotateLeftKeyHeld;
    public event Action OnRotateRightKeyHeld;

    public event Action OnShootKeyDown;
    public event Action OnWarpKeyDown;

    public Arcade _arcade;

    private void Awake()
    {
        _arcade = gameObject.FindParentCanvas().transform.parent.GetComponent<Arcade>();
    }

    private void Update()
    {
        if (!_arcade.IsPlayerPlaying)
            return;

        if (Input.GetKey(ThrustKey))
            OnThrustKeyHeld?.Invoke();

        if (Input.GetKey(RotateLeftKey))
            OnRotateLeftKeyHeld?.Invoke();

        if (Input.GetKey(RotateRightKey))
            OnRotateRightKeyHeld?.Invoke();

        if (Input.GetKeyDown(ShootKey))
            OnShootKeyDown?.Invoke();

        if (Input.GetKeyDown(WarpKey))
            OnWarpKeyDown?.Invoke();
    }
}