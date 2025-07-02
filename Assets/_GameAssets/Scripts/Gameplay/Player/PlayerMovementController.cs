using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerVisualTransform;
    [SerializeField] private Transform _orientationTransform;
    
    [Header("Movement Settings")]
    [SerializeField] private float _rotationSpeed = 5f;
    
    private void Update()
    {
        // Oyun durumu kontrolü
        bool gameStateOK = GameManager.Instance.GetCurrentGameState() == GameState.Play ||
                          GameManager.Instance.GetCurrentGameState() == GameState.Resume;
        
        if (!gameStateOK)
        {
            return;
        }

        HandlePlayerMovement();
    }

    private void HandlePlayerMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 || verticalInput != 0)
        {
            // Cinemachine kamerasından yön al
            Camera mainCamera = Camera.main;
            Vector3 forward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized;
            Vector3 right = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z).normalized;
            
            Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
            
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, 
                moveDirection.normalized, Time.deltaTime * _rotationSpeed);
        }

        // Orientation'ı kamera yönüne ayarla
        Camera cam = Camera.main;
        _orientationTransform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);
    }
}