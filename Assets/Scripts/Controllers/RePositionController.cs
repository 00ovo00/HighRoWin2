using System.Collections.Generic;
using UnityEngine;

public class RePositionController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private float roadSegmentLength = 33f; // 로드 사이의 간격
    
    private List<GameObject> _activeRoads = new List<GameObject>();  // 활성화된 로드 오브젝트 리스트
    
    private void Start()
    {
        // 2개의 로드가 서로 겹치지 않고 배치되어 생성
        Vector3 firstPosition = player.position;
        Vector3 secondPosition = player.position + Vector3.forward * roadSegmentLength;
        _activeRoads.Add(Instantiate(roadPrefab, firstPosition, Quaternion.identity));
        _activeRoads.Add(Instantiate(roadPrefab, secondPosition, Quaternion.identity));
    }

    private void Update()
    {
        // 플레이어가 하나의 로드를 완전히 지나가면
        if (player.position.z - _activeRoads[0].transform.position.z >= roadSegmentLength)
        {
            RepositionRoadSegment();    // 지나간 로드 재배치
        }
    }

    private void RepositionRoadSegment()
    {
        // 활성화된 로드 리스트에서 첫번째 요소(지나간 로드) 삭제
        GameObject passedSegment = _activeRoads[0];
        passedSegment.SetActive(false);
        _activeRoads.RemoveAt(0);

        // 현재 활성화된 로드(플레이어가 지나가고 있는 로드) 기준으로 새로운 위치 잡기
        Vector3 newPosition = _activeRoads[0].transform.position + Vector3.forward * roadSegmentLength;
        passedSegment.transform.position = newPosition;

        // 활성화된 로드 리스트에 추가하여 재배치
        _activeRoads.Add(passedSegment);
        passedSegment.SetActive(true);
    }
}