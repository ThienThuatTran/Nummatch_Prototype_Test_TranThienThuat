
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public static GemManager Instance;
    [SerializeField] private TextMeshProUGUI[] gemNumTexts;

    private int X = 7;
    private int Z = 2;

    private int[] gemNumbers = new int[] { 5, 5 };

    private int[] gemCollecteds = new int[] { 0, 0 };

    public event System.EventHandler OnAllGemColleted;
    private void Awake()
    {
        Instance = this;

        UpdateGemText(1);
        UpdateGemText(2);
        
    }
    public void UpdateGemCollected(int index)
    {
        gemCollecteds[index - 1] += 1;
        UpdateGemText(index);

        CheckWinCondition();
    }

    public void UpdateGemText(int index)
    {
        int remainGems = gemNumbers[index - 1] - gemCollecteds[index - 1];
        
        if ( remainGems <0)
        {
            remainGems = 0;
        }
        gemNumTexts[index -1].text = remainGems.ToString();
    }

    private void CheckWinCondition()
    {

        if (IsCollectedAllGems())
        {
            //Debug.Log("you win");
            OnAllGemColleted?.Invoke(this, System.EventArgs.Empty);
        }

    }

    public bool IsCollectedAllGems()
    {
        for (int i = 0; i < gemNumbers.Length; i++)
        {
            if (gemCollecteds[i] < gemNumbers[i]) return false;
        }

        return true;
    }

    public int GetX() => X;

    public int GetZ() => Z;

    public bool IsGemTile()
    {
        int randNumber = Random.Range(1, 101);

        if (randNumber <= X) return true;
        else return false;
    }

    public bool IsFullGem(int gems) => gems == Z; 

    public int GetValidIndex()
    {
        List<int> validList = new List<int>();
        for (int i = 0; i <gemNumbers.Length; i++)
        {
            if (gemCollecteds[i] < gemNumbers[i]) validList.Add(i);
        }
        int randomIndex = Random.Range(0, validList.Count);
        return validList[randomIndex];
    }

}
