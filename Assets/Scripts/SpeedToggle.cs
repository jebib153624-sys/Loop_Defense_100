using UnityEngine;
using UnityEngine.UI;

public class SpeedToggle : MonoBehaviour
{
    public Sprite speed1xSprite;   // 1배속 이미지
    public Sprite speed2xSprite;   // 2배속 이미지

    private bool isDoubleSpeed = false;
    public Image buttonImage;

    private void Awake()
    {
        ApplySpeed(); // 초기 상태 적용
    }

    public void ToggleSpeed()
    {
        isDoubleSpeed = !isDoubleSpeed;
        AudioManager.instance.PlaySfx(10);
        ApplySpeed();
    }

    private void ApplySpeed()
    {
        if (isDoubleSpeed)
        {
            Time.timeScale = 2f;
            buttonImage.sprite = speed2xSprite;
        }
        else
        {
            Time.timeScale = 1f;
            buttonImage.sprite = speed1xSprite;
        }
    }
}