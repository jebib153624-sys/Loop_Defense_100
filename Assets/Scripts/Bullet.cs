using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour
{
    private Transform target;
    [SerializeField]
    private float moveSpeed = 2;

    public void Setup(Transform target)
    {
       
        this.target = target;        // 타워가 설정해준 target
    }

    private void Update()
    {
        if (target != null)           // target이 존재하면
        {
            // 발사체를 target의 위치로 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime; //이동방향 얻어오면 방향 바꾸고 이동함
        }
        else                          // 여러 이유로 target이 사라지면
        {
            // 발사체 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;   // 적이 아닌 대상과 부딪히면
        if (collision.transform != target) return;    // 현재 target이 적이 아닐 때

        collision.GetComponent<Enemy>().Ondie();       // 적 사망 함수 호출
        Destroy(gameObject);                           // 발사체 오브젝트 삭제
    }

}
