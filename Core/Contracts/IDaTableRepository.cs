using Core.Entities;
using System.Collections.Generic;
using System;

namespace Core.Contracts
{
    public interface IDaTableRepository
    {
        Task<int> CountAsync();
        Task AddAsync(DaTable newTable);
        Task<List<DaTable>> GetAllAsync();

        Task<List<DaTable>> GetTablesByPerson(string firstName, string lastName);
    }
}