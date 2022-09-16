using Core.Contracts;
using Core.Dtos;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Utils;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        const string FILENAME = "Bookings.csv";

        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public IDaTableRepository TableRepository { get; }
        public IPersonRepository PersonRepository { get; }
        public IBookingRepository BookingRepository { get; }

        public UnitOfWork() : this(new ApplicationDbContext())
        { }

        private UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = new ApplicationDbContext();

            TableRepository = new DaTableRepository(_dbContext);
            PersonRepository = new PersonRepository(_dbContext);
            BookingRepository = new BookingsRepository(_dbContext);
        }

        public UnitOfWork(IConfiguration configuration) : this(new ApplicationDbContext(configuration))
        { }

        public async Task<int> SaveChangesAsync()
        {
            var entities = _dbContext!.ChangeTracker.Entries()
                .Where(entity => entity.State == EntityState.Added
                                 || entity.State == EntityState.Modified)
                .Select(e => e.Entity)
                .ToArray();  // Geänderte Entities ermitteln

            // Allfällige Validierungen der geänderten Entities durchführen
            foreach (var entity in entities)
            {
                ValidateEntity(entity);
            }
            return await _dbContext.SaveChangesAsync();

        }

        private void ValidateEntity(object entity)
        {
            if (entity is Person newPerson)
            {
                if (_dbContext.Persons.Any(p => p.Id != newPerson.Id && p.LastName.ToUpper() == newPerson.LastName.ToUpper()
                  && p.FirstName.ToUpper() == newPerson.FirstName.ToUpper()))
                {
                    throw new ValidationException(
                        new ValidationResult("Es gibt bereits eine Person mit diesem Namen!",
                           new List<String> { nameof(Person.FirstName), nameof(Person.LastName) })
                        , null, newPerson);
                }
            }
        }

        public async Task DeleteDatabaseAsync() => await _dbContext!.Database.EnsureDeletedAsync();
        public async Task MigrateDatabaseAsync() => await _dbContext!.Database.MigrateAsync();
        public async Task CreateDatabaseAsync() => await _dbContext!.Database.EnsureCreatedAsync();

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await _dbContext.DisposeAsync();
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task FillDbAsync()
        {
            await this.DeleteDatabaseAsync();
            await this.MigrateDatabaseAsync();

            //Todo: Implementierung des Einlesens aus der csv-Datei Booking.csv und Perstierung in die Datenbank

            List<Person> persons;
            List<DaTable> tables;
            List<Booking> bookings;
            List<PersonTableSummary> personTableSummaries;

            string[][] csvFile = await MyFile.ReadStringMatrixFromCsvAsync(FILENAME, true);

            tables = csvFile.Select(line =>
                new DaTable()
                {
                    TableNumber = Convert.ToInt32(line[4]),
                    QRCode = Convert.ToString(line[5]),
                }).ToList();

            persons = csvFile.Select(line =>
                new Person
                {
                    LastName = Convert.ToString(line[0]),
                    FirstName = Convert.ToString(line[1]),
                    PhoneNumber = Convert.ToString(line[2]),
                    Email = Convert.ToString(line[3]),
                }).ToList();

            bookings = csvFile.Select(line => 
                new Booking() 
                { 
                    Person = persons
                                .Where(p => 
                                    p.LastName.Equals(line[0]) 
                                    && p.FirstName.Equals(line[1]) 
                                    && p.PhoneNumber.Equals(line[2]))
                                .SingleOrDefault(),
                    
                    Table = tables
                                .Where(t =>
                                    t.TableNumber.Equals(line[4])
                                    && t.QRCode.Equals(line[5]))
                                .SingleOrDefault(),
                    
                }).ToList();

            await _dbContext.Bookings.AddRangeAsync(bookings);
            await _dbContext.Persons.AddRangeAsync(persons);
            await _dbContext.Tables.AddRangeAsync(tables);

            await SaveChangesAsync();
        }
    }

   
}
