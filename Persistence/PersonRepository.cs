using Core.Contracts;
using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Core.Dtos;

namespace Persistence
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Persons.CountAsync();
        }

        public async Task<List<Person>> GetAllOrderedByLastNameAsync()
        {
            return await _dbContext.Persons.OrderBy(p => p.LastName).ToListAsync();
        }

        public async Task<Person?> GetPersonByNameAsync(string firstName, string lastName)
        {
            return await _dbContext
                .Persons
                .SingleOrDefaultAsync(p => 
                    p.FirstName.ToUpper() == firstName.ToUpper() 
                    && p.LastName.ToUpper() == lastName.ToUpper());
        }

        public async Task InsertAsync(Person newPerson)
        {
            await _dbContext.Persons.AddAsync(newPerson);
        }

    }
}