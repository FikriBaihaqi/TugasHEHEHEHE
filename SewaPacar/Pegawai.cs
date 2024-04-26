using System;
using System.Data.SqlClient;

namespace SewaPacar
{
    internal class Pegawai
    {
        private string connectionString = "Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog=SewaPacar;User ID=sa;Password=123";

        public void Main()
        {
            Pegawai pg = new Pegawai();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("\nMenu");
                        Console.WriteLine("1. Tambah Data Pegawai");
                        Console.WriteLine("2. Melihat Data Pegawai");
                        Console.WriteLine("3. Ubah Data Pegawai");
                        Console.WriteLine("4. Hapus Data Pegawai");
                        Console.WriteLine("5. Keluar");
                        Console.WriteLine("\nEnter your choice (1-5): ");

                        char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                        Console.WriteLine();

                        switch (ch)
                        {
                            case '1':
                                Console.Clear();
                                pg.InsertPegawai(conn);
                                break;
                            case '2':
                                Console.Clear();
                                Console.WriteLine("Data Pegawai\n");
                                pg.ReadPegawai(conn);
                                break;
                            case '3':
                                Console.Clear();
                                pg.UpdatePegawai(conn);
                                break;
                            case '4':
                                Console.Clear();
                                pg.DeletePegawai(conn);
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

        public void ReadPegawai(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Kd_Pegawai, Nama_Pegawai, email, no_telepon FROM Pegawai", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Kode Pegawai: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Email: {reader.GetString(2)}, No Telepon: {reader.GetString(3)}");
                }
            }
        }

        public void InsertPegawai(SqlConnection con)
        {
            Console.WriteLine("Input data Pegawai\n");
            Console.WriteLine("Masukkan Kode Pegawai (9 karakter): ");
            string kodePegawai = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pegawai: ");
            string namaPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Masukkan No Telepon: ");
            string noTelepon = Console.ReadLine();

            string query = "INSERT INTO Pegawai (Kd_Pegawai, Nama_Pegawai, email, no_telepon) VALUES (@kodePegawai, @namaPegawai, @email, @noTelepon)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@kodePegawai", kodePegawai);
            cmd.Parameters.AddWithValue("@namaPegawai", namaPegawai);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@noTelepon", noTelepon);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Pegawai berhasil ditambahkan");
        }

        public void UpdatePegawai(SqlConnection con)
        {
            Console.WriteLine("Masukkan Kode Pegawai yang ingin diubah: ");
            string kodePegawaiToUpdate = Console.ReadLine();

            Console.WriteLine("Masukkan Nama Pegawai baru: ");
            string newNamaPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan Email baru: ");
            string newEmail = Console.ReadLine();
            Console.WriteLine("Masukkan No Telepon baru: ");
            string newNoTelepon = Console.ReadLine();

            string query = "UPDATE Pegawai SET Nama_Pegawai = @newNamaPegawai, email = @newEmail, no_telepon = @newNoTelepon WHERE Kd_Pegawai = @kodePegawai";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@newNamaPegawai", newNamaPegawai);
            cmd.Parameters.AddWithValue("@newEmail", newEmail);
            cmd.Parameters.AddWithValue("@newNoTelepon", newNoTelepon);
            cmd.Parameters.AddWithValue("@kodePegawai", kodePegawaiToUpdate);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Pegawai berhasil diubah");
            else
                Console.WriteLine("Data Pegawai dengan Kode Pegawai tersebut tidak ditemukan");
        }

        public void DeletePegawai(SqlConnection con)
        {
            Console.WriteLine("Masukkan Kode Pegawai yang ingin dihapus: ");
            string kodePegawaiToDelete = Console.ReadLine();

            string query = "DELETE FROM Pegawai WHERE Kd_Pegawai = @kodePegawai";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@kodePegawai", kodePegawaiToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Pegawai berhasil dihapus");
            else
                Console.WriteLine("Data Pegawai dengan Kode Pegawai tersebut tidak ditemukan");
        }
    }
}
