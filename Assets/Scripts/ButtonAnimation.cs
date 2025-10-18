using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class ButtonAnimation : MonoBehaviour
{
    [Header("DOTween Animation Settings")]
    public float pressedScale = 0.9f;     // Scale when pressed
    public float duration = 0.2f;         // Total animation duration
    public Ease easeType = Ease.OutBack;  // Easing for bounce back

    private Button button;
    private Vector3 originalScale;

    void Awake()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;

        // Add animation to the button click
        button.onClick.AddListener(PlayClickAnimation);
    }

    public void PlayClickAnimation()
    {
        // Kill any active tweens on this object to avoid overlap
        transform.DOKill();

        // Shrink (press effect)
        transform.DOScale(originalScale * pressedScale, duration / 2)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() =>
                 {
                     // Bounce back to normal
                     transform.DOScale(originalScale, duration / 2)
                              .SetEase(easeType);
                 });
    }
}
