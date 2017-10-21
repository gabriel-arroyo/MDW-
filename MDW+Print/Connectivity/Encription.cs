using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDW_Print.Connectivity
{
    public static class Encription
    {
        public static string Encript(string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        public static string Decript(string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }

    }
}
