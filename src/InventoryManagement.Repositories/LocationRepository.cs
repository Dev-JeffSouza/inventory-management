
using InventoryManagement.Repositories.Entities;
using Oracle.ManagedDataAccess.Client;

namespace InventoryManagement.Repositories
{
    public interface ILocationRepository
    {
        int? Insert(Location location);

        IList<Location> List();
    }


    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(string connectionString) : base(connectionString)
        {

        }

        public int? Insert(Location location)
        {
            int? affectedRows = 0;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"INSERT INTO LOCAL (DESCRLOCAL, ATIVO) VALUES (:DESCRLOCAL,:ATIVO)";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;

                    cmd.Parameters.Add(new OracleParameter(":DESCRLOCAL", location.Description));
                    cmd.Parameters.Add(new OracleParameter(":ATIVO", location.Active));
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        public IList<Location> List()
        {
            var locationList = new List<Location>();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CODLOCAL, DESCRLOCAL, ATIVO FROM LOCAL
                                ORDER BY DESCRLOCAL ASC";
                using (var cmd = new OracleCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var location = new Location
                            {
                                CodLocal = Convert.ToInt32(reader["CODLOCAL"]),
                                Description = reader["DESCRLOCAL"].ToString()!,
                                Active = reader["ATIVO"].ToString()!
                            };

                            locationList.Add(location);
                        }
                    }
                }
            }
            return locationList;
        }
    }
}
