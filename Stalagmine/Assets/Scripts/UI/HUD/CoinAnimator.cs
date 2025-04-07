using DG.Tweening;
using UnityEngine;

public class CoinAnimator : MonoBehaviour
{
    [SerializeField]
    private RectTransform coinIcon;
    [SerializeField]
    private CanvasGroup coinCanvas;

    public void Animate(Vector3 targetedPos)
    {
        Sequence coinSequence = DOTween.Sequence();
        coinSequence.Append(coinIcon.DOMove(targetedPos, 1f));
        coinSequence.Join(coinCanvas.DOFade(0, 1f).SetEase(Ease.InExpo));
        coinSequence.Play();
    }
}
