using ApplicationLayer.IRepositories;
using CoreLayer.Entities;
using SqlKata;
using BC = BCrypt.Net.BCrypt;
using SqlKata.Execution;
using CoreLayer.Enums;

namespace InfrastructureLayer.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly QueryFactory _queryFactory;
        public AccountRepository(QueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public async Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            var account = await _queryFactory.Query(Table.ACCOUNTS_TABLE).Where(Table.ACCOUNTS_TABLE_EMAIL, "=", email).FirstOrDefaultAsync<Account>();
            if (account != null)
            {
                bool flag = BC.Verify(password, account.Password);
                return flag ? account : null;
            }
            return null;
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _queryFactory.Query(Table.ACCOUNTS_TABLE).Where(Table.ACCOUNTS_TABLE_EMAIL, "=", email).FirstOrDefaultAsync<Account>();
        }

        public async Task<Account?> GetByIdAsync(Guid id)
        {
            return await _queryFactory.Query(Table.ACCOUNTS_TABLE).Where(Table.ACCOUNTS_TABLE_ACCOUNTID, "=", id).FirstOrDefaultAsync<Account>();
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _queryFactory.Query(Table.ACCOUNTS_TABLE).GetAsync<Account>();
        }

        public Task<int> AddAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Account entity)
        {
            return await _queryFactory.Query(Table.ACCOUNTS_TABLE).Where(Table.ACCOUNTS_TABLE_ACCOUNTID, "=", entity.AccountId).UpdateAsync(new
            {
                Email = entity.Email,
                Password = entity.Password,
                Status = entity.Status,
                Role = entity.Role
            });
        }

        public Task<int> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}