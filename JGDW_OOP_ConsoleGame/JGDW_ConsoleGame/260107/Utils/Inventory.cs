
public class Inventory
{
    private List<Mineral> _resources = new List<Mineral>();

    public int MaxSize
    {
        get { return DataManager.MaxInventorySize(); }
    }

    public int Count
    {
        get { return _resources.Count; }
    }

    public bool IsFull
    {
        get { return Count >= MaxSize; }
    }

    public bool IsEmpty
    {
        get { return Count == 0; }
    }

    public void Add(Mineral resource)
    {
        if (IsFull)
        {
            return;
        }
        _resources.Add(resource);
    }

    public void Clear()
    {
        _resources.Clear();
    }

    public List<Mineral> AllMineral()
    {
        List<Mineral> copy = new List<Mineral>();
        for (int i = 0; i < _resources.Count; i++)
        {
            copy.Add(_resources[i]);
        }
        return copy;
    }

    public Dictionary<MineralType, int> MineralGetInfo()
    {
        Dictionary<MineralType, int> summary = new Dictionary<MineralType, int>();

        for (int i = 0; i < _resources.Count; i++)
        {
            Mineral resource = _resources[i];
            if (summary.ContainsKey(resource.Type))
            {
                summary[resource.Type]++;
            }
            else
            {
                summary[resource.Type] = 1;
            }
        }

        return summary;
    }

    public int TotalValue()
    {
        int total = 0;
        for (int i = 0; i < _resources.Count; i++)
        {
            total += _resources[i].GetPrice();
        }
        return total;
    }

    // 특정 타입 자원 개수
    public int GetCount(MineralType type)
    {
        int count = 0;
        for (int i = 0; i < _resources.Count; i++)
        {
            if (_resources[i].Type == type)
            {
                count++;
            }
        }
        return count;
    }

    // 특정 타입 자원 모두 제거하고 반환
    public List<Mineral> RemoveAll(MineralType type)
    {
        List<Mineral> removed = new List<Mineral>();

        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            if (_resources[i].Type == type)
            {
                removed.Add(_resources[i]);
                _resources.RemoveAt(i);
            }
        }

        return removed;
    }

    // 모든 자원 판매
    public int SellAll()
    {
        int totalGold = TotalValue();
        _resources.Clear();
        return totalGold;
    }
}
