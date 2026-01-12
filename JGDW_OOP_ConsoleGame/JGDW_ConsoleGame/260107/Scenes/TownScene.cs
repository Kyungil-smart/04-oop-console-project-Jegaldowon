
public class TownScene : Scene
{
    private Tile[,] field;
    private PlayerCharacter player;
    private int width = 30;
    private int height = 15;
    
    // 건물 오브젝트들
    private Shop shop;
    private Warehouse warehouse;
    private Laboratory laboratory;
    private FieldPortal fieldPortal;

    public TownScene(PlayerCharacter player)
    {
        this.player = player;
        InitializeMap();
    }

    private void InitializeMap()
    {
        field = new Tile[height, width];
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                field[y, x] = new Tile(new Vector(x, y));
            }
        }
        
        // 건물 배치
        shop = new Shop();
        shop.Position = new Vector(5, 3);
        field[3, 5].SymbolTile = shop;
        
        warehouse = new Warehouse();
        warehouse.Position = new Vector(15, 3);
        field[3, 15].SymbolTile = warehouse;
        
        laboratory = new Laboratory();
        laboratory.Position = new Vector(25, 3);
        field[3, 25].SymbolTile = laboratory;
        
        // 광산 입구 (하단 중앙)
        fieldPortal = new FieldPortal();
        fieldPortal.Position = new Vector(14, 12);
        field[12, 14].SymbolTile = fieldPortal;
    }

    public override void Enter()
    {
        player.Field = field;
        
        // 플레이어 시작 위치 (마을 중앙)
        Vector startPos = new Vector(14, 7);
        player.Position = startPos;
        field[startPos.Y, startPos.X].SymbolTile = player;
        
        Debug.Log("마을에 도착했습니다.");
    }

    public override void Update()
    {
        player.Update();
        
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
        DrawBuilding();
        DrawLegend();
        player.Render(height + 2);
        player.InventoryPanel(width + 3, 2);
    }

    public override void Exit()
    {
        // 플레이어 위치 정리
        if (player.Position.Y >= 0 && player.Position.Y < height &&
            player.Position.X >= 0 && player.Position.X < width)
        {
            field[player.Position.Y, player.Position.X].SymbolTile = null;
        }
        player.Field = null;
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
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(0, offsetY + y);
            
            for (int x = 0; x < width; x++)
            {
                Tile tile = field[y, x];
                
                if (tile.TileCheck)
                {
                    GameObject obj = tile.SymbolTile;
                    ConsoleColor color = ConsoleColor.White;
                    
                    if (obj is PlayerCharacter)
                        color = ConsoleColor.Green;
                    else if (obj is Building building)
                        color = building.Color;
                    else if (obj is Mineral resource)
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

    private void DrawBuilding()
    {
        int offsetX = width + 3;
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
        int offsetY = height + 6;
        
        Console.SetCursorPosition(0, offsetY);
        "[ 조작법 ]".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(0, offsetY + 1);
        "방향키: 이동 | Space: 상호작용 | ESC: 메뉴".Print(ConsoleColor.DarkGray);
    }
}
