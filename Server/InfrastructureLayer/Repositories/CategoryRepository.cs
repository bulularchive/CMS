﻿using ApplicationLayer.IRepositories;
using CoreLayer.Entities;
using SqlKata.Execution;

namespace InfrastructureLayer.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly QueryFactory _queryFactory;
    public CategoryRepository(QueryFactory queryFactory)
    {
        _queryFactory = queryFactory;
    }
    public Task<Category?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _queryFactory.Query(Table.CATEGORIES_TABLE).GetAsync<Category>();
    }

    public Task Add(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}