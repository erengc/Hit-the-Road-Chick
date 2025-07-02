using UnityEngine;

public class MouseCameraControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerVisualTransform;
    [SerializeField] private Transform _orientationTransform;
    
    [Header("Camera Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _cameraDistance = 6f;
    [SerializeField] private float _cameraHeight = 2f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _smoothTime = 0.1f;
    
    [Header("Control Settings")]
    [SerializeField] private bool _isCameraActive = true;
    
    private bool _isGameActive = true;
    private float _yaw = 0f;   // Y ekseni (sağ-sol)
    private float _pitch = 0f; // X ekseni (yukarı-aşağı)
    
    private void Start()
    {
        LockCursor();
        
        // Başlangıç açısını al
        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        // Oyun durumu kontrolü
        bool gameStateOK = GameManager.Instance.GetCurrentGameState() == GameState.Play ||
                          GameManager.Instance.GetCurrentGameState() == GameState.Resume;
        
        if (!gameStateOK || !_isGameActive || !_isCameraActive)
        {
            return;
        }

        HandleCameraRotation();
        HandlePlayerMovement();
    }

    private void HandleCameraRotation()
    {
        // Mouse input
        _yaw += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -30f, 60f);

        // Kamera pozisyonu hesapla
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 targetPosition = _playerTransform.position - rotation * Vector3.forward * _cameraDistance;
        targetPosition.y += _cameraHeight;

        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / _smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime / _smoothTime);
    }

    private void HandlePlayerMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Kamera yönüne göre hareket
            Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 right = new Vector3(transform.right.x, 0, transform.right.z).normalized;
            
            Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
            
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, 
                moveDirection.normalized, Time.deltaTime * _rotationSpeed);
        }

        // Orientation update
        _orientationTransform.rotation = Quaternion.Euler(0, _yaw, 0);
    }

    #region Public Control Methods
    
    // Kamera kontrolünü aktif et
    public void EnableCameraControl()
    {
        _isCameraActive = true;
        LockCursor();
    }
    
    // Kamera kontrolünü devre dışı bırak
    public void DisableCameraControl()
    {
        _isCameraActive = false;
        UnlockCursor();
    }
    
    // Oyun durumunu ayarla
    public void SetGameState(bool isActive)
    {
        _isGameActive = isActive;
        
        if (isActive)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }
    
    // Oyun pause olduğunda
    public void PauseCamera()
    {
        _isCameraActive = false;
        UnlockCursor();
    }
    
    // Oyun resume olduğunda
    public void ResumeCamera()
    {
        _isCameraActive = true;
        LockCursor();
    }
    
    #endregion

    #region Cursor Control
    
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    #endregion

    #region Debug/Testing
    
    // Test için - Inspector'dan çağırabilirsiniz
    [ContextMenu("Test Disable Camera")]
    public void TestDisableCamera()
    {
        DisableCameraControl();
    }
    
    [ContextMenu("Test Enable Camera")]
    public void TestEnableCamera()
    {
        EnableCameraControl();
    }
    
    #endregion
}