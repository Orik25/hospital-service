using Npgsql;
using System;

class Program
{
    static void Main(string[] args)
    {
        string connstring = String.Format("Server={0};Port={1};" +
            "User Id={2};Password={3};Database={4};",
            "ep-damp-wood-56616604.eu-central-1.aws.neon.tech", 5432,
            "Orik25", "fnF9SZOoqQR7", "neondb");

        using (var conn = new NpgsqlConnection(connstring))
        {
            conn.Open();

            // Очищаємо таблиці перед заповненням
            ClearTable(conn, "appointments");
            ClearTable(conn, "users");
            ClearTable(conn, "roles");

            // Заповнюємо таблицю roles
            for (int i = 1; i <= 50; i++)
            {
                if (!RecordExists(conn, "roles", "id", i))
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO roles (id, name) VALUES (@id, @name)";
                        cmd.Parameters.AddWithValue("id", i);
                        cmd.Parameters.AddWithValue("name", "Role" + i);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Заповнюємо таблицю users
            for (int i = 1; i <= 50; i++)
            {
                if (!RecordExists(conn, "users", "id", i))
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO users (id, email, first_name, last_name, patronymic, phone, password, type, role_id) VALUES (@id, @email, @first_name, @last_name, @patronymic, @phone, @password, @type, @role_id)";
                        cmd.Parameters.AddWithValue("id", i);
                        cmd.Parameters.AddWithValue("email", "user" + i + "@example.com");
                        cmd.Parameters.AddWithValue("first_name", "FirstName" + i);
                        cmd.Parameters.AddWithValue("last_name", "LastName" + i);
                        cmd.Parameters.AddWithValue("patronymic", "Patronymic" + i);
                        cmd.Parameters.AddWithValue("phone", "+38050123456" + i);
                        cmd.Parameters.AddWithValue("password", "Password" + i);
                        cmd.Parameters.AddWithValue("type", "Type" + i);
                        cmd.Parameters.AddWithValue("role_id", i);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Заповнюємо таблицю appointments
            for (int i = 1; i <= 50; i++)
            {
                if (!RecordExists(conn, "appointments", "id", i))
                {
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO appointments (id, date_and_time, message, patient_id, doctor_id) VALUES (@id, @date_and_time, @message, @patient_id, @doctor_id)";
                        cmd.Parameters.AddWithValue("id", i);
                        cmd.Parameters.AddWithValue("date_and_time", DateTime.Now.AddDays(i));
                        cmd.Parameters.AddWithValue("message", "Message" + i);
                        cmd.Parameters.AddWithValue("patient_id", i);
                        cmd.Parameters.AddWithValue("doctor_id", (i % 10) + 1); // Доктори від 1 до 10
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Виводимо дані з таблиць
            PrintTableData(conn, "roles");
            PrintTableData(conn, "users");
            PrintTableData(conn, "appointments");
        }
    }

    static bool RecordExists(NpgsqlConnection conn, string tableName, string columnName, object value)
    {
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @value";
            cmd.Parameters.AddWithValue("value", value);

            return ((long)cmd.ExecuteScalar()) > 0;
        }
    }

    static void ClearTable(NpgsqlConnection conn, string tableName)
    {
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = conn;
            cmd.CommandText = $"DELETE FROM {tableName}";

            cmd.ExecuteNonQuery();
        }
    }

    static void PrintTableData(NpgsqlConnection conn, string tableName)
    {
        Console.WriteLine("\nData from table " + tableName + ":");

        using (var cmd = new NpgsqlCommand("SELECT * FROM " + tableName, conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write(reader[i] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
