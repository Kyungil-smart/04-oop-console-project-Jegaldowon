
public abstract class Building : GameObject, IInteractable
{
    public string Name { get; protected set; }
    public string Description { get; set; } // 일단 s
    public ConsoleColor Color { get; set; } = ConsoleColor.White;

    public abstract void Interact(PlayerCharacter player);
}

// 창고 - 인벤토리 확장
public class Warehouse : Building
{
    public Warehouse()
    {
        Symbol = 'W';
        Name = "창고";
        Description = "인벤토리 용량을 늘려줍니다";
        Color = ConsoleColor.DarkYellow;
    }

    public override void Interact(PlayerCharacter player)
    {
        SceneManager.Change("창고 건물 업그레이드");
    }
}

// 연구소 - 희귀 자원 확률 증가
public class Laboratory : Building
{
    public Laboratory()
    {
        Symbol = 'L';
        Name = "연구소";
        Description = "희귀 자원 출현 확률을 높여줍니다";
        Color = ConsoleColor.Magenta;
    }

    public override void Interact(PlayerCharacter player)
    {
        SceneManager.Change("실험실 건물 업그레이드");
    }
}

// 상점 건물 - 판매 가격 증가
public class Shop : Building
{
    public Shop()
    {
        Symbol = 'S';
        Name = "상점";
        Description = "자원을 판매하고 장비를 구매합니다";
        Color = ConsoleColor.Green;
    }

    public override void Interact(PlayerCharacter player)
    {
        SceneManager.Change("상점");
    }
}

// 필드 입장 포탈
public class FieldPortal : Building
{
    public FieldPortal()
    {
        Symbol = 'M';
        Name = "광산 입구";
        Description = "광산으로 들어갑니다";
        Color = ConsoleColor.Cyan;
    }

    public override void Interact(PlayerCharacter player)
    {
        SceneManager.Change("Field");
    }
}

// 마을 귀환 포탈
public class TownPortal : Building
{
    public TownPortal()
    {
        Symbol = 'T';
        Name = "마을 귀환";
        Description = "마을로 돌아갑니다";
        Color = ConsoleColor.Yellow;
    }

    public override void Interact(PlayerCharacter player)
    {
        SceneManager.Change("Town");
    }
}
