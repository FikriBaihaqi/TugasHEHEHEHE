using System;
using System.Data.SqlClient;

namespace SewaPacar
{
    internal class PenyediaJasa
    {
        private const string connectionString = "Data Source=LOSTVAYNE\\BAIHAQI;Initial Catalog=SewaPacar;User ID=sa;Password=123";

        public void Main()
        {
            PenyediaJasa pr = new PenyediaJasa();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    Console.Clear();

                    while (true)
                    {
                        Console.WriteLine("\nMenu");
                        Console.WriteLine("1. Melihat Seluruh Data");
                        Console.WriteLine("2. Tambah Data Penyedia Jasa");
                        Console.WriteLine("3. Hapus Data Penyedia Jasa");
                        Console.WriteLine("4. Cari Data Penyedia Jasa");
                        Console.WriteLine("5. Perbarui Data Penyedia Jasa");
                        Console.WriteLine("6. Keluar");
                        Console.WriteLine("\nMasukan Pilihan (1-6): ");

                        char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                        Console.WriteLine();

                        switch (ch)
                        {
                            case '1':
                                Console.Clear();
                                Console.WriteLine("Data Penyedia Jasa\n");
                                pr.ReadPenyediaJasa(conn);
                                break;
                            case '2':
                                Console.Clear();
                                pr.InsertPenyediaJasa(conn);
                                break;
                            case '3':
                                Console.Clear();
                                Console.WriteLine("Data Penyedia Jasa\n");
                                pr.ReadPenyediaJasa(conn);
                                pr.DeletePenyediaJasa(conn);
                                break;
                            case '4':
                                Console.Clear();
                                pr.SearchPenyediaJasa(conn);
                                break;
                            case '5':
                                Console.Clear();
                                Console.WriteLine("Data Penyedia Jasa\n");
                                pr.ReadPenyediaJasa(conn);
                                pr.UpdatePenyediaJasa(conn);
                                break;
                            case '6':
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

        public void ReadPenyediaJasa(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT ID_PenyediaJasa, Nama_PenyediaJasa, JenisKelamin_PenyediaJasa, Biaya_per_Jam, email, no_telepon FROM PenyediaJasa", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Penyedia Jasa: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Jenis Kelamin: {reader.GetString(2)}, Biaya per Jam: {reader.GetString(3)}, Email: {reader.GetString(4)}, No Telepon: {reader.GetString(5)}");
                }
            }
        }

        public void InsertPenyediaJasa(SqlConnection con)
        {
            Console.WriteLine("Input data Penyedia Jasa\n");
            Console.WriteLine("Masukkan ID Penyedia Jasa (9 karakter, angka saja): ");
            string id = Console.ReadLine();
            if (!IsValidNumericId(id))
            {
                Console.WriteLine("ID Penyedia Jasa harus terdiri dari 9 karakter angka.");
                return;
            }
            if (IsIdExists(con, id))
            {
                Console.WriteLine("ID Penyedia Jasa sudah ada dalam database.");
                return;
            }

            Console.WriteLine("Masukkan Nama Penyedia Jasa: ");
            string nama = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nama))
            {
                Console.WriteLine("Nama Penyedia Jasa tidak boleh kosong.");
                return;
            }

            Console.WriteLine("Masukkan Jenis Kelamin (L/P): ");
            char jenisKelamin = Char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();
            if (jenisKelamin != 'L' && jenisKelamin != 'P')
            {
                Console.WriteLine("Jenis Kelamin harus diisi dengan 'L' atau 'P'.");
                return;
            }

            Console.WriteLine("Masukkan Biaya per Jam: ");
            string biayaPerJam = Console.ReadLine();
            if (!IsValidNumeric(biayaPerJam))
            {
                Console.WriteLine("Biaya per Jam harus berupa angka.");
                return;
            }

            Console.WriteLine("Masukkan Email: ");
            string email = Console.ReadLine();
            if (!IsValidEmail(email))
            {
                Console.WriteLine("Format Email tidak valid.");
                return;
            }
            if (IsEmailExists(con, email))
            {
                Console.WriteLine("Email sudah ada dalam database.");
                return;
            }

