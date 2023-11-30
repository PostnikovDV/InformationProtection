using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace information_protection_lab
{
    public partial class RSAForm : Form
    {
        public RSAForm()
        {
            InitializeComponent();
        }

        static bool IsPrime(BigInteger number)
        {
            if (number <= 1)
                return false;

            if (number == 2)
                return true;

            if (number % 2 == 0)
                return false;

            for (int i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
        static BigInteger GenerateRandomBigInteger(BigInteger min, BigInteger max)
        {
            Random random = new Random();
            byte[] bytes = new byte[(max.ToByteArray().Length)];
            random.NextBytes(bytes);
            BigInteger result = new BigInteger(bytes);

            if (result < 0)
                result = -result;
            if (result >= min && result <= max)
                return result;
            else
            return result%(max-min+1)+min;
        }

        
        static BigInteger GenerateRandomPrime(BigInteger min, BigInteger max)
        {
            Random random = new Random();
            while (true)
            {
                BigInteger candidate = GenerateRandomBigInteger(min, max + 1);

                if (IsPrime(candidate))
                    return candidate;
            }
        }
        static BigInteger FindNOD(BigInteger temp)
        {
            Random rnd = new Random();
            BigInteger e1;
            do
            {
                 e1 = GenerateRandomBigInteger(2, temp-1);
            } while (Euclid(e1, temp) != 1);
           

            return e1;
        }
        static BigInteger Euclid(BigInteger a, BigInteger b)
        {
            while (b != 0 && a != 0)
            {
                if (a > b)
                {
                    a = a % b;
                }
                else
                {
                    b = b % a;
                }
            }
            
            return a+b;
        }
        static BigInteger ExtendedEuclidean(BigInteger a, BigInteger b, out BigInteger x, out BigInteger Y)
        {
            if (b == 0)
            {
                x = 1;
                Y = 0;
                return a;
            }

            BigInteger x1, y1;
            BigInteger gcd = ExtendedEuclidean(b, a % b, out x1, out y1);

            x = y1;
            Y = x1 - (a / b) * y1;

            return gcd;
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        BigInteger p, q, n, E, m,y;

        private void button3_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || textBox1.Text == "")
            {
                var errorForm = new ErrorForm();
                errorForm.ShowDialog();
            }
            else
            {
                char[] decrypted = new char[encrypted.Length];
                for (int i = 0; i < encrypted.Length; i++)
                {
                    BigInteger c = encrypted[i];
                    BigInteger m2 = BigInteger.ModPow(c, PrivateKey, n);
                    decrypted[i] = (char)m2;
                }
                richTextBox2.Text= new string(decrypted);
            }
            
        }

        BigInteger PrivateKey;
        private void button1_Click(object sender, EventArgs e)
        {
            
            y = -1;
            int BitDepth = 12;
            

            BigInteger min = (BigInteger)Math.Pow(2, BitDepth - 1); // Минимальное значение
            BigInteger max = (BigInteger)Math.Pow(2, BitDepth) - 1; // Максимальное значение

            p = GenerateRandomPrime(min, max);
            textBox1.Text = p.ToString();
            q = GenerateRandomPrime(min, max);
            textBox2.Text = q.ToString();
            n = p * q;
            textBox4.Text = n.ToString();
            m = (p-1)*(q-1);
            textBox3.Text = m.ToString();
            do
            {
                E = FindNOD(m);
            } while (E >= n);
               
           
            textBox5.Text = E.ToString();
            textBox7.Text = y.ToString();
            BigInteger x, y1;

            BigInteger gcd = ExtendedEuclidean(E, m, out x, out y1);
            if (x < 0)
            {
                x += m;
            }
            PrivateKey = x;
            textBox6.Text = PrivateKey.ToString();

         


        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        BigInteger[] encrypted;
        private void button2_Click(object sender, EventArgs e)
        {
            if(richTextBox1.Text == "" || textBox1.Text=="")
            {
                var errorForm = new ErrorForm();
                errorForm.ShowDialog();
            }
            else
            {
                 encrypted = new BigInteger[richTextBox1.Text.Length];
                for (int i = 0; i < richTextBox1.Text.Length; i++)
                {
                    BigInteger m1 = new BigInteger(richTextBox1.Text[i]);
                    BigInteger c = BigInteger.ModPow(m1, E, n);
                    encrypted[i] = c;
                }
                string tmp = string.Join(",", encrypted);
                richTextBox2.Text= tmp;
            }
          


        }
    }
}
