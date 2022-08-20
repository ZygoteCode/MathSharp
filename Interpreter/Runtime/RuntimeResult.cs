public class RuntimeResult
{
    public Number Value { get; set; }
    public Error Error { get; set; }

    public Number Register(RuntimeResult result)
    {
        if (result.Error != null)
        {
            Error = result.Error;
        }

        return result.Value;
    }

    public RuntimeResult Success(Number value)
    {
        Value = value;
        return this;
    }

    public RuntimeResult Failure(Error error)
    {
        Error = error;
        return this;
    }
}