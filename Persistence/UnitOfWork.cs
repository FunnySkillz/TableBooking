using Core.Contracts;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
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

        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
            TableRepository = new DaTableRepository(_dbContext);
            PersonRepository = new PersonRepository(_dbContext);
        }


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


            //Todo: Implementierung des Einlesens aus der csv-Datei runresults.csv und Perstierung in die Datenbank

            List<Person> persons;
            List<DaTable> tables;

            string[][] csvFile = await MyFile.ReadStringMatrixFromCsvAsync(FILENAME, true);

            persons = csvFile.GroupBy(line => new { firstName = line[0], lastName = line[1] }).Select(grp =>
                new Person
                {
                    FirstName = grp.Key.firstName,
                    LastName = grp.Key.lastName
                }).ToList();

            tables = csvFile.Select(line =>
                new DaTable()
                {
                    Person = persons.Where(t => t.FirstName.Equals(line[0]) && t.LastName.Equals(line[1])).SingleOrDefault(),
                    TableName = Convert.ToString(line[2]),
                    QRCode = Convert.ToString(line[3]),
                }).ToList();

            await _dbContext.Persons.AddRangeAsync(persons);
            await _dbContext.Tables.AddRangeAsync(tables);


            await SaveChangesAsync();
        }
    }

   
}
