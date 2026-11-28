using InventoryManagement.Repositories.Entities;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace InventoryManagement.Repositories
{
    public interface IProductRepository
    {
        int? Insert(Product product);

        int? Update(Product product);

        int? Delete(int codProd, int codLocal, int codSupplier);

        Product? GetById(int codProd, int idLoc, int idForn);

        IList<Product> List();

        IList<Volume> GetVolume();

        IList<Partner> GetPartner();

        IList<Location> GetLocation(int? codLocal = null);
    }

    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionString) : base(connectionString)
        {

        }

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
                            cmdProduct.BindByName = true;

                            cmdProduct.Parameters.Add(new OracleParameter(":DESCRPROD", product.Description.ToUpper()));
                            cmdProduct.Parameters.Add(new OracleParameter(":CODVOL", product.Codvol));
                            cmdProduct.Parameters.Add(new OracleParameter(":MARCA", product.Mark.ToUpper()));
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
                                    cmdStock.BindByName = true;

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
                                    cmdSupplier.BindByName = true;

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

        //REVER ESTE MÉTODO (IRÁ CONFLITAR COM TRANFERÊNCIAS ENTRE LOCAIS)
        public int? Update(Product product)
        {
            var affectedRows = 0;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string productQuery = @"UPDATE PRODUTO SET                                         
                                        DESCRPROD = NVL(:DESCRIPTION,'SEM DESCRIÇÃO'),                                        
                                        CODVOL    = NVL(:CODVOL, CODVOL),                                        
                                        MARCA     = NVL(:MARK,'SEM MARCA'),                                        
                                        PRECO     = NVL(:PRICE, 0),                                        
                                        ATIVO     = NVL(:ACTIVE,'S')                                      
                                      WHERE CODPROD = :CODPROD";

                        using (var cmdProduct = new OracleCommand(productQuery, conn))
                        {
                            cmdProduct.Transaction = transaction;
                            cmdProduct.BindByName = true;

                            cmdProduct.Parameters.Add(new OracleParameter(":DESCRIPTION", product.Description.ToUpper()));
                            cmdProduct.Parameters.Add(new OracleParameter(":CODVOL", product.Codvol));
                            cmdProduct.Parameters.Add(new OracleParameter(":MARK", product.Mark));
                            cmdProduct.Parameters.Add(new OracleParameter(":PRICE", product.Price));
                            cmdProduct.Parameters.Add(new OracleParameter(":ACTIVE", product.Active.ToUpper()));
                            cmdProduct.Parameters.Add(new OracleParameter(":CODPROD", product.CodProd));

                            affectedRows = cmdProduct.ExecuteNonQuery();
                        }

                        if (product.Stock != null && product.Stock.Count > 0)
                        {
                            string stockQuery = @"UPDATE ESTOQUE SET                                                
                                                    QUANTIDADE = NVL(:QUANTIDADE, 0),                                                
                                                    ESTMIN = NVL(:ESTOCKMIN, 0),                                                
                                                    ESTMAX = NVL(:ESTOCKMAX, 0),                                                
                                                    ATIVO = NVL(:ACTIVE,'S')                                             
                                                 WHERE CODPROD = :CODPROD AND CODLOCAL = :CODLOCAL";

                            string stockQueryInsert = @"INSERT INTO ESTOQUE (CODPROD, CODLOCAL, QUANTIDADE, ESTMIN, ESTMAX, ATIVO)                                                 
                                                        VALUES (:CODPROD, NVL(:CODLOCAL, 1), NVL(:QUANTITY, 0), NVL(:ESTOCKMIN, 0),                                                         
                                                        NVL(:ESTOCKMAX, 0), NVL(:ACTIVE,'S'))";

                            foreach (var stock in product.Stock)
                            {
                                using (var cmdStock = new OracleCommand(stockQuery, conn))
                                {
                                    cmdStock.Transaction = transaction;
                                    cmdStock.BindByName = true;

                                    cmdStock.Parameters.Add(new OracleParameter(":CODPROD", product.CodProd));
                                    cmdStock.Parameters.Add(new OracleParameter(":CODLOCAL", stock.Codlocal));
                                    cmdStock.Parameters.Add(new OracleParameter(":QUANTIDADE", stock.Quantity));
                                    cmdStock.Parameters.Add(new OracleParameter(":ESTOCKMIN", stock.StockMin));
                                    cmdStock.Parameters.Add(new OracleParameter(":ESTOCKMAX", stock.StockMax));
                                    cmdStock.Parameters.Add(new OracleParameter(":ACTIVE", 'S'.ToString()));

                                    cmdStock.ExecuteNonQuery();

                                    affectedRows = cmdStock.ExecuteNonQuery();

                                    if (affectedRows == 0)
                                    {
                                        using (var cmdInsert = new OracleCommand(stockQueryInsert, conn))
                                        {
                                            cmdInsert.Transaction = transaction;
                                            cmdInsert.BindByName = true;

                                            cmdInsert.Parameters.Add(new OracleParameter(":CODPROD", product.CodProd));
                                            cmdInsert.Parameters.Add(new OracleParameter(":CODLOCAL", (object?)stock.Codlocal ?? DBNull.Value));
                                            cmdInsert.Parameters.Add(new OracleParameter(":QUANTITY", (object?)stock.Quantity ?? DBNull.Value));
                                            cmdInsert.Parameters.Add(new OracleParameter(":ESTOCKMIN", (object?)stock.StockMin ?? DBNull.Value));
                                            cmdInsert.Parameters.Add(new OracleParameter(":ESTOCKMAX", (object?)stock.StockMax ?? DBNull.Value));
                                            cmdInsert.Parameters.Add(new OracleParameter(":ACTIVE", (object?)stock.Active ?? DBNull.Value));

                                            cmdInsert.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }

                        if (product.Supplier != null && product.Supplier.Count > 0)
                        {
                            string supplierQuery = @"UPDATE FORNECEDOR SET                                                
                                                     CUSTO = NVL(:COST, 0)                                              
                                                    WHERE CODPROD = :CODPROD AND CODPARC = :CODPARC";

                            string supplierQueryInsert = @"INSERT INTO FORNECEDOR (CODPROD, CODPARC, CUSTO)                                                 
                                                           VALUES (:CODPROD, :CODPARC, :CUSTO)";

                            foreach (var supplier in product.Supplier)
                            {
                                using (var cmdSupplier = new OracleCommand(supplierQuery, conn))
                                {
                                    cmdSupplier.Transaction = transaction;
                                    cmdSupplier.BindByName = true;

                                    cmdSupplier.Parameters.Add(new OracleParameter(":CODPROD", product.CodProd));
                                    cmdSupplier.Parameters.Add(new OracleParameter(":CODPARC", supplier.CodParc));
                                    cmdSupplier.Parameters.Add(new OracleParameter(":COST", supplier.Cost));

                                    cmdSupplier.ExecuteNonQuery();

                                    affectedRows = cmdSupplier.ExecuteNonQuery();

                                    if (affectedRows == 0)
                                    {
                                        using (var cmdInsertSup = new OracleCommand(supplierQueryInsert, conn))
                                        {
                                            cmdInsertSup.Transaction = transaction;
                                            cmdInsertSup.BindByName = true;

                                            cmdInsertSup.Parameters.Add(new OracleParameter(":CODPROD", product.CodProd));
                                            cmdInsertSup.Parameters.Add(new OracleParameter(":CODPARC", supplier.CodParc));
                                            cmdInsertSup.Parameters.Add(new OracleParameter(":CUSTO", supplier.Cost));

                                            cmdInsertSup.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                        return affectedRows;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public int? Delete(int codProd, int codLocal, int codSupplier)
        {
            int affectedRows = 0;

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string supplierQuery = @"DELETE FROM FORNECEDOR WHERE CODPROD = :CODPROD AND CODPARC = :CODPARC";

                        using (var cmdSupplier = new OracleCommand(supplierQuery, conn))
                        {
                            cmdSupplier.Transaction = transaction;
                            cmdSupplier.BindByName = true;

                            cmdSupplier.Parameters.Add(new OracleParameter(":CODPROD", codProd));
                            cmdSupplier.Parameters.Add(new OracleParameter(":CODPARC", codSupplier));
                            cmdSupplier.ExecuteNonQuery();
                        }

                        string stockQuery = @"DELETE FROM ESTOQUE WHERE CODPROD = :CODPROD AND CODLOCAL = :CODLOCAL";

                        using (var cmdStock = new OracleCommand(stockQuery, conn))
                        {
                            cmdStock.Transaction = transaction;
                            cmdStock.BindByName = true;

                            cmdStock.Parameters.Add(new OracleParameter(":CODPROD", codProd));
                            cmdStock.Parameters.Add(new OracleParameter(":CODLOCAL", codLocal));
                            cmdStock.ExecuteNonQuery();
                        }

                        string productQuery = @"DELETE FROM PRODUTO WHERE CODPROD = :CODPROD";

                        using (var cmdProduct = new OracleCommand(productQuery, conn))
                        {
                            cmdProduct.Transaction = transaction;
                            cmdProduct.BindByName = true;

                            cmdProduct.Parameters.Add(new OracleParameter(":CODPROD", codProd));
                            affectedRows = cmdProduct.ExecuteNonQuery();

                            if (affectedRows == 0)
                            {
                                transaction.Rollback();
                                return affectedRows;
                            }
                        }

                        transaction.Commit();
                        return affectedRows;
                    }

                    catch(OracleException ex) when (ex.Number == 2292)
                    {
                        transaction.Rollback();
                        return -2292;
                    }
                }
            }
        }

        public Product? GetById(int codProd, int idloc, int idsup)
        {
            Product? product = null;
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT 
                                    PRO.CODPROD, 
                                    PRO.DESCRPROD, 
                                    PRO.MARCA,   
                                    PRO.ATIVO,
                                    PRO.PRECO, 
                                    PRO.DHCAD, 
                                    PRO.CODVOL,
                                    NVL(FORN.CUSTO, 0) AS CUSTO, 
                                    FORN.CODPARC,
                                    NVL(EST.QUANTIDADE, 0) AS QUANTIDADE, 
                                    NVL(EST.ESTMIN, 0) AS ESTMIN, 
                                    NVL(EST.ESTMAX, 0) AS ESTMAX,
                                    NVL(EST.CODLOCAL, 0) AS CODLOCAL
                                FROM PRODUTO PRO
                                LEFT JOIN FORNECEDOR FORN 
                                   ON FORN.CODPROD = PRO.CODPROD 
                                   AND FORN.CODPARC = :CODPARC
                                LEFT JOIN ESTOQUE EST 
                                   ON EST.CODPROD = PRO.CODPROD AND EST.CODLOCAL = :CODLOCAL
                                WHERE
                                    PRO.CODPROD = :CODPROD";
                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;

                    cmd.Parameters.Add(new OracleParameter(":CODPARC", idsup));
                    cmd.Parameters.Add(new OracleParameter(":CODLOCAL", idloc));
                    cmd.Parameters.Add(new OracleParameter(":CODPROD", codProd));

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                CodProd = Convert.ToInt32(reader["CODPROD"]),
                                Description = reader["DESCRPROD"].ToString()!,
                                Codvol = Convert.ToInt32(reader["CODVOL"]),
                                Mark = reader["MARCA"].ToString()!,
                                Price = Convert.ToDecimal(reader["PRECO"]),
                                DhCad = Convert.ToDateTime(reader["DHCAD"]),
                                Active = reader["ATIVO"].ToString()!,

                                Stock = new List<Stock>
                                {
                                    new Stock
                                    {
                                        Codprod = Convert.ToInt32(reader["CODPROD"]),
                                        Codlocal = Convert.ToInt32(reader["CODLOCAL"]),
                                        Quantity = Convert.ToDecimal(reader["QUANTIDADE"]),
                                        StockMin = Convert.ToDecimal(reader["ESTMIN"]),
                                        StockMax = Convert.ToDecimal(reader["ESTMAX"]),
                                        Active = reader["ATIVO"].ToString()!
                                    }
                                },

                                Supplier = new List<Supplier>
                                {
                                    new Supplier
                                    {   CodProd = Convert.ToInt32(reader["CODPROD"]),
                                        CodParc = Convert.ToInt32(reader["CODPARC"]),
                                        Cost = Convert.ToDecimal(reader["CUSTO"])
                                    }
                                }
                            };
                        }
                    }
                }
            }
            return product;
        }



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

        public IList<Location> GetLocation(int? codLocal = null)
        {
            var locationList = new List<Location>();
            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT CODLOCAL, DESCRLOCAL, ATIVO FROM LOCAL
                                WHERE (:CODLOCAL IS NULL OR CODLOCAL = :CODLOCAL)
                                ORDER BY DESCRLOCAL ASC";
                using (var cmd = new OracleCommand(query, conn))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add(new OracleParameter(":CODLOCAL", (object?)codLocal ?? DBNull.Value));

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

        public IList<Product> List()
        {
            var productList = new List<Product>();

            using (var conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"
                                SELECT 
                                    PRO.CODPROD, 
                                    PRO.DESCRPROD, 
                                    PRO.CODVOL, 
                                    VOL.DESCRVOL, 
                                    FORN.CODPARC, 
                                    NVL(FORN.CUSTO, 0) AS CUSTO, 
                                    NVL(EST.QUANTIDADE, 0) AS QUANTIDADE, 
                                    NVL(EST.ESTMIN, 0) AS ESTMIN, 
                                    NVL(EST.ESTMAX, 0) AS ESTMAX, 
                                    LOC.CODLOCAL, 
                                    NVL(LOC.DESCRLOCAL,'SEM LOCAL') AS DESCRLOCAL, 
                                    NVL(PAR.NOMEPARC,'SEM FORNECEDOR') AS NOMEPARC, 
                                    PRO.MARCA, 
                                    PRO.PRECO, 
                                    PRO.DHCAD, 
                                    PRO.DHALTER, 
                                    DECODE(PRO.ATIVO,'S','Sim','N','Não') AS ATIVO
                                FROM PRODUTO PRO
                                JOIN ESTOQUE EST ON EST.CODPROD = PRO.CODPROD
                                JOIN LOCAL LOC ON LOC.CODLOCAL = EST.CODLOCAL
                                JOIN VOLUME VOL ON VOL.CODVOL = PRO.CODVOL
                                JOIN FORNECEDOR FORN ON FORN.CODPROD = PRO.CODPROD
                                JOIN PARCEIRO PAR ON PAR.CODPARC = FORN.CODPARC
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
                                Codvol = Convert.ToInt32(reader["CODVOL"]),
                                Price = Convert.ToDecimal(reader["PRECO"]),
                                Active = reader["ATIVO"].ToString()!,
                                DhCad = Convert.ToDateTime(reader["DHCAD"]),
                                Dhalter = reader["DHALTER"] != DBNull.Value ? Convert.ToDateTime(reader["DHALTER"]) : null,

                                Volume = new Volume
                                {
                                    Codvol = Convert.ToInt32(reader["CODVOL"]),
                                    DescrVol = reader["DESCRVOL"].ToString()!
                                },

                                Supplier = new List<Supplier>
                                {
                                    new Supplier

                                    {   CodProd = Convert.ToInt32(reader["CODPROD"]),
                                        CodParc = Convert.ToInt32(reader["CODPARC"]),
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
                                        Codprod = Convert.ToInt32(reader["CODPROD"]),
                                        Codlocal = Convert.ToInt32(reader["CODLOCAL"]),
                                        Quantity = Convert.ToDecimal(reader["QUANTIDADE"]),
                                        StockMin = Convert.ToDecimal(reader["ESTMIN"]),
                                        StockMax = Convert.ToDecimal(reader["ESTMAX"]),
                                        Active = reader["ATIVO"].ToString()!,

                                        Location = new Location
                                        {
                                            CodLocal = Convert.ToInt32(reader["CODLOCAL"]),
                                            Description = reader["DESCRLOCAL"].ToString()!,
                                            Active = reader["ATIVO"].ToString()!
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