            Console.WriteLine("Masukkan No Telepon: ");
            string noTelepon = Console.ReadLine();
            if (!IsValidNumeric(noTelepon))
            {
                Console.WriteLine("Nomor Telepon harus berupa angka.");
                return;
            }
            if (IsNoTeleponExists(con, noTelepon))
            {
                Console.WriteLine("Nomor Telepon sudah ada dalam database.");
                return;
            }

            string query = "INSERT INTO PenyediaJasa (ID_PenyediaJasa, Nama_PenyediaJasa, JenisKelamin_PenyediaJasa, Biaya_per_Jam, email, no_telepon) VALUES (@id, @nama, @jenisKelamin, @biayaPerJam, @email, @noTelepon)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@jenisKelamin", jenisKelamin);
            cmd.Parameters.AddWithValue("@biayaPerJam", biayaPerJam);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@noTelepon", noTelepon);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Penyedia Jasa berhasil ditambahkan");
        }

        public void DeletePenyediaJasa(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Penyedia Jasa yang ingin dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM PenyediaJasa WHERE ID_PenyediaJasa = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Penyedia Jasa berhasil dihapus");
            else
                Console.WriteLine("Data Penyedia Jasa dengan ID tersebut tidak ditemukan");
        }

        public void SearchPenyediaJasa(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Penyedia Jasa yang ingin dicari: ");
            string idToSearch = Console.ReadLine();

            string query = "SELECT ID_PenyediaJasa, Nama_PenyediaJasa FROM PenyediaJasa WHERE ID_PenyediaJasa = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID Penyedia Jasa: {reader.GetString(0)}, Nama: {reader.GetString(1)}");
                }
                else
                {
                    Console.WriteLine("Data Penyedia Jasa tidak ditemukan");
                }
            }
        }

        public void UpdatePenyediaJasa(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Penyedia Jasa yang ingin diperbarui: ");
            string idToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Nama_PenyediaJasa, email, no_telepon FROM PenyediaJasa WHERE ID_PenyediaJasa = @id";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentNama = reader.GetString(0);
                    string currentEmail = reader.GetString(1);
                    string currentNoTelepon = reader.GetString(2);

                    Console.WriteLine($"Data saat ini - Nama: {currentNama}, Email: {currentEmail}, No Telepon: {currentNoTelepon}");

                    Console.WriteLine("\nMasukkan informasi baru (kosongkan jika tidak ingin mengubah):");

                    Console.WriteLine("Nama Penyedia Jasa: ");
                    string newNama = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNama))
                        newNama = currentNama;

                    Console.WriteLine("Email: ");
                    string newEmail = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newEmail))
                        newEmail = currentEmail;

                    Console.WriteLine("No Telepon: ");
                    string newNoTelepon = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNoTelepon))
                        newNoTelepon = currentNoTelepon;

                    reader.Close();

                    string updateQuery = "UPDATE PenyediaJasa SET Nama_PenyediaJasa = @nama, email = @email, no_telepon = @noTelepon WHERE ID_PenyediaJasa = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@nama", newNama);
                    updateCmd.Parameters.AddWithValue("@email", newEmail);
                    updateCmd.Parameters.AddWithValue("@noTelepon", newNoTelepon);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Penyedia Jasa berhasil diperbarui");
                    else
                        Console.WriteLine("Data Penyedia Jasa gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data Penyedia Jasa tidak ditemukan");
                }
            }
        }

        private bool IsValidNumericId(string id)
        {
            return id.Length == 9 && IsNumeric(id);
        }

        private bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private bool IsValidNumeric(string input)
        {
            decimal result;
            return Decimal.TryParse(input, out result);
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

        private bool IsIdExists(SqlConnection con, string id)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PenyediaJasa WHERE ID_PenyediaJasa = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private bool IsEmailExists(SqlConnection con, string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PenyediaJasa WHERE email = @email", con);
            cmd.Parameters.AddWithValue("@email", email);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private bool IsNoTeleponExists(SqlConnection con, string noTelepon)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM PenyediaJasa WHERE no_telepon = @noTelepon", con);
            cmd.Parameters.AddWithValue("@noTelepon", noTelepon);
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }
}
