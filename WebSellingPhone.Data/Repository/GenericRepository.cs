using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WebSellingPhone.Data.Repository
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly PhoneWebDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(PhoneWebDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            else
            {
                throw new ArgumentException($"Entity with id {id} not found.");
            }
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return orderBy != null ? orderBy(query) : query;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T? GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetQuery()
        {
            return _dbSet;
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task<T?> GetByIdAsync(Guid id, Guid userId)
        {
            if (id == Guid.Empty || userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id hoặc UserId bị rỗng!");
            }

            // Thay đổi truy vấn này theo logic của bạn. 
            // Ví dụ: nếu đối tượng T có thuộc tính UserId để lọc, bạn cần kiểm tra xem nó có thuộc tính UserId không.
            return await _dbSet
                .Where(entity => EF.Property<Guid>(entity, "Id") == id && EF.Property<Guid>(entity, "UserId") == userId)
                .FirstOrDefaultAsync();
        }
    }
}
