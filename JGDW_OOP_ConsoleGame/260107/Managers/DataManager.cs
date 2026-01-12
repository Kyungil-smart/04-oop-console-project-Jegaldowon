public enum MineralType
{
    Copper, // 동 (가장 흔함)
    Silver, // 은 (중간)
    Gold, // 금 (희귀)
    Diamond // 다이아 (극희귀)
}

public static class DataManager
{
    // 플레이어 재화
    public static int Gold { get; set; } = 100;


    // 창고 레벨
    public static int WarehouseLevel { get; set; } = 1;
    // 연구소 레벨
    public static int LaboratoryLevel { get; set; } = 0;
    // 상점 레벨
    public static int ShopLevel { get; set; } = 0;
    // 장비
    public static int PickaxeLevel { get; set; } = 1; 

    // 기본 5칸 하고 레벨업 하면 3칸 추가
    public static int MaxInventory()
    {
        return 5 + (WarehouseLevel * 3); 
    }

    // 연구소 레벨업 -> 자원 확률 증가
    public static float RareBonus()
    {
        return 1.0f + (LaboratoryLevel * 0.2f); //  20% 증가
    }

    // 상점 레벨업 -> 판매 가격 보너스 
    public static float PriceBonus()
    {
        return 1.0f + (ShopLevel * 0.15f); // 15% 증가
    }

    // 곡괭이 레벌업 -> 채굴 효율 보너스 
    public static int MiningPower()
    {
        return PickaxeLevel; 
    }

    // 건물 업그레이드 비용
    public static int UpgradeCost(string buildingType, int currentLevel)
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
                baseCost = 0;
                break;
        }

        return baseCost * (currentLevel + 1);
    }

    // 통계
    public static int TotalCollected { get; set; } = 0;
    public static int TotalGold { get; set; } = 0;

    // 자원 가격표
    public static int GetResourcePrice(MineralType type)
    {
        int basePrice;

        switch (type)
        {
            case MineralType.Diamond:
                basePrice = 100;
                break;

            case MineralType.Gold:
                basePrice = 50;
                break;

            case MineralType.Silver:
                basePrice = 25;
                break;

            case MineralType.Copper:
                basePrice = 10;
                break;

            default:
                basePrice = 0;
                break;
        }

        return (int)(basePrice * PriceBonus());
    }

}

