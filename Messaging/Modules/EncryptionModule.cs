using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Modules
{
    public class EncryptionModule
    {
        private string plainText { get; set; }

        private string key { get; set; }

        public EncryptionModule(string text)
        {
            GenerateKey();
            plainText = text;
        }


        public string getKey()
        {
            return key;
        }

        public void setKey(string newKey)
        {
            this.key = newKey;
        }

        private void GenerateKey()
        {

            int keyLength = 16;
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < keyLength; i++)
            {
                sb.Append((char)random.Next(32, 127));
            }
            key = sb.ToString();
        }

        public string Encrypt()
        {
            StringBuilder encryptedText = new StringBuilder();
            for (int i = 0; i < plainText.Length; i++)
            {
                encryptedText.Append((char)(plainText[i] ^ this.key[i % this.key.Length]));
            }
            plainText = encryptedText.ToString();
            return plainText;
        }

        public string Decrypt()
        {
            return Encrypt();
        }
    }
}
