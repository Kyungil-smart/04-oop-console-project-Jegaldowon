
public class TitleScene : Scene
{
    private MenuList titleMenu;

    public TitleScene()
    {
        Init();
    }

    public void Init()
    {
        titleMenu = new MenuList();
        titleMenu.Add("게임 시작", GameStart);
        titleMenu.Add("게임 방법", HowPlay);
        titleMenu.Add("게임 종료", GameQuit);
    }

    public override void Enter()
    {
        titleMenu.Reset();
        Debug.Log("타이틀 씬 진입");
    }

    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.UpArrow))
            titleMenu.SelectUp();

        if (InputManager.GetKey(ConsoleKey.DownArrow))
            titleMenu.SelectDown();

        if (InputManager.GetKey(ConsoleKey.Enter))
            titleMenu.Select();
    }

    public override void Render()
    {
        DrawLogo();
        titleMenu.Render(19, 7);
        DrawBottom();
    }

    public override void Exit()
    {
    }

    private void DrawLogo()
    {
        Console.SetCursorPosition(5, 2);
        "--------------------------------------------".Print(ConsoleColor.DarkYellow);
        Console.SetCursorPosition(5, 3);
        "            수집 제작 타운라이프            ".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(5, 4);
        "--------------------------------------------".Print(ConsoleColor.DarkYellow);
    }

    private void DrawBottom()
    {
        Console.SetCursorPosition(5, 14);
        "--------------------------------------------".Print(ConsoleColor.DarkGray);

        Console.SetCursorPosition(5, 15);
        "방향키: 선택 이동 | Enter: 선택".Print(ConsoleColor.DarkGray);
    }

    public void GameQuit()
    {
        GameManager.IsGameOver = true;
    }

    public void GameStart()
    {
        SceneManager.Change("Town");
    }

    public void HowPlay()
    {
        SceneManager.Change("How Play");
    }
}
