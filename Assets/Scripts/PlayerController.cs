using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

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
    private Collider[] _interactableColliders;
    private Interactable _interactable;
    private List<Item> _items = new();

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
        _playerInputActions.Player.Crouch.performed += OnCrouch;
        _playerInputActions.Player.Crouch.canceled += OnCrouch;
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
        _playerInputActions.Player.Crouch.performed -= OnCrouch;
        _playerInputActions.Player.Crouch.canceled -= OnCrouch;
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
        
        _interactableColliders = new Collider[10];
        var size = Physics.OverlapSphereNonAlloc(thisTransform.position, interactDistance, _interactableColliders, interactableMask);
        if (size > 0)
        {
            var interactable = _interactableColliders.Where(i => i is not null)
                .OrderBy(i => Vector3.Distance(thisTransform.position, i.transform.position)).First();
            _interactable = interactable.GetComponent<Interactable>();
            _interactable.ResetGlowTimer();
        }
        else _interactable = null;

        if (IsFocused)
        {
            var count = Physics.OverlapSphereNonAlloc(thisTransform.position, interactDistance * 5, _interactableColliders, interactableMask);
            for (int i = 0; i < count; i++)
            {
                _interactableColliders[i].GetComponent<Interactable>().Highlight();
            }
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
        characterCamera.fieldOfView *= context.performed ? 1.5f : 1/1.5f;
    }

    private void OnFire(InputAction.CallbackContext context)
    {
    }

    private void OnFocus(InputAction.CallbackContext context)
    {
        if (context.performed) characterCamera.fieldOfView *= 0.5f;
        else characterCamera.fieldOfView *= 2f;
        IsFocused = context.performed;
    }

    private void OnSwitchMode(InputAction.CallbackContext context)
    {
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (_interactable is not null)
        {
            _items.Add(_interactable.ItemType);
            Destroy(_interactable.gameObject);
            Debug.Log(_items[^1]);
        }
    }
    
    private void OnCrouch(InputAction.CallbackContext context)
    {
        _characterController.height *= context.performed ? 0.5f : 2;
        cameraTransform.localPosition += Vector3.up * (context.performed ? -1 : 1);
        groundCheck.localPosition += Vector3.up * (context.performed ? 0.5f : -0.5f);
        movementSpeed *= context.performed ? 0.5f : 2f;
        characterCamera.fieldOfView *= context.performed ? 1/1.5f : 1.5f;
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.05f);
        _verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * GRAVITY);
        _characterController.Move(_verticalVelocity * Time.deltaTime);
        IsJumping = false;
    }
}