using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_API_Test
{
    public static class DbSetMockExtensions
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(this IEnumerable<T> source) where T : class
        {
            var queryable = source.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(s => source.ToList().Add(s));
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(s => source.ToList().AddRange(s));
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(s => source.ToList().Remove(s));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(s => source.ToList().RemoveAll(s.Contains));

            return mockSet;
        }
    }
}
