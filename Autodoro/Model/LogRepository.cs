using System;
using System.Collections.Generic;
using System.Configuration;
using LiteDB;

namespace Autodoro.Model
{
    public class LogRepository
    {
        private readonly ILiteCollection<Log> _collection;
        private readonly string _connectionString;
        private readonly ILiteDatabase _db;

        public LogRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["LiteDB"].ConnectionString;
            _db = new LiteDatabase(_connectionString);
            _collection = _db.GetCollection<Log>("logs");
        }

        public void Add(Log log)
        {
            _collection.Insert(log);
        }

        public IEnumerable<Log> FindAllToday()
        {
            return _collection.Query()
                .Where(x => x.EndTime.Date == DateTime.Now.Date)
                .ToList();
        }
    }
}