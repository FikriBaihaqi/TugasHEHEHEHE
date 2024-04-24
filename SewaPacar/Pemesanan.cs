﻿using System;
using System.Data.SqlClient;

namespace SewaPacar
{
    internal class Pemesanan
    {
        public void Main()
        {
            Pemesanan pm = new Pemesanan();
            string connectionString = "Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog={0};User ID=sa;Password=123";

            while (true)
            {
                try
                {
                    Console.Write("\nKetik 'k' untuk terhubung ke database atau 'E' untuk keluar dari aplikasi: ");
                    char chr = Char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();

                    switch (chr)
                    {
                        case 'K':
                            Console.Clear();
                            Console.WriteLine("Masukkan nama database yang dituju kemudian tekan Enter: ");
                            string dbName = Console.ReadLine().Trim();

                            // Membuat database jika belum ada
                            pm.CreateDatabase(dbName);

                            using (SqlConnection conn = new SqlConnection(string.Format(connectionString, dbName)))
                            {
                                conn.Open();
                                Console.Clear();

                                while (true)
                                {
                                    Console.WriteLine("\nMenu");
                                    Console.WriteLine("1. Tambah Data Pemesanan");
                                    Console.WriteLine("2. Melihat Data Pemesanan");
                                    Console.WriteLine("3. Keluar");
                                    Console.WriteLine("\nEnter your choice (1-3): ");

                                    char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                                    Console.WriteLine();

                                    switch (ch)
                                    {
                                        case '1':
                                            Console.Clear();
                                            pm.InsertPemesanan(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            Console.WriteLine("Data Pemesanan\n");
                                            pm.ReadPemesanan(conn);
                                            break;
                                        case '3':
                                            conn.Close();
                                            Console.Clear();
                                            Console.WriteLine("Exiting application...");
                                            return;
                                        default:
                                            Console.Clear();
                                            Console.WriteLine("\nInvalid option");
                                            break;
                                    }
                                }
                            }

                        case 'E':
                            Console.WriteLine("Exiting application...");
                            return;
                        default:
                            Console.WriteLine("\nInvalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}\n");
                    Console.ResetColor();
                }
            }
        }

        public void CreateDatabase(string dbName)
        {
            string masterConnectionString = "Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog=master;User ID=sa;Password=123";
            string createDbQuery = $"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE {dbName}";

            using (SqlConnection conn = new SqlConnection(masterConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(createDbQuery, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void ReadPemesanan(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT ID_Pemesanan, KD_Admin, ID_PenggunaJasa, Nama_PenggunaJasa, Nama_PenyediaJasa, Tanggal_Pemesanan FROM Pemesanan", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Pemesanan: {reader.GetString(0)}, Kode Admin: {reader.GetString(1)}, ID Pengguna Jasa: {reader.GetString(2)}, Nama Pengguna Jasa: {reader.GetString(3)}, Nama Penyedia Jasa: {reader.GetString(4)}, Tanggal Pemesanan: {reader.GetDateTime(5)}");
                }
            }
        }

        public void InsertPemesanan(SqlConnection con)
        {
            Console.WriteLine("Input data Pemesanan\n");
            Console.WriteLine("Masukkan ID Pemesanan (8 karakter): ");
            string idPemesanan = Console.ReadLine();
            Console.WriteLine("Masukkan Kode Admin (9 karakter): ");
            string kodeAdmin = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pengguna Jasa (9 karakter): ");
            string idPenggunaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pengguna Jasa: ");
            string namaPenggunaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Penyedia Jasa: ");
            string namaPenyediaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Tanggal Pemesanan (YYYY-MM-DD HH:MM:SS): ");
            DateTime tanggalPemesanan = DateTime.Parse(Console.ReadLine());

            string query = "INSERT INTO Pemesanan (ID_Pemesanan, KD_Admin, ID_PenggunaJasa, Nama_PenggunaJasa, Nama_PenyediaJasa, Tanggal_Pemesanan) VALUES (@idPemesanan, @kodeAdmin, @idPenggunaJasa, @namaPenggunaJasa, @namaPenyediaJasa, @tanggalPemesanan)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPemesanan", idPemesanan);
            cmd.Parameters.AddWithValue("@kodeAdmin", kodeAdmin);
            cmd.Parameters.AddWithValue("@idPenggunaJasa", idPenggunaJasa);
            cmd.Parameters.AddWithValue("@namaPenggunaJasa", namaPenggunaJasa);
            cmd.Parameters.AddWithValue("@namaPenyediaJasa", namaPenyediaJasa);
            cmd.Parameters.AddWithValue("@tanggalPemesanan", tanggalPemesanan);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Pemesanan berhasil ditambahkan");
        }
    }
}