namespace HoloCommon.Interfaces
{
    public interface IBinarySerializable
    {
        byte[] Serialize();
        IBinarySerializable Deserialize(byte[] bytes);
    }
}