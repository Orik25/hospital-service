﻿using Npgsql;
using System;
using Bogus;
using BCrypt.Net;
using System.Collections.Generic;


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

            //ClearTable(conn, "appointments");
            //ClearTable(conn, "users");
            //ClearTable(conn, "roles");

            //FillRoles(conn);
            //FillUsers(conn,GetRolesID(conn));
            //FillAppointments(conn,GetDoctorsID(conn),GetPatientID(conn));

            //PrintTableData(conn, "roles");
            //PrintTableData(conn, "users");
            PrintTableData(conn, "appointments");

            //int[] array = GetPatientID(conn);

            //for (int i = 0; i < array.Length; i++)
            //{
            //    Console.WriteLine(array[i]);
            //}

        }
    }
    static string[] roles = { "ROLE_DOCTOR", "ROLE_PATIENT", "ROLE_ADMIN" };

    static void FillRoles(NpgsqlConnection conn)
    {
        for (int i = 1; i <= 3; i++)
        {
            if (!RecordExists(conn, "roles", "role_id", i))
            {
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO roles (role_id, name) VALUES (@role_id, @name)";
                    cmd.Parameters.AddWithValue("role_id", i);
                    cmd.Parameters.AddWithValue("name", roles[i - 1]);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    static int[] GetRolesID(NpgsqlConnection conn)
    {
        List<int> IDs = new List<int>();
        using (var cmd = new NpgsqlCommand("SELECT role_id FROM roles", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                IDs.Add(Convert.ToInt32(reader[0]));

            }
        }
        return IDs.ToArray();

    }
    static int[] GetDoctorsID(NpgsqlConnection conn)
    {
        List<int> IDs = new List<int>();
        using (var cmd = new NpgsqlCommand("SELECT user_id FROM users WHERE role_ref = 1", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                IDs.Add(Convert.ToInt32(reader[0]));

            }
        }
        return IDs.ToArray();
    }
    static int[] GetPatientID(NpgsqlConnection conn)
    {
        List<int> IDs = new List<int>();
        using (var cmd = new NpgsqlCommand("SELECT user_id FROM users WHERE role_ref = 2", conn))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                IDs.Add(Convert.ToInt32(reader[0]));

            }
        }
        return IDs.ToArray();
    }

    static void FillUsers(NpgsqlConnection conn, int[] roleIDs)
    {
        var faker = new Bogus.Faker();
        var random = new Random();

        for (int i = 1; i <= 50; i++)
        {
            if (!RecordExists(conn, "users", "user_id", i))
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO users (user_id, email, first_name, last_name, patronymic, phone, password, type, role_ref) " +
                        "VALUES (@user_id, @email, @first_name, @last_name, @patronymic, @phone, @password, @type, @role_ref)";

                    cmd.Parameters.AddWithValue("user_id", i);
                    cmd.Parameters.AddWithValue("email", faker.Internet.Email());
                    cmd.Parameters.AddWithValue("first_name", faker.Name.FirstName());
                    cmd.Parameters.AddWithValue("last_name", faker.Name.LastName());
                    cmd.Parameters.AddWithValue("patronymic", faker.Name.LastName());
                    cmd.Parameters.AddWithValue("phone", faker.Phone.PhoneNumber());

                    string plainPassword = faker.Internet.Password();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

                    cmd.Parameters.AddWithValue("password", hashedPassword);

                    int randomRoleID = roleIDs[random.Next(roleIDs.Length)];
                    cmd.Parameters.AddWithValue("role_ref", randomRoleID);

                    if (randomRoleID == 1)
                    {
                        string[] doctorTypes = { "surgeon", "therapist", "pediatrician" };
                        string randomDoctorType = doctorTypes[random.Next(doctorTypes.Length)];
                        cmd.Parameters.AddWithValue("type", randomDoctorType);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("type", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    static void FillAppointments(NpgsqlConnection conn, int[] doctorsIDs, int[] patientsIDs)
    {
        var faker = new Faker();
        for (int i = 1; i <= 50; i++)
        {
            if (!RecordExists(conn, "appointments", "appointment_id", i))
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO appointments (appointment_id, date_and_time, message, patient_ref, doctor_ref) VALUES (@appointment_id, @date_and_time, @message, @patient_ref, @doctor_ref)";
                    cmd.Parameters.AddWithValue("appointment_id", i);
                    cmd.Parameters.AddWithValue("date_and_time", DateTime.Now.AddDays(i));
                    cmd.Parameters.AddWithValue("message", faker.Lorem.Sentence());
                    cmd.Parameters.AddWithValue("patient_ref", patientsIDs[new Random().Next(patientsIDs.Length)]);
                    cmd.Parameters.AddWithValue("doctor_ref", doctorsIDs[new Random().Next(doctorsIDs.Length)]);
                    cmd.ExecuteNonQuery();
                }
            }
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