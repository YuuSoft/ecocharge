using System.Net;
using System.Web.Http;
using System.Net.Http;
using System;
using Models;
using BLL.Service;
using System.Linq;

namespace WebAPI
{
    public class ConsumoController : ApiController
    {
        [HttpGet]
        [AcceptVerbs("GET")]
        public IHttpActionResult Register([FromUri] string serial, [FromUri] string watts, [FromUri] string ciclos)
        {
            try
            {
                using (var servicoAparelho = new Service<Aparelho>())
                using (var servicoEcoSense = new Service<EcoSense>())
                using (var servicoSerial = new Service<SerialAparelho>())
                {
                    if (serial == null)
                        throw new Exception("Argumentos não fornecidos.");

                    if (watts == null)
                        throw new Exception("Argumentos não fornecidos.");

                    if (ciclos == null)
                        throw new Exception("Argumentos não fornecidos.");

                    var teste = (from TabelaSerial in servicoSerial.GetRepository()
                                 where TabelaSerial.Serial == serial
                                 join TabelaEcoSense in servicoEcoSense.GetRepository() on TabelaSerial.Serial equals TabelaEcoSense.SerialAparelho.Serial
                                 where TabelaEcoSense.Ativo == true
                                 join TabelaAparelho in servicoAparelho.GetRepository() on TabelaEcoSense.Id equals TabelaAparelho.EcoSenseId
                                 where TabelaAparelho.Ativo == true
                                 select TabelaAparelho).FirstOrDefault();

                    var consumo = new Consumo();
                    consumo.Serial = serial;
                    consumo.PotenciaWatts = Convert.ToDecimal(watts);
                    consumo.Ciclos = Convert.ToInt32(ciclos);

                    var response = new { cor = "", tensao = "" };
                    var serverResponse = Request.CreateResponse(HttpStatusCode.OK, response);

                    return ResponseMessage(serverResponse);
                }
            }
            catch (Exception ex)
            {
                HttpResponseMessage serverResponse = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);

                return ResponseMessage(serverResponse);
            }
        }
    }
}