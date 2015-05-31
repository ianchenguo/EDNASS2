using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ENETCare.Business;

namespace ENETCare.Presentation.MVC.Models
{
    public class AuditViewModels
    {
        [Required]
        public string Barcode { get; set; }

        public int MedicationTypeId { get; set; }

        public string MedicationTypeName { get; set; }

        public List<MedicationPackage> LostPackages { get; set; }

    }
}