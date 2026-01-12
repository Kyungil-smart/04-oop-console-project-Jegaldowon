
public class FieldScene : Scene
{
    private Tile[,] _field;
    private PlayerCharacter _player;
    private TownPortal _exitPortal;
    private Random _random = new Random();
    
    private const int MAP_WIDTH = 35;
    private const int MAP_HEIGHT = 18;
    private const int INITIAL_RESOURCE_COUNT = 25;
    
    private int _resourcesOnMap = 0;
    private int _collectedThisTrip = 0;

    public FieldScene(PlayerCharacter player)
    {
        _player = player;
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
        
        // 출구 포탈 (상단 중앙)
        _exitPortal = new TownPortal();
        _exitPortal.Position = new Vector(MAP_WIDTH / 2, 0);
        _field[0, MAP_WIDTH / 2].OnTileObject = _exitPortal;
        
        // 자원 생성
        SpawnResources(INITIAL_RESOURCE_COUNT);
    }

    private void SpawnResources(int count)
    {
        int spawned = 0;
        int maxAttempts = count * 10;
        int attempts = 0;
        
        while (spawned < count && attempts < maxAttempts)
        {
            attempts++;
            
            int x = _random.Next(1, MAP_WIDTH - 1);
            int y = _random.Next(2, MAP_HEIGHT - 1);
            
            if (_field[y, x].OnTileObject == null)
            {
                Resource resource = Resource.CreateRandom();
                resource.Position = new Vector(x, y);
                _field[y, x].OnTileObject = resource;
                spawned++;
                _resourcesOnMap++;
            }
        }
    }

    public override void Enter()
    {
        InitializeMap();
        _collectedThisTrip = 0;
        
        _player.Field = _field;
        
        // 플레이어 시작 위치 (출구 근처)
        Vector startPos = new Vector(MAP_WIDTH / 2, 1);
        _player.Position = startPos;
        _field[startPos.Y, startPos.X].OnTileObject = _player;
        
        Debug.Log("광산에 입장했습니다! 자원을 수집하세요.");
    }

    public override void Update()
    {
        _player.Update();
        
        // 자원이 일정 수 이하면 리스폰
        if (_resourcesOnMap < 10)
        {
            SpawnResources(5);
        }
        
        // 자원 수집 카운트 업데이트
        int currentInventoryCount = _player.Inventory.Count;
        if (currentInventoryCount > _collectedThisTrip)
        {
            _resourcesOnMap--;
            _collectedThisTrip = currentInventoryCount;
        }
        
        // ESC로 마을 귀환
        if (InputManager.GetKey(ConsoleKey.Escape))
        {
            SceneManager.Change("Town");
        }
    }

    public override void Render()
    {
        DrawTitle();
        DrawMap();
        DrawMiningInfo();
        _player.Render(MAP_HEIGHT + 2);
        _player.RenderInventoryPreview(MAP_WIDTH + 3, 2);
    }

    public override void Exit()
    {
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
        "--- 광산 (채굴장) ---".Print(ConsoleColor.DarkCyan);
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

    private void DrawMiningInfo()
    {
        int offsetX = MAP_WIDTH + 3;
        int offsetY = 9;
        
        Console.SetCursorPosition(offsetX, offsetY);
        "[ 광산 정보 ]".Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(offsetX, offsetY + 1);
        $"남은 자원: {_resourcesOnMap}개".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 2);
        $"곡괭이 Lv.{DataManager.PickaxeLevel}".Print(ConsoleColor.Cyan);
        
        Console.SetCursorPosition(offsetX, offsetY + 4);
        "[ 자원 가치 ]".Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(offsetX, offsetY + 5);
        "◆".Print(ConsoleColor.Cyan);
        $" 다이아: {DataManager.GetResourcePrice(ResourceType.Diamond)}G".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 6);
        "●".Print(ConsoleColor.Yellow);
        $" 금: {DataManager.GetResourcePrice(ResourceType.Gold)}G".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 7);
        "●".Print(ConsoleColor.White);
        $" 은: {DataManager.GetResourcePrice(ResourceType.Silver)}G".Print();
        
        Console.SetCursorPosition(offsetX, offsetY + 8);
        "●".Print(ConsoleColor.DarkYellow);
        $" 동: {DataManager.GetResourcePrice(ResourceType.Copper)}G".Print();
        
        // 조작법
        Console.SetCursorPosition(offsetX, offsetY + 10);
        "ESC: 마을 귀환".Print(ConsoleColor.DarkGray);
    }
}
