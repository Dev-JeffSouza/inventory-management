using InventoryManagement.Repositories.Entities;
using Oracle.ManagedDataAccess.Client;

namespace InventoryManagement.Repositories
{   
    public interface IUserRepository
    {
        User? GetByLogin(string Login);
    }


    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository (string connectionString) :base(connectionString)
        {
            //Construtor para inicialização da connectionString
        }

        //Método Obter por login
        public User? GetByLogin(string Login)
        {
            User? user = null;

            using (var conn = new OracleConnection(connectionString))
            {
                string query = @"SELECT CODUSU, NOMEUSU, EMAIL, SENHA, ATIVO, LOGIN 
                                FROM USUARIO 
                                WHERE LOGIN = :LOGIN";

                var cmd = new OracleCommand(query, conn);
                cmd.Parameters.Add(new OracleParameter(":LOGIN", Login));
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        CodUsu = reader.GetInt32(reader.GetOrdinal("CODUSU")),
                        UserName = reader.GetString(reader.GetOrdinal("NOMEUSU")),
                        Email = reader.GetString(reader.GetOrdinal("EMAIL")),
                        Password = reader.GetString(reader.GetOrdinal("SENHA")),
                        Active = reader.GetString(reader.GetOrdinal("ATIVO")),
                        Login = reader.GetString(reader.GetOrdinal("LOGIN"))

                    };
                }

            }

            return user;

        }

    }
}
