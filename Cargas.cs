namespace CargaPesada
{

    /// <summary>
    /// Classe que retorna as cargas possíveis no problema 
    /// </summary>
    public class Carga
    {
        private int largura;
        private int altura;
        private int x;
        private int y;
        public Tipos Tipo { get; set; }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Altura
        {
            get { return altura; }
            set { altura = value; }
        }
        public int Largura
        {
            get { return largura; }
            set { largura = value; }
        }

        public Carga(int x, int y, int largura, int altura)
        {
            this.x = x;
            this.y = y;
            this.altura = altura;
            this.largura = largura;
        }

        public Carga()
        {
            // TODO: Complete member initialization
        }
    }
}
