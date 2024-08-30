using Moq;
using Microsoft.EntityFrameworkCore;

namespace WebSellingPhone.UnitTest
{
    public static class DbSetMock
    {
        public static DbSet<T> Create<T>(IEnumerable<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(item =>
            {
                var list = queryableData.ToList();
                list.Add(item);
                queryableData = list.AsQueryable();
            });

            dbSetMock.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(item =>
            {
                var list = queryableData.ToList();
                list.Remove(item);
                queryableData = list.AsQueryable();
            });

            return dbSetMock.Object;
        }
    }
}