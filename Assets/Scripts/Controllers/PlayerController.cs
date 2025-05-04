using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerController : MonoBehaviour
{
    private float _moveDistance = 1f;    // 한번에 이동하는 거리
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 _targetPosition;
    private bool _shouldMove = false;
    
    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;
    [SerializeField] private float swipeThreshold = 50f;  // 스와이프 인식 임계값
    
    [SerializeField] private LayerMask obstacleLayer;  // 장애물 레이어
    [SerializeField] private float raycastDistance = 1.1f;  // 레이캐스트 거리
    
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        Touch.onFingerUp += OnFingerUp;
    }
    
    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    // 터치 첫 인식
    private void OnFingerDown(Finger finger)
    {
        if (!GameManager.Instance.isPlaying) return;
        
        // UI 요소 위에서 터치가 시작되었는지 확인
        if (IsPointerOverUIObject(finger.screenPosition)) 
        {
            return;
        }
        
        _touchStartPos = finger.screenPosition;
    }
    
    // 화면에서 손가락 떼었을 때
    private void OnFingerUp(Finger finger)
    {
        if (!GameManager.Instance.isPlaying) return;
        
        // UI 요소 위에서 터치가 끝났거나, 시작 위치가 저장되지 않은 경우 무시
        if (IsPointerOverUIObject(finger.screenPosition) || _touchStartPos == Vector2.zero) 
        {
            _touchStartPos = Vector2.zero;
            return;
        }
        
        _touchEndPos = finger.screenPosition;
        ProcessSwipe();
        
        // 터치 처리 후 초기화
        _touchStartPos = Vector2.zero;
    }
    
    // UI 요소 위에 포인터가 있는지 확인
    private bool IsPointerOverUIObject(Vector2 position)
    {
        // 이벤트 시스템에서 현재 위치에 UI 요소가 있는지 확인
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = position;
        
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        return results.Count > 0;
    }
    
    private void ProcessSwipe()
    {
        Vector2 swipeDelta = _touchEndPos - _touchStartPos;
        
        if (swipeDelta.magnitude < swipeThreshold)  // 스와이프 거리가 임계값 이상인 경우에만 처리
        {
            HandleMovement(Vector2.up); // 짧은 터치는 전진으로 처리
            return;
        }
        
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))  // 수평 스와이프인지 수직 스와이프인지 확인
        {
            // 수평 스와이프
            if (swipeDelta.x > 0)
            {
                HandleMovement(Vector2.right);  // 오른쪽 스와이프
            }
            else
            {
                HandleMovement(Vector2.left);   // 왼쪽 스와이프
            }
        }
        else
        {
            // 수직 스와이프 (위로 스와이프)
            if (swipeDelta.y > 0)
            {
                HandleMovement(Vector2.up);
            }
        }
    }
    
    // 이동 가능한지 체크
    private bool CanMove(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance, obstacleLayer))
        {
            // SoundManager.Instance.PlayBlockedSFX();  // 막힘 효과음 재생
            return false;
        }
        
        return true;
    }
    
    private void HandleMovement(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
        
        if (direction != Vector3.zero)
        {
            // 방향에 따라 회전 적용
            if (input == Vector2.up) // W 또는 위쪽 터치
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (input == Vector2.left) // A 또는 왼쪽 스와이프
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (input == Vector2.right) // D 또는 오른쪽 스와이프
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            
            if (CanMove(direction))
            {
                // 이동 가능하면 목표 지점 설정하고 움직여야 하는 상태로 전환
                _targetPosition = transform.position + direction * _moveDistance;
                _shouldMove = true;
                SoundManager.Instance.PlayMoveSFX();    // 이동하는 효과음 재생
                
                // 전진할 때만 점수 증가
                if (input == Vector2.up)
                {
                    DataManager.Instance.RowCount++;
                }
            }
        }
    }

    // 키보드 입력(WAD)
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.isPlaying) return;    // 플레이 중이 아닐 때는 키입력 무시
        
        if (context.phase == InputActionPhase.Started)
        {
            // 입력 정규화
            Vector2 input = context.ReadValue<Vector2>();
            HandleMovement(input);
        }
    }

    private void FixedUpdate()
    {
        if (_shouldMove) // 움직여야하는 상태면
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.fixedDeltaTime);
            transform.position = newPosition;   // 목표 지점으로 이동

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)   // 목표 지점에 근접하면
            {
                // 목표 지점 도착한 것으로 설정하고 움직이지 않아야하는 상태로 전환
                transform.position = _targetPosition;
                _shouldMove = false;
            }
        }
    }
}