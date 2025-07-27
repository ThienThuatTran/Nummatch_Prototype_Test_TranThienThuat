using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Generate : MonoBehaviour
{
    private Dictionary<int, int> frequencyDict = new Dictionary<int, int>();
    private List<int> allNumbers = new List<int>();

    private Dictionary<Vector2, int> generateDic = new Dictionary<Vector2, int>();
    private Dictionary<Vector2, int> seedDict = new Dictionary<Vector2, int>();

    private Queue<int> allNumbersQueue = new Queue<int>();
    private Queue<int> tempQueue = new Queue<int>();

    private int rowNum = 3;
    private int colNum = 9;

    private bool isResetGenerate = false;

    private List<Vector2> gemPositions = new List<Vector2>();


    private void SetFrequencyDict()
    {
        frequencyDict[1] = 3;
        frequencyDict[2] = 3;
        frequencyDict[3] = 3;
        frequencyDict[4] = 2;
        frequencyDict[5] = 4;
        frequencyDict[6] = 4;
        frequencyDict[7] = 3;
        frequencyDict[8] = 2;
        frequencyDict[9] = 3;
    }

    private void Start()
    {
        //CreateNewStage();
    }


    private void Seed(int numMatch)
    {
        for (int i = 0; i < numMatch; i++)
        {
            Vector2 key = GetKeyOfRandomSeed();
            int val = generateDic[key];

            Vector2 matchKey = GetMatchKey(key);
            int matchVal = GetMatchValue(val);

            generateDic[matchKey] = matchVal;
            //Debug.Log(val + " " + matchVal);
            
            seedDict[key] = val;
            seedDict[matchKey] = matchVal;
        }
    }

    private Vector2 GetKeyOfRandomSeed()
    {
        int r1 = Random.Range(1, rowNum + 1);
        int c1 = Random.Range(1, colNum + 1);
        Vector2 key1 = new Vector2(r1, c1);

        while (generateDic.ContainsKey(key1))
        {
            r1 = Random.Range(1, rowNum + 1);
            c1 = Random.Range(1, colNum + 1);
            key1 = new Vector2(r1, c1);

        }


        int val1 = Random.Range(1, 10);

        while (frequencyDict[val1] <= 1)
        {
            val1 = Random.Range(1, 10);
        }

        frequencyDict[val1]--;

        generateDic[key1] = val1;

        return key1;
    }

    private Vector2 GetMatchKey(Vector2 key1)
    {
        int x = (int)key1.x;
        int y = (int)key1.y;

        var list = new List<Vector2>();

        list.Add(new Vector2(x - 1, y));
        list.Add(new Vector2(x - 1, y + 1));
        list.Add(new Vector2(x , y + 1));
        list.Add(new Vector2(x + 1, y + 1));

        list.Add(new Vector2(x + 1, y));
        list.Add(new Vector2(x + 1, y - 1));
        list.Add(new Vector2(x, y - 1));
        list.Add(new Vector2(x - 1, y - 1));

        var validList = new List<Vector2>();

        foreach (var item in list)
        {
            if ( (item.x > 0 && item.x <= 3) && (item.y >0 && item.y <= 9))
            {
                validList.Add(item);
            }
        }

        Vector2 matchKey = validList.OrderBy(x => Random.value).First();
        while (generateDic.ContainsKey(matchKey))
        {
            matchKey = validList.OrderBy(x => Random.value).First();
        }

        return matchKey;
    }

    private int GetMatchValue(int val1)
    {
        int[] array = new int[] { val1, 10 - val1 };
        int index = Random.Range(0, 2);

        while (frequencyDict[array[index]] <= 0)
        {
            index = Random.Range(0, 2);
        }

        frequencyDict[array[index]]--;

        return array[index];
    }

    public void GenerateNumbers()
    {
        SetAllNumbersQueue();

        int Y = Mathf.CeilToInt( (27 + 1) / 2);
        int gemCount = Y;
        int gems = 0;
        gemPositions.Clear();
        for (int i = 1; i <= rowNum; i++)
        {
            for (int j = 1; j <= colNum; j++)
            {
                bool isGem = false;
                Vector2 key = new Vector2(i, j);
                if (!GemManager.Instance.IsFullGem(gems))
                {
                    gemCount--;
                    if (GemManager.Instance.IsGemTile() || gemCount <= 0)
                    {
                        gemPositions.Add(key);
                        gems++;
                        gemCount = Y;
                        isGem = true;
                    }
                }

                if (generateDic.ContainsKey(key))
                {
                    if (isGem)
                    {
                        isResetGenerate = true;
                        ResetNumbers();
                        return;
                    }
                    continue;
                }

                if (allNumbersQueue.Count == 0) ResetAllNumbersQueue();
                int value = allNumbersQueue.Dequeue();

                int count = 0;

                while (IsMatch(key, value))
                {
                    
                    if (count > 27 || isGem)
                    {
                        isResetGenerate = true;
                        ResetNumbers();
                        return;
                    }
                    count++;

                    tempQueue.Enqueue(value);
                    if (allNumbersQueue.Count == 0) ResetAllNumbersQueue();

                    value = allNumbersQueue.Dequeue();
                }

                generateDic[key] = value;
            }
        }
        
    }

    private void SetAllNumbersExceptSeed()
    {
        

        foreach (var item in frequencyDict)
        {
            for (int i = 0; i < item.Value; i++)
            {
                allNumbers.Add(item.Key);
                //Debug.Log(item.Key);
            }
        }

        

        
    }

    private void SetAllNumbersQueue()
    {
        allNumbersQueue.Clear();
        tempQueue.Clear();
        allNumbers = allNumbers.OrderBy(x => Random.value).ToList();
        foreach (int num in allNumbers)
        {
            //Debug.Log(num);
            allNumbersQueue.Enqueue(num);
        }

        //Debug.Log(allNumbersQueue.Count);
    }

    private bool IsMatch(Vector2 position, int expectedValue)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        List<Vector2> allPosition = new List<Vector2>();
        allPosition.Add(new Vector2(x - 1, y));
        allPosition.Add(new Vector2(x - 1, y + 1));
        allPosition.Add(new Vector2(x, y + 1));
        allPosition.Add(new Vector2(x + 1, y + 1));

        allPosition.Add(new Vector2(x + 1, y));
        allPosition.Add(new Vector2(x + 1, y - 1));
        allPosition.Add(new Vector2(x, y - 1));
        allPosition.Add(new Vector2(x - 1, y - 1));



        foreach (var pos in allPosition)
        {
            if (!generateDic.ContainsKey(pos)) continue;

            int checkValue = generateDic[pos];
            if (checkValue == expectedValue || checkValue + expectedValue == 10) return true;
        }

        return false;
    }

    private void ResetAllNumbersQueue()
    {
        allNumbersQueue.Clear();
        //Debug.Log(tempQueue.Count);
        while (tempQueue.Count > 0)
        {
            
            int num = tempQueue.Dequeue();
            
            allNumbersQueue.Enqueue(num);
        }
    }

    private void ResetNumbers()
    {
        generateDic.Clear();

        foreach (var item in seedDict)
        {
            generateDic[item.Key] = item.Value;
        }
    }

    public void CreateNewStage()
    {
        int numMatch = GridManager.Instance.GetNumMatchOfStage();

        generateDic.Clear();
        allNumbers.Clear();
        allNumbersQueue.Clear();
        seedDict.Clear();
        frequencyDict.Clear();

        SetFrequencyDict();

        Seed(numMatch);
        SetAllNumbersExceptSeed();

        do
        {
            isResetGenerate = false;
            GenerateNumbers();
        } while (isResetGenerate);

        //Debug.Log(generateDic.Count);

        foreach (var item in generateDic)
        {
            int x = (int)item.Key.x;
            int y = (int)item.Key.y;
            GridManager.Instance.GetTileObject(x, y).Initialize(x, y, item.Value, false);
        }
        SetGems();
    }

    private void SetGems()
    {
        foreach (var item in gemPositions)
        {
            int x = (int)item.x;
            int y = (int)item.y;

            GridManager.Instance.GetTileObject(x, y).SetGem();
        }
    }

    

}
