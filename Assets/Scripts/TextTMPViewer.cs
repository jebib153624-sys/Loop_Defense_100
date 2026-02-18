using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textEnemyCount; // 적 카운트 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI textGold;       // 골드 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI textEnergy;       // 에너지 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI textCurrentWave;// 현재 웨이브 텍스트 UI

    [SerializeField]
    private PlayerHP playerHP;              // 플레이어의 체력 정보
    [SerializeField]
    private Gold Gold;                      // 골드 정보
    [SerializeField]
    private WavaSystem currentWave;         // 골드 정보

    [SerializeField] 
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Slider slider;

  
    private void Update()
    {
        textEnemyCount.text = playerHP.currentEnemy.ToString();

        slider.value = playerHP.currentEnemy / 100;
        textGold.text = Gold.CurrentGold.ToString();
        textEnergy.text = Gold.CurrentEnergy.ToString();
        textCurrentWave.text = "Wave : " + (currentWave.currentWavaIndex + 1).ToString();



        currentWave.timeBetweenWaves -= Time.deltaTime;

        // 음수 방지
        float time = Mathf.Max(currentWave.timeBetweenWaves, 0f);

        // 분과 초 계산
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // 00:00 형식으로 출력
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);


    }
}
