using Core.Entities;
using System.Collections.Generic;
using System;

namespace Core.Contracts
{
    public interface IDaTableRepository
    {
        Task<int> CountAsync();
        Task<List<DaTable>> GetTablesByPersonNameAsync(string firstName, string lastName);
        Task AddAsync(DaTable newTable);
        Task<List<DaTable>> GetAllAsync(int? person_id);
        Task<List<DaTable>> GetAll();

    }
}