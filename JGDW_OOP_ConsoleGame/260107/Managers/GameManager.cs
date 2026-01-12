using System;

public class GameManager
{
    public static bool IsGameOver { get; set; }
    public const string GameName = "수집 제작 타운라이프";
    private PlayerCharacter _player;

    public void Run()
    {
        Init();

        while (!IsGameOver)
        {
            // 렌더링
            Console.Clear();
            SceneManager.Render();
            
            // 키입력 받기
            InputManager.GetUserInput();

            // 디버그 로그 보기 (L키)
            if (InputManager.GetKey(ConsoleKey.L))
            {
                SceneManager.Change("Log");
            }

            // 데이터 처리
            SceneManager.Update();
        }
        
        // 게임 종료 메시지
        Console.Clear();
        Console.SetCursorPosition(0, 5);
        "---------------------------------------".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(0, 6);
        "      게임을 종료합니다. 감사합니다!   ".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(0, 7);
        "---------------------------------------".Print(ConsoleColor.Yellow);
        Console.SetCursorPosition(0, 9);
        $"  최종 골드: {DataManager.Gold}G".Print();
        Console.SetCursorPosition(0, 10);
        $"  수집한 자원: {DataManager.TotalResourcesCollected}개".Print();
        Console.SetCursorPosition(0, 11);
        $"  총 수익: {DataManager.TotalGoldEarned}G".Print();
        Console.SetCursorPosition(0, 14);
    }

    private void Init()
    {
        IsGameOver = false;
        
        // 콘솔 설정
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        // 이벤트 연결
        SceneManager.OnChangeScene += InputManager.ResetKey;
        
        // 플레이어 생성
        _player = new PlayerCharacter();

        // 씬 등록
        SceneManager.AddScene("Title", new TitleScene());
        SceneManager.AddScene("Town", new TownScene(_player));
        SceneManager.AddScene("Field", new FieldScene(_player));
        SceneManager.AddScene("Shop", new ShopScene(_player));
        SceneManager.AddScene("HowToPlay", new HowToPlayScene());
        SceneManager.AddScene("Log", new LogScene());
        
        // 건물 업그레이드 씬들
        SceneManager.AddScene("BuildingUpgrade_Warehouse", 
            new BuildingUpgradeScene(_player, BuildingUpgradeScene.BuildingType.Warehouse));
        SceneManager.AddScene("BuildingUpgrade_Laboratory", 
            new BuildingUpgradeScene(_player, BuildingUpgradeScene.BuildingType.Laboratory));
        SceneManager.AddScene("BuildingUpgrade_Market", 
            new BuildingUpgradeScene(_player, BuildingUpgradeScene.BuildingType.Market));

        // 시작 씬
        SceneManager.Change("Title");

        Debug.Log("게임 데이터 초기화 완료");
        Debug.Log("수집 제작 타운라이프에 오신 것을 환영합니다!");
    }
}
