using UnityEngine.UI;
using UnityEditor.ShaderGraph.Internal;
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
        _blackBackgroundObject.SetActive(true);
        _winPopup.SetActive(true);

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _winPopuptransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutExpo);
    }

public void OnGameLose()
{
    if (_blackBackgroundImage == null || _losePopuptransform == null) return;

    // Buradaki koşulu kaldırıyoruz çünkü popup aktif değilse açacağız.
    // if (!_losePopuptransform.gameObject.activeInHierarchy) return;

    _blackBackgroundObject.SetActive(true);
    _losePopup.SetActive(true);

    _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
    _losePopuptransform.localScale = Vector3.zero;  // Başlangıç ölçeği 0 yap (varsa)
    _losePopuptransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutExpo);
}

}
