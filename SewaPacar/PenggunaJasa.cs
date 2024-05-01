using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

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
                            Console.WriteLine("1. Tambah Pengguna Jasa");
                            Console.WriteLine("2. Lihat Pengguna Jasa");
                            Console.WriteLine("3. Edit Pengguna Jasa");
                            Console.WriteLine("4. Hapus Pengguna Jasa");
                            Console.WriteLine("5. Keluar");
                            Console.WriteLine("\nMasukan Pilihan (1-5): ");

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
                                    Console.WriteLine("List Pengguna Jasa\n");
                                    pj.ReadPenggunaJasa(conn);
                                    break;
                                case '3':
                                    Console.Clear();
                                    pj.ReadPenggunaJasa(conn);
                                    Console.WriteLine("List Pengguna Jasa\n");
                                    pj.UpdatePenggunaJasa(conn);
                                    break;
                                case '4':
                                    Console.Clear();
                                    pj.ReadPenggunaJasa(conn);
                                    Console.WriteLine("List Pengguna Jasa\n");
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

            // Validasi ID Pengguna Jasa
            if (string.IsNullOrEmpty(id) || id.Length != 9)
            {
                Console.WriteLine("ID Pengguna Jasa harus terdiri dari 9 karakter.");
                return;
            }

            // Periksa apakah ID Pengguna Jasa sudah ada dalam database
            if (IsIdPenggunaJasaExists(con, id))
            {
                Console.WriteLine("ID Pengguna Jasa sudah ada dalam database.");
                return;
            }

            Console.WriteLine("Masukkan Nama Pengguna Jasa: ");
            string nama = Console.ReadLine();

            // Validasi Nama Pengguna Jasa
            if (string.IsNullOrEmpty(nama) || !IsValidName(nama))
            {
                Console.WriteLine("Nama Pengguna Jasa tidak boleh kosong dan hanya boleh mengandung huruf.");
                return;
            }

            Console.WriteLine("Masukkan Jenis Kelamin (L/P): ");
            string jenisKelamin = Console.ReadLine();

            // Validasi Jenis Kelamin
            if (string.IsNullOrEmpty(jenisKelamin) || (jenisKelamin.ToUpper() != "L" && jenisKelamin.ToUpper() != "P"))
            {
                Console.WriteLine("Jenis Kelamin harus diisi dengan 'L' atau 'P'.");
                return;
            }

            Console.WriteLine("Masukkan Email: ");
            string email = Console.ReadLine();

            // Validasi Email
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Format Email tidak valid.");
                return;
            }

            // Periksa apakah Email sudah ada dalam database
            if (IsEmailExists(con, email))
            {
                Console.WriteLine("Email sudah ada dalam database.");
                return;
            }

            Console.WriteLine("Masukkan No Telepon: ");
            string noTelepon = Console.ReadLine();

            // Validasi Nomor Telepon
            if (string.IsNullOrEmpty(noTelepon))
            {
                Console.WriteLine("Nomor Telepon tidak boleh kosong.");
                return;
            }

            // Periksa apakah Nomor Telepon sudah ada dalam database
            if (IsNoTeleponExists(con, noTelepon))
            {
                Console.WriteLine("Nomor Telepon sudah ada dalam database.");
                return;
            }

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

            // Periksa apakah ID Pengguna Jasa ada dalam database
            if (!IsIdPenggunaJasaExists(con, idToUpdate))
            {
                Console.WriteLine("ID Pengguna Jasa tidak ditemukan dalam database.");
                return;
            }

            // Tampilkan data Pengguna Jasa yang akan diperbarui
            SqlCommand selectCmd = new SqlCommand("SELECT Nama_PenggunaJasa, JenisKelamin_PenggunaJasa, email_Cust, no_telepon_Cust FROM PenggunaJasa WHERE ID_PenggunaJasa = @id", con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string nama = reader.GetString(0);
                    string jenisKelamin = reader.GetString(1);
                    string email = reader.GetString(2);
                    string noTelepon = reader.GetString(3);

                    Console.WriteLine($"Data saat ini: Nama: {nama}, Jenis Kelamin: {jenisKelamin}, Email: {email}, No Telepon: {noTelepon}");

                    Console.WriteLine("\nMasukkan informasi baru (kosongkan jika tidak ingin mengubah):");

                    Console.WriteLine("Nama Pengguna Jasa: ");
                    string newNama = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNama))
                        newNama = nama;

                    Console.WriteLine("Jenis Kelamin (L/P): ");
                    string newJenisKelamin = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJenisKelamin))
                        newJenisKelamin = jenisKelamin;

                    Console.WriteLine("Email: ");
                    string newEmail = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newEmail))
                        newEmail = email;
                    else if (!IsValidEmail(newEmail))
                    {
                        Console.WriteLine("Format Email tidak valid.");
                        return;
                    }

                    Console.WriteLine("No Telepon: ");
                    string newNoTelepon = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNoTelepon))
                        newNoTelepon = noTelepon;
                    // Validasi Nomor Telepon
                    else if (!IsValidPhoneNumber(newNoTelepon))
                    {
                        Console.WriteLine("Format Nomor Telepon tidak valid.");
                        return;
                    }

                    // Update data Pengguna Jasa
                    string updateQuery = "UPDATE PenggunaJasa SET Nama_PenggunaJasa = @nama, JenisKelamin_PenggunaJasa = @jenisKelamin, email_Cust = @email, no_telepon_Cust = @noTelepon WHERE ID_PenggunaJasa = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@nama", newNama);
                    updateCmd.Parameters.AddWithValue("@jenisKelamin", newJenisKelamin);
                    updateCmd.Parameters.AddWithValue("@email", newEmail);
                    updateCmd.Parameters.AddWithValue("@noTelepon", newNoTelepon);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Pengguna Jasa berhasil diperbarui");
                    else
                        Console.WriteLine("Data Pengguna Jasa gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data Pengguna Jasa tidak ditemukan");
                }
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

        private bool IsIdPenggunaJasaExists(SqlConnection con, string id)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PenggunaJasa WHERE ID_PenggunaJasa = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private bool IsEmailExists(SqlConnection con, string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PenggunaJasa WHERE email_Cust = @email", con);
            cmd.Parameters.AddWithValue("@email", email);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private bool IsNoTeleponExists(SqlConnection con, string noTelepon)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PenggunaJasa WHERE no_telepon_Cust = @noTelepon", con);
            cmd.Parameters.AddWithValue("@noTelepon", noTelepon);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidName(string name)
        {
            // Regular expression pattern to allow only letters and spaces
            string pattern = "^[a-zA-Z ]+$";
            return Regex.IsMatch(name, pattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Regular expression pattern to validate phone number
            string pattern = @"^\d{10,15}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
