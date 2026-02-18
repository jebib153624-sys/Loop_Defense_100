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

  
    private Transform attackTarget = null;   // 공격 대상
    private EnemySpawner enemySpawner;        // 게임에 존재하는 적 정보 획득용

    private TowerType towerType;
    public SpawnPosition spawnPosition;
    //private Gold gold;

    public TowerVisual towerVisual; // 타워의 시각적 효과를 담당하는 컴포넌트 참조
    public TowerMover towerMover; // 타워의 이동 상태를 확인하기 위한 컴포넌트 참조

    public GameObject iceRange;
    private void Awake()
    {
        spawnPoint = this.transform;
        towerType = GetComponent<TowerType>();
        towerVisual = GetComponent<TowerVisual>();
        towerMover = GetComponent<TowerMover>();
    }

   
    public bool prevClicking;

    void Update()
    {
        if (towerMover.IsDragging != prevClicking)
        {
            iceRange.SetActive(towerMover.IsDragging);
            prevClicking = towerMover.IsDragging;
        }
    }
    public void Setup(EnemySpawner enemySpawner, SpawnPosition spawnPosition)
    {
        this.enemySpawner = enemySpawner;
        this.spawnPosition = spawnPosition;
        //this.gold = gold;
        // 최초 상태를 WeaponState.SearchTarget으로 설정
        if (towerType.towerType == TowerTypes.WarriorTower)
        {
            iceRange.SetActive(false);
            StartCoroutine(AttackTowerRoutine());

        }
        else
        {
            iceRange.SetActive(false);
            StartCoroutine(SlowTowerRoutine());
        }
    }
    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Bullet>().Setup(attackTarget, towerType.states.damage);
    }
    private void AttackTower()
    {
   
        List<EnemyHP> targets = new List<EnemyHP>();

        for (int i = 0; i < enemySpawner.EnemyList.Count; ++i)
        {
            Enemy enemy = enemySpawner.EnemyList[i];
            if (enemy == null) continue;

            float d = Vector3.Distance(enemy.transform.position, transform.position); // 타워와 적 사이의 거리 계산
            if (d < towerType.states.Range)// 적이 타워의 공격 범위 내에 있는지 확인
            {
                EnemyHP hp = enemy.GetComponent<EnemyHP>();//적의 HP 컴포넌트 가져오기
                if (hp != null) targets.Add(hp); // 적의 HP 컴포넌트가 존재하면 리스트에 추가
            }
        }

        if (targets.Count == 0) return;

        // Attack_Hit 이벤트 시점에 동시에 데미지
        towerVisual.PlayAttackOnce( () =>
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                EnemyHP hp = targets[i];
                if (hp == null) continue;
                hp.TakeDamage(towerType.states.damage);
            }
        });

        if (towerVisual == null || towerVisual.IsAttackPlaying) return;
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
                enemy.currentSpeed = enemy.moveSpeed * (1f - towerType.states.slow);
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
    var mover = GetComponent<TowerMover>();

    while (true)
    {
        if (mover.IsDragging)
        {
            yield return null;
            continue;
        }

        SlowTower();
        yield return new WaitForSeconds(0.2f);
    }
}
    IEnumerator AttackTowerRoutine()
    {
        var mover = GetComponent<TowerMover>();

        while (true)
        {
            if (mover.IsDragging)
            {
                yield return null;
                continue;
            }

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
