using UnityEngine;
using UnityEngine.UI;

public class ui : MonoBehaviour
{
    [Header("설정창")]
    private bool settingState = false;
    public GameObject panelSetting;

    [Header("업그레이드창")]
    private bool upgradePanelState = false;
    public GameObject panelUpgrade;

    [Header("엘리트 보스 소환창")]
    private bool elitePanelState = false;
    public GameObject panelUElite;

    [Header("버튼")]
    public Sprite buttonOn;
    public Sprite buttonOff;

    private bool ButtonState = false;

    [Header("엔딩 창")]
    public GameObject panelEnding;

    public PlayerHP playerHP;

    private void Start()
    {
        if (panelSetting != null) panelSetting.SetActive(settingState);
        if (panelUpgrade != null) panelUpgrade.SetActive(upgradePanelState);
        if (panelUElite != null) panelUElite.SetActive(elitePanelState);
        if (panelUElite != null) panelEnding.SetActive(false);

    }

    private void Update()
    {
        if (playerHP == null)
            return;

        if(playerHP.currentEnemy >= 100f)
        {
            panelEnding.SetActive(true);
        }
    }

    public void SettingButton()
    {
        AudioManager.instance.PlaySfx(10);
        if (panelSetting == null) return;

        settingState = !settingState;
        panelSetting.SetActive(settingState);

        Time.timeScale = settingState ? 0f : 1f;
    }


    public void ToggleSettingButton(Button clickedButton)
    {
        AudioManager.instance.PlaySfx(10);
        if (clickedButton == null || clickedButton.image == null) return;

        ButtonState = !ButtonState;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayBgm(ButtonState);
        }

        clickedButton.image.sprite = ButtonState ? buttonOn : buttonOff;
    }

    public void UpgradePanelState()
    {
        AudioManager.instance.PlaySfx(10);
        if (panelUpgrade == null) return;

        upgradePanelState = !upgradePanelState;
        panelUpgrade.SetActive(upgradePanelState);
    }

    public void ElitePanelState()
    {
        AudioManager.instance.PlaySfx(10);
        if (panelUElite == null) return;

        elitePanelState = !elitePanelState;
        panelUElite.SetActive(elitePanelState);
    }
}
