using System.Runtime.Serialization.Formatters.Binary;

namespace RedBjorn.Utils
{
    public static class BinarySerializer
    {
        public static byte[] Serialize<T>(T obj)
        {
            byte[] data;
            using (var stream = new System.IO.MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                data = stream.ToArray();
            }
            return data;
        }

        public static T Deserialize<T>(byte[] data)
        {
            object obj;
            using (var stream = new System.IO.MemoryStream(data))
            {
                var formatter = new BinaryFormatter();
                obj = formatter.Deserialize(stream);
            }
            return (T)obj;
        }
    }
}
