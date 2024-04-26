using System;
using System.Data.SqlClient;

namespace SewaPacar
{
    internal class Pemesanan
    {
        public void Main()
        {
            Pemesanan pm = new Pemesanan();

            string connectionString = $"Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog=SewaPacar;User ID=sa;Password=123";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
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
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}\n");
                Console.ResetColor();
            }
        }


        public void ReadPemesanan(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT ID_Pemesanan, KD_Pegawai, ID_PenggunaJasa, Nama_PenggunaJasa, Nama_PenyediaJasa, Tanggal_Pemesanan FROM Pemesanan", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime tanggalPemesanan = reader.GetDateTime(5);
                    Console.WriteLine($"ID Pemesanan: {reader.GetString(0)}, Kode Pegawai: {reader.GetString(1)}, ID Pengguna Jasa: {reader.GetString(2)}, Nama Pengguna Jasa: {reader.GetString(3)}, Nama Penyedia Jasa: {reader.GetString(4)}, Tanggal Pemesanan: {tanggalPemesanan.ToString("dd/MM/yyyy HH:mm:ss")}");
                }
            }
        }

        public void InsertPemesanan(SqlConnection con)
        {
            Console.WriteLine("Input data Pemesanan\n");
            Console.WriteLine("Masukkan ID Pemesanan (8 karakter): ");
            string idPemesanan = Console.ReadLine();
            Console.WriteLine("Masukkan Kode Pegawai (9 karakter): ");
            string kodePegawai = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pengguna Jasa (9 karakter): ");
            string idPenggunaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pengguna Jasa: ");
            string namaPenggunaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Penyedia Jasa: ");
            string namaPenyediaJasa = Console.ReadLine();
            Console.WriteLine("Masukkan Tanggal Pemesanan (YYYY-MM-DD HH:mm:ss): ");
            string inputDateTime = Console.ReadLine();

            // Format tanggal dan jam yang diharapkan
            string format = "yyyy-MM-dd HH:mm:ss";

            // Parsing input menjadi objek DateTime sesuai format yang diharapkan
            DateTime tanggalPemesanan;
            while (!DateTime.TryParseExact(inputDateTime, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tanggalPemesanan))
            {
                Console.WriteLine("Format tanggal dan jam tidak valid. Masukkan kembali (YYYY-MM-DD HH:mm:ss): ");
                inputDateTime = Console.ReadLine();
            }

            string query = "INSERT INTO Pemesanan (ID_Pemesanan, KD_Pegawai, ID_PenggunaJasa, Nama_PenggunaJasa, Nama_PenyediaJasa, Tanggal_Pemesanan) VALUES (@idPemesanan, @kodePegawai, @idPenggunaJasa, @namaPenggunaJasa, @namaPenyediaJasa, @tanggalPemesanan)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPemesanan", idPemesanan);
            cmd.Parameters.AddWithValue("@kodePegawai", kodePegawai);
            cmd.Parameters.AddWithValue("@idPenggunaJasa", idPenggunaJasa);
            cmd.Parameters.AddWithValue("@namaPenggunaJasa", namaPenggunaJasa);
            cmd.Parameters.AddWithValue("@namaPenyediaJasa", namaPenyediaJasa);
            cmd.Parameters.AddWithValue("@tanggalPemesanan", tanggalPemesanan);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Pemesanan berhasil ditambahkan");
        }
    }
}
