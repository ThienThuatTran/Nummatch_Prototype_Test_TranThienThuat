using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCheck : MonoBehaviour
{
    public bool HasMatchs(Dictionary<Vector2, TileObject> tileObjectDict, Vector2 endPos)
    {
        //Debug.Log(endPos);
        int endXPos = (int)endPos.x;
        int endYPos = (int)endPos.y;
        int height = GridManager.Instance.GetHeight();
        int width = GridManager.Instance.GetWidth();
        for (int i = 1; i <= endXPos; i++)
        {
            for (int j = 1; j <= width; j++)
            {
                Vector2 key = new Vector2(i, j);

                TileObject tileObject = tileObjectDict[key];

                if (tileObject == null) return false;

                Tile tile = tileObject.GetTile();
                if (tile == null) continue;
                else
                {
                    
                    if (tile.IsDisable()) continue;

                    //Debug.Log("-------------------------" + tile.GetValue());

                    if (HasMatchOnVertical(tileObjectDict, tileObject, endXPos)) return true;
                    //Debug.Log("no vertical match");
                    if (HasMatchOnHorizontal(tileObjectDict, tileObject, endXPos)) return true;
                    //Debug.Log("no horizontal match");
                    if (HasMatchOnDiogonal(tileObjectDict, tileObject, endXPos)) return true;
                    //Debug.Log("no diogonal match");

                    if (HasMatchOnStarightLine(tileObjectDict, tileObject, endXPos)) return true;
                    //Debug.Log("no match");

                }


            }
        }
        //Debug.Log("lose");
        return false;
    }

    private bool HasMatchOnVertical(Dictionary<Vector2, TileObject> tileObjectDict, TileObject tileObject, int endXPos)
    {

        int y = tileObject.GetTile().GetY();
        int x = tileObject.GetTile().GetX();
        int currentX = x;

        while (currentX < endXPos)
        {
            currentX++;

            Vector2 currkey = new Vector2(currentX, y);
            if (!tileObjectDict.ContainsKey(currkey)) break;

            TileObject currTileObject = tileObjectDict[currkey];
            Tile currTile = currTileObject.GetTile();
            if (currTile != null)
            {
                if (currTile.IsDisable()) continue;

                if (MatchManager.Instance.CanMatchValue(tileObject, currTileObject)) return true;
                else break;


            }
            else break;
        }

        return false;
    }

    private bool HasMatchOnHorizontal(Dictionary<Vector2, TileObject> tileObjectDict, TileObject tileObject, int endXPos)
    {

        int y = tileObject.GetTile().GetY();
        int x = tileObject.GetTile().GetX();
        int currentY = y;
        while (currentY < GridManager.Instance.GetWidth())
        {
            currentY++;

            Vector2 currkey = new Vector2(x, currentY);
            if (!tileObjectDict.ContainsKey(currkey)) break;

            TileObject currTileObject = tileObjectDict[currkey];
            Tile currTile = currTileObject.GetTile();
            if (currTile != null)
            {
                if (currTile.IsDisable()) continue;

                if (MatchManager.Instance.CanMatchValue(tileObject, currTileObject)) return true;
                else break;
            }
            else break;
        }



        return false;
    }

    private bool HasMatchOnDiogonal(Dictionary<Vector2, TileObject> tileObjectDict, TileObject tileObject, int endXPos)
    {

        int y = tileObject.GetTile().GetY();
        int x = tileObject.GetTile().GetX();

        int currentX = x;
        int currentY = y;
        while (currentY < GridManager.Instance.GetWidth() && currentX < endXPos)
        {
            currentX++;
            currentY++;

            Vector2 currkey = new Vector2(currentX, currentY);
            if (!tileObjectDict.ContainsKey(currkey)) break;

            TileObject currTileObject = tileObjectDict[currkey];
            Tile currTile = currTileObject.GetTile();
            if (currTile != null)
            {
                if (currTile.IsDisable()) continue;

                if (MatchManager.Instance.CanMatchValue(tileObject, currTileObject)) return true;
                else break;
            }
            else break;
        }
        currentX = x;
        currentY = y;

        while (currentY > 1 && currentX < endXPos)
        {
            currentX++;
            currentY--;

            Vector2 currkey = new Vector2(currentX, currentY);
            if (!tileObjectDict.ContainsKey(currkey)) break;

            TileObject currTileObject = tileObjectDict[currkey];
            Tile currTile = currTileObject.GetTile();
            if (currTile != null)
            {
                if (currTile.IsDisable()) continue;

                if (MatchManager.Instance.CanMatchValue(tileObject, currTileObject)) return true;
                else break;
            }
            else break;
        }



        return false;
    }
    private bool HasMatchOnStarightLine(Dictionary<Vector2, TileObject> tileObjectDict, TileObject tileObject, int endXPos)
    {
        int y = tileObject.GetTile().GetY();

        int x = tileObject.GetTile().GetX();

        int currentY = 0;

        for (int i = y + 1; i <= GridManager.Instance.GetWidth(); i++)
        {
            Vector2 currkey = new Vector2(x, i);

            if (!tileObjectDict.ContainsKey(currkey)) return false;

            TileObject currTileObject = tileObjectDict[currkey];
            Tile currTile = currTileObject.GetTile();
            if (currTile != null)
            {
                if (currTile.IsDisable()) continue;
                else return false;
            }
        }

        while (currentY < GridManager.Instance.GetWidth())
        {
            currentY++;

            Vector2 currkey = new Vector2(x + 1, currentY);
            if (!tileObjectDict.ContainsKey(currkey)) break;

            TileObject currTileObject = tileObjectDict[currkey];
            Tile currTile = currTileObject.GetTile();
            if (currTile != null)
            {
                if (currTile.IsDisable()) continue;

                if (MatchManager.Instance.CanMatchValue(tileObject, currTileObject)) return true;
                else return false;


            }
            else break;
        }

        return false;
    }


}
