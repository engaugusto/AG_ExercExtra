using System;
using System.Collections.Generic;

namespace CargaPesada
{
    /// <summary>
    /// Classe onde deve ser implementado o algoritmo genético
    /// </summary>
    public class GA
    {
        private int numTipoA;
        private int numTipoB;
        private int numTipoC;
        private EQChromosome[] _population;
        // Tamanho da população
        private int populationSize;
        // Taxa de mutação
        private double mutation_rate;
        // Número máximo de gerações e geração atual 
        private int generations, generation;

        // Gerador de números randômicos
        private static Random random = new Random(DateTime.Now.Millisecond);
        private object getSize;
        /// <summary>
        /// Construtor padrão que recebe e armazena o número de caixas de cada tipo que devem ser armazenadas
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
        /// Método que encontrará a solução, retornando a lista com as posições das caixas
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
        /// Gerando os filhos da população atual utilizando operadores de crossover e mutação.
        /// Para controlar o tamanho da população, o número de filhos gerados não deve ultrapassar
        ///  o de indivíduos eliminados pela seleção.
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
        /// Operação de seleção, que elimina um número de indivíduos da população. 
        /// </summary>
        /// <param name="killnumber"> Quantos indivíduos devem ser eliminados. </param>
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
        /// Retorna o indivíduo com o melhor fitness
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
