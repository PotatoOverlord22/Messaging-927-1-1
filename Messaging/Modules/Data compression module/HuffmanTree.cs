using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace data_compression
{
    internal class HuffmanTree
    {
        public HuffmanTreeNode? Root { get; set; }
        private List<HuffmanTreeNode> listOfTreeNodes;

        public HuffmanTree()
        {
            this.listOfTreeNodes = new List<HuffmanTreeNode>();
            this.Root = null;
        }


        public class HuffmanTreeNode
        {
            public char Symbol { get; set; }
            public int Frequency { get; set; }
            public HuffmanTreeNode? Left { get; set; }
            public HuffmanTreeNode? Right { get; set; }

            public HuffmanTreeNode(char symbol, int frequency)
            {
                Left = Right = null;
                Symbol = symbol;
                Frequency = frequency;
            }
        }

        public void BuildTreeFromDictionary(Dictionary<char, int> characterFrequencyDictionary)
        {
            foreach (var character in characterFrequencyDictionary)
            {
                listOfTreeNodes.Add(new HuffmanTreeNode(character.Key, character.Value));
            }
            BuildTree();
        }   

        public void BuildTree()
        {
            while (listOfTreeNodes.Count > 1)
            {
               
                listOfTreeNodes.Sort((fistNode, secondNode) => fistNode.Frequency.CompareTo(secondNode.Frequency));

                var leftChild = listOfTreeNodes[0];
                var rightChild = listOfTreeNodes[1];

                var parent = new HuffmanTreeNode('\0', leftChild.Frequency + rightChild.Frequency);
                parent.Left = leftChild;
                parent.Right = rightChild;

                listOfTreeNodes.RemoveAt(0);
                listOfTreeNodes.RemoveAt(0);
                listOfTreeNodes.Add(parent);
            }

            this.Root = listOfTreeNodes[0];
            
        }


        public Dictionary<char, string> GetCharacterCodeDictionary()
        {
            Dictionary<char, string> characterCodeDictionary = new Dictionary<char, string>();
            TraverseTreeAndBuildCode(this.Root, "", characterCodeDictionary);
            return characterCodeDictionary;
        }

        private void TraverseTreeAndBuildCode(HuffmanTreeNode? currentNode,string code, Dictionary<char, string> characterCodeDictionary)
        {

            if (currentNode == null)
            {
                return;
            }

            if (currentNode.Left == null && currentNode.Right == null)
            {
                characterCodeDictionary.Add(currentNode.Symbol, code);

            }
            else
            {
                TraverseTreeAndBuildCode(currentNode.Left, code + "0", characterCodeDictionary);
                TraverseTreeAndBuildCode(currentNode.Right, code + "1", characterCodeDictionary);
            }
        }

    }
}
