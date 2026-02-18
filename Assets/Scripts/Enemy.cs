using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{

    public Transform[] wayPoint; //각각의 위치 포인트들
    public int nextIndex = 0; // 다음으로 이동할 인덱스 

    public float moveSpeed;
    public float currentSpeed;
    public Vector3 moveDirection;

    private EnemySpawner enemySpawner;

    [SerializeField]
    private int gold; // 적 처치시 얻는 골드량
    [SerializeField]
    private int energy; // 적이 가진 에너지량
    private void Start()
    {
        SetDirectionToNextWaypoint();
        currentSpeed = moveSpeed;
    }
    private void Update()
    {
        transform.position += moveDirection * currentSpeed * Time.deltaTime; //이동방향 얻어오면 방향 바꾸고 이동함
        float distance = Vector3.Distance(transform.position, wayPoint[nextIndex].position); //거리계산 <- 처음이라면 0이니까 

        if (Vector3.Distance(transform.position, wayPoint[nextIndex].position) < 0.02f * moveSpeed)
        {
            //여기가 실행될거임
            transform.position = wayPoint[nextIndex].position;
            MoveToNextWaypoint();
            
        }

    }
    public void Setup(EnemySpawner enemySpawner,Transform[] wayPoints)
    {
        //이 함수는 
        this.wayPoint = wayPoints;
        this.enemySpawner = enemySpawner;

        transform.position = wayPoint[0].position;
        nextIndex++;
    }
    void MoveToNextWaypoint()
    {
        nextIndex++;

        //밑 if문은 마지막 체크포인트를 지나면 처음으로 돌아가는 if문
        if(nextIndex == wayPoint.Length)
        {
            nextIndex = 0;
        }
        SetDirectionToNextWaypoint();
    }

    void SetDirectionToNextWaypoint()
    {
        //다음 채크 포인트의 방향을 얻어오는 함수 
        Vector3 dir = (wayPoint[nextIndex].position - transform.position).normalized;
        moveDirection = dir;
    }

    public void Ondie()
    {
        enemySpawner.DestroyEnemy(this , gold , energy);
    }
}
