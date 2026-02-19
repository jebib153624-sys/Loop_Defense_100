using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

//이 클래스는 가장 가까이 있는 적을 탐색하고
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;    // 총알 프리팹

    [SerializeField]
    private Transform spawnPoint; // 발사체 생성 위치

    private Transform attackTarget = null;   // 공격 대상
    private EnemySpawner enemySpawner;       // 게임에 존재하는 적 정보 획득용

    private TowerType towerType;
    public SpawnPosition spawnPosition;

    public TowerVisual towerVisual; // 타워의 시각적 효과를 담당하는 컴포넌트 참조
    public TowerMover towerMover;   // 타워의 이동 상태를 확인하기 위한 컴포넌트 참조

    [Header("Legacy (기존 연결값)")]
    public GameObject iceRange;

    [Header("Effects")]
    [SerializeField] private GameObject floorEffect; // 항상 켜둘 바닥 이펙트
   // [SerializeField] private GameObject rangeEffect; // 드래그/공격 순간만 표시할 범위 이펙트

    private Coroutine freezeRangeRoutine;

    private void Awake()
    {
        spawnPoint = this.transform;
        towerType = GetComponent<TowerType>();
        towerVisual = GetComponent<TowerVisual>();
        towerMover = GetComponent<TowerMover>();

        ResolveEffectReferences();
    }

    private void ResolveEffectReferences()
    {
        // 1) FloorEffect는 이름으로 우선 탐색
        if (floorEffect == null)
        {
            Transform floor = transform.Find("FloorEffect");
            if (floor == null)
            {
                Transform[] all = GetComponentsInChildren<Transform>(true);
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].name == "FloorEffect")
                    {
                        floor = all[i];
                        break;
                    }
                }
            }

            if (floor != null)
                floorEffect = floor.gameObject;
        }


      

    }

   
    public bool prevClicking;

    void Update()
    {
        if (towerMover == null)
            return;

       
    }

    public void Setup(EnemySpawner enemySpawner, SpawnPosition spawnPosition)
    {
        this.enemySpawner = enemySpawner;
        this.spawnPosition = spawnPosition;

        // FloorEffect는 항상 켜둠
        if (floorEffect != null)
            floorEffect.SetActive(true);

      

        StartCoroutine(AttackTowerRoutine());
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

            float d = Vector3.Distance(enemy.transform.position, transform.position);
            if (d < towerType.states.Range)
            {
                EnemyHP hp = enemy.GetComponent<EnemyHP>();
                if (hp != null) targets.Add(hp);
            }
        }

        if (targets.Count == 0) return;

       

        // 애니 이벤트(Attack_Hit / Spell_Trigger) 시점에 데미지 적용
        if (towerVisual != null)
        {
            towerVisual.PlayAttackOnce(() =>
            {
                for (int i = 0; i < targets.Count; ++i)
                {
                    EnemyHP hp = targets[i];
                    if (hp == null) continue;
                    hp.TakeDamage(towerType.states.damage);
                }
            });
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
                enemy.currentSpeed = enemy.moveSpeed * (1f - towerType.states.slow);
            }
            else
            {
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
        Gizmos.DrawWireSphere(transform.position, towerType.states.Range);
    }
}

