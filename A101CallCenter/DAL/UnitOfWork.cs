using A101CallCenter.Db;
using A101CallCenter.Models;

namespace A101CallCenter.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        readonly A101DbContext Context;

        public UnitOfWork(A101DbContext context)
        {
            Context = context;
            CustomerRepository = new Repository<Customer>(context);
        }

        public IRepository<Customer> CustomerRepository { get; private set; } = null!;

        public async Task<int> CompleteTransactions()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
