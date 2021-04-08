namespace HoloCommon.Interfaces
{
    public interface IBinarySerialization<T>
    {
        byte[] Serialize(T obj);
        T Deserialize(byte[] bytes);
    }
}