using A101CallCenter.Models;

namespace A101CallCenter.DAL
{
    public interface IUnitOfWork
    {
        IRepository<Customer> CustomerRepository { get; }
        Task<int> CompleteTransactions();
    }
}
