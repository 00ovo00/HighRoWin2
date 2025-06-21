using UnityEngine;

public class DayNightTimeController : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColor;                  // 낮→밤 색상 그라데이션
    [SerializeField] private AnimationCurve lightIntensityCurve;   // 시간대별 광원 세기

    [Header("TimeScale Settings")]
    [SerializeField] private AnimationCurve timeScaleCurve;        // 시간대별 timescale 변동
    [SerializeField] private float baseTimeScale;                  // 초기 기본 속도
    [SerializeField] private float timeScaleIncreasePerDay;        // 하루 지날 때마다 기본 속도가 오르는 값

    [Header("Cycle Settings")]
    [SerializeField] private float dayDuration;                    // 하루 길이(초단위)

    private float _elapsedTime;               // 실제 경과 시간 추적
    private int _daysPassed;

    private void Update()
    {
        _elapsedTime += Time.unscaledDeltaTime;  // 실제 흐른 시간 누적 (timescale 영향 없이)

        // 하루 주기 계산
        float cycleTime = _elapsedTime % dayDuration;
        float dayFraction = cycleTime / dayDuration;   // 0 → 1 사이

        _daysPassed = Mathf.FloorToInt(_elapsedTime / dayDuration);   // 지난 일자 계산

        directionalLight.intensity = lightIntensityCurve.Evaluate(dayFraction); // 광원 세기 조정
        directionalLight.color = lightColor.Evaluate(dayFraction);  // 광원 색상 조정

        float oscillation = timeScaleCurve.Evaluate(dayFraction);   // 시간 변동 수치값

        //  기본 속도(base) + 매일 증가분 × 경과 일수
        float currentBase = baseTimeScale + _daysPassed * timeScaleIncreasePerDay;

        //  최종 timescale
        float newTimeScale = currentBase * oscillation;
        Time.timeScale = newTimeScale;

        Time.fixedDeltaTime = 0.02f * newTimeScale; // 고정 프레임 물리 연산 보정
    }
}