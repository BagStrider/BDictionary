Type-Key oriented Dictionary

Example:

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



( ͡° ͜ʖ ͡°)

Made by BagStrider:
Discord: @BagStrider
