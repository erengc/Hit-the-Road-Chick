using System;
using DG.Tweening;
using MaskTransitions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private Button _oneMoreButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;

    private void OnEnable()
    {
        // Mouse'u serbest bırak - popup açıldığında
        EnsureCursorUnlocked();
        
        // Audio ve UI setup
        BackgroundMusic.Instance.PlayBackgroundMusic(false);
        AudioManager.Instance.Play(SoundType.WinSound);

        _timerText.text = _timerUI.GetFinalTime();

        // Button listeners - önce temizle
        _oneMoreButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
        
        _oneMoreButton.onClick.AddListener(OnOneMoreButtonClicked);

        _mainMenuButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.Play(SoundType.TransitionSound);
            TransitionManager.Instance.LoadLevel(Consts.SceneNames.MENU_SCENE);
        });
    }

    private void OnOneMoreButtonClicked()
    {
        AudioManager.Instance.Play(SoundType.TransitionSound);
        
        // GameManager'a restart sinyali gönder (eğer varsa)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameRestart();
        }
        
        TransitionManager.Instance.LoadLevel(Consts.SceneNames.GAME_SCENE);
    }

    private void EnsureCursorUnlocked()
    {
        // Kamera kontrolünü devre dışı bırak
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPauseMenuOpen();
        }
        
        // Force cursor unlock (güvenlik için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Debug.Log("WinPopup: Cursor unlocked");
    }

    private void OnDisable()
    {
        // Button listener'ları temizle
        _oneMoreButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
    }

    #region Debug Methods
    
    [ContextMenu("Test Cursor Unlock")]
    private void TestCursorUnlock()
    {
        EnsureCursorUnlocked();
    }
    
    #endregion
}