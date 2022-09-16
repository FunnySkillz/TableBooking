namespace Core.Contracts
{
    public interface IUnitOfWork: IDisposable
    {

        IDaTableRepository TableRepository { get; }
        IPersonRepository PersonRepository { get; }
        IBookingRepository BookingRepository { get; }

        Task<int> SaveChangesAsync();
        Task DeleteDatabaseAsync();
        Task MigrateDatabaseAsync();
        Task CreateDatabaseAsync();

        Task FillDbAsync();
    }
}
