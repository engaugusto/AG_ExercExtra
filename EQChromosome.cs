using System;
using System.Collections.Generic;
using System.Drawing;

namespace CargaPesada
{
    /// <summary>
    /// Classe que representa um cromossomo do problema das 8 rainhas
    /// </summary>
    public class EQChromosome
    {
        // Vetor com as posições das colunas de cada rainha. A linha é o índice do vetor. 
        private readonly ClassCarga[] _positions;
        // Objeto random para geração de números randômicos
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        // Propriedade que expõe o vetor para leitura
        public ClassCarga[] Positions
        {
            get
            {
                return _positions;
            }
        }

        // Construtor - recebe o vetor com as posições
        public EQChromosome(ClassCarga[] positions)
        {
            this._positions = positions;
        }

        // Método estático que instancia um cromossomo com valores randômicos
        public static EQChromosome CreateRandomChromosome(int numTipoA, int numTipoB, int numTipoC)
        {
            var lstCargas = new List<ClassCarga>();

            GeraTipo(ref lstCargas, numTipoA, Tipos.TipoA);
            GeraTipo(ref lstCargas, numTipoB, Tipos.TipoB);
            GeraTipo(ref lstCargas, numTipoC, Tipos.TipoC);

            return new EQChromosome(lstCargas.ToArray());
        }

        /// <summary>
        /// Gera uma nova Carga a Partir do Tipo e da Quantidade
        /// </summary>
        /// <param name="v"></param>
        /// <param name="tam"></param>
        /// <param name="tipo"></param>
        private static void GeraTipo(ref List<ClassCarga> v, int tam, Tipos tipo)
        {
            for (var i = 0; i < tam; i++)
            {

                var posX = Random.Next(0, 9);
                var posY = Random.Next(0, 9);
                v.Add(new ClassCarga
                          {
                              posX = posX,
                              posY = posY,
                              Tipo = tipo,
                              Largura = GetTamByTipo(tipo)[0],
                              Altura = GetTamByTipo(tipo)[1]
                          });
            }
        }

        /// <summary>
        /// Operação de crossover entre dois cromossomos. 
        /// <param name="pair"> Recebe um cromossomo (par) que "cruzará" como atual.  </param>
        /// <returns> Retorna um filho com genes do cromossomo atual e do par. </returns>
        /// </summary>
        public EQChromosome Crossover(EQChromosome pair)
        {
            var filho = new ClassCarga[_positions.Length];

            for (int i = 0; i < Positions.Length; i++)
            {
                filho[i] = (i % 2 == 0 ? _positions[i] : pair._positions[i]);
            }

            return new EQChromosome(filho);
        }


        /// <summary>
        /// Mutação - uma pequena chance de realizar mutação no cromossomo, mudando um de seus genes.
        /// </summary>
        /// <param name="rate"> Percentual da chance de mutação, número entre 0 e 1. </param>
        public void Mutate(double rate)
        {
            if (Random.Next(1) <= rate)
            {
                int positionGene = Random.Next(_positions.Length);

                _positions[positionGene].posX = Random.Next(0,10);
                _positions[positionGene].posY = Random.Next(0,10);

                if (_positions[positionGene].Tipo == Tipos.TipoB
                    || _positions[positionGene].Tipo == Tipos.TipoC
                    || _positions[positionGene].Tipo == Tipos.TipoBVirado
                    || _positions[positionGene].Tipo == Tipos.TipoCVirado)
                {
                    var storeLargura = _positions[positionGene].Largura;
                    _positions[positionGene].Largura = _positions[positionGene].Altura;
                    _positions[positionGene].Altura = storeLargura;

                    //Virando se Possivel
                if (_positions[positionGene].Tipo == Tipos.TipoB)
                    _positions[positionGene].Tipo = Tipos.TipoBVirado;
                else if (_positions[positionGene].Tipo == Tipos.TipoC)   
                    _positions[positionGene].Tipo = Tipos.TipoCVirado;
                else if (_positions[positionGene].Tipo == Tipos.TipoBVirado)
                    _positions[positionGene].Tipo = Tipos.TipoB;
                else if (_positions[positionGene].Tipo == Tipos.TipoCVirado)
                    _positions[positionGene].Tipo = Tipos.TipoC;
                }
            }
        }

        /// <summary>
        /// Fitness, o cálculo de quanto um cromossomo é bom
        /// </summary>
        /// <returns> Retorna a avaliação deste cromossomo. Valor "zero" é o cromossomo ideal, o objetivo da busca. </returns>
        public double GetFitness()
        {
            double fit = 0;
            for (int i = 0; i < Positions.Length; i++)
            {
                var q = 0;
                for (int j = i + 1; j < Positions.Length; j++)
                {
                    q += VerificaColisao(_positions[i], _positions[j]);
                }
                fit += q;
            }

            return fit;
        }

        private int VerificaColisao(ClassCarga classCarga1, ClassCarga classCarga2)
        {
            int[] tam1 = GetTamByTipo(classCarga1.Tipo);
            int[] tam2 = GetTamByTipo(classCarga2.Tipo);
            int w1, w2, h1, h2;
            w1 = tam1[0];
            h1 = tam1[1];

            w2 = tam2[0];
            h2 = tam2[1];

            //checando colisao
            if (VerificaColisaoComTabuleiro(classCarga1, w1, h1)
                || VerificaColisaoComTabuleiro(classCarga2, w2, h2))
            {
                return 2;
            }
            if (VerificaColisaoEntre(classCarga1, w1, h1, classCarga2, w2, h2))
                return 1;
            return 0;
        }

        private bool VerificaColisaoEntre(ClassCarga classCarga1, int w1, int h1, ClassCarga classCarga2, int w2, int h2)
        {
            var r1 = new Rectangle(classCarga1.posX, classCarga1.posY, w1, h1);
            var r2 = new Rectangle(classCarga2.posX, classCarga2.posY, w2, h2);

            return Rectangle.Intersect(r1, r2) != Rectangle.Empty;
        }

        private bool VerificaColisaoComTabuleiro(ClassCarga classCarga, int w, int h)
        {
            if (classCarga.posX + w > 10
                || classCarga.posX < 0)
                return true;
            if (classCarga.posY + h > 10
                || classCarga.posY < 0)
                return true;
            return false;
        }

        public static int[] GetTamByTipo(Tipos tipos)
        {
            switch (tipos)
            {
                case Tipos.TipoA:
                    return new[] { 3, 3 };
                case Tipos.TipoB:
                    return new[] { 5, 1 };
                case Tipos.TipoC:
                    return new[] { 4, 2 };
                case Tipos.TipoBVirado:
                    return new[] { 1, 5 };
                case Tipos.TipoCVirado:
                    return new[] { 2, 4 };
            }
            throw new Exception();
        }
    }
}
