using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class WinLoseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _blackBackgroundObject;
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private GameObject _losePopup;

    [Header("Settings")]
    [SerializeField] private float _animationDuration = 0.3f;

    private Image _blackBackgroundImage;
    private RectTransform _winPopuptransform;
    private RectTransform _losePopuptransform;

    private void Awake()
    {
        _blackBackgroundImage = _blackBackgroundObject.GetComponent<Image>();
        _winPopuptransform = _winPopup.GetComponent<RectTransform>();
        _losePopuptransform = _losePopup.GetComponent<RectTransform>();
    }

    public void OnGameWin()
    {
        // Mouse kontrolünü devre dışı bırak - popup açılmadan önce
        EnsureCursorUnlocked();
        
        _blackBackgroundObject.SetActive(true);
        _winPopup.SetActive(true);

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _winPopuptransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutExpo);
        
        Debug.Log("Win popup opened - Cursor should be unlocked");
    }

    public void OnGameLose()
    {
        if (_blackBackgroundImage == null || _losePopuptransform == null) return;

        // Mouse kontrolünü devre dışı bırak - popup açılmadan önce
        EnsureCursorUnlocked();

        _blackBackgroundObject.SetActive(true);
        _losePopup.SetActive(true);

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _losePopuptransform.localScale = Vector3.zero;  // Başlangıç ölçeği 0 yap
        _losePopuptransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutExpo);
        
        Debug.Log("Lose popup opened - Cursor should be unlocked");
    }

    private void EnsureCursorUnlocked()
    {
        // GameManager üzerinden kamera kontrolünü durdur
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPauseMenuOpen();
        }
        
        // Force cursor unlock (güvenlik için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Debug.Log("WinLoseUI: Cursor unlocked via EnsureCursorUnlocked");
    }

    #region Public Methods for Manual Control
    
    // Manual olarak popup'ları kapatmak için (gerekirse)
    public void ClosePopups()
    {
        _winPopup.SetActive(false);
        _losePopup.SetActive(false);
        _blackBackgroundObject.SetActive(false);
        
        // Eğer oyun devam edecekse mouse'u tekrar kilitle
        if (GameManager.Instance != null && 
            GameManager.Instance.GetCurrentGameState() == GameState.Play)
        {
            GameManager.Instance.OnPauseMenuClose();
        }
    }
    
    #endregion

    #region Debug Methods
    
    [ContextMenu("Test Win Popup")]
    private void TestWinPopup()
    {
        OnGameWin();
    }
    
    [ContextMenu("Test Lose Popup")]
    private void TestLosePopup()
    {
        OnGameLose();
    }
    
    [ContextMenu("Test Cursor Unlock")]
    private void TestCursorUnlock()
    {
        EnsureCursorUnlocked();
    }
    
    #endregion
}