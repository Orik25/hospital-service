using EF.context;
using EF.service.impl;

namespace EF
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NeondbContext())
            {

                AppointmentServiceImpl appointmentService = new AppointmentServiceImpl(context);
                //appointmentService.ArchiveById(58);
                Console.WriteLine(appointmentService.GetNumberOfAppointments());
                /*UserServiceImpl userServiceImpl = new UserServiceImpl(context);
                userServiceImpl.RegisterPatient(new DTO.User.UserDTO("firstname12", "firstname", "firstname", "firstname", "password"));*/
            }
        }
    }
}