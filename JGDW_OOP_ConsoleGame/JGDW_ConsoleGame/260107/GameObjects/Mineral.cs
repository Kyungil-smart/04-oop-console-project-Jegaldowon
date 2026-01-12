
public class Mineral : Item, IInteractable
{
    public MineralType Type { get; private set; }
    
    private static readonly Dictionary<MineralType, (char symbol, string name, ConsoleColor color)> ResourceInfo = new Dictionary<MineralType, (char, string, ConsoleColor)>
    {
        { MineralType.Copper,  ('*', "동광석", ConsoleColor.DarkYellow) },
        { MineralType.Silver,  ('*', "은광석", ConsoleColor.White) },
        { MineralType.Gold,    ('*', "금광석", ConsoleColor.Yellow) },
        { MineralType.Diamond, ('+', "다이아몬드", ConsoleColor.Cyan) }
    };
    
    public ConsoleColor Color { get; private set; }
    
    public Mineral(MineralType type)
    {
        Type = type;
        var info = ResourceInfo[type];
        Symbol = info.symbol;
        Name = info.name;
        Color = info.color;
        Description = GetDescription();
    }
    
    private string GetDescription()
    {
        switch (Type)
        {
            case MineralType.Diamond:
                return "빛나는 다이아몬드. 매우 귀중하다!";
            case MineralType.Gold:
                return "황금빛 금광석. 비싸게 팔린다.";
            case MineralType.Silver:
                return "은빛의 은광석. 적당한 가격.";
            case MineralType.Copper:
                return "흔한 동광석. 가장 쉽게 구할 수 있다.";
            default:
                return "알 수 없는 자원";
        }
    }
    
    public int GetPrice()
    {
        return DataManager.MineralPrice(Type);
    }
    
    public override void Use()
    {
        // 자원은 사용하지 않고 판매만 가능
    }

    // 상호작용 햇을 때
    public void Interact(PlayerCharacter player)
    {
        // 플레이어가 자원에 접촉했을 -> 수집
        if (player.Collect())
        {
            player.CollectMineral(this);
            DataManager.TotalResourcesCollected++;
        }
    }
    
    // 랜덤 자원 생성 (연구소 레벨에 따른 희귀도 보정)
    public static Mineral CreateRandom()
    {
        Random random = new Random();
        float rareBonus = DataManager.RareMineral();
        int rand = random.Next(100);
        
        // 기본 확률: 동 60%, 은 25%, 금 12%, 다이아 3%
        // 연구소 레벨에 따라 희귀 자원 확률 증가
        float diamondPercent = 3 * rareBonus;
        float goldPercent = 12 * rareBonus;
        float silverPercent = 25 * rareBonus;
        
        if (rand < diamondPercent)
            return new Mineral(MineralType.Diamond);
        else if (rand < diamondPercent + goldPercent)
            return new Mineral(MineralType.Gold);
        else if (rand < diamondPercent + goldPercent + silverPercent)
            return new Mineral(MineralType.Silver);
        else
            return new Mineral(MineralType.Copper);
    }
}
