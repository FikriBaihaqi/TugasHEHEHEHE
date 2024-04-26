using System;
using System.Data.SqlClient;

namespace SewaPacar
{
    internal class PenggunaJasa
    {

        private string connectionString = "Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog=SewaPacar;User ID=sa;Password=123";

        public void Main()
        {
            PenggunaJasa pj = new PenggunaJasa();

            while (true)
            {
                try
                {
                    Console.Clear();
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        while (true)
                        {
                            Console.WriteLine("\nMenu");
                            Console.WriteLine("1. Insert Pengguna Jasa");
                            Console.WriteLine("2. Read Pengguna Jasa");
                            Console.WriteLine("3. Update Pengguna Jasa");
                            Console.WriteLine("4. Delete Pengguna Jasa");
                            Console.WriteLine("5. Exit");
                            Console.WriteLine("\nEnter your choice (1-5): ");

                            char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                            Console.WriteLine();

                            switch (ch)
                            {
                                case '1':
                                    Console.Clear();
                                    pj.InsertPenggunaJasa(conn);
                                    break;
                                case '2':
                                    Console.Clear();
                                    Console.WriteLine("Pengguna Jasa Data\n");
                                    pj.ReadPenggunaJasa(conn);
                                    break;
                                case '3':
                                    Console.Clear();
                                    pj.UpdatePenggunaJasa(conn);
                                    break;
                                case '4':
                                    Console.Clear();
                                    pj.DeletePenggunaJasa(conn);
                                    break;
                                case '5':
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
        }

        public void InsertPenggunaJasa(SqlConnection con)
        {
            Console.WriteLine("Input data Pengguna Jasa\n");
            Console.WriteLine("Masukkan ID Pengguna Jasa (9 karakter): ");
            string id = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pengguna Jasa: ");
            string nama = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Kelamin (L/P): ");
            string jenisKelamin = Console.ReadLine();
            Console.WriteLine("Masukkan Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Masukkan No Telepon: ");
            string noTelepon = Console.ReadLine();

            string query = "INSERT INTO PenggunaJasa (ID_PenggunaJasa, Nama_PenggunaJasa, JenisKelamin_PenggunaJasa, email_Cust, no_telepon_Cust) VALUES (@id, @nama, @jenisKelamin, @email, @noTelepon)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@jenisKelamin", jenisKelamin);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@noTelepon", noTelepon);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Pengguna Jasa berhasil ditambahkan");
        }

        public void ReadPenggunaJasa(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT ID_PenggunaJasa, Nama_PenggunaJasa, JenisKelamin_PenggunaJasa, email_Cust, no_telepon_Cust FROM PenggunaJasa", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Pengguna Jasa: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Jenis Kelamin: {reader.GetString(2)}, Email: {reader.GetString(3)}, No Telepon: {reader.GetString(4)}");
                }
            }
        }

        public void UpdatePenggunaJasa(SqlConnection con)
        {
            Console.WriteLine("Update data Pengguna Jasa\n");
            Console.WriteLine("Masukkan ID Pengguna Jasa yang akan diupdate: ");
            string idToUpdate = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pengguna Jasa baru: ");
            string newNama = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Kelamin baru (L/P): ");
            string newJenisKelamin = Console.ReadLine();
            Console.WriteLine("Masukkan Email baru: ");
            string newEmail = Console.ReadLine();
            Console.WriteLine("Masukkan No Telepon baru: ");
            string newNoTelepon = Console.ReadLine();

            string query = "UPDATE PenggunaJasa SET Nama_PenggunaJasa = @nama, JenisKelamin_PenggunaJasa = @jenisKelamin, email_Cust = @email, no_telepon_Cust = @noTelepon WHERE ID_PenggunaJasa = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToUpdate);
            cmd.Parameters.AddWithValue("@nama", newNama);
            cmd.Parameters.AddWithValue("@jenisKelamin", newJenisKelamin);
            cmd.Parameters.AddWithValue("@email", newEmail);
            cmd.Parameters.AddWithValue("@noTelepon", newNoTelepon);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Data Pengguna Jasa berhasil diupdate");
            }
            else
            {
                Console.WriteLine("ID Pengguna Jasa tidak ditemukan");
            }
        }

        public void DeletePenggunaJasa(SqlConnection con)
        {
            Console.WriteLine("Delete data Pengguna Jasa\n");
            Console.WriteLine("Masukkan ID Pengguna Jasa yang akan dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM PenggunaJasa WHERE ID_PenggunaJasa = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", idToDelete);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Data Pengguna Jasa berhasil dihapus");
            }
            else
            {
                Console.WriteLine("ID Pengguna Jasa tidak ditemukan");
            }
        }
    }
}
