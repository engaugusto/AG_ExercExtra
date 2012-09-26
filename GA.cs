using System;
using System.Collections.Generic;

namespace CargaPesada
{
    /// <summary>
    /// Classe onde deve ser implementado o algoritmo gen�tico
    /// </summary>
    public class GA
    {
        private int numTipoA;
        private int numTipoB;
        private int numTipoC;
        private EQChromosome[] _population;
        // Tamanho da popula��o
        private int populationSize;
        // Taxa de muta��o
        private double mutation_rate;
        // N�mero m�ximo de gera��es e gera��o atual 
        private int generations, generation;

        // Gerador de n�meros rand�micos
        private static Random random = new Random(DateTime.Now.Millisecond);
        private object getSize;
        /// <summary>
        /// Construtor padr�o que recebe e armazena o n�mero de caixas de cada tipo que devem ser armazenadas
        /// </summary>
        /// <param name="numTipoA"></param>
        /// <param name="numTipoB"></param>
        /// <param name="numTipoC"></param>
        public GA(int numTipoA, int numTipoB, int numTipoC)
        {
            this.numTipoA = numTipoA;
            this.numTipoB = numTipoB;
            this.numTipoC = numTipoC;
            
            this.populationSize = 1000;
            this.generations = 2000;
            this.mutation_rate = 0.2;
            this.generation = 0;
            _population = new EQChromosome[populationSize];

            //InitializePopulation();
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < populationSize; i++)
            {
                _population[i] = EQChromosome.CreateRandomChromosome(numTipoA, numTipoB, numTipoC);
            }
        }

        /// <summary>
        /// M�todo que encontrar� a solu��o, retornando a lista com as posi��es das caixas
        /// </summary>
        /// <returns></returns>
        public List<Carga> FindSolution()
        {
            InitializePopulation();

            do
            {
                Selection(populationSize / 3);
                GenerateChildren();
                generation++;
            } while (GetBestIndividual().GetFitness() > 0 && generation < generations);

            var lstCargas = ShowChromossome(GetBestIndividual());
            return lstCargas;
        }

        private List<Carga> ShowChromossome(EQChromosome find)
        {
            var lst = new List<Carga>();
            var i = 0;
            
            foreach (var pos in find.Positions)
            {
                if (pos == null)
                    continue;
                //var getSize = EQChromosome.GetTamByTipo(pos.Tipo);
                lst.Add(new Carga
                            {
                                X = pos.posX,
                                Y = pos.posY,
                                Largura = pos.Largura,
                                Altura = pos.Altura,
                                Tipo = pos.Tipo
                            });
            }
            return lst;
        }

        

        /// <summary>
        /// Gerando os filhos da popula��o atual utilizando operadores de crossover e muta��o.
        /// Para controlar o tamanho da popula��o, o n�mero de filhos gerados n�o deve ultrapassar
        ///  o de indiv�duos eliminados pela sele��o.
        /// </summary>
        private void GenerateChildren()
        {
            for (int i = 0; i < populationSize; i++)
            {
                if (_population[i] == null)
                {
                    int pai = 0;
                    int mae = 0;

                    do
                    {
                        pai = random.Next(populationSize);
                        mae = random.Next(populationSize);
                    } while (_population[pai] == null || _population[mae] == null);

                    _population[i] = _population[pai].Crossover(_population[mae]);
                    _population[i].Mutate(mutation_rate);
                }
            }
        }

        /// <summary>
        /// Opera��o de sele��o, que elimina um n�mero de indiv�duos da popula��o. 
        /// </summary>
        /// <param name="killnumber"> Quantos indiv�duos devem ser eliminados. </param>
        private void Selection(int killnumber)
        {
            while (killnumber > 0)
            {
                int max = 0;

                for (int i = 0; i < 10; i++)
                {
                    int candidato = random.Next(populationSize);
                    if (_population[candidato] == null)
                    {
                        i--;
                        continue;
                    }
                    if (_population[max] == null) continue;

                    if (_population[candidato].GetFitness() > _population[max].GetFitness())
                    {
                        max = candidato;
                    }
                }
                _population[max] = null;
                killnumber--;
            }
        }

        /// <summary>
        /// Retorna o indiv�duo com o melhor fitness
        /// </summary>
        /// <returns></returns>
        public EQChromosome GetBestIndividual()
        {
            int min = 0;

            for (int i = 0; i < populationSize; i++)
            {
                if (_population[i].GetFitness() < _population[min].GetFitness())
                    min = i;
            }

            return _population[min];
        }
    }
}
