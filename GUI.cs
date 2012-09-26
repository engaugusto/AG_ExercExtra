using System;
using System.Collections.Generic;
using System.Drawing;

namespace CargaPesada
{
    class GUI
    {
        /// <summary>
        /// Método que recebe uma lista de caixas e retorna um bitmap com 
        /// desenho das posições das mesmas.
        /// </summary>
        /// <param name="cargas"></param>
        /// <returns></returns>
        public static Bitmap DesenhaCarga(List<Carga> cargas)
        {
            Bitmap b = new Bitmap(502, 502);
            Graphics g = Graphics.FromImage(b);

            // Agora as caixas
            foreach (Carga c in cargas)
            {
                Brush brushColor = GetByTipo(c.Tipo);
                g.DrawRectangle(new Pen(brushColor), c.X * 25 + 1, c.Y * 25 + 1, c.Largura * 25 - 2, c.Altura * 25 - 2);
                g.FillRectangle(brushColor, c.X * 25 + 1, c.Y * 25 + 1, c.Largura * 25 - 2, c.Altura * 25 - 2);
            }

            // Desenhando espaços do caminhão
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    g.DrawRectangle(new Pen(Brushes.Gray), i * 25, j * 25, 25, 25);
                }
            }

            // Retorno
            return b;
        }

        private static Brush GetByTipo(Tipos tipos)
        {
            switch (tipos)
            {
                case Tipos.TipoA:
                    return Brushes.Red;
                case Tipos.TipoB:
                    return Brushes.Purple;
                case Tipos.TipoC:
                    return Brushes.RoyalBlue;
                case Tipos.TipoBVirado:
                    return Brushes.Purple;
                case Tipos.TipoCVirado:
                    return Brushes.RoyalBlue;
            }
            throw new NotImplementedException();
        }
    }
}
