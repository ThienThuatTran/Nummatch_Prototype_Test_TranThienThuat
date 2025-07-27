using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    //index 1
    private int gemIndex;

    public void Initialize(int index)
    {
        gemIndex = index;
    }

    public int GetGemIndex()
    {
        return gemIndex;
    }
    public void CollectGem()
    {

        GemManager.Instance.UpdateGemCollected(gemIndex);


        
    }
}
