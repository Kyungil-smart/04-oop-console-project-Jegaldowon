
public static class DataManager
{
    // 플레이어 재화
    public static int Gold { get; set; } = 100;
    
    // 건물 레벨 (0 = 미건설, 1~5 = 레벨)
    public static int WarehouseLevel { get; set; } = 1;      // 창고 레벨
    public static int LaboratoryLevel { get; set; } = 0;     // 연구소 레벨
    public static int MarketLevel { get; set; } = 0;         // 상점 레벨
    
    // 장비
    public static int PickaxeLevel { get; set; } = 1;        // 곡괭이 레벨
    
    // 인벤토리 최대 용량 계산 (창고 레벨에 따라)
    public static int GetMaxInventorySize()
    {
        return 5 + (WarehouseLevel * 3); // 기본 5칸 + 레벨당 3칸
    }
    
    // 희귀 자원 확률 보너스 (연구소 레벨에 따라)
    public static float GetRareBonus()
    {
        return 1.0f + (LaboratoryLevel * 0.2f); // 레벨당 20% 증가
    }
    
    // 판매 가격 보너스 (상점 레벨에 따라)
    public static float GetPriceBonus()
    {
        return 1.0f + (MarketLevel * 0.15f); // 레벨당 15% 증가
    }
    
    // 채굴 효율 보너스 (곡괭이 레벨에 따라)
    public static int GetMiningPower()
    {
        return PickaxeLevel; // 한 번에 채굴할 수 있는 양
    }
    
    // 건물 업그레이드 비용
    public static int GetUpgradeCost(string buildingType, int currentLevel)
    {
        int baseCost;
        
        switch (buildingType)
        {
            case "Warehouse":
                baseCost = 200;
                break;
            case "Laboratory":
                baseCost = 300;
                break;
            case "Market":
                baseCost = 250;
                break;
            case "Pickaxe":
                baseCost = 150;
                break;
            default:
                baseCost = 100;
                break;
        }
        
        return baseCost * (currentLevel + 1);
    }
    
    // 통계
    public static int TotalResourcesCollected { get; set; } = 0;
    public static int TotalGoldEarned { get; set; } = 0;
    
    // 자원 가격표
    public static int GetResourcePrice(ResourceType type)
    {
        int basePrice;
        
        switch (type)
        {
            case ResourceType.Diamond:
                basePrice = 100;
                break;
            case ResourceType.Gold:
                basePrice = 50;
                break;
            case ResourceType.Silver:
                basePrice = 25;
                break;
            case ResourceType.Copper:
                basePrice = 10;
                break;
            default:
                basePrice = 5;
                break;
        }
        
        return (int)(basePrice * GetPriceBonus());
    }
}

public enum ResourceType
{
    Copper,     // 동 (가장 흔함)
    Silver,     // 은 (중간)
    Gold,       // 금 (희귀)
    Diamond     // 다이아 (극희귀)
}
