using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField]
    private int currentGold = 0;

    [SerializeField]
    private int currentEnergy = 0;

    public int CurrentGold
    {
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }

    public int CurrentEnergy
    {
        set => currentEnergy = Mathf.Max(0, value);
        get => currentEnergy;
    }
}
