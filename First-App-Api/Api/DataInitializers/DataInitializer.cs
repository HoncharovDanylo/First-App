namespace Api.DataInitializers;

public abstract class DataInitializer<T> : DataInitializerControl
{
    public IEnumerable<T> Data;

    public DataInitializer()
    {
        Data = SkipInitData ? new List<T>() : GetData();
    }

    protected abstract IList<T> GetData();
}
