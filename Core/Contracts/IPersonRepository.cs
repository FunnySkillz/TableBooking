using Core.Entities;
using System.Collections.Generic;
using System;

namespace Core.Contracts
{
    public interface IPersonRepository
    {
        Task<int> CountAsync();
        Task InsertAsync(Person newPerson);
        Task<List<Person>> GetAllOrderedByLastNameAsync();
        Task<Person?> GetPersonByNameAsync(string firstName, string lastName);
    }
}