using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    private int isBuildTower;

    public int IsBuildTower
    {
        get
        {
            return isBuildTower;
        }
        set
        {
            isBuildTower = value;
        }
    }
}
