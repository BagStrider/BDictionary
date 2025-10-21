Type-Key oriented Dictionary (Dictionary<Type, TClass>)

The Type of class is cached in key


## **Example:**

#### **Dictionary holder:**
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

#### **Parent class:**
    [Serializable]
    public abstract class Stat
    {
        public event Action<Stat> OnValueChanged;
        public abstract object Value { get; set; }
        protected void NotifyChanged() => OnValueChanged?.Invoke(this);
    }

#### **Inheritor of parent:**    

    [Serializable] public class MaxHealth : Stat<float> { }
    [Serializable] public class Speed : Stat<float> { }
    [Serializable] public class AttackSpeed : Stat<float> { }

## **In Inspector:**

<img width="523" height="376" alt="image_2025-10-21_07-32-48" src="https://github.com/user-attachments/assets/219a7466-d25e-4057-87df-2d78498df939" />


( ͡° ͜ʖ ͡°)

## **Contacts:**

Discord: @BagStrider





