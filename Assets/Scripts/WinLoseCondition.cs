using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseCondition : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    private void Start()
    {
        GemManager.Instance.OnAllGemColleted += GemManager_OnAllGemColleted;

        GridManager.Instance.OnLoseGame += GridManager_OnLoseGame;
    }

    private void GridManager_OnLoseGame()
    {
        losePanel.SetActive(true);
    }

    private void GemManager_OnAllGemColleted(object sender, System.EventArgs e)
    {
        winPanel.SetActive(true);
    }

}
