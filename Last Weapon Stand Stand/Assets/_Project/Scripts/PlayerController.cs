using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float                _jumpVelocity = 4;
    [SerializeField] private bool                 invertY;
    [SerializeField] private InputActionReference moouseLookAction;
    [SerializeField] private InputActionReference moouseLookButtonAction;
    [SerializeField] private InputActionReference playerMmoveAction;
    [SerializeField] private InputActionReference playerJumpAction;
    [SerializeField] private InputActionReference fastAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private float                lookSensitivity = 20.0f;
    [SerializeField] private float                moveSpeed       = 4.0f;
    [SerializeField] private Transform            playerBody;
    [SerializeField] private Transform            eyeCamera;


    private Rifle                   rifle;
    private DHTDebugPanel_1_Service debugPanel;
    private Vector2                 lookInput;
    private Rigidbody               _rigidbody;
    
    private void OnEnable()
    {
        moouseLookAction.action.Enable();
        moouseLookButtonAction.action.Enable();
        playerMmoveAction.action.Enable();
        playerJumpAction.action.Enable();
        fastAction.action.Enable();
        shootAction.action.Enable();

        playerJumpAction.action.started += PlayerJump;
    }


    private void OnDisable()
    {
        moouseLookAction.action.Disable();
        moouseLookButtonAction.action.Disable();
        playerMmoveAction.action.Disable();
        playerJumpAction.action.Disable();
        fastAction.action.Disable();
        shootAction.action.Disable();
    
        playerJumpAction.action.started -= PlayerJump;
    }


    private void Start()
    {
        _rigidbody = playerBody.GetComponent<Rigidbody>();
        
        Rifle[] rifles = FindObjectsByType<Rifle>(FindObjectsSortMode.None);
        rifle            = rifles[0];
        debugPanel       = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        HandleMouseLook();
        HandlePlayerMove();
        HandlePlayerShoot();
    }


    private                  float shootTimer = 0;
    [SerializeField] private float shootRate  = .2f;

    private void HandlePlayerShoot()
    {
        if (shootAction.action.ReadValue<float>() > .1)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                rifle.Fire();
                shootTimer = shootRate;
            }

            // Debug.Log("Player Shoot");
        }
        else
        {
            shootTimer = 0;
        }
    }


    private void PlayerJump(InputAction.CallbackContext obj)
    {
        // Debug.Log("Player Jumped");
        _rigidbody.linearVelocity += Vector3.up * _jumpVelocity;
    }


    float mouseX = 0;
    float mouseY = 0;

    void HandleMouseLook()
    {
        if(moouseLookButtonAction.action.ReadValue<float>() < 0.1f)
            return;
        lookInput = moouseLookAction.action.ReadValue<Vector2>();
        mouseX += lookInput.x * lookSensitivity * Time.deltaTime;
        mouseY += lookInput.y * lookSensitivity * Time.deltaTime * (invertY ? -1.0f : 1.0f);

        // eyeCamera.Rotate(Vector3.left * mouseY);
        playerBody.rotation = Quaternion.Euler(-mouseY, mouseX, 0f);
    }

    
    private void HandlePlayerMove()
    {
        bool    isFast   = fastAction.action.ReadValue<float>()>0.1f;
        Vector2 move      = playerMmoveAction.action.ReadValue<Vector2>() * moveSpeed * (isFast ? 2.0f : 1.0f);
        Vector3 deltaMove = playerBody.right * move.x + playerBody.forward * move.y;

        deltaMove.y               = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = deltaMove;
        
        debugPanel.SetElement(0, $"Keys: {move}", "");
        debugPanel.SetElement(1, $"deltaMove: {deltaMove}", "");
    }
}
