
public class TitleScene : Scene
{
    private MenuList _titleMenu;

    public TitleScene()
    {
        Init();
    }

    public void Init()
    {
        _titleMenu = new MenuList();
        _titleMenu.Add("게임 시작", GameStart);
        _titleMenu.Add("게임 방법", HowToPlay);
        _titleMenu.Add("게임 종료", GameQuit);
    }

    public override void Enter()
    {
        _titleMenu.Reset();
        Debug.Log("타이틀 씬 진입");
    }

    public override void Update()
    {
        if (InputManager.GetKey(ConsoleKey.UpArrow))
            _titleMenu.SelectUp();
        
        if (InputManager.GetKey(ConsoleKey.DownArrow))
            _titleMenu.SelectDown();
        
        if (InputManager.GetKey(ConsoleKey.Enter))
            _titleMenu.Select();
    }
    
    public override void Render()
    {
        DrawLogo();
        DrawStats();
        _titleMenu.Render(20, 12);
        DrawFooter();
    }

    public override void Exit()
    {
    }

    private void DrawLogo()
    {
        Console.SetCursorPosition(5, 2);
        "--------------------------------------------".Print(ConsoleColor.DarkYellow);
        Console.SetCursorPosition(5, 3);
        "         수집 제작 타운라이프               ".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(5, 4);
        "--------------------------------------------".Print(ConsoleColor.DarkYellow);
        
        Console.SetCursorPosition(5, 6);
        "   광부가 되어 자원을 수집하고".Print(ConsoleColor.Gray);
        Console.SetCursorPosition(5, 7);
        "   마을을 발전시키세요!".Print(ConsoleColor.Gray);
    }

    private void DrawStats()
    {
        Console.SetCursorPosition(5, 9);
        "[ 현재 진행 상황 ]".Print(ConsoleColor.Cyan);
        
        Console.SetCursorPosition(5, 10);
        $"골드: {DataManager.Gold}G | 창고: Lv.{DataManager.WarehouseLevel} | 연구소: Lv.{DataManager.LaboratoryLevel} | 상점: Lv.{DataManager.MarketLevel}".Print(ConsoleColor.Gray);
    }

    private void DrawFooter()
    {
        Console.SetCursorPosition(5, 18);
        "--------------------------------------------".Print(ConsoleColor.DarkGray);
        
        Console.SetCursorPosition(5, 19);
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

    public void HowToPlay()
    {
        SceneManager.Change("HowToPlay");
    }
}
