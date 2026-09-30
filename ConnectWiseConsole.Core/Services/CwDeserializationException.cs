namespace ConnectWiseConsole.Core.Services;

public class CwDeserializationException : Exception
{
    public Type TargetType { get; }
    public string RawJson { get; }

    private CwDeserializationException(string message, Type targetType, string rawJson, Exception? inner = null)
        : base(message, inner)
    {
        TargetType = targetType;
        RawJson = rawJson;
    }

    public static CwDeserializationException NullResult(Type targetType, string rawJson) =>
        new($"Deserializing into {targetType.Name} produced a null result.", targetType, rawJson);

    public static CwDeserializationException ParseFailure(Type targetType, string rawJson, Exception inner) =>
        new($"Failed to parse response as {targetType.Name}.", targetType, rawJson, inner);
}