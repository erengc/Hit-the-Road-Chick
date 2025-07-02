using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlayerHealtUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image[] _playerHealthImages;

    [Header("Sprites")]
    [SerializeField] private Sprite _playerHealthySprite;
    [SerializeField] private Sprite _playerUnhealthySprite;

    [Header("Settings")]
    [SerializeField] private float _scaleDuration;
    private RectTransform[] _playerHealthTransforms;

    private void Awake()
    {
        _playerHealthTransforms = new RectTransform[_playerHealthImages.Length];

        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            _playerHealthTransforms[i] = _playerHealthImages[i].gameObject.GetComponent<RectTransform>();
        }
    }
    public void AnimateDamage()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            if (_playerHealthImages[i].sprite == _playerHealthySprite)
            {
                AnimateDamageSprite(_playerHealthImages[i], _playerHealthTransforms[i]);
                break;
            }
        }
    }

    public void AnimateDamageForAll()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            AnimateDamageSprite(_playerHealthImages[i], _playerHealthTransforms[i]);
        }
    }

    private void AnimateDamageSprite(Image activeImage, RectTransform activeImageTransform)
    {
        if (activeImageTransform == null || !activeImageTransform.gameObject.activeInHierarchy) return;

        activeImageTransform.DOScale(0f, _scaleDuration).SetEase(Ease.InExpo).OnComplete(() =>
        {
        if (activeImage == null || activeImageTransform == null) return;

        activeImage.sprite = _playerUnhealthySprite;
        activeImageTransform.DOScale(1f, _scaleDuration).SetEase(Ease.OutExpo);
        });
    }

}
