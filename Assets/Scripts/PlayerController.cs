using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float viewRange;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Camera characterCamera;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactableMask;

    private CharacterController _characterController;
    private PlayerInputActions _playerInputActions;
    private Vector3 _verticalVelocity = Vector3.zero;
    private Vector3 _moveVector;
    private float _speedModifier = 1;
    private float _cameraRotation;
    private Collider[] _interactables;

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
        _playerInputActions.Player.Fire.performed += OnFire;
        _playerInputActions.Player.Focus.performed += OnFocus;
        _playerInputActions.Player.Focus.canceled += OnFocus;
        _playerInputActions.Player.SwitchMode.performed += OnSwitchMode;
        _playerInputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Jump.performed -= OnJump;
        _playerInputActions.Player.Run.performed -= OnRun;
        _playerInputActions.Player.Run.canceled -= OnRun;
        _playerInputActions.Player.Fire.performed -= OnFire;
        _playerInputActions.Player.Focus.performed -= OnFocus;
        _playerInputActions.Player.Focus.canceled -= OnFocus;
        _playerInputActions.Player.SwitchMode.performed -= OnSwitchMode;
        _playerInputActions.Player.Interact.performed -= OnInteract;
        _playerInputActions.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!_characterController.isGrounded) _verticalVelocity.y += GRAVITY * Time.deltaTime;
        else _verticalVelocity = Vector3.zero;

        if (IsGrounded) _speedModifier = IsRunning ? 2 : 1;
        IsFalling = !IsGrounded && _verticalVelocity.y < 0;
        
        var thisTransform = transform;
        
        InputCameraView();

        var inputMoveVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        IsMoving = inputMoveVector.sqrMagnitude > 0;
        if (IsGrounded) _moveVector = thisTransform.right * inputMoveVector.x + thisTransform.forward * inputMoveVector.y;
        
        _characterController.Move((_moveVector.normalized * (_speedModifier * movementSpeed) + _verticalVelocity) * Time.deltaTime);
        
        _interactables = new Collider[5];
        var size = Physics.OverlapSphereNonAlloc(thisTransform.position, interactDistance, _interactables, interactableMask);
        if (size > 0)
        {
            var interactable = _interactables.Where(i => i is not null).OrderBy(i => Vector3.Distance(thisTransform.position, i.transform.position)).First();
            interactable.GetComponent<Interactable>();
        }
    }

    private void InputCameraView()
    {
        var inputLookVector = _playerInputActions.Player.Look.ReadValue<Vector2>() * (cameraSensitivity * Time.deltaTime);
        _cameraRotation -= inputLookVector.y;
        _cameraRotation = Mathf.Clamp(_cameraRotation, -viewRange, viewRange);
        transform.Rotate(Vector3.up * inputLookVector.x);
        cameraTransform.localRotation = Quaternion.Euler(_cameraRotation, 0, 0);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!IsGrounded && !_characterController.isGrounded || IsJumping) return;
        IsJumping = true;
        StartCoroutine(Jump());
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        IsRunning = context.performed;
        if (IsRunning) characterCamera.fieldOfView *= 1.5f;
        else characterCamera.fieldOfView /= 1.5f;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
    }

    private void OnFocus(InputAction.CallbackContext context)
    {
        if (context.performed) characterCamera.fieldOfView *= 0.5f;
        else characterCamera.fieldOfView *= 2f;
    }

    private void OnSwitchMode(InputAction.CallbackContext context)
    {
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.05f);
        _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * GRAVITY);
        _characterController.Move(_verticalVelocity * Time.deltaTime);
        IsJumping = false;
    }
}