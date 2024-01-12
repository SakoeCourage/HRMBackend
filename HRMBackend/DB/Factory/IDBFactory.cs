namespace HRMBackend.DB.Factory
{
    

    public interface IDBFactory<T>
    {
        static List<T> Data { get; }
    }
}
