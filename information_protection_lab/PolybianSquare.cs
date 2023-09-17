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
        char[,] matrix = { {'a','b','c','d','e'},
                                       {'f','g','h','i','k'},
                                       {'l','m','n','o','p'},
                                       { 'q','r','s','t','u'},
                                       { 'v','w','x','y','z'},
                                       {'A','B','C','D','E'},
                                       {'F','G','H','I','K'},
                                       {'L','M','N','O','P'},
                                       { 'Q','R','S','T','U'},
                                       { 'V','W','X','Y','Z'},
                                        { '1','2','3','4','5'},
                                       { '6','7','8','9','0'},
                                       { '!','.','?',',',';'}
                                     };
       
       
        private void button2_Click(object sender, EventArgs e)
        {
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
                textBox1.Text = new_message;
                button2.Enabled = false;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (message == null)
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
                textBox1.Text = new_message;
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
    }
}
