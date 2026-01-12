

public static class SceneManager
{
    public static Action OnChangeScene;
    public static Scene current { get; private set; }
    private static Scene prev;

    private static Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

    public static void AddScene(string key, Scene state)
    {
        if (_scenes.ContainsKey(key)) return;
        
        _scenes.Add(key, state);
    }

    public static void ChangePrevScene()
    {
        Change(prev);
    }

    // 상태 바꾸는 기능
    public static void Change(string key)
    {
        if (!_scenes.ContainsKey(key)) return;
        
        Change(_scenes[key]);
    }

    public static void Change(Scene scene)
    {
        Scene next = scene;
        
        if (current == next) return;

        current?.Exit();
        next.Enter();
        
        prev = current;
        current = next;
        OnChangeScene?.Invoke();
    }

    public static void Update()
    {
        current?.Update();
    }

    public static void Render()
    {
        current?.Render();
    }
}