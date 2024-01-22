using Microsoft.Extensions.Caching.Memory;
using OMDBassess.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    public class SearchQueryTests
    {
        [Fact]
        public void SaveLatestSearchToCache_ShouldAddQueryToCache()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var searchQueries = new SearchQueries(cache);
            var queryToAdd = "testQuery";

            // Act
            searchQueries.SaveLatestSearchToCache(queryToAdd);

            // Assert
            var latestQueries = cache.Get<List<string>>("LatestQueries");
            Assert.NotNull(latestQueries);
            Assert.Single(latestQueries);
            Assert.Equal(queryToAdd, latestQueries[0]);
        }

        [Fact]
        public void GetLatestQueriesFromCache_WhenCacheIsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var searchQueries = new SearchQueries(cache);

            // Act
            var latestQueries = searchQueries.GetLatestQueriesFromCache();

            // Assert
            Assert.NotNull(latestQueries);
            Assert.Empty(latestQueries);
        }

        [Fact]
        public void GetLatestQueriesFromCache_WhenCacheIsNotEmpty_ShouldReturnLatestQueries()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var searchQueries = new SearchQueries(cache);
            var queriesToAdd = new List<string> { "query1", "query2", "query3" };
            cache.Set("LatestQueries", queriesToAdd);

            // Act
            var latestQueries = searchQueries.GetLatestQueriesFromCache();

            // Assert
            Assert.NotNull(latestQueries);
            Assert.Equal(queriesToAdd, latestQueries);
        }
    }
}
