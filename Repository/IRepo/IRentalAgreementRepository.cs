using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reolmarked.Model;

namespace Reolmarked.Repository.IRepo
{
    public interface IRentalAgreementRepository
    {
        void CreateRentalAgreement(RentalAgreement rental); // Til at oprette en ny lejeaftale
        void UpdateRentalAgreement(RentalAgreement rental); // Til at opdatere en eksisterende lejeaftale
        void DeleteRentalAgreement(int rentalAgreementId); // Til at slette en lejeaftale
        RentalAgreement GetRentalAgreementById(int rentalAgreementId); // Til at hente en lejeaftale baseret på dens ID
    }
}
