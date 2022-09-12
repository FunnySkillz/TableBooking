namespace Core.Contracts
{
    public interface IUnitOfWork: IDisposable
    {

        IDaTableRepository TableRepository { get; }
        IPersonRepository PersonRepository { get; }

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        Task CreateDatabaseAsync();

        Task FillDbAsync();
    }
}
