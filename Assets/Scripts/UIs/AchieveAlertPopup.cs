using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class AchieveAlertPopup : UIBase
{
    [SerializeField] private Image achievementImage;
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private Image[] starImgArray;

    private Vector3 _achievementImageOriginPos;

    private void Awake()
    {
        // achievementImage의 원래 위치 저장
        _achievementImageOriginPos = achievementImage.transform.localPosition;
    }

    private void Start()
    {
        canvas.sortingOrder = 3;    
    }
    
    public override void ShowAnimation(float duration)
    {
        // Achievement Image 애니메이션
        achievementImage.transform.localPosition = new Vector3(0, -Screen.height / 2, 0);
        achievementImage.transform.localScale = Vector3.zero;
        
        Sequence achievementSequence = DOTween.Sequence();
        achievementSequence.Append(achievementImage.transform.DOLocalMove(_achievementImageOriginPos, duration).SetEase(Ease.OutQuad));
        achievementSequence.Join(achievementImage.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack));
        achievementSequence.SetUpdate(true);

        // Star Images 애니메이션
        for (int i = 0; i < starImgArray.Length; i++)
        {
            Image star = starImgArray[i];
            
            // 별의 초기 상태 설정
            star.transform.localPosition = Vector3.zero;
            star.transform.localScale = Vector3.zero;
            star.color = new Color(star.color.r, star.color.g, star.color.b, 0);

            // 각 별에 대한 개별 애니메이션 시퀀스 생성
            Sequence starFlySequence = DOTween.Sequence();
            
            // 목표 위치 계산 (화면 밖으로 더 멀리)
            Vector2 targetPos = GetStarTargetPosition(i);
            Vector2 endPos = targetPos * 2.5f; // 기존 위치보다 2.5배 더 멀리 이동하여 화면 밖으로 나가도록 설정
            
            float flightDuration = duration * 1.2f; // 날아가는 시간

            // 애니메이션 정의
            starFlySequence.Append(star.transform.DOLocalMove(endPos, flightDuration).SetEase(Ease.OutCirc)); // 포물선을 그리며 날아가도록 OutCirc 사용
            starFlySequence.Join(star.transform.DOScale(1f, flightDuration * 0.7f)); // 날아가는 동안 크기가 커짐
            
            // Fade-in -> Fade-out 효과
            starFlySequence.Join(
                star.DOFade(1f, flightDuration * 0.5f) // 절반의 시간동안 Fade-in
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => {
                        star.DOFade(0f, flightDuration * 0.5f).SetEase(Ease.OutQuad); // 나머지 절반의 시간동안 Fade-out
                    })
            );
            
            starFlySequence.SetUpdate(true);
        }
        
        // 일정 시간 후 자동으로 팝업 닫기 (별 애니메이션과 무관하게 팝업만 사라짐)
        DOVirtual.DelayedCall(duration + 2.0f, () => UIManager.Instance.Hide<AchieveAlertPopup>()).SetUpdate(true);
    }
    
    // UIBase의 HideAnimation을 오버라이드
    public override void HideAnimation(float duration, Action onComplete)
    {
        Sequence hideSequence = DOTween.Sequence();

        // Achievement Image 위로 사라지는 애니메이션
        hideSequence.Append(achievementImage.transform.DOLocalMoveY(Screen.height / 2 + achievementImage.rectTransform.rect.height, duration).SetEase(Ease.InQuad));
        
        hideSequence.OnComplete(() => onComplete?.Invoke());
        hideSequence.SetUpdate(true);
    }
    
    // 별들의 최종 위치 계산
    private Vector2 GetStarTargetPosition(int index)
    {
        float angle = 180f / (starImgArray.Length - 1) * index;
        float radius = Screen.width / 2f; 
        float yOffset = Screen.height / 4f; 

        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = (radius / 2) * Mathf.Sin(angle * Mathf.Deg2Rad) + yOffset;
        
        return new Vector2(x, y);
    }

    public void SetAchievementName(string name)
    {
        achievementText.text = name;
    }
}