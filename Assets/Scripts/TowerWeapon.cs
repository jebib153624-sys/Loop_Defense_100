using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponState {SearchTarget = 0 , AttackToTarget }

//이 클래스는 가장 가까이 있는 적을 탐색하고  
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;    // 총알 프리팹

    [SerializeField]
    private Transform spawnPoint;            // 발사체 생성 위치

    /*[SerializeField]
    private float attackRate = 0.5f;         // 공격 속도

    [SerializeField]
    private float attackRange = 2.0f;         // 공격 범위
    [SerializeField]
    private int attackDamage = 1;         // 공격력 */

    private WeaponState weaponState = WeaponState.SearchTarget; // 타워 무기의 상태
    private Transform attackTarget = null;   // 공격 대상
    private EnemySpawner enemySpawner;        // 게임에 존재하는 적 정보 획득용

    private TowerType towerType;
    public SpawnPosition spawnPosition;
    //private Gold gold;


    private void Awake()
    {
        spawnPoint = this.transform;
        towerType = GetComponent<TowerType>();

    }
    public void Setup(EnemySpawner enemySpawner, SpawnPosition spawnPosition)
    {
        this.enemySpawner = enemySpawner;
        this.spawnPosition = spawnPosition;
        //this.gold = gold;
        // 최초 상태를 WeaponState.SearchTarget으로 설정
        if (towerType.towerType == TowerTypes.WarriorTower)
        {
            // ChangeState(WeaponState.SearchTarget);
            StartCoroutine(AttackTowerRoutine());

        }
        else
        {
            StartCoroutine(SlowTowerRoutine());
        }
    }

    public void ChangeState(WeaponState newState)
    {
        // 이전에 재생중이던 상태 종료
        StopCoroutine(weaponState.ToString());  // <- 코루틴 실행하려고 문자열 넣은거 처음보고 뭔 개소린가 싶었네 ㅋㅋ
        // 상태 변경
        weaponState = newState;
        // 새로운 상태 재생
        StartCoroutine(weaponState.ToString());
    }// 아 그니까 이함수는 지금까지 실행중인 코루틴 멈추고 매개변수로 새롭게 가져온 코루틴을 실행해! 라는뜻 

 
    /*private IEnumerator SearchTarget()
    {
        while (true)
        {
            // 제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
            float closestDistSqr = Mathf.Infinity;

            // EnemySpawner의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
            for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(
                    enemySpawner.EnemyList[i].transform.position,
                    transform.position
                );

                // 현재 검사중인 적과의 거리가 공격범위 내에 있고,
                // 현재까지 검사한 적보다 거리가 가까우면
                if (distance <= towerType.states.Range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }

            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }

            yield return null;
        }
    }
    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            // 1. target이 있는지 검사 (다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제 등)
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 2. target이 공격 범위 안에 있는지 검사 (공격 범위를 벗어나면 새로운 적 탐색)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > towerType.states.Range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            // 3. attackRate 시간만큼 대기
            yield return new WaitForSeconds(towerType.states.rate);

            // 4. 공격 (발사체 생성)
            SpawnProjectile();
        }
    }*/
    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Bullet>().Setup(attackTarget, towerType.states.damage);
    }
    private void AttackTower()
    {
        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            Enemy enemy = enemySpawner.EnemyList[i];
            if (enemy == null) continue;

            float distance = Vector3.Distance(
                enemy.transform.position,
                transform.position
            );

            if (distance < towerType.states.Range)
            {
                enemySpawner.EnemyList[i].GetComponent<EnemyHP>().TakeDamage(towerType.states.damage);
            }
          
        }
    }
    private void SlowTower()
    {
        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            Enemy enemy = enemySpawner.EnemyList[i];
            if (enemy == null) continue;

            float distance = Vector3.Distance(
                enemy.transform.position,
                transform.position
            );

            if (distance < towerType.states.Range)
            {
                // 20% 슬로우라면 0.8배 속도
                enemy.currentSpeed =
                    enemy.moveSpeed * (1f - towerType.states.slow);
            }
            else
            {
                // 범위 밖이면 원래 속도로 복구
                enemy.currentSpeed = enemy.moveSpeed;
            }
        }
    }

    IEnumerator SlowTowerRoutine()
    {
        while (true)
        {
            SlowTower();
            Debug.Log("슬로우 타워 작동 중");
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator AttackTowerRoutine()
    {
        while (true)
        {
            AttackTower();
            
            yield return new WaitForSeconds(towerType.states.rate);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position , towerType.states.Range);
    }
}
