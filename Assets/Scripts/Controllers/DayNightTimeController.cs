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

    private float _elapsedTime; // 실제 경과 시간 추적
    private int _daysPassed;    // 지난 일자 누적

    private void Awake()
    {
        if (directionalLight == null)
            directionalLight = GetComponent<Light>();

        // Gradient 기본값 설정
        if (lightColor == null || lightColor.colorKeys.Length == 0)
        {
            // 새 Gradient 생성
            var defaultGrad = new Gradient();
            defaultGrad.mode = GradientMode.Blend;  

            // 색상 키 설정
            GradientColorKey[] colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.white, 0f),
                new GradientColorKey(new Color(1f, 0.84f, 0f), 0.15f),
                new GradientColorKey(new Color(0.05f, 0.05f, 0.4f), 0.30f),
                new GradientColorKey(new Color(0.05f, 0.05f, 0.4f), 0.70f),
                new GradientColorKey(new Color(1f, 0.84f, 0f), 0.90f),
                new GradientColorKey(Color.white, 1f),
            };

            // 알파키 모두 불투명으로 설정
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 1f),
            };

            defaultGrad.SetKeys(colorKeys, alphaKeys);
            lightColor = defaultGrad;
        }

        // 광원 세기
        if (lightIntensityCurve == null || lightIntensityCurve.length == 0)
        {
            lightIntensityCurve = new AnimationCurve();

            // 키 프레임 추가
            lightIntensityCurve.AddKey(new Keyframe(0, 1));
            lightIntensityCurve.AddKey(new Keyframe(0.5f, 0.5f));
            lightIntensityCurve.AddKey(new Keyframe(1, 1));

            // 부드러운 보간 적용
            for (int i = 0; i < lightIntensityCurve.length; i++)
                lightIntensityCurve.SmoothTangents(i, 0f);
        }

        // 시간 변동
        if (timeScaleCurve == null || timeScaleCurve.length == 0)
        {
            timeScaleCurve = new AnimationCurve();

            // 키 프레임 추가
            timeScaleCurve.AddKey(new Keyframe(0, 0.7f));
            timeScaleCurve.AddKey(new Keyframe(0.5f, 0.8f));
            timeScaleCurve.AddKey(new Keyframe(1, 0.7f));

            // 부드러운 보간 적용
            for (int i = 0; i < timeScaleCurve.length; i++)
                timeScaleCurve.SmoothTangents(i, 0f);
        }
        
        baseTimeScale = 1;
        timeScaleIncreasePerDay = 0.0001f;
        dayDuration = 60;
    }

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