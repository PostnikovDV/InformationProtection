using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace information_protection_lab
{
    public partial class PolybianSquare : Form
    {
        public PolybianSquare()
        {
            InitializeComponent();
            button2.Enabled = false;
        }
        string message;
        bool f = true;
        char[,] matrix = {
                                       {'f','g','h','i','k'},
                                       {'a','b','c','d','e'},
                                       {'l','m','n','o','p'},
                                       {'A','B','C','D','E'},
                                       {'q','r','s','t','u'},
                                       {'v','w','x','y','z'},
                                       {'F','G','H','I','K'},
                                       {'Q','R','S','T','U'},
                                       {'L','M','N','O','P'},
                                       {'1','2','3','4','5'},
                                       {'V','W','X','Y','Z'},
                                       {'6','7','8','9','0'},
                                       {'!','.','?',',',';'}
                                     };
       
       
        private void button2_Click(object sender, EventArgs e)
        {
            message = textBox2.Text;
            if (textBox1.Text == "")
            {
                var errorForm = new ErrorForm();
                errorForm.ShowDialog();
            }
            else
            {
                string new_message = "";
                for (int i = 0; i < message.Length; i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        for (int k = 0; k < matrix.GetLength(1); k++)
                        {
                            if (matrix[j, k] == message[i])
                            {
                                if (j == 0)
                                    new_message += matrix[matrix.GetLength(0) - 1, k];
                                else
                                    new_message += matrix[j - 1, k];
                                break;
                            }

                        }

                    }


                }
                textBox2.Text = new_message;
                button2.Enabled = false;
       
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            message = textBox1.Text;
            if (textBox1.Text == "")
            {
                var errorForm = new ErrorForm();
                errorForm.ShowDialog();
            }
            else
            {
                string new_message = "";
                for (int i = 0; i < message.Length; i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        for (int k = 0; k < matrix.GetLength(1); k++)
                        {
                            if (matrix[j, k] == message[i])
                            {
                                if (j == matrix.GetLength(0) - 1)
                                    new_message += matrix[0, k];
                                else
                                    new_message += matrix[j + 1, k];
                                break;
                            }

                        }

                    }


                }
                textBox2.Text = new_message;
                button2.Enabled = true;
  
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            message = textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
