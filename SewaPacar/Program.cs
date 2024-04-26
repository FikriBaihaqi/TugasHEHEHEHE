using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SewaPacar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("\n Halaman Utama");
                    Console.WriteLine("1. Menu Penyedia Jasa");
                    Console.WriteLine("2. Menu Jam Kerja");
                    Console.WriteLine("3. Menu Pengguna Jasa");
                    Console.WriteLine("4. Menu Pemesanan");
                    Console.WriteLine("5. Menu Pegawai");

                    Console.Write("Masukan Pilihan (1-5) : ");
                    char choice = Convert.ToChar(Console.ReadLine());

                    switch (choice)
                    {
                        case '1':
                            PenyediaJasa penyediajasa = new PenyediaJasa();
                            penyediajasa.Main();
                            break;
                        case '2':
                            JamKerja jamkerja = new JamKerja();
                            jamkerja.Main();
                            break;
                        case '3':
                            PenggunaJasa penggunajasa = new PenggunaJasa();
                            penggunajasa.Main();
                            break;
                        case '4':
                            Pemesanan pemesanan = new Pemesanan();
                            pemesanan.Main();
                            break;
                        case '5':
                            Pegawai pegawai = new Pegawai();
                            pegawai.Main();
                            break;
                        default:
                            Console.WriteLine("Pilihan Tidak Tersedia!");
                            break;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("\nCheck for the value entered");
                }
            }
        }
    }
}