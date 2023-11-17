using EF.context;
using EF.service.impl;
using Org.BouncyCastle.Crypto.Macs;

namespace EF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NeondbContext())
            {

                UserServiceImpl appointmentService = new UserServiceImpl(context);
                appointmentService.ChangePasswordByEmail("Nigel_Rice57@gmail.com");
                
            }
        }
    }
}