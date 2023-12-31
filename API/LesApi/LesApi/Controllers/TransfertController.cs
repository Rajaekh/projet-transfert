using LesApi.Models;
using LesApi.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfertController : ControllerBase
    {
        private readonly ITransfert _transfert;
        private readonly IUser _user;
        private readonly IFrais _frais;


        public TransfertController(ITransfert transfert, IUser user, IFrais frais)
        {
            _transfert = transfert;

            _user = user;
            _frais = frais;

        }
        // POST api/<TransfertController>
        [HttpPost]
        [HttpPost]
        public ActionResult<Transfert> Post([FromBody] Transfert transfert)
        {
            // Vérification du Montant
            if (transfert != null && transfert.IdClient != null)
            {
               

                transfert.Montant = _frais.CalculerFrais(transfert.Montant, transfert.Frais, transfert.Notified);
                var user = _user.GetUserById(transfert.IdClient);

                if (transfert != null && transfert.TypeTransfert.Equals("En espèce") && user.Role.Equals("AGENT"))
                {
                    transfert.IdClient = transfert.Idagent;
                    transfert.PlafondMaximal = 80000;
                }

                // Vérification si le montant du transfert dépasse le plafond annuel
                bool depasseAnnuel = _transfert.DepasseMontantAnnuel(transfert.IdClient, transfert.DataeTransfert, transfert.Montant, transfert.PlafondAnnuel);

                // Ajoutez la condition pour vérifier si le transfert dépasse le montant annuel autorisé
                // condition annuel pour client seulemet
                if (depasseAnnuel &&  user.Role.Equals("CLIENT"))
                {
                    return BadRequest(new { error = "Le transfert ne peut pas être effectué : le montant annuel autorisé serait dépassé." });
                }
                else if (transfert.Montant > transfert.PlafondMaximal)
                {
                    return BadRequest(new { error = $"Le transfert ne peut pas être effectué : le montant du transfert > plafond maximal du transfert {transfert.PlafondMaximal}" });
                }
                else if (transfert.Montant > user.Montant)
                {
                    return BadRequest(new { error = "Le transfert ne peut pas être effectué : le montant du transfert > solde de compte de paiement du client." });
                }

                // Soustraction du montant du transfert du solde du compte utilisateur
                user.Montant -= transfert.Montant;

                // Mise à jour de l'utilisateur dans la base de données
                _user.EditUser(user);
                _transfert.AddTransfert(transfert);

                return Ok(new { message = "Le transfert a été effectué avec succès." });
            }

            return BadRequest(new { error = "Les informations de transfert sont invalides." });
        }



    }
}
