namespace _260107.Managers;

public enum MineralType
{
    Copper,
    Silver,
    Gold,
    Diamond
    // 다이아, 금, 은, 동
}

// public enum UpgradeType
// {
//     Warehouse,
//     Laboratory,
//     Market,
//     Pickaxe
// }

public class DataManager
{
    // 플레이어 재화
    public static int Gold { get; set; } = 100; // 기본 재화

    public static int WarehouseLevel { get; set; } = 1; // 창고(인벤토리)
    public static int LaboratoryLevel { get; set; } = 0; // 연구소
    public static int StoreLevel { get; set; } = 0; // 상점
    public static int PickaxeLevel { get; set; } = 1; // 기본 곡괭이


    // 창고 레벨업 -> 인벤토리 확장
    public static int MaxInventory()
    {
        return 5 + (WarehouseLevel * 3);
    }

    // 연구소 레벨업 -> 희귀광물 드랍 
    public static float RareMineral()
    {
        return 1.0f + (LaboratoryLevel * 0.3f); // 30% 증가는 너무 혜자인가?
    }

    // 상점 레벨업 -> 광물 판매 가격 상승
    public static float PriceBonus()
    {
        return 1.0f + (StoreLevel * 0.1f); // 10% 
    }

    // 곡괭이 레벨업 -> 채굴 보너스
    public static int MiningPower()
    {
        return PickaxeLevel; // 한 번에 채굴할 수 있는 양
    }


    // 업그레이드를 하려고 할 때
    public static int UpgradeCost(string buildingName, int currentLevel)
    {
        int baseCost;

        switch (buildingName)
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
}