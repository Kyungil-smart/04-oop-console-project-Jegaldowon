
public class FieldScene : Scene
{
    private Tile[,] field;
    private PlayerCharacter player;
    private TownPortal exitPortal;
    private Random random = new Random();

    private const int width = 35;
    private const int height = 18;
    private const int mineeralCount = 25;

    public FieldScene(PlayerCharacter player)
    {
        this.player = player;
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

        // 출구 포탈 (상단 중앙)
        exitPortal = new TownPortal();
        exitPortal.Position = new Vector(width / 2, 0);
        field[0, width / 2].SymbolTile = exitPortal;

        // 자원 생성
        SpawnMineral(mineeralCount);
    }

    private void SpawnMineral(int count)
    {
        int spawned = 0;
        int maxAttempts = count * 10;
        int attempts = 0;

        while (spawned < count && attempts < maxAttempts)
        {
            attempts++;

            int x = random.Next(1, width - 1);
            int y = random.Next(2, height - 1);

            if (field[y, x].SymbolTile == null)
            {
                Mineral resource = Mineral.CreateRandom();
                resource.Position = new Vector(x, y);
                field[y, x].SymbolTile = resource;
                spawned++;
            }
        }
    }

    public override void Enter()
    {
        InitializeMap();

        player.Field = field;

        // 플레이어 시작 위치 (출구 근처)
        Vector startPos = new Vector(width / 2, 1);
        player.Position = startPos;
        field[startPos.Y, startPos.X].SymbolTile = player;

        Debug.Log("광산에 입장했습니다! 자원을 수집하세요.");
    }

    public override void Update()
    {
        player.Update();


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
        player.Render(height + 2);
        player.InventoryPanel(width + 3, 2);
    }

    // 광산 탈출
    // 플레이어의 좌표가 맵 안에 있는지 확인
    // 타일에서 필드를 제거 하고 
    // 필드를 초기화 
    public override void Exit()
    {
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
        " --- 광산 (채굴장) --- ".Print(ConsoleColor.DarkCyan);
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

    // public void DrawingInfoPanel() // UI 는 추후에 작성
}
