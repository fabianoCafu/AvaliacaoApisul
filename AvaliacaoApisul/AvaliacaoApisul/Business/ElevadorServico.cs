using AvaliacaoApisul.Models;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace AvaliacaoApisul.Business
{
    public class ElevadorServico : IElevadorService
    {
        private readonly string pathFile;
        private readonly CalculaPercentual calculaPercentual;

        #region Construtor 

        public ElevadorServico (CalculaPercentual calculaPercentual)
        {
            pathFile = HttpContext.Current.Server.MapPath(Path.Combine(ConfigurationManager.AppSettings["PathFile"].ToString()));
            this.calculaPercentual = calculaPercentual;
        }

        #endregion

        public List<int> AndarMenosUtilizado()
        {
            return GetAndaresMenosUtilizados(pathFile);
        }

        public List<char> ElevadorMaisFrequentado()
        {
            return GetElevadorMaisFrequentado(pathFile);
        }

        public List<char> ElevadorMenosFrequentado()
        {
            return GetElevadorMenosFrequentado(pathFile);
        }

        public List<char> PeriodoDeMaiorTransitoElevadorMaisFrequentado()
        {
            return GetPeriodoDeMaioFluxoElevadorMaisFrequentado(pathFile);
        }

        public List<char> PeriodoDeMenorTransitoElevadorMenosFrequentado()
        {
            return GetPeriodoDeMenorFluxoElevadorMenosFrequentado(pathFile);
        }

        public List<char> PeriodoDeMaiorTransitoNoConjuntoDeElevadores()
        {
            return GetPeriodoUtilizacaoTodosElevadores(pathFile);
        }

        public float PercentualDeUsoElevadorA()
        {
            return GetPercentual(pathFile, 'A');
        }

        public float PercentualDeUsoElevadorB()
        {
            return GetPercentual(pathFile, 'B');
        }

        public float PercentualDeUsoElevadorC()
        {
            return GetPercentual(pathFile, 'C');
        }

        public float PercentualDeUsoElevadorD()
        {
            return GetPercentual(pathFile, 'D');
        }

        public float PercentualDeUsoElevadorE()
        {
            return GetPercentual(pathFile, 'E');
        }

        #region Metodos private

        private float GetPercentual(
            string path, 
            char elevador)
        {
            var totalDeEntrevistados = Readfile(path).Count();
            var totalUso = Readfile(pathFile).Where(a => a.Elevador == elevador)
                                             .Count();

            return calculaPercentual.Percentual(totalDeEntrevistados, totalUso);
        }

        private List<int> GetAndaresMenosUtilizados(string path)
        {
            var andares = Readfile(path).GroupBy(f => f.Andar)
                                        .OrderBy(a => a.Max(x => x.Andar));

            var quantidade = andares.OrderBy(f => f.Count())
                                    .First()
                                    .Count();

            return andares.Where(a => a.Count() == quantidade)
                          .OrderByDescending(a => a.Count())
                          .Select(a => a.Key) 
                          .ToList();

        }

        private List<char> GetPeriodoUtilizacaoTodosElevadores(string path)
        {
            var periodosElevadores = Readfile(path).GroupBy(e => e.Elevador)
                                                   .SelectMany(e => e.OrderBy(r => r.Turno))
                                                   .Select(r => new { turno = r.Turno })
                                                   .ToList();

            return periodosElevadores.GroupBy(r => r.turno)
                                     .OrderByDescending(e => e.Count())
                                     .First()
                                     .Select(r => r.turno)
                                     .Distinct()
                                     .ToList();
        }

        private List<char> GetPeriodoDeMenorFluxoElevadorMenosFrequentado(string path)
        {

            var periodoMenorFluxiMaisUtilizado = Readfile(path).GroupBy(e => e.Elevador)
                                                               .SelectMany(e => e.OrderBy(x => x.Turno))
                                                               .Select(r => new { turno = r.Turno })
                                                               .ToList();

            return periodoMenorFluxiMaisUtilizado.GroupBy(a => a.turno)
                                                 .OrderBy(e => e.Count())
                                                 .First()
                                                 .Select(a => a.turno)
                                                 .Distinct()
                                                 .ToList();
        }

        private List<char> GetPeriodoDeMaioFluxoElevadorMaisFrequentado(string path)
        {
            var periodoMaiorFluxiMaisUtilizado = Readfile(path).GroupBy(e => e.Elevador)
                                                               .SelectMany(e => e.OrderBy(x => x.Turno))
                                                               .Select(r => new { turno = r.Turno })
                                                               .ToList();

            return periodoMaiorFluxiMaisUtilizado.GroupBy(a => a.turno)
                                                 .OrderByDescending(e => e.Count())
                                                 .First()
                                                 .Select(a => a.turno)
                                                 .Distinct()
                                                 .ToList();
        }

        private List<char> GetElevadorMenosFrequentado(string path)
        {
            var menosFrequentados = Readfile(path).GroupBy(e => e.Elevador)
                                                  .OrderBy(a => a.Count())
                                                  .First()
                                                  .Count();

            return Readfile(pathFile).GroupBy(e => e.Elevador)
                                     .Where(e => e.Count() == menosFrequentados)
                                     .Select(e => e.Key)
                                     .ToList();
        }

        private List<char> GetElevadorMaisFrequentado(string path)
        {
            var maisFrequentados = Readfile(pathFile).GroupBy(e => e.Elevador)
                                                     .OrderByDescending(a => a.Count())
                                                     .First()
                                                     .Count();

            return Readfile(pathFile).GroupBy(e => e.Elevador)
                                     .Where(e => e.Count() == maisFrequentados)
                                     .Select(e => e.Key)
                                     .ToList();
        }

        private List<ControleDeElevadore> GetFile(string pathFile)
        {
            var js = new JavaScriptSerializer();
            var listaDadosEntrada = new List<ControleDeElevadore>();
            
            if (File.Exists(Path.Combine(pathFile)))
            {
                var file = File.ReadAllText(Path.Combine(pathFile));
                listaDadosEntrada = js.Deserialize<List<ControleDeElevadore>>(file);
            }

            return listaDadosEntrada;
        }

        private List<ControleDeElevadore> Readfile(string path)
        {    
            var retornoListaDadosEntrada = new List<ControleDeElevadore>();
            
            if (!string.IsNullOrWhiteSpace(path))
            {
                retornoListaDadosEntrada = GetFile(path);
            }
            
            return retornoListaDadosEntrada;
        }

        #endregion
    }
}