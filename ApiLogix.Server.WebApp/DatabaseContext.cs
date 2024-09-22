using System.Data;
using Microsoft.Data.SqlClient;

namespace ApiLogix.Server.WebApp;

public class DatabaseContext : IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public DatabaseContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
        _connection = new SqlConnection(_connectionString);
    }

    public IDbConnection GetConnection()
    {
        if (_connection == null || _connection.State == ConnectionState.Closed)
        {
            _connection = new SqlConnection(_connectionString);
        }

        return _connection;
    }

    public void Dispose()
    {
        if (_connection != null && _connection.State != ConnectionState.Closed)
        {
            _connection.Close();
        }

        _connection.Dispose();
    }
}
