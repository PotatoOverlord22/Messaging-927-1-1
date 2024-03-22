using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_compression
{
    internal class HuffmanDataCompression
    {

        string inputFilePath;
        HuffmanTree huffmanTree;
        char[]? allCharactersFromInputFile;

        public HuffmanDataCompression(string inputFilePath)
        {
            this.inputFilePath = inputFilePath;
            Dictionary<char, int> characterFrequencyDictionary = ReadInputFile();
            huffmanTree = new HuffmanTree();
            huffmanTree.BuildTreeFromDictionary(characterFrequencyDictionary);
            CreateKeyFile();
            CompressData();
        }


        public Dictionary<char, int> ReadInputFile()
        {
            string inputData = "";
            if (File.Exists(inputFilePath))
            {
                inputData = File.ReadAllText(inputFilePath);
            }

            this.allCharactersFromInputFile = inputData.ToCharArray();
            Dictionary<char, int> characterFrequencyDictionary = new Dictionary<char, int>();
            foreach (char c in allCharactersFromInputFile)
            {
                if (characterFrequencyDictionary.ContainsKey(c))
                {
                    characterFrequencyDictionary[c] = characterFrequencyDictionary[c] + 1;
                }
                else
                {
                    characterFrequencyDictionary.Add(c, 1);
                }
            }

            return characterFrequencyDictionary;
        }


        public void CreateKeyFile()
        {

            Dictionary<char, string> characterCodeDictionary = huffmanTree.GetCharacterCodeDictionary();

            string keyFilePath = inputFilePath.Replace("input.txt", "key.txt");
            using (StreamWriter sw = new StreamWriter(keyFilePath))
            {
                foreach (var character in characterCodeDictionary)
                {
                    sw.WriteLine(character.Key + " " + character.Value);
                }
            }

        }


        public void CompressData()
        {

            Dictionary<char, string> characterCodeDictionary = huffmanTree.GetCharacterCodeDictionary();

            string encodedData = "";
            if (this.allCharactersFromInputFile != null)
            {

                foreach (char c in allCharactersFromInputFile)
                {
                    encodedData += characterCodeDictionary[c];
                }

            }

            if (encodedData.Length < 8)
            {
                int padding = 8 - encodedData.Length;
                encodedData = encodedData.PadLeft(encodedData.Length + padding, '0');
            }

            if (encodedData.Length % 8 != 0)
            {
                int padding = 8 - encodedData.Length % 8;
                encodedData = encodedData.PadRight(encodedData.Length + padding, '0');
            }


            var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)
                          .TakeWhile(i => i < encodedData.Length)
                          .Select(i => encodedData.Substring(i, 8)) 
                          .Select(s => Convert.ToByte(s, 2)) 
                          .ToArray();

            string outputFilePath = inputFilePath.Replace("input.txt", "output.bin");
            File.WriteAllBytes(outputFilePath, byteArray);

        }
    }
}
