using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularCameraController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private List<GameObject> playerList = new List<GameObject>();  // instantiated player objects list
    [SerializeField] private int numberOfPlayers;
    [SerializeField] private float radius;  // radius of the player circle positioned based on the camera
    [SerializeField] private float playerYOffset;

    [Header("Camera Settings")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool smoothRotation;   // camera smooth rotation application
    [SerializeField] private float camYOffset;

    [Header("Runtime")]
    [SerializeField] private List<GameObject> playerPrefabList = new List<GameObject>();    // player prefab list to instantiate
    // Make playerList by playerPrefabList that sets on Inspector view
    
    public bool IsRotating { get; private set; }    // check camera is rotating
    private Vector3 _centerPosition;    // position to set center

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
        // Instantiate players and set circle formation
        SetPlayersTransform();
        
        // set camera on world space origin and make it look at first player
        mainCamera.transform.position = _centerPosition;
        RotateCameraToPlayer(CharacterManager.Instance.curCharacterIdx, false);
    }

    // players are positioned in a circle around the camera
    private void SetPlayersTransform()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            // calculate angle (divide 360 by player number)
            float angle = -i * 360f / numberOfPlayers;
            float angleInRadians = angle * Mathf.Deg2Rad;
            
            // calculate circle position (based on world space origin)
            Vector3 position = new Vector3(
                Mathf.Cos(angleInRadians) * radius,
                playerYOffset,
                Mathf.Sin(angleInRadians) * radius
            );
            
            // instantiate player
            GameObject player = Instantiate(playerPrefabList[i], position, Quaternion.identity);
            player.name = $"Player_{i + 1}";
            player.GetComponent<Animator>().enabled = false;    // animator disable(for adjust rotation)
            playerList.Add(player);
            
            // rotate towards origin (make it look at origin)
            Vector3 directionToCenter = (_centerPosition - position).normalized;
            player.transform.rotation = Quaternion.LookRotation(directionToCenter);
        }
    }

    // rotate camera towards previous index character
    public void RotateToPrev()
    {
        if (IsRotating || playerPrefabList.Count == 0) return;
        
        CharacterManager.Instance.ChangeToPreviousCharacter();
        RotateCameraToPlayer(CharacterManager.Instance.curCharacterIdx, smoothRotation);
    }
    
    // rotate camera towards next index character
    public void RotateToNext()
    {
        if (IsRotating || playerPrefabList.Count == 0) return;
        
        CharacterManager.Instance.ChangeToNextCharacter();
        RotateCameraToPlayer(CharacterManager.Instance.curCharacterIdx, smoothRotation);
    }

    // rotate camera towards particular index character
    private void RotateCameraToPlayer(int playerIndex, bool animate = true)
    {
        if (playerIndex < 0 || playerIndex >= playerList.Count || mainCamera == null)
            return;

        // position of target player
        Vector3 targetPosition = playerList[playerIndex].transform.position;
        
        // calculate direction world space origin(camera position) to player
        Vector3 direction = (targetPosition - _centerPosition).normalized;
        
        // calculate camera rotation (to make it look at player)
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        if (animate && smoothRotation)  // if apply smooth rotation
        {
            StartCoroutine(SmoothCameraRotation(targetRotation));
        }
        else
        {
            mainCamera.transform.rotation = targetRotation;
            mainCamera.transform.position = new Vector3(0, camYOffset, 0);
        }
    }

    // coroutine apply smooth camera rotation
    IEnumerator SmoothCameraRotation(Quaternion targetRotation)
    {
        IsRotating = true;  // make camera state rotating
        
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < 1f)    // rotate camera slightly
        {
            elapsed += Time.deltaTime * rotationSpeed;
            float t = Mathf.SmoothStep(0, 1, elapsed);
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }
        
        mainCamera.transform.rotation = targetRotation; // set rotation at the end
        IsRotating = false; // make camera state done rotating
    }


    // visualize by using Gizmo
    private void OnDrawGizmos()
    {
        // display the circular formation area around the world space origin
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, radius);
        
        // display world space origin
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Vector3.zero, 0.1f);
        
        // display player position
        for (int i = 0; i < numberOfPlayers; i++)
        {
            float angle = -i * 360f / numberOfPlayers;
            float angleInRadians = angle * Mathf.Deg2Rad;
            
            Vector3 position = new Vector3(
                Mathf.Cos(angleInRadians) * radius,
                playerYOffset,
                Mathf.Sin(angleInRadians) * radius
            );
            
            int currentIdx = Application.isPlaying ? CharacterManager.Instance.curCharacterIdx : 0;
            Gizmos.color = (i == currentIdx) ? Color.green : Color.red;
            Gizmos.DrawSphere(position, 0.2f);
            
            // draw a line between player and world space origin
            if (i == currentIdx)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(Vector3.zero, position);
            }
        }
        
        // display camera position (fixed in world space origin)
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(Vector3.zero, 0.15f);
        
        // display direction of camera
        if (Application.isPlaying && mainCamera != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(Vector3.zero, mainCamera.transform.forward * 2f);
        }
    }

    // update editor in real time
    private void OnValidate()
    {
        if (Application.isPlaying)
            return;
            
        #if UNITY_EDITOR
        UnityEditor.SceneView.RepaintAll();
        #endif
    }
}