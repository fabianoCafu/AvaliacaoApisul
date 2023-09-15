using AvaliacaoApisul.Business;
using System.Web.Mvc;

namespace AvaliacaoApisul.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            var servicoElevador = new ElevadorServico(new CalculaPercentual());
            
            ViewData["percentualA"] = servicoElevador.PercentualDeUsoElevadorA();
            ViewData["percentualB"] = servicoElevador.PercentualDeUsoElevadorB();
            ViewData["percentualC"] = servicoElevador.PercentualDeUsoElevadorC();
            ViewData["percentualD"] = servicoElevador.PercentualDeUsoElevadorD();
            ViewData["percentualE"] = servicoElevador.PercentualDeUsoElevadorE();

            ViewBag.ListaAndarMenosUtilizado = servicoElevador.AndarMenosUtilizado();
            ViewBag.ListaElevadorMaisUtilizado = servicoElevador.ElevadorMaisFrequentado();
            ViewBag.ListaElevadorMenosUtilizado = servicoElevador.ElevadorMenosFrequentado();
            ViewBag.ListaPeriodoMaiorFluxoElevadorMaisFrequentado = servicoElevador.PeriodoDeMaiorTransitoElevadorMaisFrequentado();
            ViewBag.ListaPeriodoMaiorFluxoElevadorMenosFrequentado = servicoElevador.PeriodoDeMenorTransitoElevadorMenosFrequentado();
            ViewBag.ListaPeriodoMaiorMaiorUltilizacaoAmbosElevadores = servicoElevador.PeriodoDeMaiorTransitoNoConjuntoDeElevadores();

            return View();
        }
    }
}
