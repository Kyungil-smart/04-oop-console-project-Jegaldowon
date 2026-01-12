
public class ResourceInventory
{
    private List<Resource> _resources = new List<Resource>();

    public int MaxSize => DataManager.GetMaxInventorySize();
    public int Count => _resources.Count;
    public bool IsFull => Count >= MaxSize;
    public bool IsEmpty => Count == 0;

    public void Add(Resource resource)
    {
        if (IsFull) return;
        _resources.Add(resource);
    }

    public void Clear()
    {
        _resources.Clear();
    }

    public List<Resource> GetAllResources()
    {
        return new List<Resource>(_resources);
    }

    public Dictionary<ResourceType, int> GetResourceSummary()
    {
        var summary = new Dictionary<ResourceType, int>();

        foreach (var resource in _resources)
        {
            if (summary.ContainsKey(resource.Type))
                summary[resource.Type]++;
            else
                summary[resource.Type] = 1;
        }

        return summary;
    }

    public int GetTotalValue()
    {
        int total = 0;
        foreach (var resource in _resources)
        {
            total += resource.GetPrice();
        }
        return total;
    }

    // 특정 타입 자원 개수
    public int GetCount(ResourceType type)
    {
        return _resources.Count(r => r.Type == type);
    }

    // 특정 타입 자원 모두 제거하고 반환
    public List<Resource> RemoveAll(ResourceType type)
    {
        var removed = _resources.Where(r => r.Type == type).ToList();
        _resources.RemoveAll(r => r.Type == type);
        return removed;
    }

    // 모든 자원 판매
    public int SellAll()
    {
        int totalGold = GetTotalValue();
        _resources.Clear();
        return totalGold;
    }
}
