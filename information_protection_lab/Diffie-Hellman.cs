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

namespace information_protection_lab
{
    public partial class Diffie_Hellman : Form
    {
        public Diffie_Hellman()
        {
            InitializeComponent();
        }

        int BitDepth = 12;

        BigInteger n;
        BigInteger q;
        BigInteger x;
        BigInteger y;
        BigInteger kx;
        BigInteger ky;
        BigInteger A;
        BigInteger B;
        BigInteger[] encrypted;
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
                return result % (max - min + 1) + min;
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
        private BigInteger CalculateAandB(BigInteger x, BigInteger q,BigInteger n)
        {
            BigInteger AB = BigInteger.ModPow(q, x, n);
            return AB;
        }
        private BigInteger CalculatePrivateKey(BigInteger AB, BigInteger xy, BigInteger n)
        {
            BigInteger KxKy = BigInteger.ModPow(AB, xy, n);
            return KxKy;
        }
        private void Diffie()
        {
            BigInteger min = (BigInteger)Math.Pow(2, BitDepth - 1); // Минимальное значение
            BigInteger max = (BigInteger)Math.Pow(2, BitDepth) - 1; // Максимальное значение
            n = 647;
            textBox1.Text = n.ToString();
            q = 691;
            textBox2.Text = q.ToString();
            x = GenerateRandomPrime(min, max);
            textBox3.Text = x.ToString();
            y = GenerateRandomPrime(min, max);
            textBox4.Text = y.ToString();
            A = CalculateAandB(x, q, n);
            textBox5.Text = A.ToString();
            B = CalculateAandB(y, q, n);
            textBox6.Text = B.ToString();
            kx = CalculatePrivateKey(B, x, n);
            textBox7.Text = kx.ToString();
            ky = CalculatePrivateKey(A, y, n);
            textBox8.Text = ky.ToString();


        }
        private void Genetatiom_Click(object sender, EventArgs e)
        {
            Diffie();

        }


        private void encrypt_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || textBox1.Text == "")
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
                   
                    BigInteger tangen = CalculateTangent(kx,500);
                    BigInteger c = m1 * tangen;
                    encrypted[i] = c;
                }
                string tmp = string.Join(",", encrypted);
                richTextBox3.Text = tmp;
            }
        }
        
        private void dencrypt_Click(object sender, EventArgs e)
        {
            if (richTextBox3.Text == "" || richTextBox1.Text == "" || textBox1.Text == "")
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
                    BigInteger m2 = c/ CalculateTangent(kx,500);
                    decrypted[i] = (char)m2;
                }
                richTextBox2.Text = new string(decrypted);
            }
        }

         static BigInteger Sin(BigInteger x, int precision)
        {
            BigInteger result = 0;

            for (int n = 0; n <= precision; n+=2)
            {
                BigInteger sin = BigInteger.Pow(-1, n/2) * BigInteger.Pow(x, n + 1) / Factorial( n + 1);
                result += sin;
            }

            return result;

        }

        static BigInteger Cos(BigInteger x, int precision)
        {
            BigInteger result = 0;
            for (int n = 0; n <= precision; n++)
            {
                BigInteger cos = BigInteger.Pow(-1, n/2) * BigInteger.Pow(x, n) / Factorial(n);
                result += cos;
            }

            return result;
         
        }
        static BigInteger CalculateTangent(BigInteger x, int precision)
        {
            BigInteger result = 0;

            
            BigInteger sin = Sin(x, precision);
            BigInteger cos = Cos(x, precision);
            Console.WriteLine(sin);
            Console.WriteLine(cos);
           result = sin / cos;
            

            return result;
        }

        static BigInteger Factorial(BigInteger n)
        {
            if (n == 0)
            {
                return 1;
            }

            BigInteger result = 1;
            for (BigInteger i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

       

        private void label11_Click(object sender, EventArgs e)
        {

        }

       
    }
}
