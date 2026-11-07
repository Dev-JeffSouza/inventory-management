using InventoryManagement.Repositories.Entities;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace InventoryManagement.Repositories
{   
    public interface IProductRepository
    {
        int? Insert(Product product);
        IList<Product> List();

        IList<Volume> GetVolume();
        IList<Partner> GetPartner();
        IList<Location> GetLocation();
    }

    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionString) : base(connectionString)
        {

        }

        //Inserindo um novo produto no banco de dados
        public int? Insert(Product product)
        {
            int? productId = null;
            int? affectedRows = null;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string productQuery = @"INSERT INTO PRODUTO (DESCRPROD, CODVOL, MARCA, PRECO, ATIVO, DHCAD, DHALTER) 
                                            VALUES (:DESCRPROD, :CODVOL, :MARCA, :PRECO, :ATIVO, :DHCAD, :DHALTER)
                                            RETURNING CODPROD INTO :NEW_ID";

                        using (var cmdProduct = new OracleCommand(productQuery, conn))
                        {
                            cmdProduct.Transaction = transaction;

                            cmdProduct.Parameters.Add(new OracleParameter(":DESCRPROD", product.Description));
                            cmdProduct.Parameters.Add(new OracleParameter(":CODVOL", product.Codvol));
                            cmdProduct.Parameters.Add(new OracleParameter(":MARCA", product.Mark));
                            cmdProduct.Parameters.Add(new OracleParameter(":PRECO", product.Price));
                            cmdProduct.Parameters.Add(new OracleParameter(":ATIVO", product.Active));
                            cmdProduct.Parameters.Add(new OracleParameter(":DHCAD", product.DhCad));
                            cmdProduct.Parameters.Add(new OracleParameter(":DHALTER", (object?)product.Dhalter ?? DBNull.Value));

                            var idParam = new OracleParameter(":NEW_ID", OracleDbType.Int32, ParameterDirection.Output);
                            cmdProduct.Parameters.Add(idParam);

                            affectedRows = cmdProduct.ExecuteNonQuery();

                            if (idParam.Value != null && idParam.Value != DBNull.Value)
                            {
                                productId = int.Parse(idParam.Value.ToString()!);
                            }
                        }

                        if (productId == null)
                        {
                            return null;
                        }

                        if (product.Stock != null && product.Stock.Count > 0)
                        {
                            string stockQuery = @"INSERT INTO ESTOQUE (CODPROD, CODLOCAL, QUANTIDADE, ESTMIN, ESTMAX, ATIVO) 
                                                VALUES (:CODPROD, :CODLOCAL, :QUANTIDADE, :ESTMIN, :ESTMAX, :ATIVO)";

                            foreach (var stock in product.Stock)
                            {
                                using (var cmdStock = new OracleCommand(stockQuery, conn))
                                {
                                    cmdStock.Transaction = transaction; 

                                    cmdStock.Parameters.Add(new OracleParameter(":CODPROD", productId.Value));
                                    cmdStock.Parameters.Add(new OracleParameter(":CODLOCAL", stock.Codlocal));
                                    cmdStock.Parameters.Add(new OracleParameter(":QUANTIDADE", stock.Quantity));
                                    cmdStock.Parameters.Add(new OracleParameter(":ESTMIN", stock.StockMin));
                                    cmdStock.Parameters.Add(new OracleParameter(":ESTMAX", stock.StockMax));
                                    cmdStock.Parameters.Add(new OracleParameter(":ATIVO", stock.Active));

                                    cmdStock.ExecuteNonQuery();
                                }
                            }
                        }

                        if (product.Supplier != null && product.Supplier.Count > 0)
                        {
                            string supplierQuery = @"INSERT INTO FORNECEDOR (CODPROD, CODPARC, CUSTO) 
                                                    VALUES (:CODPROD, :CODPARC, :CUSTO)";

                            foreach (var supplier in product.Supplier)
                            {
                                using (var cmdSupplier = new OracleCommand(supplierQuery, conn))
                                {
                                    cmdSupplier.Transaction = transaction; 

                                    cmdSupplier.Parameters.Add(new OracleParameter(":CODPROD", productId.Value));
                                    cmdSupplier.Parameters.Add(new OracleParameter(":CODPARC", supplier.CodParc));
                                    cmdSupplier.Parameters.Add(new OracleParameter(":CUSTO", supplier.Cost));

                                    cmdSupplier.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return affectedRows;
        }

        //obtendo a lista de volumes do banco de dados
        public IList<Volume> GetVolume()
        {
            var volumeList = new List<Volume>();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CODVOL, DESCRVOL FROM VOLUME
                                ORDER BY DESCRVOL ASC";
                using (var cmd = new OracleCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var volume = new Volume
                            {
                                Codvol = Convert.ToInt32(reader["CODVOL"]),
                                DescrVol = reader["DESCRVOL"].ToString()!
                            };
                    
                            volumeList.Add(volume);
                        }
                    }
                }
            }

            return volumeList;
        }

        //obtendo a lista de parceiros do banco de dados
        public IList<Partner> GetPartner()
        {
            var partnerList = new List<Partner>();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CODPARC, NOMEPARC, ATIVO, DHCAD FROM PARCEIRO
                                ORDER BY NOMEPARC ASC";
                using (var cmd = new OracleCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var partner = new Partner
                            {
                                CodParc = Convert.ToInt32(reader["CODPARC"]),
                                NameParc = reader["NOMEPARC"].ToString()!,
                                Active = reader["ATIVO"].ToString()!,
                                DhCad = Convert.ToDateTime(reader["DHCAD"])

                            };

                            partnerList.Add(partner);
                        }
                    }
                }
            }
            return partnerList;
        }

        //obtendo a lista de locais de estoque do banco de dados
        public IList<Location> GetLocation()
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

        //obtendo a lista full de produtos do banco de dados
        public IList<Product> List()
        {
            var productList = new List<Product>();

            using(var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT 
                                    PRO.CODPROD, PRO.DESCRPROD, VOL.DESCRVOL, FORN.CUSTO, 
                                    EST.QUANTIDADE, EST.ESTMIN,EST.ESTMAX, LOC.DESCRLOCAL, PAR.NOMEPARC, PRO.MARCA,
                                    PRO.PRECO, PRO.DHCAD, PRO.DHALTER, DECODE(PRO.ATIVO,'S','SIM') AS ATIVO
                                FROM PRODUTO PRO
                                LEFT JOIN ESTOQUE EST ON EST.CODPROD = PRO.CODPROD
                                LEFT JOIN LOCAL LOC ON LOC.CODLOCAL = EST.CODLOCAL
                                LEFT JOIN VOLUME VOL ON VOL.CODVOL = PRO.CODVOL
                                LEFT JOIN FORNECEDOR FORN ON FORN.CODPROD = PRO.CODPROD
                                LEFT JOIN PARCEIRO PAR ON PAR.CODPARC = FORN.CODPARC

                                ORDER BY PRO.CODPROD ASC
                                FETCH FIRST 100 ROWS ONLY";

                using (var cmd = new OracleCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                CodProd = Convert.ToInt32(reader["CODPROD"]),
                                Description = reader["DESCRPROD"].ToString()!,
                                Mark = reader["MARCA"].ToString()!,
                                Price = Convert.ToDecimal(reader["PRECO"]),
                                Active = reader["ATIVO"].ToString()!,
                                DhCad = Convert.ToDateTime(reader["DHCAD"]),
                                Dhalter = reader["DHALTER"] != DBNull.Value ? Convert.ToDateTime(reader["DHALTER"]) : null,

                                Volume = new Volume
                                {
                                    DescrVol = reader["DESCRVOL"].ToString()!
                                },

                                Supplier = new List<Supplier>
                                {
                                    new Supplier
                                    {
                                        Cost = reader["CUSTO"] != DBNull.Value ? Convert.ToDecimal(reader["CUSTO"]) : 0,

                                        Partner = new Partner
                                        {
                                            NameParc = reader["NOMEPARC"].ToString()!,
                                            Active = reader["ATIVO"].ToString()!,
                                            DhCad = Convert.ToDateTime(reader["DHCAD"])
                                        }
                                    }
                                },

                                Stock = new List<Stock>
                                {
                                    new Stock
                                    {
                                        Quantity = reader["QUANTIDADE"] != DBNull.Value ? Convert.ToDecimal(reader["QUANTIDADE"]) : 0,
                                        StockMin = reader["ESTMIN"] != DBNull.Value ? Convert.ToDecimal(reader["ESTMIN"]) : 0,
                                        StockMax = reader["ESTMAX"] != DBNull.Value ? Convert.ToDecimal(reader["ESTMAX"]) : 0,
                                        Active = reader["ATIVO"] != DBNull.Value ? reader["ATIVO"].ToString()! : "NÃO",

                                        Location = new Location
                                        {
                                            Description = reader["DESCRLOCAL"].ToString()!,
                                            Active = reader["ATIVO"] != DBNull.Value ? reader["ATIVO"].ToString()! : "NÃO"
                                        }
                                    }
                                },
                            };

                            productList.Add(product);
                        }

                        return productList;
                    }
                }
            }
        }

    }
}
