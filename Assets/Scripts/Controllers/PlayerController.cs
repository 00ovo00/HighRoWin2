using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerController : MonoBehaviour
{
    private float _moveDistance = 1f;    // moving distance at once
    [SerializeField] private float moveSpeed = 5f;
    private Vector3 _targetPosition;
    private bool _shouldMove = false;   // check player should move or not
    
    private Vector2 _touchStartPos;
    private Vector2 _touchEndPos;
    [SerializeField] private float swipeThreshold = 50f;  // swipe aware threshold
    
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float raycastDistance = 1.1f;
    
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

    // when input first touch
    private void OnFingerDown(Finger finger)
    {
        if (!GameManager.Instance.isPlaying) return;
        
        // check that touch has started above UI elements
        if (IsPointerOverUIObject(finger.screenPosition)) 
        {
            return;
        }
        
        _touchStartPos = finger.screenPosition;
    }
    
    // when take finger off the screen
    private void OnFingerUp(Finger finger)
    {
        if (!GameManager.Instance.isPlaying) return;
        
        // ignore if touch is done on UI element or start position is not saved
        if (IsPointerOverUIObject(finger.screenPosition) || _touchStartPos == Vector2.zero) 
        {
            _touchStartPos = Vector2.zero;
            return;
        }
        
        _touchEndPos = finger.screenPosition;
        ProcessSwipe();
        
        // reset after touch processing
        _touchStartPos = Vector2.zero;
    }
    
    // check that there is a pointer above the UI element
    private bool IsPointerOverUIObject(Vector2 position)
    {
        // check that the current location in the event system has UI elements
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = position;
        
        System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        return results.Count > 0;
    }
    
    // processing swipe input
    private void ProcessSwipe()
    {
        Vector2 swipeDelta = _touchEndPos - _touchStartPos;
        
        if (swipeDelta.magnitude < swipeThreshold)  // process only if the swipe distance is above the threshold
        {
            HandleMovement(Vector2.up); // short touch is handled as forward
            return;
        }
        
        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))  // Check if it is a horizontal swipe or a vertical swipe
        {
            // horizontal swipe
            if (swipeDelta.x > 0)
            {
                HandleMovement(Vector2.right);  // right swipe
            }
            else
            {
                HandleMovement(Vector2.left);   // left swipe
            }
        }
        else
        {
            // vertical swipe (upward swipe)
            if (swipeDelta.y > 0)
            {
                HandleMovement(Vector2.up);
            }
        }
    }
    
    private bool CanMove(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastDistance, obstacleLayer))
        {
            // SoundManager.Instance.PlayBlockedSFX();
            return false;
        }
        
        return true;
    }
    
    // set direction and prepare to move
    private void HandleMovement(Vector2 input)
    {
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
        
        if (direction != Vector3.zero)
        {
            // apply rotation according to direction
            if (input == Vector2.up) // W or upward swipe
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (input == Vector2.left) // A or left swipe
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
            }
            else if (input == Vector2.right) // D or right swipe
            {
                transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            
            if (CanMove(direction))
            {
                // set a target point and move to a state should move if it can move
                _targetPosition = transform.position + direction * _moveDistance;
                _shouldMove = true;
                SoundManager.Instance.PlayMoveSFX();
                
                // increment score only move forward
                if (input == Vector2.up)
                {
                    DataManager.Instance.RowCount++;
                }
            }
        }
    }

    // keyboard input (WAD)
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.isPlaying) return;    // ignore key input when it is not playing
        
        if (context.phase == InputActionPhase.Started)
        {
            // normalize input
            Vector2 input = context.ReadValue<Vector2>();
            HandleMovement(input);
        }
    }

    private void FixedUpdate()
    {
        if (_shouldMove) // if it should move
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.fixedDeltaTime);
            transform.position = newPosition;   // move towards target position

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)   // if close to target position
            {
                // set position to target position and transit to a state should not move
                transform.position = _targetPosition;
                _shouldMove = false;
            }
        }
    }
}