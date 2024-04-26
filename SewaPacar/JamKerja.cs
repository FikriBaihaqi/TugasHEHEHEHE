using System;
using System.Data.SqlClient;

namespace SewaPacar
{
    internal class JamKerja
    {
        private const string connectionString = "Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog={0};User ID=sa;Password=123";

        public void Main()
        {
            JamKerja jk = new JamKerja();

            try
            {
               
                using (SqlConnection conn = new SqlConnection(string.Format(connectionString, "SewaPacar")))
                {
                    conn.Open();
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("\nMenu");
                        Console.WriteLine("1. Tambah Data Jam Kerja");
                        Console.WriteLine("2. Melihat Data Jam Kerja");
                        Console.WriteLine("3. Keluar");
                        Console.WriteLine("\nEnter your choice (1-3): ");

                        char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                        Console.WriteLine();

                        switch (ch)
                        {
                            case '1':
                                Console.Clear();
                                jk.InsertJamKerja(conn);
                                break;
                            case '2':
                                Console.Clear();
                                Console.WriteLine("Data Jam Kerja\n");
                                jk.ReadJamKerja(conn);
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
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}\n");
                Console.ResetColor();
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

        public void ReadJamKerja(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT ID_JamKerja, KD_Pegawai, ID_PenyediaJasa, Nama_PenyediaJasa, hari, Pukul FROM JamKerja", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Jam Kerja: {reader.GetString(0)}, Kode Pegawai: {reader.GetString(1)}, ID Penyedia Jasa: {reader.GetString(2)}, Nama Penyedia Jasa: {reader.GetString(3)}, Hari: {reader.GetString(4)}, Pukul: {reader.GetString(5)}");
                }
            }
        }

        public void InsertJamKerja(SqlConnection con)
        {
            Console.WriteLine("Input data Jam Kerja\n");
            Console.WriteLine("Masukkan ID Jam Kerja (8 karakter): ");
            string idJamKerja = Console.ReadLine();
            Console.WriteLine("Masukkan Kode Pegawai (9 karakter): ");
            string kdPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan ID Penyedia Jasa (9 karakter): ");
            string idPenyediaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Penyedia Jasa: ");
            string namaPenyediaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Hari: ");
            string hari = Console.ReadLine();
            Console.WriteLine("Masukkan Pukul: ");
            string pukul = Console.ReadLine();

            string query = "INSERT INTO JamKerja (ID_JamKerja, KD_Pegawai, ID_PenyediaJasa, Nama_PenyediaJasa, hari, Pukul) VALUES (@idJamKerja, @kdPegawai, @idPenyediaJasa, @namaPenyediaJasa, @hari, @pukul)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idJamKerja", idJamKerja);
            cmd.Parameters.AddWithValue("@kdPegawai", kdPegawai);
            cmd.Parameters.AddWithValue("@idPenyediaJasa", idPenyediaJasa);
            cmd.Parameters.AddWithValue("@namaPenyediaJasa", namaPenyediaJasa);
            cmd.Parameters.AddWithValue("@hari", hari);
            cmd.Parameters.AddWithValue("@pukul", pukul);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Jam Kerja berhasil ditambahkan");
        }
    }
}
