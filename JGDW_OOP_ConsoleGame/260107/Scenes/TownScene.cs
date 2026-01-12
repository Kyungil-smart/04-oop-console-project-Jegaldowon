
public class TownScene : Scene
{
    private Tile[,] _field;
    private PlayerCharacter _player;
    private const int MAP_WIDTH = 30;
    private const int MAP_HEIGHT = 15;
    
    // 건물 오브젝트들
    private Market _market;
    private Warehouse _warehouse;
    private Laboratory _laboratory;
    private FieldPortal _fieldPortal;

    public TownScene(PlayerCharacter player)
    {
        _player = player;
        InitializeMap();
    }

    private void InitializeMap()
    {
        _field = new Tile[MAP_HEIGHT, MAP_WIDTH];
        
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                _field[y, x] = new Tile(new Vector(x, y));
            }
        }
        
        // 건물 배치
        _market = new Market();
        _market.Position = new Vector(5, 3);
        _field[3, 5].OnTileObject = _market;
        
        _warehouse = new Warehouse();
        _warehouse.Position = new Vector(15, 3);
        _field[3, 15].OnTileObject = _warehouse;
        
        _laboratory = new Laboratory();
        _laboratory.Position = new Vector(25, 3);
        _field[3, 25].OnTileObject = _laboratory;
        
        // 광산 입구 (하단 중앙)
        _fieldPortal = new FieldPortal();
        _fieldPortal.Position = new Vector(14, 12);
        _field[12, 14].OnTileObject = _fieldPortal;
    }

    public override void Enter()
    {
        _player.Field = _field;
        
        // 플레이어 시작 위치 (마을 중앙)
        Vector startPos = new Vector(14, 7);
        _player.Position = startPos;
        _field[startPos.Y, startPos.X].OnTileObject = _player;
        
        Debug.Log("마을에 도착했습니다.");
    }

    public override void Update()
    {
        _player.Update();
        
        // ESC로 메뉴 열기
        if (InputManager.GetKey(ConsoleKey.Escape))
        {
            SceneManager.Change("Title");
        }
    }

    public override void Render()
    {
        DrawTitle();
        DrawMap();
        DrawBuildings();
        DrawLegend();
        _player.Render(MAP_HEIGHT + 2);
        _player.RenderInventoryPreview(MAP_WIDTH + 3, 2);
    }

    public override void Exit()
    {
        // 플레이어 위치 정리
        if (_player.Position.Y >= 0 && _player.Position.Y < MAP_HEIGHT &&
            _player.Position.X >= 0 && _player.Position.X < MAP_WIDTH)
        {
            _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        }
        _player.Field = null;
    }

    private void DrawTitle()
    {
        Console.SetCursorPosition(0, 0);
        "--- 광부의 마을 ---".Print(ConsoleColor.DarkYellow);
    }

    private void DrawMap()
    {
        int offsetY = 1;
        
        // 맵 내용
        for (int y = 0; y < MAP_HEIGHT; y++)
        {
            Console.SetCursorPosition(0, offsetY + y);
            
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                Tile tile = _field[y, x];
                
                if (tile.HasGameObject)
                {
                    GameObject obj = tile.OnTileObject;
                    ConsoleColor color = ConsoleColor.White;
                    
                    if (obj is PlayerCharacter)
                        color = ConsoleColor.Green;
                    else if (obj is Building building)
                        color = building.Color;
                    else if (obj is Resource resource)
                        color = resource.Color;
                    
                    obj.Symbol.Print(color);
                }
                else
                {
                    ' '.Print();
                }
            }
        }
    }

    private void DrawBuildings()
    {
        int offsetX = MAP_WIDTH + 3;
        int offsetY = 8;
        
        Console.SetCursorPosition(offsetX, offsetY);
        "[ 건물 정보 ]".Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(offsetX, offsetY + 1);
        "S".Print(ConsoleColor.Green);
        $" 상점 - 자원 판매".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 2);
        "W".Print(ConsoleColor.DarkYellow);
        $" 창고 Lv.{DataManager.WarehouseLevel}".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 3);
        "L".Print(ConsoleColor.Magenta);
        $" 연구소 Lv.{DataManager.LaboratoryLevel}".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 4);
        "M".Print(ConsoleColor.Cyan);
        " 광산 입구".Print();
    }

    private void DrawLegend()
    {
        int offsetY = MAP_HEIGHT + 6;
        
        Console.SetCursorPosition(0, offsetY);
        "[ 조작법 ]".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(0, offsetY + 1);
        "방향키: 이동 | Space: 상호작용 | ESC: 메뉴".Print(ConsoleColor.DarkGray);
    }
}
