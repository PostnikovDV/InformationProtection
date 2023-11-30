using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace information_protection_lab
{
    public partial class BlockEncryptionForm : Form
    {
        public BlockEncryptionForm()
        {
            InitializeComponent();
        }
        private  string decryptedPath = "";
        private  string encryptedPath = "";
        private string decryptedText = "";
        private string encryptedText = "";


        private const int sizeOfBlock = 32;
        private const int sizeOfChar = 16; // Размер одного символа (in Unicode 16 bit)
        private const int sizeKey = 32;
        private const int Rounds = 16;
        private const int key_chars = sizeKey / sizeOfChar;
        private const int keyShift = 1;
        private const int symbolsInBlock = sizeOfBlock / sizeOfChar;
        private ulong keyUlong;

        private string Load(string filter)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = filter;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            return "";
        }
      
        private string Read(string path)
        {
            return File.ReadAllText(path);
        }
        private void Print(string path, string text)
        {
            File.WriteAllText(path, text);
        }
        private ulong StringToUlong(string str)
        {
            ulong result = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                result = result << sizeOfChar;
                result += str[i];
            }
            return result;
        }

        private string UlongToString(ulong ul)
        {
            string str = "";
            for (int i = 0; i < symbolsInBlock / 2; ++i)
            {
                str += (char)ul;
                ul = ul >> sizeOfChar;
            }
            return str;
        }

        private void ShiftLeft(ref string oldKey)
        {
            string newKey = oldKey.Substring(keyShift);
            newKey += oldKey.Substring(0, keyShift);
            oldKey = newKey;
            keyUlong = StringToUlong(oldKey);
        }

        private void ShiftRight(ref string oldKey)
        {
            string newKey = oldKey.Substring(oldKey.Length - keyShift);
            newKey += oldKey.Substring(0, oldKey.Length - keyShift);
            oldKey = newKey;
            keyUlong = StringToUlong(oldKey);
        }

        private string PadString(string str)
        {
            int count = symbolsInBlock - (str.Length % symbolsInBlock);
            if (count != symbolsInBlock)
            {
                for (int i = 0; i < count; ++i)
                {
                    str += " ";
                }
            }
            return str;
        }

        private string EncryptBlock(string block)
        {
            for (int i = 0; i < Rounds; ++i)
            {
                string stringLeft = block.Substring(0, block.Length / 2);
                string stringRight = block.Substring(block.Length / 2);

                ulong ulongLeft = StringToUlong(stringLeft);
                ulong ulongRight = StringToUlong(stringRight);

                ulong tmp = ulongLeft ^ (ulongRight ^ keyUlong);
                ulongLeft = ulongRight;
                ulongRight = tmp;

                stringLeft = UlongToString(ulongLeft);
                stringRight = UlongToString(ulongRight);

                block = stringLeft + stringRight;
            }
            return block;
        }

        private string DecryptBlock(string block)
        {
            for (int i = 0; i < Rounds; ++i)
            {
                string stringLeft = block.Substring(0, block.Length / 2);
                string stringRight = block.Substring(block.Length / 2);

                ulong ulongLeft = StringToUlong(stringLeft);
                ulong ulongRight = StringToUlong(stringRight);

                ulong tmp = ulongRight ^ (ulongLeft ^ keyUlong);
                ulongRight = ulongLeft;
                ulongLeft = tmp;

                stringLeft = UlongToString(ulongLeft);
                stringRight = UlongToString(ulongRight);

                block = stringLeft + stringRight;
            }
            return block;
        }

        private void Initialize(string key)
        {
            keyUlong = StringToUlong(key);
        }

        public string Encrypt(string text, string key)
        {
            Initialize(key);
            string result = "";
            text = PadString(text);
            int count = text.Length / symbolsInBlock;
            for (int i = 0; i < count; ++i)
            {
                int start = i * symbolsInBlock;
                result += EncryptBlock(text.Substring(start, symbolsInBlock));
                ShiftRight(ref key);
            }
            return result;
        }

        public string Decrypt(string text, string key)
        {
            string tmpKey = key;

            Initialize(key);

            if (text.Length % symbolsInBlock != 0)
            {
                return "Ошибка: зашифрованная строка в неверном формате.";
            }

            string result = "";
            int count = text.Length / symbolsInBlock;
            for (int i = count - 1; i >= 0; --i)
            {
                ShiftLeft(ref key);
                int start = i * symbolsInBlock;
                result = DecryptBlock(text.Substring(start, symbolsInBlock)) + result;
            }

            key = tmpKey;
            text = Encrypt(result, key);

            key = tmpKey;
            Initialize(key);
            result = "";
            count = text.Length / symbolsInBlock;
            for (int i = count - 1; i >= 0; --i)
            {
                ShiftLeft(ref key);
                int start = i * symbolsInBlock;
                result = DecryptBlock(text.Substring(start, symbolsInBlock)) + result;
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Key = GenerateRandomKey();
            textBox1.Text = Key;
            button2.Enabled = true;
      

        }
        private string GenerateRandomKey()
        {

            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string key = "";

            Random random = new Random();

            for (int i = 0; i < sizeKey; i++)
            {
                char temp = alphabet[random.Next(alphabet.Length)];
                key += temp;
            }

            return key;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                var tmp = new ErrorForm();
                tmp.ShowDialog();
                return;
            }
            decryptedPath = Load("text files (*.txt)|*.txt");
            textBox2.Text = decryptedPath;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                var tmp = new ErrorForm();
                tmp.ShowDialog();
                return;
            }
            string filePath = textBox2.Text; 
            string key = textBox1.Text;
            encryptedPath = Load("text files (*.txt)|*.txt");

            decryptedText = Read(filePath);
            string txt = Encrypt(decryptedText, key);
            Print(encryptedPath, txt);
            textBox3.Text = encryptedPath;
            button4.Enabled = true;
        }
      
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                var tmp = new ErrorForm();
                tmp.ShowDialog();
                return;
            }
            encryptedText = Read(encryptedPath);
            string txt = Decrypt(encryptedText, textBox1.Text);
            decryptedPath = Load("text files (*.txt)|*.txt");
            Print(decryptedPath, txt);
            textBox4.Text = decryptedPath;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
