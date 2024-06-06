using System;
using com.davidhopetech.core.Run_Time.Scripts.Service_Locator;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _jumpVelocity = 4;
    [SerializeField] private bool invertY;
    [SerializeField] private InputActionReference mouseLookAction;
    [SerializeField] private InputActionReference mouseLookButtonAction;
    [SerializeField] private InputActionReference playerMoveAction;
    [SerializeField] private InputActionReference playerJumpAction;
    [SerializeField] private InputActionReference fastAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference InteractAction;
    [SerializeField] private InputActionReference ReloadAction;
    [SerializeField] private float lookSensitivity = .2f;

    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform eyeCamera;
    [SerializeField] private LayerMask interactionLayer;

    [SerializeField] private float webLookSensitivityScale = .5f;
    [SerializeField] private bool _canJump = false;
    [SerializeField] private MusicManagerLevel _musicManagerLevel;
    [SerializeField] private WaveEngine _waveEngine;
    [SerializeField] private Vector3 mousePosition;

    private DHTLogService _logService;

    IAlienCounter _alienCounter;
    private RifleWithAmmo rifle;
    private DHTDebugPanel_1_Service debugPanel;
    private Vector2 lookInput;
    private Rigidbody _rigidbody;
    internal int _score = 0;
    private int _creditScore = 400;
    TMP_Text scoreBoardTMP;
    TMP_Text creditBoardTMP;
    TMP_Text interactionText;
    private GameObject _weaponStand;

    private float mouseDeltaMultiplier;
    private float pauseInteractionTextTimer;

    RaycastHit[] hits = new RaycastHit[4];
    float maxDistance = 5;


    private void OnEnable()
    {
        mouseLookAction.action.Enable();
        mouseLookButtonAction.action.Enable();
        playerMoveAction.action.Enable();
        playerJumpAction.action.Enable();
        fastAction.action.Enable();
        shootAction.action.Enable();
        InteractAction.action.Enable();
        ReloadAction.action.Enable();

        mouseLookAction.action.performed += MouseMove;
        playerJumpAction.action.started += PlayerJump;
        InteractAction.action.started += Interact;
        ReloadAction.action.started += Reload;
    }

    private void Reload(InputAction.CallbackContext obj)
    {
        if (rifle.GetAmmo().TryReload())
        {
            Debug.Log("Reloading");
            //Reload sound & animation
        }
    }


    private void Interact(InputAction.CallbackContext obj)
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.Log("Trying to interact");
        if (Physics.RaycastNonAlloc(ray, hits, maxDistance, interactionLayer) > 0)
        {
            var hit = hits[0];

            if (hit.collider.gameObject.TryGetComponent(out ShopUpgrade shop))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);

                UpdateCredit(-shop.TryBuy(_creditScore));
            }

            if (hit.collider.gameObject.TryGetComponent(out AmmoPickup ammoPickup))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name);


                ammoPickup.TakeAmmo(rifle.GetAmmo());
            }
        }
    }

    private void UpdateInteractionText()
    {
        if (rifle.GetAmmo().Reloading)
        {
            return;
        }

        if (pauseInteractionTextTimer > 0)
        {
            pauseInteractionTextTimer -= Time.deltaTime;
            return;
        }

        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        string text = "";

        int numhits;
        if ((numhits = Physics.RaycastNonAlloc(ray, hits, maxDistance, interactionLayer)) > 0)
        {
            var hit = hits[numhits - 1];

            debugPanel.SetElement(2, $"Num Hits: {numhits}, {hit.transform.name}");

            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                text = interactable.GetInteractionText();
            }
        }

        interactionText.text = text;
        pauseInteractionTextTimer = .2f;
    }

    private void MouseMove(InputAction.CallbackContext obj)
    {
        Vector2 mouseDelta = obj.ReadValue<Vector2>();


#if UNITY_WEBGL && !UNITY_EDITOR
		   var mouseDeltaMultiplier = lookSensitivity * webLookSensitivityScale;
#else
        var mouseDeltaMultiplier = lookSensitivity;
#endif

        mouseDelta.y *= mouseDeltaMultiplier * (invertY ? -1.0f : 1.0f);

        // mousePosition += mouseDelta;
    }


    private void OnDisable()
    {
        mouseLookAction.action.Disable();
        mouseLookButtonAction.action.Disable();
        playerMoveAction.action.Disable();
        playerJumpAction.action.Disable();
        fastAction.action.Disable();
        shootAction.action.Disable();
        InteractAction.action.Disable();
        ReloadAction.action.Disable();

        mouseLookButtonAction.action.performed -= MouseMove;
        playerJumpAction.action.started -= PlayerJump;
        InteractAction.action.started -= Interact;
        ReloadAction.action.started -= Reload;
    }


    private void Start()
    {
        _logService = DHTServiceLocator.Get<DHTLogService>();

        _weaponStand = FindFirstObjectByType<WeaponStand>().gameObject;
        _rigidbody = playerBody.GetComponent<Rigidbody>();
        var go = FindFirstObjectByType<ScoreBoard>().gameObject;
        scoreBoardTMP = go.GetComponent<TMP_Text>();
        creditBoardTMP = FindFirstObjectByType<CreditBoard>().GetComponent<TMP_Text>();
        interactionText = FindFirstObjectByType<InteractionText>().GetComponent<TMP_Text>();
        UpdateCredit(400);

        RifleWithAmmo[] rifles = FindObjectsByType<RifleWithAmmo>(FindObjectsSortMode.None);
        rifle = rifles[0];
        debugPanel = DHTServiceLocator.Get<DHTDebugPanel_1_Service>();
        _alienCounter = GameObjectExtensions.FindObjectsOfTypeWithInterface<IAlienCounter>()[0];

        Cursor.lockState = CursorLockMode.Locked;

        firstShotFired = false;
    }


    private void Update()
    {
        
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            UpdateScore(100);
        }
