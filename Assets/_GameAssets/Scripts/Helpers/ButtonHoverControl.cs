using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverControl : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(SoundType.ButtonHoverSound);
    }
}
