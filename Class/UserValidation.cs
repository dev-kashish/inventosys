using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace InventoryMangementSystem.Class
{
    internal class UserValidation
    {
        private static readonly string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public bool validateUser(string username, string passkey)
        {
            username = generateHash(username);
            passkey = generateHash(passkey);
            using SqlConnection con = new(ConString);
            string CmdString = "SELECT * FROM UData WHERE UserId = @username AND Passkey = @passkey";
            SqlCommand cmd = new(CmdString, con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@passkey", passkey);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            bool isValid = reader.HasRows;
            return isValid;
        }

        public bool validatePassword(string passkey)
        {
            passkey = generateHash(passkey);
            using SqlConnection con = new(ConString);
            string CmdString = "SELECT * FROM UData WHERE Passkey = @passkey";
            SqlCommand cmd = new(CmdString, con);
            cmd.Parameters.AddWithValue("@passkey", passkey);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            bool isValid = reader.HasRows;
            con.Close();
            return isValid;
        }

        public void changePassword(string newPass)
        {
            using SqlConnection con = new(ConString);
            newPass = generateHash(newPass);
            string CmdString = "UPDATE UData SET Passkey=@newPass";
            SqlCommand cmd = new(CmdString, con);
            cmd.Parameters.AddWithValue("@newPass", newPass);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void changeUsername(string newUser)
        {
            using SqlConnection con = new(ConString);
            newUser = generateHash(newUser);
            string CmdString = "UPDATE UData SET UserId=@newUser";
            SqlCommand cmd = new(CmdString, con);
            cmd.Parameters.AddWithValue("@newUser", newUser);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private static string generateHash(string txtString)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(txtString));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
