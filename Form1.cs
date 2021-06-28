using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private readonly Game _game;

        public Form1()
        {
            InitializeComponent();
            _game = new Game(pontos, canvas, timer);
        }

       
        private void Form_KeyDown(object sender, KeyEventArgs e)
            => _game.KeyInput(e.KeyCode);

        private void Timer_Tick(object sender, EventArgs e)
            => _game.TimeTick();

        private void Paint_Canvas(object sender, PaintEventArgs e)
            => _game.PaintCanvas(e);
        
    }

}
