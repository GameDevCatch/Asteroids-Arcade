using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{

    [SerializeField]
    private Camera PlayerCamera;
    [SerializeField]
    private GameObject HUD_Dot;

    [Space]

    [SerializeField]
    private float WalkingSpeed = 7.5f;
    [SerializeField]
    private float Gravity = 20.0f;
    [SerializeField]
    private float LookSpeed = 2.0f;
    [SerializeField]
    private float LookXLimit = 45.0f;

    [Space]

    [SerializeField]
    private float WalkingBobbingSpeed = 12f;
    [SerializeField]
    private float BobbingAmount = 0.03f;

    private CharacterController _charController;
    private Vector3 _moveDirection;
    private bool _controllerEnabled = true;

    private float _rotationX;
    private float _startCamYPos;
    private float _camBobTimer;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _startCamYPos = PlayerCamera.transform.localPosition.y;

        // Lock cursor
        if (_controllerEnabled)
            LockCurser(true);
    }

    void Update()
    {
        if (!_controllerEnabled) return;

        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = WalkingSpeed * Input.GetAxis("Vertical");
        float curSpeedY = WalkingSpeed * Input.GetAxis("Horizontal");
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        _moveDirection.y = movementDirectionY;

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!_charController.isGrounded)
        {
            _moveDirection.y -= Gravity * Time.deltaTime;
        }

        // Move the controller
        _charController.Move(_moveDirection * Time.deltaTime);

        // Player and Camera rotation
        _rotationX += -Input.GetAxis("Mouse Y") * LookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -LookXLimit, LookXLimit);
        PlayerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * LookSpeed, 0);

        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            //Bob Camera
            _camBobTimer += Time.deltaTime * WalkingBobbingSpeed;
            PlayerCamera.transform.localPosition = new Vector3(PlayerCamera.transform.localPosition.x, _startCamYPos + Mathf.Sin(_camBobTimer) * BobbingAmount, PlayerCamera.transform.localPosition.z);
        }
        else
        {
            //Idle
            _camBobTimer = 0;
            PlayerCamera.transform.localPosition = new Vector3(PlayerCamera.transform.localPosition.x, Mathf.Lerp(PlayerCamera.transform.localPosition.y, _startCamYPos, Time.deltaTime * WalkingBobbingSpeed), PlayerCamera.transform.localPosition.z);
        }
    }

    public void LockCurser(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            HUD_Dot.SetActive(true);
        } else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            HUD_Dot.SetActive(false);
        }
    }

    public void Enable()
    {
        LockCurser(true);
        _controllerEnabled = true;
    }

    public void Disable()
    {
        LockCurser(false);
        _camBobTimer = 0;
        PlayerCamera.transform.localPosition = new Vector3(PlayerCamera.transform.localPosition.x, _startCamYPos, PlayerCamera.transform.localPosition.z);
        _controllerEnabled = false;
    }

    public void SetLookPosition(Quaternion euler)
    {
        PlayerCamera.transform.rotation = euler;
    }
}