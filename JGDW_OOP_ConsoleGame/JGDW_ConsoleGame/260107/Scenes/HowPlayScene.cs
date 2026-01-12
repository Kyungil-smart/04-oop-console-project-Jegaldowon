
public class HowPlayScene : Scene
{
    public override void Enter()
    {
        Debug.Log("게임 방법 화면 진입");
    }

    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.Enter) || InputManager.GetKey(ConsoleKey.Escape))
        {
            SceneManager.Change("Title");
        }
    }

    public override void Render()
    {
        DrawTitle();
        DrawContent();
        DrawBottom();
    }

    public override void Exit()
    {
    }

    private void DrawTitle()
    {
        Console.SetCursorPosition(0, 0);
        "--------------------------------------------".Print(ConsoleColor.Cyan);
        Console.SetCursorPosition(0, 1);
        "                 게임 방법                  ".Print(ConsoleColor.Cyan);
        Console.SetCursorPosition(0, 2);
        "--------------------------------------------".Print(ConsoleColor.Cyan);
    }

    private void DrawContent()
    {
        int y = 4;
        
        // 게임 목표
        Console.SetCursorPosition(3, y++);
        "[ 게임 목표 ]".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(3, y++);
        "  광산에서 자원을 수집하고, 마을의 건물을 업그레이드하세요".Print();
        y++;
        
        // 기본 조작법
        Console.SetCursorPosition(3, y++);
        "[ 조작법 ]".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(3, y++);
        "  방향키  : 이동".Print();
        Console.SetCursorPosition(3, y++);
        "  Space   : 상호작용 (건물 입장)".Print();
        Console.SetCursorPosition(3, y++);
        "  Enter   : 선택/확인".Print();
        Console.SetCursorPosition(3, y++);
        "  ESC     : 뒤로가기/메뉴".Print();
        y++;
        
        // 자원 정보
        Console.SetCursorPosition(3, y++);
        "[ 자원 종류 ]".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(3, y++);
        "  ● ".Print(ConsoleColor.DarkYellow);
        "동광석   - 가장 흔함, 낮은 가격".Print();
        Console.SetCursorPosition(3, y++);
        "  ● ".Print(ConsoleColor.White);
        "은광석   - 보통, 적당한 가격".Print();
        Console.SetCursorPosition(3, y++);
        "  ● ".Print(ConsoleColor.Yellow);
        "금광석   - 희귀, 높은 가격".Print();
        Console.SetCursorPosition(3, y++);
        "  ◆ ".Print(ConsoleColor.Cyan);
        "다이아   - 매우 희귀, 최고 가격".Print();
        y++;
        
        // 건물 정보
        Console.SetCursorPosition(3, y++);
        "[ 건물 ]".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(3, y++);
        "  S ".Print(ConsoleColor.Green);
        "상점    - 자원 판매, 장비 구매".Print();
        Console.SetCursorPosition(3, y++);
        "  W ".Print(ConsoleColor.DarkYellow);
        "창고    - 인벤토리 용량 증가".Print();
        Console.SetCursorPosition(3, y++);
        "  L ".Print(ConsoleColor.Magenta);
        "연구소  - 희귀 자원 확률 증가".Print();
        Console.SetCursorPosition(3, y++);
        "  M ".Print(ConsoleColor.Cyan);
        "광산입구 - 채굴장 입장".Print();
    }

    private void DrawBottom()
    {
        Console.SetCursorPosition(3, 26);
        "--------------------------------------------".Print(ConsoleColor.DarkGray);
        Console.SetCursorPosition(3, 27);
        "Enter 또는 ESC를 눌러 돌아가기".Print(ConsoleColor.DarkGray);
    }
}
