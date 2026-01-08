
public interface IIteractable
{
    public void Interact(PlayerCharacter player);
}

public class Box : Item, IIteractable
{
    public override void Use()
    {
        
    }

    public void Interact(PlayerCharacter player)
    {
        
    }
}