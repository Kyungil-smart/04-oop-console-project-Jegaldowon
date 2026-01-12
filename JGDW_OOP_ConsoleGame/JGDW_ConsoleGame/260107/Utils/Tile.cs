

using System.ComponentModel.Design;

public struct Tile
{
    private GameObject _symbolTile;
    private Vector _position;

    // 타일 위에 뭐가 올라와있는지?
    public GameObject SymbolTile
    {
        get { return _symbolTile; }
        set { _symbolTile = value; }
    }

    // 타일 위에 올라서면 발생해야 하는 이벤트
    public event Action OnPlayer;

    // 자신의 좌표
    public Vector Position
    {
        get { return _position; }
        set { _position = value; }
    }

    // 뭔가 있는지 체크
    public bool TileCheck
    {
        get { return _symbolTile != null; }
    }

    public Tile(Vector position)
    {
        _symbolTile = null;
        _position = position;
        OnPlayer = null;
    }

    public void Print()
    {
        if (TileCheck)
        {
            _symbolTile.Symbol.Print();
        }
        else
        {
            ' '.Print();
        }
    }
}