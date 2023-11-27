using Npgsql;
using System;
using Bogus;
using BCrypt.Net;
using System.Collections.Generic;
using ConsoleApp3;

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

            var dbFiller = new DatabaseFiller(conn);
            //ClearTable(conn, "appointments");
            //ClearTable(conn, "users");
            //ClearTable(conn, "roles");

            /*FillRoles(conn);
            FillUsers(conn,GetRolesID(conn));
            FillAppointments(conn,GetDoctorsID(conn),GetPatientID(conn));*/

            dbFiller.PrintTableData(conn, "roles");
            dbFiller.PrintTableData(conn, "users");
            dbFiller.PrintTableData(conn, "appointments");

        }
    }
   
}