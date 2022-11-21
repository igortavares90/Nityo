using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Nityo.DigitalAccount.Infrastructure.Data.DataContext
{
    public class DigitalAccountContext
    {
        private IConfiguration _configuration { get; set; }

        private MySqlConnection _connection { get; set; }
        private IDbTransaction _transaction { get; set; }
        private bool _disposed { get; set; }
        public MySqlConnection Connection { get; }

        public DigitalAccountContext(IConfiguration configuration)
        {
            _configuration = configuration;

            Initializer();
            _connection.Open();
            Connection = _connection;
        }

        public void Begin()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
            Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            Dispose();
        }

        private void Initializer()
        {
            string sqlConnectionStringName = _configuration["MySqlConnection:ConnectionString"];
            _connection = new MySqlConnection(_configuration[sqlConnectionStringName]);
        }

        public void Dispose()
        {
            if (_transaction != null) { _transaction.Dispose(); }
            if (_connection != null) { _connection.Dispose(); }

            _transaction = null;
            _connection = null;

            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        _connection.Close();
                    }
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                    }
                }

                _disposed = true;
            }
        }

    }
}
