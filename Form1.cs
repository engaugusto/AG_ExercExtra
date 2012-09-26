using System;
using System.Windows.Forms;

namespace CargaPesada
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Procurando melhor solu��o...";
            Application.DoEvents();

            // Instancianto GA
            GA ga = new GA(
                Convert.ToInt32(txtTipoA.Text),
                Convert.ToInt32(txtTipoB.Text),
                Convert.ToInt32(txtTipoC.Text)
                );

            // Chamando a fun��o que desenha as cargas
            pictureBox1.Image = GUI.DesenhaCarga(ga.FindSolution());
            lblStatus.Text = "Melhor solu��o encontrada.";
        }
    }
}