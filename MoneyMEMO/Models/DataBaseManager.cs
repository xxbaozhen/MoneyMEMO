using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMEMO.Models
{
    public class DataBaseManager
    {
        private SQLiteConnection connection;
        public DataBaseManager()
        {
            connection = InitializeDatabase();
        }
        ~DataBaseManager()
        {
            connection?.Dispose();
            connection = null!;
        }
        private SQLiteConnection InitializeDatabase()
        {
            // 获取路径
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appName = "MoneyMEMO";
            string databaseFolder = Path.Combine(appData, appName, "data");

            if (!Directory.Exists(databaseFolder))
            {
                Directory.CreateDirectory(databaseFolder);
            }

            string dbPath = Path.Combine(databaseFolder, "mydatabase.db");

            // 创建数据库（如果不存在）
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }
            var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            conn.Open();
            { // 创建表（如果不存在）
                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS MoneyInfo (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    Category INTEGER NOT NULL,
                    Amount REAL NOT NULL,
                    Note TEXT
                );";
                using var command = new SQLiteCommand(createTableSql, conn);
                command.ExecuteNonQuery();
            }
            return conn;
        }
        public void ClearAndDeleteDatabase()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appName = "MoneyMEMO";
            string databaseFolder = Path.Combine(appData, appName, "data");
            string dbPath = Path.Combine(databaseFolder, "mydatabase.db");
            connection.Close();
            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }
        public int Insert(MoneyInfo info)
        {
            const string sql = @"
                INSERT INTO MoneyInfo (Date, Category, Amount, Note)
                VALUES (@Date, @Category, @Amount, @Note);";

            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@Date", info.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Category", (int)info.Category);
            command.Parameters.AddWithValue("@Amount", info.Amount);
            command.Parameters.AddWithValue("@Note", info.Note);

            command.ExecuteNonQuery();

            // 获取插入的自增 Id 并设置回对象
            long lastId = connection.LastInsertRowId;
            info.Id = (int)lastId;
            return info.Id;
        }
        public int Delete(int id)
        {
            const string sql = "DELETE FROM MoneyInfo WHERE Id = @Id";

            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);

            return command.ExecuteNonQuery();
        }
        public void Update(int id, MoneyInfo info)
        {
            const string sql = @"
                UPDATE MoneyInfo
                SET Date = @Date,
                    Category = @Category,
                    Amount = @Amount,
                    Note = @Note
                WHERE Id = @Id;";

            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Date", info.Date.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@Category", (int)info.Category);
            command.Parameters.AddWithValue("@Amount", info.Amount);
            command.Parameters.AddWithValue("@Note", info.Note);

            command.ExecuteNonQuery();
        }
        public List<MoneyInfo> QueryAll()
        {
            const string sql = "SELECT * FROM MoneyInfo ORDER BY Date DESC";
            using var command = new SQLiteCommand(sql, connection);
            using var reader = command.ExecuteReader();

            var list = new List<MoneyInfo>();
            while (reader.Read())
            {
                list.Add(new MoneyInfo
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Date = DateTime.Parse(reader["Date"].ToString()!),
                    Category = (MoneyCategory)Convert.ToInt32(reader["Category"]),
                    Amount = Convert.ToDecimal(reader["Amount"]),
                    Note = reader["Note"]?.ToString() ?? string.Empty
                });
            }
            return list;
        }
        public List<MoneyInfo> Query(MoneyQueryCondition condition)
        {
            var sql = new StringBuilder("SELECT * FROM MoneyInfo WHERE 1=1");
            var parameters = new Dictionary<string, object>();

            // 日期范围
            if (condition.StartDate.HasValue)
            {
                sql.Append(" AND date(Date) >= date(@StartDate)");
                parameters["@StartDate"] = condition.StartDate.Value.ToString("yyyy-MM-dd");
            }

            if (condition.EndDate.HasValue)
            {
                sql.Append(" AND date(Date) <= date(@EndDate)");
                parameters["@EndDate"] = condition.EndDate.Value.ToString("yyyy-MM-dd");
            }

            // 金额范围
            if (condition.MinAmount.HasValue)
            {
                sql.Append(" AND Amount >= @MinAmount");
                parameters["@MinAmount"] = condition.MinAmount.Value;
            }

            if (condition.MaxAmount.HasValue)
            {
                sql.Append(" AND Amount <= @MaxAmount");
                parameters["@MaxAmount"] = condition.MaxAmount.Value;
            }

            // 备注模糊查询
            if (!string.IsNullOrWhiteSpace(condition.NoteKeyword))
            {
                sql.Append(" AND Note LIKE @Note");
                parameters["@Note"] = $"%{condition.NoteKeyword}%";
            }

            sql.Append(" ORDER BY Date DESC");

            using var command = new SQLiteCommand(sql.ToString(), connection);
            foreach (var p in parameters)
                command.Parameters.AddWithValue(p.Key, p.Value);

            using var reader = command.ExecuteReader();
            var list = new List<MoneyInfo>();

            while (reader.Read())
            {
                list.Add(new MoneyInfo
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Date = DateTime.Parse(reader["Date"].ToString()!),
                    Category = (MoneyCategory)Convert.ToInt32(reader["Category"]),
                    Amount = Convert.ToDecimal(reader["Amount"]),
                    Note = reader["Note"]?.ToString() ?? string.Empty
                });
            }

            return list;
        }
         
    }
}