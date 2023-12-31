using LesApi.Models;
using MongoDB.Driver;

namespace LesApi.Services
{
    public class BeneficiaireService : IBeneficiaire
    {
        private IMongoCollection<Beneficiaire> _beneficiaire;

        public BeneficiaireService(ITransfertDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _beneficiaire = database.GetCollection<Beneficiaire>(settings.BeneficiaireCollectionName);
        }

        public Beneficiaire AddBeneficiaire(Beneficiaire beneficiaire)
        {
            _beneficiaire.InsertOne(beneficiaire);
            return beneficiaire;
        }

        public Beneficiaire GetBeneficiaireByGSM(string gsm)
        {
            return _beneficiaire.Find(Beneficiaire => Beneficiaire.NumeroGsm == gsm).FirstOrDefault();
        }

        public Beneficiaire GetBeneficiaireById(string IdBeneficiaire)
        {
            return _beneficiaire.Find(Beneficiaire => Beneficiaire.Id == IdBeneficiaire).FirstOrDefault();
        }
    }
}
