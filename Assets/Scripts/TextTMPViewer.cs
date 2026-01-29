using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textEnemyCount; // 적 카운트 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI textGold;       // 골드 텍스트 UI
    [SerializeField]
    private TextMeshProUGUI textCurrentWave;// 현재 웨이브 텍스트 UI

    [SerializeField]
    private PlayerHP playerHP;              // 플레이어의 체력 정보
    [SerializeField]
    private Gold Gold;                      // 골드 정보
    [SerializeField]
    private WavaSystem currentWave;                      // 골드 정보
    private void Update()
    {
        textEnemyCount.text = playerHP.currentEnemy.ToString();
        textGold.text = Gold.CurrentGold.ToString();
        textCurrentWave.text = (currentWave.currentWavaIndex + 1).ToString();
    }
}
