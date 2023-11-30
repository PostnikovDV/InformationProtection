using System.Windows.Forms;

namespace information_protection_lab
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
        private void button1_Click_1(object sender, EventArgs e)
        {
            var  Lab1Window = new PolybianSquare();
            Lab1Window.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var Lab1Window = new RSAForm();
            Lab1Window.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Lab1Window = new Diffie_Hellman();
            Lab1Window.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var Lab1Window = new BlockEncryptionForm();
            Lab1Window.ShowDialog();
        }
    }
}