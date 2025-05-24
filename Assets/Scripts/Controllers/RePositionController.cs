using System.Collections.Generic;
using UnityEngine;

public class RePositionController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private float roadSegmentLength = 33f; // length between roads
    
    private List<GameObject> _activeRoads = new List<GameObject>();
    
    private void Start()
    {
        // instantiate and batch two roads not overlapping
        Vector3 firstPosition = player.position;
        Vector3 secondPosition = player.position + Vector3.forward * roadSegmentLength;
        _activeRoads.Add(Instantiate(roadPrefab, firstPosition, Quaternion.identity));
        _activeRoads.Add(Instantiate(roadPrefab, secondPosition, Quaternion.identity));
    }

    private void Update()
    {
        // if player cross single road completely
        if (player.position.z - _activeRoads[0].transform.position.z >= roadSegmentLength)
        {
            RepositionRoadSegment();    // reposition passed road
        }
    }

    private void RepositionRoadSegment()
    {
        // remove first element of active road list (passed road)
        GameObject passedSegment = _activeRoads[0];
        passedSegment.SetActive(false);
        _activeRoads.RemoveAt(0);

        // get new position based on current active road(road that player is passing)
        Vector3 newPosition = _activeRoads[0].transform.position + Vector3.forward * roadSegmentLength;
        passedSegment.transform.position = newPosition;

        // reposition by adding active road list
        _activeRoads.Add(passedSegment);
        passedSegment.SetActive(true);
    }
}