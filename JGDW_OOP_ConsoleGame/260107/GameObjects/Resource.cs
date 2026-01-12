using System.Resources;

public class Resource : Item, IInteractable
{
    public MineralType Type { get; private set; }

    // 문양, 광물 이름 + (색상)
    private static readonly Dictionary<MineralType, (char symbol, string name, ConsoleColor color)> ResourceInfo =
        new Dictionary<MineralType, (char symbol, string name, ConsoleColor color)>()
        {
            { MineralType.Copper, ('●', "동광석", ConsoleColor.DarkYellow) },
            { MineralType.Silver, ('●', "은광석", ConsoleColor.White) },
            { MineralType.Gold, ('●', "금광석", ConsoleColor.Yellow) },
            { MineralType.Diamond, ('◆', "다이아몬드", ConsoleColor.Cyan) }
        };

    public ConsoleColor Color { get; private set; }
    
    // 모든 정보 
    public Resource(MineralType type)
    {
        Type = type;
        var info = ResourceInfo[type];
        Symbol = info.symbol;
        Name = info.name;
        Color = info.color;
        Description = Discription();
    }
    


    private string Discription()
    {
        switch (Type)
        {
            case MineralType.Copper:
                return "동광석, 쉽게 구할 수 있다.";
            case MineralType.Silver:
                return "은광석, 잘 찾아보면 쉽게 구할 수 있다.";
            case MineralType.Gold:
                return "금광석, 비싸게 팔린다.";
            case MineralType.Diamond:
                return "다이아몬드, 매우 귀중한 광물이다.";
            default:
                return "알 수 없는 광물";
           
        }
        
    }


    public int GetPrice()
    {
        return DataManager.GetResourcePrice(Type);
    }


    public override void Use()
    {
        // 자원은 판매만 
    }


    public void Interact(PlayerCharacter player)
    {
        
    }
    
    public static Resource CreateRandom()
    {
        Random random = new Random();
        float priceBonus = DataManager.RareBonus();
        int roll = random.Next(100);
        
        // 기본 확률: 동 60%, 은 25%, 금 12%, 다이아 3%
        // 연구소 레벨에 따라 희귀 자원 확률 증가
        float diamondChance = 3 * priceBonus;
        float goldChance = 12 * priceBonus;
        float silverChance = 25 * priceBonus;
        
        if (roll < diamondChance)
            return new Resource(MineralType.Diamond);
        else if (roll < diamondChance + goldChance)
            return new Resource(MineralType.Gold);
        else if (roll < diamondChance + goldChance + silverChance)
            return new Resource(MineralType.Silver);
        else
            return new Resource(MineralType.Copper);
    }
}