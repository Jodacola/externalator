using SimpleBase;

public static class GuidExtensions
{
    public static string ToBase58(this Guid guid)
    {
        return Base58.Bitcoin.Encode(guid.ToByteArray());
    }
}
