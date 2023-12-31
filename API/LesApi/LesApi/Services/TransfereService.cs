using LesApi.Models;
using MongoDB.Driver;

namespace LesApi.Services
{
    public class TransfereService : ITransfert
    {
        private readonly IBeneficiaire _IBeneficiaire;
        private readonly IMongoCollection<Transfert> _transfert;
        private readonly IUser _user;
        public TransfereService(ITransfertDatabaseSettings settings, IMongoClient mongoClient, IUser user)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _user = user;
            _transfert = database.GetCollection<Transfert>(settings.TransfertCollectionName);
       
        }
        public Transfert AddTransfert(Transfert transfert)
        {
            _transfert.InsertOne(transfert);
            return transfert;
        }
        // verifier si le montant de tansfert  depasse le plafond annuel ;
        public bool DepasseMontantAnnuel(string idClient, DateTime dateActuelle, double montantTransfert, double PlafondAnnuel)
        {
            if (idClient != null)
            {
                // Obtenez la date du premier transfert pour le client
                DateTime datePremierTransfert = _user.GetDatePremierTransfert(idClient);

                // Vérifiez si l'utilisateur est trouvé
                var user = _user.GetUserById(idClient);

                if (user != null)
                {
                    // Vérifiez si la date actuelle est après un an à partir de la date du premier transfert
                    if (dateActuelle > datePremierTransfert.AddYears(1))
                    {
                        // Si nous sommes après un an, réinitialisez le montant annuel de transfert
                        user.MontantTransfertAnnuel = montantTransfert;
                        // Mettez à jour la date du premier transfert
                        user.DatePremierTransfert = dateActuelle;
                        return false;
                    }
                    else
                    {
                        // Si nous sommes toujours dans la même année, vérifiez le montant annuel par rapport au plafond
                        if (user.MontantTransfertAnnuel + montantTransfert > PlafondAnnuel)
                        {
                            // Le transfert ne peut pas être effectué car le plafond annuel serait dépassé
                            return true;
                        }
                        else
                        {
                            // Mettez à jour le montant annuel de transfert
                            user.MontantTransfertAnnuel += montantTransfert;
                            return false;
                        }
                    }
                }
                else
                {
                    
                    return false;
                }
            }
            else
            {
              
                return false;
            }
        }

    }
}
