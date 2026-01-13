using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     // 적 프리팹

    [SerializeField]
    private float spawnTime;            // 적 생성 주기

    [SerializeField]
    private Transform[] wayPoints;      // 현재 스테이지의 이동 경로

    [SerializeField]
    private float enemySpeed; //적 속도 스폰 할떄 마다 적한테 넘겨줌 

    private List<Enemy> enemyList; //맵에 있는 적들의 정보 리스트 

    public List<Enemy> EnemyList => enemyList; // 프로퍼티 근데 set기능은 없음 lead onry

    [SerializeField]
    private GameObject enemyHPSliderPrefab;   // 적 체력을 나타내는 Slider UI 프리팹

    [SerializeField]
    private Transform canvasTransform;        // UI를 표현하는 Canvas 오브젝트의 Transform

    [SerializeField]
    private Gold gold; //골드 스크립트

    [SerializeField]
    private PlayerHP playerHP; //플레이어 hp 스크립트
    private void Awake()
    {
        //적 리스트 메모리 할당
        enemyList = new List<Enemy>();
        // 적 생성 코루틴 함수 호출
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);     // 적 오브젝트 생성
            Enemy enemy = clone.GetComponent<Enemy>();       // 방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(this , wayPoints);                           // wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy);
            enemy.moveSpeed = enemySpeed;

            SpawnEnemyHPSlider(clone);   // 적 체력을 나타내는 Slider UI 생성 및 설정

            playerHP.EnemyCountUpdate(); //적이 죽을때마다 플레이어 hp스크립트에 적 카운트 업데이트 요청
            yield return new WaitForSeconds(spawnTime);       // spawnTime 시간 동안 대기
        }
    }

    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        // 적 체력을 나타내는 Slider UI 생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);

        // Slider UI 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        // Tip. UI는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다
        sliderClone.transform.SetParent(canvasTransform);

        // 계층 수정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        sliderClone.transform.localScale = Vector3.one;

        // Slider UI가 쫓아다닐 대상 원인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);

        // Slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }


    public void DestroyEnemy(Enemy enemy , int gold)
    {
        this.gold.CurrentGold += gold; //적이 죽을때마다 골드 스크립트에 골드 추가
        enemyList.Remove(enemy); //적 리스트에서 제거
        Destroy(enemy.gameObject); //적 오브젝트 파괴
    }
}
