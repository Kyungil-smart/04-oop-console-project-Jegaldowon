
public class PlayerCharacter : GameObject
{
    public Tile[,] Field { get; set; }
    public ResourceInventory Inventory { get; private set; }
    public bool IsActiveControl { get; private set; }

    // 상호작용 정보
    private GameObject _nearbyObject;
    private string _interactionHint = "";

    public PlayerCharacter()
    {
        Symbol = 'P';
        IsActiveControl = true;
        Inventory = new ResourceInventory();
    }

    public void Update()
    {
        if (!IsActiveControl) return;

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
            if (_nearbyObject is IInteractable interactable)
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
        GameObject nextObject = nextTile.OnTileObject;

        // 건물이나 벽은 통과 불가
        if (nextObject is Building)
        {
            _nearbyObject = nextObject;
            Building building = (Building)nextObject;
            _interactionHint = "[Space] " + building.Name;
            return;
        }

        // 자원은 수집 가능
        if (nextObject is Resource resource)
        {
            if (CanCollect())
            {
                CollectResource(resource);
                Field[nextPos.Y, nextPos.X].OnTileObject = null;
            }
            else
            {
                _interactionHint = "인벤토리가 가득 찼습니다!";
                return;
            }
        }

        // 이동 처리
        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;
        Position = nextPos;

        // 주변 오브젝트 체크 리셋
        _nearbyObject = null;
        _interactionHint = "";
        CheckNearbyObjects();
    }

    private void CheckNearbyObjects()
    {
        Vector[] directions = { Vector.Up, Vector.Down, Vector.Left, Vector.Right };

        foreach (var dir in directions)
        {
            Vector checkPos = Position + dir;

            if (checkPos.Y < 0 || checkPos.Y >= Field.GetLength(0) ||
                checkPos.X < 0 || checkPos.X >= Field.GetLength(1))
                continue;

            GameObject obj = Field[checkPos.Y, checkPos.X].OnTileObject;

            if (obj is Building building)
            {
                _nearbyObject = obj;
                _interactionHint = "[Space] " + building.Name;
                return;
            }
        }
    }

    public bool CanCollect()
    {
        return !Inventory.IsFull;
    }

    public void CollectResource(Resource resource)
    {
        Inventory.Add(resource);
    }

    public void Render(int uiY)
    {
        // 상태 정보 UI 렌더링
        Console.SetCursorPosition(0, uiY);
        "---------------------------------------".Print(ConsoleColor.DarkGray);

        Console.SetCursorPosition(0, uiY + 1);
        (" 골드: " + DataManager.Gold.ToString().PadLeft(6) + "G  ").Print(ConsoleColor.Yellow);
        ("| 인벤토리: " + Inventory.Count + "/" + Inventory.MaxSize).Print(ConsoleColor.White);

        Console.SetCursorPosition(0, uiY + 2);
        "---------------------------------------".Print(ConsoleColor.DarkGray);

        // 상호작용 힌트
        if (!string.IsNullOrEmpty(_interactionHint))
        {
            Console.SetCursorPosition(0, uiY + 3);
            _interactionHint.Print(ConsoleColor.Cyan);
        }
    }

    public void RenderInventoryPreview(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        "[ 보유 자원 ]".Print(ConsoleColor.Yellow);

        var summary = Inventory.GetResourceSummary();
        int line = 1;

        foreach (var pair in summary)
        {
            Console.SetCursorPosition(x, y + line);

            string name;
            ConsoleColor color;

            switch (pair.Key)
            {
                case ResourceType.Diamond:
                    name = "◆ 다이아";
                    color = ConsoleColor.Cyan;
                    break;
                case ResourceType.Gold:
                    name = "● 금    ";
                    color = ConsoleColor.Yellow;
                    break;
                case ResourceType.Silver:
                    name = "● 은    ";
                    color = ConsoleColor.White;
                    break;
                case ResourceType.Copper:
                    name = "● 동    ";
                    color = ConsoleColor.DarkYellow;
                    break;
                default:
                    name = "? ???   ";
                    color = ConsoleColor.Gray;
                    break;
            }

            name.Print(color);
            (": " + pair.Value + "개").Print();
            line++;
        }

        if (summary.Count == 0)
        {
            Console.SetCursorPosition(x, y + 1);
            "(비어있음)".Print(ConsoleColor.DarkGray);
        }
    }
}
