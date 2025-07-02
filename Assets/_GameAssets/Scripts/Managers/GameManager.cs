using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action<GameState> OnGameStateChanged;

    [Header("References")]
    [SerializeField] private MouseCameraControl _cameraController;
    [SerializeField] private CatController _catController;
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private WinLoseUI _winLoseUI;
    [SerializeField] private PlayerHealtUI _playerHealthUI;

    [Header("Settings")]
    [SerializeField] private int _maxEggCount = 5;
    [SerializeField] private float _delay;

    private GameState _currentGameState;

    private bool _hasGameOverTriggered = false;

    private int _currentEggCount;
    private bool _isCatCatched;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HealthManager.Instance.OnPlayerDeath += HealthManager_OnPlayerDeath;
        _catController.OnCatCatched += CatController_OnCatCatched;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CatController_OnCatCatched()
    {
        if (!_isCatCatched)
        {
            _playerHealthUI.AnimateDamageForAll();
            StartCoroutine(OnGameOver(true));
            CameraShake.Instance.ShakeCamera(1.5f, 2f, 0.5f);
            _isCatCatched = true;
        }
    }

    private void HealthManager_OnPlayerDeath()
    {
        StartCoroutine(OnGameOver(false));
    }

    private void OnEnable()
    {
        ChangeGameState(GameState.Cutscene);
        BackgroundMusic.Instance.PlayBackgroundMusic(true);
    }

    public void ChangeGameState(GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState = gameState;
        Debug.Log("Game State: " + gameState);
        
        // Mouse kontrolünü oyun durumuna göre ayarla
        HandleCameraControlForGameState(gameState);
    }

    private void HandleCameraControlForGameState(GameState gameState)
    {
        if (_cameraController == null) return;

        switch (gameState)
        {
            case GameState.Play:
            case GameState.Resume:
                _cameraController.EnableCameraControl();
                break;
                
            case GameState.Cutscene:
            case GameState.GameOver:
            case GameState.Pause:
                _cameraController.DisableCameraControl();
                break;
                
            default:
                _cameraController.DisableCameraControl();
                break;
        }
    }

    public void OnEggCollected()
    {
        _currentEggCount++;
        _eggCounterUI.SetEggCounterText(_currentEggCount, _maxEggCount);

        if (_currentEggCount == _maxEggCount)
        {
            _eggCounterUI.SetEggCompleted();
            ChangeGameState(GameState.GameOver);
            _winLoseUI.OnGameWin();
            
            // Oyun kazanıldığında mouse'u serbest bırak
            _cameraController.DisableCameraControl();
        }
    }

    private IEnumerator OnGameOver(bool isCatCatched)
    {
        if (_hasGameOverTriggered) yield break;
        _hasGameOverTriggered = true;

        yield return new WaitForSeconds(_delay);
        ChangeGameState(GameState.GameOver);
        _winLoseUI.OnGameLose();
        
        // Oyun kaybedildiğinde mouse'u serbest bırak
        _cameraController.DisableCameraControl();

        if (_isCatCatched)
        {
            AudioManager.Instance.Play(SoundType.CatSound);
        }
    }

    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

    #region Public Camera Control Methods (UI için)
    
    // Pause menu açıldığında çağırın
    public void OnPauseMenuOpen()
    {
        if (_cameraController != null)
        {
            _cameraController.PauseCamera();
        }
    }
    
    // Pause menu kapandığında çağırın
    public void OnPauseMenuClose()
    {
        if (_cameraController != null)
        {
            _cameraController.ResumeCamera();
        }
    }
    
    // Oyunu yeniden başlatırken çağırın
    public void OnGameRestart()
    {
        _hasGameOverTriggered = false;
        _currentEggCount = 0;
        _isCatCatched = false;
        
        ChangeGameState(GameState.Play);
    }
    
    // Oyunu başlatırken çağırın (cutscene sonrası)
    public void StartGame()
    {
        ChangeGameState(GameState.Play);
    }
    
    #endregion
}