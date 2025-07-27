
using System.Collections;

public class Tile: IComparer
{
    private int x;
    private int y;
    private int value;
    private bool isDisable;

    public Tile(int x, int y, int value, bool isDisable)
    {
        this.x = x;
        this.y = y;
        this.value = value;
        this.isDisable = isDisable;
    }

    public void UpdatePosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int GetX() => x;
    public int GetY() => y;
    public int GetValue() => value;

    public bool IsDisable() => isDisable;
    public bool SetIsDisable(bool value) => isDisable = value;

    public int Compare(object a, object b)
    {
        Tile tileA = a as Tile;
        Tile tileB = b as Tile;

        if (tileA.GetX() == tileB.GetX() && tileA.GetY() == tileB.GetY()) return 1;
        else return -1;
    }
}
