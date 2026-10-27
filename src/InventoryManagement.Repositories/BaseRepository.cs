using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManagement.Repositories
{
    public class BaseRepository
    {
        private readonly string _connectionString;

        public BaseRepository (string connectionString)
        {
            _connectionString = connectionString;
        }

        public string connectionString { get { return _connectionString; } }
    }
}
