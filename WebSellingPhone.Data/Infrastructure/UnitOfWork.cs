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
        private IGenericRepository<RefreshToken>? _refreshTokenRepository;

        public UnitOfWork(PhoneWebDbContext context)
        {
            _context = context;
        }

        public PhoneWebDbContext Context => _context;

        public IGenericRepository<Users> UserRepository => _userRepository ??= new GenericRepository<Users>(_context);

        public IGenericRepository<Role> RoleRepository => _roleRepository ??= new GenericRepository<Role>(_context);

        public IGenericRepository<Review> ReviewRepository => _reviewRepository ??= new GenericRepository<Review>(_context);

        public IGenericRepository<Promotion> PromotionRepository => _promotionRepository ??= new GenericRepository<Promotion>(_context);

        public IGenericRepository<Product> ProductRepository => _productRepository ??= new GenericRepository<Product>(_context);

        public IGenericRepository<Order> OrderRepository => _orderRepository ??= new GenericRepository<Order>(_context);

        public IGenericRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository ??= new GenericRepository<OrderDetail>(_context);

        public IGenericRepository<Brand> BrandRepository => _brandRepository ??= new GenericRepository<Brand>(_context);

        public IGenericRepository<RefreshToken> RefreshTokenRepository => _refreshTokenRepository;

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
