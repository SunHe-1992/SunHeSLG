namespace RedBjorn.Utils
{
    public class Locker
    {
        int Value;

        public void Lock()
        {
            Value++;
        }

        public void Unlock()
        {
            Value--;
        }

        bool Locked { get { return Value > 0; } }

        public static implicit operator bool(Locker locker)
        {
            return !object.ReferenceEquals(locker, null) && locker.Locked;
        }
    }
}