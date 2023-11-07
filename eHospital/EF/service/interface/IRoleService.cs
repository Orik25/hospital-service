using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.service.@interface
{
    internal interface IRoleService
    {
        Role GetPatientRole();
        Role GetDoctorRole();
        Role GetAdminRole();
    }
}
