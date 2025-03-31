using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float _moveDistance = 1f;    // 한번에 이동하는 거리
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 _targetPosition;
    private bool _shouldMove = false;

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            // 입력 정규화
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

            if (direction != Vector3.zero)
            {
                // 현재 위치에서 한번에 이동하는 거리만큼 목표 지점 설정하고 움직여야 하는 상태로 전환
                _targetPosition = transform.position + direction * _moveDistance;
                _shouldMove = true;

                if (input == Vector2.up) // W 
                {
                    DataManager.Instance.RowCount++;    // 전진할 때만 점수 증가
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    return;
                }
                if (input == Vector2.left) // A
                {
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    return;
                }
                else  // D
                {
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    return;
                }
            }
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
