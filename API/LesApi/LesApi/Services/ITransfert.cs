using LesApi.Models;

namespace LesApi.Services
{
    public interface ITransfert
    {
        Transfert AddTransfert(Transfert transfert);
        public bool DepasseMontantAnnuel(string idClient, DateTime dateActuelle, double montantTransfert, double PlafondAnnuel);
    }
}
