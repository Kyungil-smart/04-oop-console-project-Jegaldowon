
public class ShopScene : Scene
{
    private PlayerCharacter _player;
    private MenuList _shopMenu;
    private string _message = "";
    private ConsoleColor _messageColor = ConsoleColor.White;

    public ShopScene(PlayerCharacter player)
    {
        _player = player;
        InitMenu();
    }

    private void InitMenu()
    {
        _shopMenu = new MenuList();
        _shopMenu.Add("모든 자원 판매", SellAllResources);
        _shopMenu.Add("곡괭이 업그레이드", UpgradePickaxe);
        _shopMenu.Add("돌아가기", GoBack);
    }

    public override void Enter()
    {
        _shopMenu.Reset();
        _message = "어서오세요! 자원을 판매하거나 장비를 구매하세요.";
        _messageColor = ConsoleColor.Green;
        Debug.Log("상점에 입장했습니다.");
    }

    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.UpArrow))
            _shopMenu.SelectUp();
        
        if (InputManager.GetKey(ConsoleKey.DownArrow))
            _shopMenu.SelectDown();
        
        if (InputManager.GetKey(ConsoleKey.Enter))
            _shopMenu.Select();
        
        if (InputManager.GetKey(ConsoleKey.Escape))
            GoBack();
    }

    public override void Render()
    {
        DrawTitle();
        DrawShopInfo();
        DrawInventoryInfo();
        DrawPriceList();
        DrawEquipmentShop();
        
        _shopMenu.Render(5, 18);
        
        // 메시지 출력
        Console.SetCursorPosition(5, 25);
        _message.Print(_messageColor);
    }

    public override void Exit()
    {
    }

    private void DrawTitle()
    {
        Console.SetCursorPosition(0, 0);
        "--------------------------------------------".Print(ConsoleColor.Green);
        Console.SetCursorPosition(0, 1);
        "              상점 - 자원 거래소            ".Print(ConsoleColor.Green);
        Console.SetCursorPosition(0, 2);
        "--------------------------------------------".Print(ConsoleColor.Green);
    }

    private void DrawShopInfo()
    {
        Console.SetCursorPosition(5, 4);
        ("보유 골드: " + DataManager.Gold + " G").Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(5, 5);
        ("상점 레벨: Lv." + DataManager.MarketLevel + " (가격 보너스: +" + (int)((DataManager.GetPriceBonus() - 1) * 100) + "%)").Print(ConsoleColor.Cyan);
    }

    private void DrawInventoryInfo()
    {
        Console.SetCursorPosition(5, 7);
        "[ 보유 자원 현황 ]".Print(ConsoleColor.Yellow);
        
        var summary = _player.Inventory.GetResourceSummary();
        int line = 8;
        int totalValue = 0;
        
        if (summary.Count == 0)
        {
            Console.SetCursorPosition(5, line);
            "(자원이 없습니다)".Print(ConsoleColor.DarkGray);
            line++;
        }
        else
        {
            foreach (var pair in summary.OrderByDescending(p => (int)p.Key))
            {
                string name;
                ConsoleColor color;
                
                switch (pair.Key)
                {
                    case ResourceType.Diamond:
                        name = "◆ 다이아몬드";
                        color = ConsoleColor.Cyan;
                        break;
                    case ResourceType.Gold:
                        name = "● 금광석    ";
                        color = ConsoleColor.Yellow;
                        break;
                    case ResourceType.Silver:
                        name = "● 은광석    ";
                        color = ConsoleColor.White;
                        break;
                    case ResourceType.Copper:
                        name = "● 동광석    ";
                        color = ConsoleColor.DarkYellow;
                        break;
                    default:
                        name = "? 알수없음  ";
                        color = ConsoleColor.Gray;
                        break;
                }
                
                int price = DataManager.GetResourcePrice(pair.Key);
                int subtotal = price * pair.Value;
                totalValue += subtotal;
                
                Console.SetCursorPosition(5, line);
                name.Print(color);
                (" x" + pair.Value + " = " + subtotal + "G").Print();
                line++;
            }
        }
        
        Console.SetCursorPosition(5, line + 1);
        ("총 가치: " + totalValue + "G").Print(ConsoleColor.Yellow);
    }

    private void DrawPriceList()
    {
        int offsetX = 40;
        Console.SetCursorPosition(offsetX, 7);
        "[ 현재 시세 ]".Print(ConsoleColor.Yellow);
        
        Console.SetCursorPosition(offsetX, 8);
        "◆".Print(ConsoleColor.Cyan);
        (" 다이아: " + DataManager.GetResourcePrice(ResourceType.Diamond) + "G").Print();
        
        Console.SetCursorPosition(offsetX, 9);
        "●".Print(ConsoleColor.Yellow);
        (" 금    : " + DataManager.GetResourcePrice(ResourceType.Gold) + "G").Print();
        
        Console.SetCursorPosition(offsetX, 10);
        "●".Print(ConsoleColor.White);
        (" 은    : " + DataManager.GetResourcePrice(ResourceType.Silver) + "G").Print();
        
        Console.SetCursorPosition(offsetX, 11);
        "●".Print(ConsoleColor.DarkYellow);
        (" 동    : " + DataManager.GetResourcePrice(ResourceType.Copper) + "G").Print();
    }

    private void DrawEquipmentShop()
    {
        int offsetX = 40;
        Console.SetCursorPosition(offsetX, 13);
        "[ 장비 상점 ]".Print(ConsoleColor.Yellow);
        
        int pickaxeCost = DataManager.GetUpgradeCost("Pickaxe", DataManager.PickaxeLevel);
        Console.SetCursorPosition(offsetX, 14);
        ("곡괭이 Lv." + DataManager.PickaxeLevel).Print(ConsoleColor.Cyan);
        
        Console.SetCursorPosition(offsetX, 15);
        if (DataManager.PickaxeLevel >= 5)
            "(최대 레벨)".Print(ConsoleColor.DarkGray);
        else
            ("업그레이드: " + pickaxeCost + "G").Print(ConsoleColor.Gray);
    }

    private void SellAllResources()
    {
        if (_player.Inventory.IsEmpty)
        {
            _message = "판매할 자원이 없습니다!";
            _messageColor = ConsoleColor.Red;
            return;
        }
        
        int totalGold = _player.Inventory.SellAll();
        DataManager.Gold += totalGold;
        DataManager.TotalGoldEarned += totalGold;
        
        _message = "자원을 판매하여 " + totalGold + "G를 획득했습니다!";
        _messageColor = ConsoleColor.Yellow;
        
        Debug.Log("자원 판매: +" + totalGold + "G");
    }

    private void UpgradePickaxe()
    {
        if (DataManager.PickaxeLevel >= 5)
        {
            _message = "곡괭이가 이미 최대 레벨입니다!";
            _messageColor = ConsoleColor.Red;
            return;
        }
        
        int cost = DataManager.GetUpgradeCost("Pickaxe", DataManager.PickaxeLevel);
        
        if (DataManager.Gold < cost)
        {
            _message = "골드가 부족합니다! (필요: " + cost + "G)";
            _messageColor = ConsoleColor.Red;
            return;
        }
        
        DataManager.Gold -= cost;
        DataManager.PickaxeLevel++;
        
        _message = "곡괭이를 Lv." + DataManager.PickaxeLevel + "로 업그레이드했습니다!";
        _messageColor = ConsoleColor.Cyan;
        
        Debug.Log("곡괭이 업그레이드: Lv." + DataManager.PickaxeLevel);
    }

    private void GoBack()
    {
        SceneManager.Change("Town");
    }
}
