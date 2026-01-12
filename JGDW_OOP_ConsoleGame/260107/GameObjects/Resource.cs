
public class Resource : Item, IInteractable
{
    public ResourceType Type { get; private set; }
    
    private static readonly Dictionary<ResourceType, (char symbol, string name, ConsoleColor color)> ResourceInfo = new Dictionary<ResourceType, (char, string, ConsoleColor)>
    {
        { ResourceType.Copper,  ('*', "동광석", ConsoleColor.DarkYellow) },
        { ResourceType.Silver,  ('*', "은광석", ConsoleColor.White) },
        { ResourceType.Gold,    ('*', "금광석", ConsoleColor.Yellow) },
        { ResourceType.Diamond, ('+', "다이아몬드", ConsoleColor.Cyan) }
    };
    
    public ConsoleColor Color { get; private set; }
    
    public Resource(ResourceType type)
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
            case ResourceType.Diamond:
                return "빛나는 다이아몬드. 매우 귀중하다!";
            case ResourceType.Gold:
                return "황금빛 금광석. 비싸게 팔린다.";
            case ResourceType.Silver:
                return "은빛의 은광석. 적당한 가격.";
            case ResourceType.Copper:
                return "흔한 동광석. 가장 쉽게 구할 수 있다.";
            default:
                return "알 수 없는 자원";
        }
    }
    
    public int GetPrice()
    {
        return DataManager.GetResourcePrice(Type);
    }
    
    public override void Use()
    {
        // 자원은 사용하지 않고 판매만 가능
    }
    
    public void Interact(PlayerCharacter player)
    {
        // 플레이어가 자원에 접촉했을 때 수집
        if (player.CanCollect())
        {
            player.CollectResource(this);
            DataManager.TotalResourcesCollected++;
        }
    }
    
    // 랜덤 자원 생성 (연구소 레벨에 따른 희귀도 보정)
    public static Resource CreateRandom()
    {
        Random random = new Random();
        float rareBonus = DataManager.GetRareBonus();
        int roll = random.Next(100);
        
        // 기본 확률: 동 60%, 은 25%, 금 12%, 다이아 3%
        // 연구소 레벨에 따라 희귀 자원 확률 증가
        float diamondChance = 3 * rareBonus;
        float goldChance = 12 * rareBonus;
        float silverChance = 25 * rareBonus;
        
        if (roll < diamondChance)
            return new Resource(ResourceType.Diamond);
        else if (roll < diamondChance + goldChance)
            return new Resource(ResourceType.Gold);
        else if (roll < diamondChance + goldChance + silverChance)
            return new Resource(ResourceType.Silver);
        else
            return new Resource(ResourceType.Copper);
    }
}