#endif
        
#if UNITY_WEBGL && !UNITY_EDITOR
		var mouseDeltaMultiplier = lookSensitivity * webLookSensitivityScale;
#else
        var mouseDeltaMultiplier = lookSensitivity;
#endif

        mousePosition += Input.mousePositionDelta * mouseDeltaMultiplier;

        HandleMouseLook();
        HandlePlayerMove();
        HandlePlayerShoot();
        UpdateInteractionText();
    }


    private float shootTimer = 0;
    [SerializeField] private float shootRate = .2f;
    private bool firstShotFired = false;
    [SerializeField] private GameObject pauseMenu;

    private void HandlePlayerShoot()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (shootAction.action.ReadValue<float>() > .1)
        {
            if (!firstShotFired && (_waveEngine.waveState != WaveEngine.WaveState.WaitingToStartWave) &&
                _alienCounter.Count != 0)
            {
                _musicManagerLevel.TransitonToTrack(1);
            }

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                if (rifle.Fire())
                {
                    shootTimer = shootRate;
                }
                else
                {
                    if (!rifle.GetAmmo().Reloading)
                    {
                        if (rifle.GetAmmo().TotalAmmoCount == 0)
                        {
                            interactionText.text = "Pickup Ammo";
                            pauseInteractionTextTimer = 1;
                        }
                        else
                        {
                            interactionText.text = "[ R ] Reload";
                            pauseInteractionTextTimer = 1;
                        }
                    }
                }
            }
        }
        else
        {
            shootTimer = 0;
        }
    }


    public void AlianUnalived(GameObject alien)
    {
        var dist = Vector3.Distance(_weaponStand.transform.position, alien.transform.position);

        int score = (int) MathF.Floor(dist / 10) * 10;

        UpdateScore(score);
        UpdateCredit(score);
        if (_alienCounter.Count == 0)
        {
            _musicManagerLevel.TransitonToTrack(0);
        }
    }

    public void UpdateScore(int scoreDelta)
    {
        _score += scoreDelta;
        if (LeaderBoardManager.Instance)
        {
            LeaderBoardManager.Instance.AddScore(_score);
        }

        scoreBoardTMP.text = _score.ToString();
    }

    void UpdateCredit(int amount)
    {
        _creditScore += amount;
        creditBoardTMP.text = _creditScore.ToString();
    }

    private void PlayerJump(InputAction.CallbackContext obj)
    {
        if (!_canJump)
            return;

        _rigidbody.linearVelocity += Vector3.up * _jumpVelocity;
    }


    void HandleMouseLook()
    {
        if (!Cursor.visible || Time.timeScale == 0)
            return;

        if (!Input.GetKey(KeyCode.Space))
        {
            // _logService.Log($"Mouse-x: {mousePosition.x}");
        }

        lookInput = mouseLookAction.action.ReadValue<Vector2>();

        var mouseX = mousePosition.x;
        var mouseY = mousePosition.y;

        mouseY = math.clamp(mouseY, -85, 85);

        // debugPanel.SetElement(2, $"mouseY: {mouseY}", "");

        // eyeCamera.Rotate(Vector3.left * mouseY);
        eyeCamera.rotation = Quaternion.Euler(-mouseY, mouseX, 0f);
    }


    private void HandlePlayerMove()
    {
        var mouseX = mousePosition.x;
        var moveDirection = Quaternion.Euler(0, mouseX, 0);

        bool isFast = fastAction.action.ReadValue<float>() > 0.1f;
        Vector2 move = playerMoveAction.action.ReadValue<Vector2>() * moveSpeed * (isFast ? 2.0f : 1.0f);
        Vector3 deltaMove = moveDirection * Vector3.right * move.x + moveDirection * Vector3.forward * move.y;

        deltaMove.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = deltaMove;

        // debugPanel.SetElement(0, $"Keys: {move}", "");
        // debugPanel.SetElement(1, $"deltaMove: {deltaMove}", "");
    }

    public void OnPause()
    {
        if (Time.timeScale == 0)
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        else
        {
            pauseMenu.SetActive(true);
        }
    }

    public void Quit()
    {
#if (UNITY_EDITOR)
        // UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_WEBGL)
//    Application.OpenURL("about:blank");
	Debug.Log("Web Quit");
#elif (UNITY_STANDALONE)
    Application.Quit();
#endif
    }
}