
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
        Debug.Log(GetBuildingName() + " 건물 메뉴 진입");
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
        
        _menu.Render(5, 16);
        
        // 메시지 출력
        Console.SetCursorPosition(5, 21);
        _message.Print(_messageColor);
        
        // 골드 표시
        Console.SetCursorPosition(5, 23);
        ("보유 골드: " + DataManager.Gold + " G").Print(ConsoleColor.Yellow);
    }

    public override void Exit()
    {
    }

    private void DrawTitle()
    {
        string title;
        ConsoleColor color;
        
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                title = "창고";
                color = ConsoleColor.DarkYellow;
                break;
            case BuildingType.Laboratory:
                title = "연구소";
                color = ConsoleColor.Magenta;
                break;
            case BuildingType.Market:
                title = "상점";
                color = ConsoleColor.Green;
                break;
            default:
                title = "건물";
                color = ConsoleColor.White;
                break;
        }
        
        Console.SetCursorPosition(0, 0);
        "--------------------------------------------".Print(color);
        Console.SetCursorPosition(0, 1);
        ("                   " + title + "                  ").Print(color);
        Console.SetCursorPosition(0, 2);
        "--------------------------------------------".Print(color);
    }

    private void DrawBuildingInfo()
    {
        int level = GetCurrentLevel();
        string description;
        
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                description = "인벤토리 최대 용량을 증가시킵니다.";
                break;
            case BuildingType.Laboratory:
                description = "희귀 자원의 출현 확률을 증가시킵니다.";
                break;
            case BuildingType.Market:
                description = "자원 판매 가격을 증가시킵니다.";
                break;
            default:
                description = "";
                break;
        }
        
        Console.SetCursorPosition(5, 4);
        ("현재 레벨: Lv." + level).Print(ConsoleColor.Cyan);
        
        Console.SetCursorPosition(5, 5);
        description.Print(ConsoleColor.Gray);
    }

    private void DrawUpgradeInfo()
    {
        int level = GetCurrentLevel();
        int cost = DataManager.GetUpgradeCost(GetBuildingKey(), level);
        
        Console.SetCursorPosition(5, 7);
        "[ 업그레이드 정보 ]".Print(ConsoleColor.Yellow);
        
        if (level >= 5)
        {
            Console.SetCursorPosition(5, 8);
            "최대 레벨 달성!".Print(ConsoleColor.Yellow);
        }
        else
        {
            Console.SetCursorPosition(5, 8);
            ("Lv." + level + " -> Lv." + (level + 1)).Print(ConsoleColor.Cyan);
            
            Console.SetCursorPosition(5, 9);
            ("비용: " + cost + " G").Print(DataManager.Gold >= cost ? ConsoleColor.Green : ConsoleColor.Red);
            
            Console.SetCursorPosition(5, 10);
            string effect = GetUpgradeEffect(level + 1);
            effect.Print(ConsoleColor.White);
        }
    }

    private void DrawCurrentStats()
    {
        Console.SetCursorPosition(5, 13);
        "[ 현재 효과 ]".Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(5, 14);
        string currentEffect;
        
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                currentEffect = "인벤토리 용량: " + DataManager.GetMaxInventorySize() + "칸";
                break;
            case BuildingType.Laboratory:
                currentEffect = "희귀 자원 확률: +" + (int)((DataManager.GetRareBonus() - 1) * 100) + "%";
                break;
            case BuildingType.Market:
                currentEffect = "판매 가격 보너스: +" + (int)((DataManager.GetPriceBonus() - 1) * 100) + "%";
                break;
            default:
                currentEffect = "";
                break;
        }
        
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
            _message = "골드가 부족합니다! (필요: " + cost + "G)";
            _messageColor = ConsoleColor.Red;
            return;
        }
        
        DataManager.Gold -= cost;
        SetLevel(level + 1);
        
        _message = GetBuildingName() + "을(를) Lv." + (level + 1) + "로 업그레이드했습니다!";
        _messageColor = ConsoleColor.Green;
        
        Debug.Log(GetBuildingName() + " 업그레이드: Lv." + (level + 1));
    }

    private void GoBack()
    {
        SceneManager.Change("Town");
    }

    private string GetBuildingName()
    {
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                return "창고";
            case BuildingType.Laboratory:
                return "연구소";
            case BuildingType.Market:
                return "상점";
            default:
                return "건물";
        }
    }

    private string GetBuildingKey()
    {
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                return "Warehouse";
            case BuildingType.Laboratory:
                return "Laboratory";
            case BuildingType.Market:
                return "Market";
            default:
                return "";
        }
    }

    private int GetCurrentLevel()
    {
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                return DataManager.WarehouseLevel;
            case BuildingType.Laboratory:
                return DataManager.LaboratoryLevel;
            case BuildingType.Market:
                return DataManager.MarketLevel;
            default:
                return 0;
        }
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
                DataManager.MarketLevel = level;
                break;
        }
    }

    private string GetUpgradeEffect(int newLevel)
    {
        switch (_buildingType)
        {
            case BuildingType.Warehouse:
                return "인벤토리 +3칸 (총 " + (5 + newLevel * 3) + "칸)";
            case BuildingType.Laboratory:
                return "희귀 확률 +20% (총 +" + (newLevel * 20) + "%)";
            case BuildingType.Market:
                return "판매가격 +15% (총 +" + (newLevel * 15) + "%)";
            default:
                return "";
        }
    }
}
