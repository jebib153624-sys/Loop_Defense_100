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

    public bool TrySpendGold(int amount)
    {
        if (amount <= 0)
            return true;

        if (currentGold < amount)
            return false;

        CurrentGold -= amount;
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentEnergy += 100;
            CurrentGold += 100;
        }
    }
}
