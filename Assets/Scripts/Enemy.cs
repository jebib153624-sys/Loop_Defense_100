using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    /*private int wayPointCount;                 // 이동 경로 개수
    private Transform[] wayPoints;              // 이동 경로 정보
    private int currentIndex = 0;               // 현재 목적지점 인덱스
    private Movement2D movement2D;               // 오브젝트 이동 제어

    public void Setup(Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();

        // 적 이동 경로 WayPoints 정보 설정
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        // 적의 위치를 첫번째 wayPoint 위치로 설정
        transform.position = wayPoints[currentIndex].position;

        // 적 이동/목표지점 설정 코루틴 함수 시작
        StartCoroutine("OnMove");
    }
    private IEnumerator OnMove()
    {
        // 다음 이동 방향 설정
        NextMoveTo();

        while (true)
        {
            // 적의 현재위치와 목표위치의 거리가 0.02 * movement2D.MoveSpeed보다 작을 때 if 조건문 실행
            // Tip. movement2D.MoveSpeed를 곱해주는 이유는 속도가 빠르면 한 프레임에 0.02보다 크게 움직이기 때문에
            // if 조건문에 걸리지 않고 경로를 탈주하는 오브젝트가 발생할 수 있다.
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                // 다음 이동 방향 설정
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        // 아직 이동할 wayPoints가 남아있다면
        if (currentIndex < wayPointCount - 1)
        {
            // 적의 위치를 정확하게 목표 위치로 설정
            transform.position = wayPoints[currentIndex].position;

            // 이동 방향 설정 => 다음 목표지점(wayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        // 현재 위치가 마지막 wayPoints이면
        else
        {
            // 적 오브젝트 삭제
            Destroy(gameObject);
        }
    } */


    public Transform[] wayPoint; //각각의 위치 포인트들
    public int nextIndex = 0; // 다음으로 이동할 인덱스 

    public float moveSpeed;
    public Vector3 moveDirection;
    private void Start()
    {
        SetDirectionToNextWaypoint();
    }
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime; //이동방향 얻어오면 방향 바꾸고 이동함
        float distance = Vector3.Distance(transform.position, wayPoint[nextIndex].position); //거리계산 <- 처음이라면 0이니까 

        if (distance <= 0.2f)
        {
            //여기가 실행될거임
            MoveToNextWaypoint();
        }

    }
    public void Setup(Transform[] wayPoints)
    {
        //이 함수는 
        this.wayPoint = wayPoints;

        transform.position = wayPoint[0].position;
        nextIndex++;
    }
    void MoveToNextWaypoint()
    {
        nextIndex++;
        if(nextIndex == wayPoint.Length)
        {
            nextIndex = 0;
        }
        SetDirectionToNextWaypoint();
    }

    void SetDirectionToNextWaypoint()
    {
        Vector3 dir = (wayPoint[nextIndex].position - transform.position).normalized;
        moveDirection = dir;
    }

}
