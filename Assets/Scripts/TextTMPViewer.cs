using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTowerCount; //  타워 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI textEnemySliderCount; // 적 카운트 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI[] textGold;       // 골드 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI[] textEnergy;       // 에너지 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI[] textCurrentWave;// 현재 웨이브 텍스트 UI

    [SerializeField]
    private PlayerHP playerHP;              // 플레이어의 체력 정보
    [SerializeField]
    private Gold Gold;                      // 골드 정보
    [SerializeField]
    private WavaSystem currentWave;  
    // 골드 정보
    [SerializeField]
    private TowerSpawner towerSpawner;         // 골드 정보

    [SerializeField] 
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Slider slider;

  
    private void Update()
    {
        textTowerCount.text = towerSpawner.towerList.Count.ToString();

        slider.value = (float)(playerHP.currentEnemy) / 100f;
        textGold[0].text = Gold.CurrentGold.ToString();
        textEnergy[0].text = Gold.CurrentEnergy.ToString();
        textGold[1].text = Gold.CurrentGold.ToString();
        textEnergy[1].text = Gold.CurrentEnergy.ToString();
        textGold[2].text = Gold.CurrentGold.ToString();
        textEnergy[2].text = Gold.CurrentEnergy.ToString();
        textCurrentWave[0].text = "WAVE " + (currentWave.currentWavaIndex + 1).ToString();
        textCurrentWave[1].text = (currentWave.currentWavaIndex + 1).ToString();

        // 음수 방지
        float time = Mathf.Max(currentWave.timeBetweenWaves, 0f);

        // 분과 초 계산
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // 00:00 형식으로 출력
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        textEnemySliderCount.text = (playerHP.currentEnemy + " / 100").ToString();
    }
}

