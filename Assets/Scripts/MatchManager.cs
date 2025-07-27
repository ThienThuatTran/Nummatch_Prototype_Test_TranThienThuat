using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public bool CanMatchValue(TileObject tileObjectA, TileObject tileObjectB)
    {
        Tile tileA = tileObjectA.GetTile();
        Tile tileB = tileObjectB.GetTile();

        int aVal = tileA.GetValue();
        int bVal = tileB.GetValue();

        if (aVal == bVal || aVal + bVal == 10)
        {
            return true;
        }
        return false;
    }
    public bool CanMatchPosition(TileObject tileObjectA, TileObject tileObjectB)
    {
        Tile tileA = tileObjectA.GetTile();
        Tile tileB = tileObjectB.GetTile();

        int aVal = tileA.GetValue();
        int bVal = tileB.GetValue();

        if (tileA.GetX() == tileB.GetX())
        {
            if (CanMatchOnHorizontal(tileA, tileB)) return true;
        }

        if (tileA.GetY() == tileB.GetY() )
        {
            if (CanMatchOnVertical(tileA, tileB)) return true;
        }
        if (Math.Abs(tileA.GetX() - tileB.GetX()) == Math.Abs(tileA.GetY() - tileB.GetY()) ) 
        {
            if (CanMatchOnDiagonal(tileA, tileB)) return true;
        }

        if (CanMatchOnStraightLine(tileA, tileB)) return true;

        return false;
    }
    private bool CanMatchOnVertical(Tile tileA, Tile tileB)
    {
        int step = 1;
    
        if (tileA.GetX() > tileB.GetX()) step = -1;
        int xTemp = tileA.GetX() + step;
        int y = tileA.GetY();
        while (xTemp != tileB.GetX())
        {
            if (!GridManager.Instance.IsTileDisable(xTemp, y)) return false;
            xTemp += step;
        }
        return true;
    }

    private bool CanMatchOnHorizontal(Tile tileA, Tile tileB)
    {
        int step = 1;

        if (tileA.GetY() > tileB.GetY()) step = -1;
        int yTemp = tileA.GetY() + step;
        int x = tileA.GetX();
        while (yTemp != tileB.GetY())
        {
            if (!GridManager.Instance.IsTileDisable(x, yTemp)) return false;
            yTemp += step;
        }
        return true;
    }

    private bool CanMatchOnDiagonal(Tile tileA, Tile tileB)
    {
        int xStep = 1;
        int yStep = 1;
        
        if (tileA.GetX() > tileB.GetX()) xStep = -1;
        if (tileA.GetY() > tileB.GetY()) yStep = -1;

        int xTemp = tileA.GetX() + xStep;
        int yTemp = tileA.GetY() + yStep;

        while (xTemp != tileB.GetX() && yTemp != tileB.GetY())
        {
            if (!GridManager.Instance.IsTileDisable(xTemp, yTemp)) return false;

            xTemp += xStep;
            yTemp += yStep;
        }
        return true;
    }

    private bool CanMatchOnStraightLine(Tile tileA, Tile tileB)
    {
        
        int startRow = tileA.GetX();
        int endRow = tileB.GetX();
        
        int width = GridManager.Instance.GetWidth();

        int startColumn = tileA.GetY();
        int endColumn = tileB.GetY();

        if (startRow > endRow)
        {
            startRow = tileB.GetX();
            endRow = tileA.GetX();

            startColumn = tileB.GetY();
            endColumn = tileA.GetY();
        }
        if (endRow - startRow > 1) return false;

        for (int i = startColumn + 1; i <= width; i++)
        {
            if (!GridManager.Instance.IsTileDisable(startRow, i)) return false;
        }

        for (int i = 1; i < endColumn; i++)
        {
            if (!GridManager.Instance.IsTileDisable(endRow, i)) return false;
        }
        return true;

    }
}
