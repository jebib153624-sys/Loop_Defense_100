using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SmoothFill : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float duration = 3.5f;
    [SerializeField, Range(0f, 1f)] private float startNormalized = 0f; // 0이면 완전 빈칸 시작

    private Coroutine co;

    private void OnEnable()
    {
        StartFill();
    }

    public void StartFill()
    {
        if (slider == null) slider = GetComponent<Slider>();

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(FillRoutine());
    }

    private IEnumerator FillRoutine()
    {
        // 다른 Start()에서 slider 값을 바꾸는 경우를 피하려고 1프레임 대기
        yield return null;

        slider.interactable = false;
        slider.wholeNumbers = false;

        float min = slider.minValue;
        float max = slider.maxValue;

        float start = Mathf.Lerp(min, max, startNormalized);
        slider.SetValueWithoutNotify(start);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            p = p * p * (3f - 2f * p); // SmoothStep
            slider.SetValueWithoutNotify(Mathf.Lerp(start, max, p));
            yield return null;
        }

        slider.SetValueWithoutNotify(max);
        co = null;
    }

    private void Update()
    {
        if(slider.value == 1)
        {
            SceneManager.LoadScene(1);
        }
    }
}
