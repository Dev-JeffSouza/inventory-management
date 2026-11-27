
using InventoryManagement.Repositories.Entities;
using Oracle.ManagedDataAccess.Client;

namespace InventoryManagement.Repositories
{
    public interface ILocationRepository
    {
        int? Insert(Location location);

        int? Update(Location location);

        int? Delete(int id);

        int? GetConstraints(int id);

        Location? GetById(int id);

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

        public int? Update(Location location)
        {
            var affectedRows = 0;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"UPDATE LOCAL SET DESCRLOCAL = :DESCRIPTION, ATIVO = :ACTIVE
                                WHERE CODLOCAL = :CODLOCAL";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;

                    cmd.Parameters.Add(new OracleParameter(":DESCRIPTION", location.Description));
                    cmd.Parameters.Add(new OracleParameter(":ACTIVE", location.Active));
                    cmd.Parameters.Add(new OracleParameter(":CODLOCAL", location.CodLocal));

                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        public Location? GetById(int id)
        {
            Location? location = null;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CODLOCAL, DESCRLOCAL, ATIVO 
                                FROM LOCAL WHERE CODLOCAL = :CODLOCAL";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(new OracleParameter(":CODLOCAL", id));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            location = new Location
                            {
                                CodLocal = Convert.ToInt32(reader["CODLOCAL"]),
                                Description = reader["DESCRLOCAL"].ToString()!,
                                Active = reader["ATIVO"].ToString()!
                            };
                        }
                    }
                }
            }
            return location;
        }

        public IList<Location> List()
        {
            var locationList = new List<Location>();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CODLOCAL, DESCRLOCAL, DECODE(ATIVO,'S','Sim','N','Não') AS ATIVO FROM LOCAL
                                ORDER BY CODLOCAL ASC";
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

        public int? Delete(int id)
        {
            var affectedRows = 0;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"DELETE FROM LOCAL WHERE CODLOCAL = :CODLOCAL";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(new OracleParameter(":CODLOCAL", id));
                    affectedRows = cmd.ExecuteNonQuery();
                }
            }
            return affectedRows;
        }

        public int? GetConstraints(int id)
        {
            var exists = 0;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT COUNT(1) FROM ESTOQUE 
                                 WHERE CODLOCAL = :CODLOCAL";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(new OracleParameter(":CODLOCAL", id));
                    exists = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            return exists;
        }
    }
}
