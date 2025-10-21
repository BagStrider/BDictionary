Type-Key oriented Dictionary (Dictionary<Type, TClass>)


## **Example:**

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private SerializedTypeDictionary<Stat> _stats = new ();

    public Stat GetStat<T>() where T : Stat
    {
        if (!_stats.TryGet(out Stat stat))
        {
            throw new KeyNotFoundException("No stat found with name " + typeof(T).Name);
        }
            
        return stat;
    }
}

## **In Inspector:**




( ͡° ͜ʖ ͡°)

Made by BagStrider:
Discord: @BagStrider


