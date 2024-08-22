using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Data.Models;
using WebSellingPhone.Data.Repository;

namespace WebSellingPhone.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        PhoneWebDbContext Context { get; }

        IGenericRepository<Users> UserRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<Review> ReviewRepository { get; }
        IGenericRepository<Promotion> PromotionRepository {  get; }
        IGenericRepository<Product> ProductRepository { get; }
        IGenericRepository<Order> OrderRepository { get; }

        IGenericRepository<OrderDetail> OrderDetailRepository { get; }
        IGenericRepository<Brand> BrandRepository { get; }
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();

    }
}
