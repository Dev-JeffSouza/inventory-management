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
        public UserRepository(string connectionString) : base(connectionString)
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
                                 WHERE UPPER(RTRIM(LOGIN)) = UPPER(RTRIM(:LOGIN))";

                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(new OracleParameter(":LOGIN", Login.Trim()));
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                CodUsu = Convert.ToInt32(reader["CODUSU"]),
                                UserName = reader["NOMEUSU"].ToString()!,
                                Email = reader["EMAIL"].ToString()!,
                                Password = reader["SENHA"].ToString()!,
                                Active = reader["ATIVO"].ToString()!,
                                Login = reader["LOGIN"].ToString()!,
                            };
                        }
                    }
                }
            }

            return user;
        }

    }
}
