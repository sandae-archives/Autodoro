using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Autodoro.Model
{
    public class LogRepository
    {
        private readonly string _connectionString;
        private readonly ILiteDatabase _db;
        private readonly ILiteCollection<Log> _collection;

        public LogRepository()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LiteDB"].ConnectionString;
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
