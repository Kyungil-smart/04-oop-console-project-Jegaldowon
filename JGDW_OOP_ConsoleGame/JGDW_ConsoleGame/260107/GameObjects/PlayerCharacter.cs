
public class PlayerCharacter : GameObject
{
    public Tile[,] Field { get; set; }
    public Inventory Inventory { get; private set; }
    public bool IsActive { get; private set; }

    // 상호작용 정보
    private GameObject nearObject;
    private string interactionHint = "";

    public PlayerCharacter()
    {
        Symbol = 'P';
        IsActive = true;
        Inventory = new Inventory();
    }

    public void Update()
    {
        if (!IsActive) return;

        if (InputManager.GetKey(ConsoleKey.UpArrow))
            Move(Vector.Up);

        if (InputManager.GetKey(ConsoleKey.DownArrow))
            Move(Vector.Down);

        if (InputManager.GetKey(ConsoleKey.LeftArrow))
            Move(Vector.Left);

        if (InputManager.GetKey(ConsoleKey.RightArrow))
            Move(Vector.Right);

        // Space로 상호작용
        if (InputManager.GetKey(ConsoleKey.Spacebar))
        {
            if (nearObject is IInteractable interactable)
            {
                interactable.Interact(this);
            }
        }
    }

    private void Move(Vector direction)
    {
        if (Field == null) return;

        Vector nextPos = Position + direction;

        // 맵 범위 체크
        if (nextPos.Y < 0 || nextPos.Y >= Field.GetLength(0) ||
            nextPos.X < 0 || nextPos.X >= Field.GetLength(1))
            return;

        Tile nextTile = Field[nextPos.Y, nextPos.X];
        GameObject nextObject = nextTile.SymbolTile;

        // 건물이나 벽은 통과 불가
        if (nextObject is Building)
        {
            nearObject = nextObject;
            Building building = (Building)nextObject;
            interactionHint = "[Space] " + building.Name;
            return;
        }

        // 자원은 수집 가능
        if (nextObject is Mineral mineral)
        {
            if (Collect())
            {
                CollectMineral(mineral);
                Field[nextPos.Y, nextPos.X].SymbolTile = null;
            }
            else
            {
                interactionHint = "인벤토리가 가득 찼습니다!";
                return;
            }
        }

        // 이동 처리
        Field[Position.Y, Position.X].SymbolTile = null;
        Field[nextPos.Y, nextPos.X].SymbolTile = this;
        Position = nextPos;

        // 주변 오브젝트 체크 리셋
        nearObject = null;
        interactionHint = "";
        CheckNearObject();
    }

    // 상하좌우 체크하고
    private void CheckNearObject()
    {
        // 상
        CheckDirection(Position.X, Position.Y - 1);
        if (nearObject != null) return;

        // 하
        CheckDirection(Position.X, Position.Y + 1);
        if (nearObject != null) return;

        // 좌
        CheckDirection(Position.X - 1, Position.Y);
        if (nearObject != null) return;

        // 우
        CheckDirection(Position.X + 1, Position.Y);
    }

    // 심볼타일의 정보(상호작용 어떤 타일인지)
    private void CheckDirection(int x, int y)
    {
        // 맵 범위 체크
        if (y < 0 || y >= Field.GetLength(0))
        {
            return;
        }
        if (x < 0 || x >= Field.GetLength(1))
        {
            return;
        }

        // 해당 위치의 오브젝트 확인
        GameObject obj = Field[y, x].SymbolTile;

        if (obj is Building)
        {
            Building building = (Building)obj;
            nearObject = obj;
            interactionHint = "[Space] " + building.Name;
        }
    }

    public bool Collect()
    {
        return !Inventory.IsFull;
    }

    public void CollectMineral(Mineral mineral)
    {
        Inventory.Add(mineral);
    }

    public void Render(int uiRender)
    {
        // 상태 정보 UI 렌더링
        Console.SetCursorPosition(0, uiRender);
        "---------------------------------------".Print(ConsoleColor.DarkGray);

        Console.SetCursorPosition(0, uiRender + 1);
        (" 골드 : " + DataManager.Gold.ToString().PadLeft(6) + " G ").Print(ConsoleColor.Yellow);
        ("| 인벤토리 : " + Inventory.Count + "/" + Inventory.MaxSize).Print(ConsoleColor.White);

        Console.SetCursorPosition(0, uiRender + 2);
        "---------------------------------------".Print(ConsoleColor.DarkGray);

        // 상호작용 힌트
        if (!string.IsNullOrEmpty(interactionHint))
        {
            Console.SetCursorPosition(0, uiRender + 3);
            interactionHint.Print(ConsoleColor.Cyan);
        }
    }

    public void InventoryPanel(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        "[ 보유 자원 ]".Print(ConsoleColor.Yellow);

        var summary = Inventory.MineralGetInfo();
        int line = 1;

        foreach (var pair in summary)
        {
            Console.SetCursorPosition(x, y + line);

            string name;
            ConsoleColor color;

            switch (pair.Key)
            {
                case MineralType.Diamond:
                    name = "◆ 다이아";
                    color = ConsoleColor.Cyan;
                    break;
                case MineralType.Gold:
                    name = "● 금 ";
                    color = ConsoleColor.Yellow;
                    break;
                case MineralType.Silver:
                    name = "● 은";
                    color = ConsoleColor.White;
                    break;
                case MineralType.Copper:
                    name = "● 동";
                    color = ConsoleColor.DarkYellow;
                    break;
                default:
                    name = "None";
                    color = ConsoleColor.Gray;
                    break;
            }

            name.Print(color);
            (" : " + pair.Value + "개").Print();
            line++;
        }

        if (summary.Count == 0)
        {
            Console.SetCursorPosition(x, y + 1);
            "(비어있음)".Print(ConsoleColor.DarkGray);
        }
    }
}
