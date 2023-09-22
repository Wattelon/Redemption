using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float viewRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private protected Transform cameraTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _characterController;
    private PlayerInputActions _playerInputActions;
    private Vector3 _verticalVelocity = Vector3.zero;
    private Vector3 _moveVector;
    private float _speedModifier = 1;
    private bool _directionalFall;
    private float _cameraRotation;

    private const float GRAVITY = -9.81f;

    public bool IsMoving { get; private protected set; }
    public bool IsRunning { get; private protected set; }
    public bool IsJumping { get; private protected set; }
    public bool IsFalling { get; private protected set; }
    public bool IsGrounded { get; private protected set; }
    public bool IsFocused { get; private protected set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Jump.performed += OnJump;
        _playerInputActions.Player.Run.performed += OnRun;
        _playerInputActions.Player.Run.canceled += OnRun;
        _playerInputActions.Player.Fire.performed += OnAttack;
        _playerInputActions.Player.Focus.performed += OnBlock;
        _playerInputActions.Player.Focus.canceled += OnBlock;
        _playerInputActions.Player.SwitchMode.performed += OnSwitchMode;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Jump.performed -= OnJump;
        _playerInputActions.Player.Run.performed -= OnRun;
        _playerInputActions.Player.Run.canceled -= OnRun;
        _playerInputActions.Player.Fire.performed -= OnAttack;
        _playerInputActions.Player.Focus.performed -= OnBlock;
        _playerInputActions.Player.Focus.canceled -= OnBlock;
        _playerInputActions.Player.SwitchMode.performed -= OnSwitchMode;
        _playerInputActions.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!_characterController.isGrounded)
        {
            _verticalVelocity.y += GRAVITY * Time.deltaTime;
        }
        else
        {
            _verticalVelocity = Vector3.zero;
        }

        if (IsGrounded) _speedModifier = IsRunning ? 2 : 1;
        IsFalling = !IsGrounded && _verticalVelocity.y < 0;
        
        var thisTransform = transform;
        
        var inputLookVector = _playerInputActions.Player.Look.ReadValue<Vector2>() * (cameraSensitivity * Time.deltaTime);
        _cameraRotation -= inputLookVector.y;
        _cameraRotation = Mathf.Clamp(_cameraRotation, -viewRange, viewRange);
        transform.Rotate(Vector3.up * inputLookVector.x);
        cameraTransform.localRotation = Quaternion.Euler(_cameraRotation, 0, 0);
        
        var inputMoveVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        IsMoving = inputMoveVector.sqrMagnitude > 0;
        if (IsGrounded) _moveVector = thisTransform.right * inputMoveVector.x + thisTransform.forward * inputMoveVector.y;
        _characterController.Move((_moveVector.normalized * (_speedModifier * movementSpeed) + _verticalVelocity) * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded && !_characterController.isGrounded || IsJumping) return;
        IsJumping = true;
        _directionalFall = IsMoving;
        StartCoroutine(Jump());
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        IsRunning = context.performed;
    }

    private protected void OnAttack(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private protected void OnBlock(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private protected void OnSwitchMode(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.05f);
        _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * GRAVITY);
        _characterController.Move(_verticalVelocity * Time.deltaTime);
        IsJumping = false;
    }
}