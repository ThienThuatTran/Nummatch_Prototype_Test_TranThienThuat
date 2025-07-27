using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image numberImage;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image gemImage;

    [SerializeField] private Color hightlightColor;
    [SerializeField] private Color unhighlightColor;
    [SerializeField] private Color fadeColor;

    [SerializeField] private Sprite[] gemIcons;

    private Tile tile;
    private Color startNumberColor;

    private Gem gem;

    private void Awake()
    {
        startNumberColor = numberImage.color;
    }
    public void Initialize(int x, int y, int value, bool isDisable)
    {
        tile = new Tile(x, y, value, isDisable);
        numberImage.enabled = true;
        numberImage.sprite = TileManager.Instance.GetNumberSprite(value);

        if (isDisable) numberImage.color = fadeColor;
        else
        {
            numberImage.color = startNumberColor;
            fadeImage.color = unhighlightColor;
        }
    }
    public void UpdatePosition(int x, int y)
    {
        tile.UpdatePosition(x, y);
    }

    public void ResetTileObject()
    {
        //numberImage.sprite = null;

        numberImage.enabled = false;
        fadeImage.color = unhighlightColor;

        numberImage.color = startNumberColor;
        tile = null;
        RemoveGem();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (tile == null) return;
        if (tile.IsDisable()) return;

        SetHighLight();

        SoundManager.Instance.PlayChooseSoundFX();
        TileManager.Instance.SetSelectedTileObject(this);
        
    }

    public void SetHighLight()
    {
        fadeImage.color = hightlightColor;
    }
    public void ResetHighlight()
    {
        fadeImage.color = unhighlightColor;
    }

    public Tile GetTile() => tile;

    public void SetTileObjectDisable()
    {
        if (gem != null) CollectGem();

        tile.SetIsDisable(true);
        //fadeImage.color = fadeColor;
        fadeImage.color = unhighlightColor;
        numberImage.color = fadeColor;
    }

    public void ResetFadeImage()
    {
        fadeImage.color = fadeColor;
    }

    public void SetGem(int index = -1)
    {
        
        gem = gameObject.AddComponent<Gem>();
        gemImage.enabled = true;
        if (index < 0) index = GemManager.Instance.GetValidIndex();
        //index 0
        gemImage.sprite = gemIcons[index];
        //index 1
        gem.Initialize(index + 1);
    }

    public void CollectGem()
    {
        gem.CollectGem();
        Destroy(gem);


        gem = null;
        gemImage.sprite = null;
        gemImage.enabled = false;
        
    }

    
    public void ChangeGem(TileObject aboveTileObject)
    {

        if (gem != null)
        {
            // index 0
            aboveTileObject.SetGem(gem.GetGemIndex() - 1);
        }
        RemoveGem();
    }

    private void RemoveGem()
    {
        if (gem == null) return;
        Destroy(gem);
        gem = null;

        gemImage.enabled = false;
        gemImage.sprite = null;
    }

}
