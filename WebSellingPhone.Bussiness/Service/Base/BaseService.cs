using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Infrastructure;

namespace WebSellingPhone.Bussiness.Service.Base
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly ILogger<BaseService<T>> _logger;
        protected readonly IUnitOfWork _unitOfWork;

        public BaseService(IUnitOfWork unitOfWork, ILogger<BaseService<T>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public virtual async Task<int> AddAsync(T entity)
        {
            if (entity == null)
            {
                _logger.LogError("Entity is null!");
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.GenericRepository<T>().Add(entity);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
                _logger.LogInformation("Entity added successfully!");
            else
                _logger.LogError("Entity add failed!");

            return result;
        }

        public virtual bool Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Id is empty!");
                throw new ArgumentNullException(nameof(id));
            }

            _unitOfWork.GenericRepository<T>().Delete(id);

            var result = _unitOfWork.SaveChanges();

            if (result > 0)
                _logger.LogInformation("Delete successfully!");
            else
                _logger.LogError("Delete failed!");

            return result > 0;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null)
            {
                _logger.LogError("Entity is null!");
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.GenericRepository<T>().Delete(entity);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
                _logger.LogInformation("Delete successfully!");
            else
                _logger.LogError("Delete failed!");

            return result > 0;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("Id is empty!");
                throw new ArgumentNullException(nameof(id));
            }

            _unitOfWork.GenericRepository<T>().Delete(id);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
                _logger.LogInformation("Delete successfully!");
            else
                _logger.LogError("Delete failed!");

            return result > 0;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.GenericRepository<T>().GetAllAsync();
        }

        public virtual async Task<PaginatedResult<T>> GetAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int pageIndex = 1,
            int pageSize = 20)
        {
            var query = _unitOfWork.GenericRepository<T>().Get(filter, orderBy, includeProperties);

            return await PaginatedResult<T>.CreateAsync(query, pageIndex, pageSize);
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _unitOfWork.GenericRepository<T>().GetByIdAsync(id);
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                _logger.LogError("Entity is null!");
                throw new ArgumentNullException(nameof(entity));
            }

            _unitOfWork.GenericRepository<T>().Update(entity);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result > 0)
                _logger.LogInformation("Update successfully!");
            else
                _logger.LogError("Update failed!");

            return result;
        }
    }
}
