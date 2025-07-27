
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance; 
    [SerializeField] private int width = 9;
    [SerializeField] private int height = 9;
    
    [SerializeField] private GameObject blankTilePrefab;
    [SerializeField] private Transform layoutGroupTransform;
    [SerializeField] private TextMeshProUGUI addNumberTimesText;
    [SerializeField] private TextMeshProUGUI stageText;

    private Generate generateStage;
    private LoseCheck loseCheck;
    private TileObject[] tileObjects;
    private Dictionary<Vector2, TileObject> tileObjectDict;
    private int currentStage = 1;

    private int lastRow;
    private int numRow = 3;

    private int addNumberTimes = 0;
    
    private int startAddNumberTimes = 6;

    private Vector2 endPos;

    public event System.Action OnLoseGame;
    private void Awake()
    {
        Instance = this;
        tileObjectDict = new Dictionary<Vector2, TileObject>();
        generateStage = GetComponent<Generate>();

        loseCheck = GetComponent<LoseCheck>();
        
    }
    private void Start()
    {
        CreateBlankGrid();
        RandomTile();
        generateStage.CreateNewStage();

        ResetAddNumberTimes();
        stageText.text = "Stage: 1";

        //RandomTile();
    }

    private void CreateBlankGrid()
    {
        tileObjects = new TileObject[width * height];

        for (int i = 1; i <= height; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                GameObject tileGO = Instantiate(blankTilePrefab, layoutGroupTransform);
                Vector2 key = new Vector2(i, j);

                tileObjectDict[key] = tileGO.GetComponent<TileObject>();
            }
        }

    }

    public TileObject GetTileObject(int x, int y)
    {
        //return tileObjects[Get1ArrayIndex(x, y)];
        Vector2 key = new Vector2(x, y);
        return tileObjectDict[key];
    }

    public int Get1ArrayIndex(int x, int y) => (x - 1) * width + y - 1;

    private void RandomTile()
    {
        for (int i = 1; i <= 3; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                TileObject tileObject = GetTileObject(i, j);
                int randomValue = Random.Range(1, 3);
                //randomValue = i;
                tileObject.Initialize(i, j, randomValue, false);
            }
        }
        lastRow = 3;
        endPos = new Vector2(3, 9);

    }

    public bool IsTileDisable(int x, int y)
    {
        Tile tile = GetTileObject(x, y).GetTile();
        if (tile != null) return tile.IsDisable();
        else return true;


    }

    public bool IsTileDisable(int oneArrayIndex)
    {
        return tileObjects[oneArrayIndex].GetTile().IsDisable();
    }

    public void CheckRowBlank(int row)
    {
        for (int i = 1; i <= width; i++)
        {
            if (!IsTileDisable(row, i)) return;
        }

        DeleteRow(row);
    }

    public void DeleteRow(int row)
    {
        if (row > lastRow) return;
        for (int i = row; i < lastRow; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                Vector2 key = new Vector2(i, j);

                Vector2 belowKey = new Vector2(i + 1, j);
                if (!tileObjectDict.ContainsKey(belowKey))
                {
                    tileObjectDict[key].ResetTileObject();
                    continue;
                }

                Tile belowTile = tileObjectDict[belowKey].GetTile();

                
                if (belowTile != null)
                {
                    TileObject belowTileObject = tileObjectDict[belowKey];
                    int val = belowTileObject.GetTile().GetValue();
                    bool isDisable = belowTileObject.GetTile().IsDisable();

                    TileObject tileObject = tileObjectDict[key];
                    tileObject.Initialize(i, j, val, isDisable);

                    belowTileObject.ChangeGem(tileObject);
                }
                else
                {
                    tileObjectDict[key].ResetTileObject();
                }


            }
        }

        for (int i = 1; i <= width; i++)
        {
            Vector2 rewmoveKey = new Vector2(lastRow, i);

            tileObjectDict[rewmoveKey].ResetTileObject();

        }
        SoundManager.Instance.PlayRowClearSoundFX();

        if (row == lastRow) endPos = new Vector2(lastRow - 1, 9);
        else endPos.x -= 1;

        lastRow--;
        if (lastRow == 0) Invoke(nameof(GoToNewStage), 1f);
    }

    public void AddNumber()
    {

        //Debug.Log(endPos);
        int endXPos = (int)endPos.x;
        int endYPos = (int)endPos.y;

        var remainList = GetRemainList(endXPos, endYPos);

        int Y = Mathf.CeilToInt( (remainList.Count +1) / 2);
        int gemCount = Y;
        int gems = 1;

        foreach (var value in remainList) 
        {
            endYPos++;
            if (endYPos > 9)
            {
                endYPos = 1;
                endXPos++;
                if (endXPos > height) AddOneRowOnGrid();
            }
            Vector2 newKey = new Vector2(endXPos, endYPos);
            tileObjectDict[newKey].Initialize(endXPos, endYPos, value, false);

            if (!GemManager.Instance.IsFullGem(gems))
            {
                gemCount--;
                if (GemManager.Instance.IsGemTile() || gemCount <= 0)
                {
                    GetTileObject(endXPos, endYPos).SetGem();
                    gems++;
                    gemCount = Y;
                }
            }
        }
        endPos = new Vector2(endXPos, endYPos);
        lastRow = endXPos;

        //Debug.Log(endPos);
    }

    private List<int> GetRemainList(int endXPos, int endYPos)
    {
        var remainList = new List<int>();
        for (int i = 1; i <= endXPos; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                if (i == endXPos && j > endYPos) return remainList;

                Tile tile = GetTileObject(i, j).GetTile();
                if (tile.IsDisable()) continue;

                remainList.Add(tile.GetValue());
            }
        }

        return remainList;
    }

    private void AddOneRowOnGrid()
    {
        height++;
        for (int i = 1; i <= width; i++)
        {
            GameObject tileGO = Instantiate(blankTilePrefab, layoutGroupTransform);
            Vector2 key = new Vector2(height, i);

            tileObjectDict[key] = tileGO.GetComponent<TileObject>();
        }
    }

    public int GetNumMatchOfStage()
    {
        if (currentStage == 1) return 3;
        else if (currentStage == 2) return 2;
        else return 1;
    }
    private void GoToNewStage()
    {
        currentStage++;
        generateStage.CreateNewStage();

        lastRow = 3;
        endPos = new Vector2(3, 9);

        ResetAddNumberTimes();
        stageText.text = "Stage: " + currentStage.ToString();
    }

    private void ResetAddNumberTimes()
    {
        addNumberTimes = startAddNumberTimes;

        addNumberTimesText.text = addNumberTimes.ToString();
    }

    public void UseAddNumbersButton()
    {
        addNumberTimes--;
        if (addNumberTimes >= 0)
        {
            AddNumber();
            addNumberTimesText.text = addNumberTimes.ToString();
        }
        

        
    }

    public int GetWidth() => width;
    public int GetHeight() => height;

    public void CheckLoseCondition()
    {
        if (addNumberTimes > 0) return;

        if (GemManager.Instance.IsCollectedAllGems()) return;
        if (lastRow == 0) return;

        if (loseCheck.HasMatchs(tileObjectDict, endPos)) return;

        OnLoseGame?.Invoke();
    }

}
