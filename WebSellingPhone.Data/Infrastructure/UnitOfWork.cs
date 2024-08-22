using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSellingPhone.Data.Models;
using WebSellingPhone.Data.Repository;

namespace WebSellingPhone.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PhoneWebDbContext _context;
        private IGenericRepository<Users>? _userRepository;
        private IGenericRepository<Role>? _roleRepository;
        private IGenericRepository<Review>? _reviewRepository;
        private IGenericRepository<Promotion>? _promotionRepository;
        private IGenericRepository<Product>? _productRepository;
        private IGenericRepository<Order>? _orderRepository;
        private IGenericRepository<OrderDetail>? _orderDetailRepository;
        private IGenericRepository<Brand>? _brandRepository;

        public UnitOfWork(PhoneWebDbContext context)
        {
            _context = context;
        }

        public PhoneWebDbContext Context => _context;

        public IGenericRepository<Users> UserRepository => _userRepository;

        public IGenericRepository<Role> RoleRepository => _roleRepository;

        public IGenericRepository<Review> ReviewRepository => _reviewRepository;

        public IGenericRepository<Promotion> PromotionRepository => _promotionRepository;

        public IGenericRepository<Product> ProductRepository => _productRepository;

        public IGenericRepository<Order> OrderRepository => _orderRepository;

        public IGenericRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository;

        public IGenericRepository<Brand> BrandRepository => _brandRepository;

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class
        {
            return new GenericRepository<TEntity>(_context);
        }

        public async Task RollBackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
