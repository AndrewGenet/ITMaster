using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;


namespace IT_Master_Ap
{
    static class Program
    {
        //global loginid, admin value, and methods
        public static string LoginID;
        public static int Admin;
        public static String hash(string password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password.ToString());
            var sb = new StringBuilder(data.Length * 2);

            byte[] result;

            SHA1 sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            result = sha.ComputeHash(data);
            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new Login());
       
        }
    }
}
