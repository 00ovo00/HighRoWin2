using UnityEngine;
using DG.Tweening;
using System;

public class UIBase : MonoBehaviour
{
    public Canvas canvas;

    public virtual void ShowAnimation(float duration)
    {
        // 기본 팝업 애니메이션
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, duration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    public virtual void HideAnimation(float duration, Action onComplete)
    {
        // 기본 닫기 애니메이션
        transform.DOScale(Vector3.zero, duration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => onComplete?.Invoke());
    }
}