using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    [SerializeField] private Sprite[] numberSprites;

    private TileObject selectedTileObject;
    private void Awake()
    {
        Instance = this;
    }
    public Sprite GetNumberSprite(int value)
    {
        return numberSprites[value-1];
    }

    public void SetSelectedTileObject(TileObject tileObject)
    {
        
        if (selectedTileObject != null)
        {
            
            if (tileObject == selectedTileObject)
            {
                selectedTileObject.ResetHighlight();
                selectedTileObject = null;
                return;
            }


            if (MatchManager.Instance.CanMatchValue(tileObject, selectedTileObject))
            {
                if (MatchManager.Instance.CanMatchPosition(tileObject, selectedTileObject))
                {
                    MatchHandle(tileObject, selectedTileObject);
                    
                }
                else
                {
                    selectedTileObject.ResetHighlight();
                    tileObject.ResetHighlight();
                }

                selectedTileObject = null;
                return;
            }

            
            selectedTileObject.ResetHighlight();
        }
        selectedTileObject = tileObject;
    }

    private void MatchHandle(TileObject tileObjectA, TileObject tileObjectB)
    {
        SoundManager.Instance.PlayPairClearSoundFX();

        Tile tileA = tileObjectA.GetTile();
        Tile tileB = tileObjectB.GetTile();
        int xA = tileA.GetX();
        int xB = tileB.GetX();

        if (xA > xB)
        {
            tileObjectA.SetTileObjectDisable();
            tileObjectB.SetTileObjectDisable();

            GridManager.Instance.CheckRowBlank(xA);
            
            GridManager.Instance.CheckRowBlank(xB);
        }
        else
        {
            tileObjectB.SetTileObjectDisable();
            tileObjectA.SetTileObjectDisable();

            GridManager.Instance.CheckRowBlank(xB);
            
            GridManager.Instance.CheckRowBlank(xA);
        }

        GridManager.Instance.CheckLoseCondition();
        
    }


}
