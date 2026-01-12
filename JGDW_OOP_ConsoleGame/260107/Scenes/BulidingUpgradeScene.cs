
public class BuildingUpgradeScene : Scene
{
    public enum BuildingType { Warehouse, Laboratory, Market }
    
    private PlayerCharacter _player;
    private BuildingType _buildingType;
    private MenuList _menu;
    private string _message = "";
    private ConsoleColor _messageColor = ConsoleColor.White;

    public BuildingUpgradeScene(PlayerCharacter player, BuildingType type)
    {
        _player = player;
        _buildingType = type;
        InitMenu();
    }

    private void InitMenu()
    {
        _menu = new MenuList();
        _menu.Add("업그레이드", Upgrade);
        _menu.Add("돌아가기", GoBack);
    }

    public override void Enter()
    {
        _menu.Reset();
        _message = "";
        Debug.Log($"{GetBuildingName()} 건물 메뉴 진입");
    }

    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.UpArrow))
            _menu.SelectUp();
        
        if (InputManager.GetKey(ConsoleKey.DownArrow))
            _menu.SelectDown();
        
        if (InputManager.GetKey(ConsoleKey.Enter))
            _menu.Select();
        
        if (InputManager.GetKey(ConsoleKey.Escape))
            GoBack();
    }

    public override void Render()
    {
        DrawTitle();
        DrawBuildingInfo();
        DrawUpgradeInfo();
        DrawCurrentStats();
        
        _menu.Render(5, 18);
        
        // 메시지 출력
        Console.SetCursorPosition(5, 23);
        _message.Print(_messageColor);
        
        // 골드 표시
        Console.SetCursorPosition(5, 25);
        $"보유 골드: {DataManager.Gold} G".Print(ConsoleColor.Yellow);
    }

    public override void Exit()
    {
    }

    private void DrawTitle()
    {
        string title = _buildingType switch
        {
            BuildingType.Warehouse => "🏠 창고",
            BuildingType.Laboratory => "🔬 연구소",
            BuildingType.Market => "🏪 상점",
            _ => "건물"
        };
        
        ConsoleColor color = _buildingType switch
        {
            BuildingType.Warehouse => ConsoleColor.DarkYellow,
            BuildingType.Laboratory => ConsoleColor.Magenta,
            BuildingType.Market => ConsoleColor.Green,
            _ => ConsoleColor.White
        };
        
        Console.SetCursorPosition(0, 0);
        "╔══════════════════════════════════════════════════╗".Print(color);
        Console.SetCursorPosition(0, 1);
        $"║                   {title}                      ║".Print(color);
        Console.SetCursorPosition(0, 2);
        "╚══════════════════════════════════════════════════╝".Print(color);
    }

    private void DrawBuildingInfo()
    {
        int level = GetCurrentLevel();
        string description = _buildingType switch
        {
            BuildingType.Warehouse => "인벤토리 최대 용량을 증가시킵니다.",
            BuildingType.Laboratory => "희귀 자원의 출현 확률을 증가시킵니다.",
            BuildingType.Market => "자원 판매 가격을 증가시킵니다.",
            _ => ""
        };
        
        Console.SetCursorPosition(5, 4);
        $"현재 레벨: Lv.{level}".Print(ConsoleColor.Cyan);
        
        Console.SetCursorPosition(5, 5);
        description.Print(ConsoleColor.Gray);
    }

    private void DrawUpgradeInfo()
    {
        int level = GetCurrentLevel();
        int cost = DataManager.GetUpgradeCost(GetBuildingKey(), level);
        
        Console.SetCursorPosition(5, 7);
        "┌─────────────────────────────────────┐".Print(ConsoleColor.DarkGray);
        Console.SetCursorPosition(5, 8);
        "│         [ 업그레이드 정보 ]         │".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(5, 9);
        "├─────────────────────────────────────┤".Print(ConsoleColor.DarkGray);
        
        if (level >= 5)
        {
            Console.SetCursorPosition(5, 10);
            "│     ★ 최대 레벨 달성! ★            │".Print(ConsoleColor.Yellow);
            Console.SetCursorPosition(5, 11);
            "└─────────────────────────────────────┘".Print(ConsoleColor.DarkGray);
        }
        else
        {
            Console.SetCursorPosition(5, 10);
            "│ ".Print(ConsoleColor.DarkGray);
            $"Lv.{level} → Lv.{level + 1}".Print(ConsoleColor.Cyan);
            "                         │".Print(ConsoleColor.DarkGray);
            
            Console.SetCursorPosition(5, 11);
            "│ ".Print(ConsoleColor.DarkGray);
            $"비용: {cost} G".Print(DataManager.Gold >= cost ? ConsoleColor.Green : ConsoleColor.Red);
            "                         │".Print(ConsoleColor.DarkGray);
            
            Console.SetCursorPosition(5, 12);
            "│ ".Print(ConsoleColor.DarkGray);
            string effect = GetUpgradeEffect(level + 1);
            effect.Print(ConsoleColor.White);
            Console.SetCursorPosition(42, 12);
            "│".Print(ConsoleColor.DarkGray);
            
            Console.SetCursorPosition(5, 13);
            "└─────────────────────────────────────┘".Print(ConsoleColor.DarkGray);
        }
    }

    private void DrawCurrentStats()
    {
        Console.SetCursorPosition(5, 15);
        "[ 현재 효과 ]".Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(5, 16);
        string currentEffect = _buildingType switch
        {
            BuildingType.Warehouse => $"인벤토리 용량: {DataManager.GetMaxInventorySize()}칸",
            BuildingType.Laboratory => $"희귀 자원 확률: +{(int)((DataManager.GetRareBonus() - 1) * 100)}%",
            BuildingType.Market => $"판매 가격 보너스: +{(int)((DataManager.GetPriceBonus() - 1) * 100)}%",
            _ => ""
        };
        currentEffect.Print(ConsoleColor.Cyan);
    }

    private void Upgrade()
    {
        int level = GetCurrentLevel();
        
        if (level >= 5)
        {
            _message = "이미 최대 레벨입니다!";
            _messageColor = ConsoleColor.Red;
            return;
        }
        
        int cost = DataManager.GetUpgradeCost(GetBuildingKey(), level);
        
        if (DataManager.Gold < cost)
        {
            _message = $"골드가 부족합니다! (필요: {cost}G)";
            _messageColor = ConsoleColor.Red;
            return;
        }
        
        DataManager.Gold -= cost;
        SetLevel(level + 1);
        
        _message = $"{GetBuildingName()}을(를) Lv.{level + 1}로 업그레이드했습니다!";
        _messageColor = ConsoleColor.Green;
        
        Debug.Log($"{GetBuildingName()} 업그레이드: Lv.{level + 1}");
    }

    private void GoBack()
    {
        SceneManager.Change("Town");
    }

    private string GetBuildingName()
    {
        return _buildingType switch
        {
            BuildingType.Warehouse => "창고",
            BuildingType.Laboratory => "연구소",
            BuildingType.Market => "상점",
            _ => "건물"
        };
    }

    private string GetBuildingKey()
    {
        return _buildingType switch
        {
            BuildingType.Warehouse => "Warehouse",
            BuildingType.Laboratory => "Laboratory",
            BuildingType.Market => "Market",
            _ => ""
        };
    }

    private int GetCurrentLevel()
    {
        return _buildingType switch
        {
            BuildingType.Warehouse => DataManager.WarehouseLevel,
            BuildingType.Laboratory => DataManager.LaboratoryLevel,
            BuildingType.Market => DataManager.ShopLevel,
            _ => 0
        };
    }

    private void SetLevel(int level)
    {
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                DataManager.WarehouseLevel = level;
                break;
            case BuildingType.Laboratory:
                DataManager.LaboratoryLevel = level;
                break;
            case BuildingType.Market:
                DataManager.ShopLevel = level;
                break;
        }
    }

    private string GetUpgradeEffect(int newLevel)
    {
        return _buildingType switch
        {
            BuildingType.Warehouse => $"인벤토리 +3칸 (총 {5 + newLevel * 3}칸)",
            BuildingType.Laboratory => $"희귀 확률 +20% (총 +{newLevel * 20}%)",
            BuildingType.Market => $"판매가격 +15% (총 +{newLevel * 15}%)",
            _ => ""
        };
    }
}
