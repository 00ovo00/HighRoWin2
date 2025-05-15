using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularCameraController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();
    [SerializeField] private int numberOfPlayers;
    [SerializeField] private float radius;
    [SerializeField] private float playerYOffset;

    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool smoothRotation;
    [SerializeField] private float camYOffset;

    [Header("Runtime")]
    [SerializeField] private List<GameObject> playerPrefabList = new List<GameObject>();

    public bool IsRotating { get; private set; }
    private Vector3 _centerPosition;

    private void Awake()
    {
        numberOfPlayers = playerPrefabList.Count;
        radius = 5;
        playerYOffset = 0;

        if (mainCamera == null) mainCamera = Camera.main;
        rotationSpeed = 2;
        smoothRotation = true;
        camYOffset = 1f;
        
        IsRotating = false;
        _centerPosition = Vector3.zero;
    }

    private void Start()
    {
        // 플레이어 생성 및 배치
        SetPlayersTransform();
        
        // 카메라를 월드 원점에 위치시키고 첫 번째 플레이어 바라보기
        mainCamera.transform.position = _centerPosition;
        RotateCameraToPlayer(CharacterManager.Instance.curCharacterIdx, false);
    }

    private void SetPlayersTransform()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            // 각도 계산 (360도를 플레이어 수로 나눔)
            float angle = i * 360f / numberOfPlayers;
            float angleInRadians = angle * Mathf.Deg2Rad;
            
            // 원형 위치 계산 (월드 원점 기준)
            Vector3 position = new Vector3(
                Mathf.Cos(angleInRadians) * radius,
                playerYOffset,
                Mathf.Sin(angleInRadians) * radius
            );
            
            // 플레이어 생성
            GameObject player = Instantiate(playerPrefabList[i], position, Quaternion.identity);
            player.name = $"Player_{i + 1}";
            player.GetComponent<Animator>().enabled = false;
            playerList.Add(player);
            
            // 중심을 향해 회전 (월드 원점을 바라보도록)
            Vector3 directionToCenter = (_centerPosition - position).normalized;
            player.transform.rotation = Quaternion.LookRotation(directionToCenter);
        }
    }

    public void RotateToPrev()
    {
        if (IsRotating || playerPrefabList.Count == 0) return;
        
        CharacterManager.Instance.ChangeToPreviousCharacter();
        RotateCameraToPlayer(CharacterManager.Instance.curCharacterIdx, smoothRotation);
    }

    public void RotateToNext()
    {
        if (IsRotating || playerPrefabList.Count == 0) return;
        
        CharacterManager.Instance.ChangeToNextCharacter();
        RotateCameraToPlayer(CharacterManager.Instance.curCharacterIdx, smoothRotation);
    }

    private void RotateCameraToPlayer(int playerIndex, bool animate = true)
    {
        if (playerIndex < 0 || playerIndex >= playerList.Count || mainCamera == null)
            return;

        // 타겟 플레이어의 위치
        Vector3 targetPosition = playerList[playerIndex].transform.position;
        
        // 월드 원점(카메라 위치)에서 플레이어로의 방향 계산
        Vector3 direction = (targetPosition - _centerPosition).normalized;
        
        // 카메라 회전 계산 (플레이어를 바라보도록)
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (animate && smoothRotation)
        {
            StartCoroutine(SmoothCameraRotation(targetRotation));
        }
        else
        {
            mainCamera.transform.rotation = targetRotation;
            mainCamera.transform.position = new Vector3(0, camYOffset, 0);
        }
    }

    IEnumerator SmoothCameraRotation(Quaternion targetRotation)
    {
        IsRotating = true;
        
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * rotationSpeed;
            
            // 부드러운 곡선을 위한 Smoothstep 사용
            float t = Mathf.SmoothStep(0, 1, elapsed);
            
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            
            yield return null;
        }
        
        mainCamera.transform.rotation = targetRotation;
        IsRotating = false;
    }


    // Gizmo를 통한 시각화
    private void OnDrawGizmos()
    {
        // 월드 원점을 중심으로 원형 배치 영역 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        
        // 월드 원점 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Vector3.zero, 0.1f);
        
        // 플레이어 위치 표시
        for (int i = 0; i < numberOfPlayers; i++)
        {
            float angle = i * 360f / numberOfPlayers;
            float angleInRadians = angle * Mathf.Deg2Rad;
            
            Vector3 position = new Vector3(
                Mathf.Cos(angleInRadians) * radius,
                playerYOffset,
                Mathf.Sin(angleInRadians) * radius
            );
            
            int currentIdx = Application.isPlaying ? CharacterManager.Instance.curCharacterIdx : 0;
            Gizmos.color = (i == currentIdx) ? Color.green : Color.red;
            Gizmos.DrawSphere(position, 0.2f);
            
            // 원점에서 플레이어로의 선 그리기
            if (i == currentIdx)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(Vector3.zero, position);
            }
        }
        
        // 카메라 위치 표시 (월드 원점에 고정)
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(Vector3.zero, 0.15f);
        
        // 카메라의 시선 방향 표시
        if (Application.isPlaying && mainCamera != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(Vector3.zero, mainCamera.transform.forward * 2f);
        }
    }

    // 에디터에서 실시간 업데이트
    private void OnValidate()
    {
        if (Application.isPlaying)
            return;
            
        #if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
        #endif
    }
}