using EF.context;
using EF.DTO.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.service.impl
{
    public class AppointmentServiceImpl : IAppointmentService
    {
        private readonly NeondbContext context;
        private readonly UserServiceImpl userService;

        public AppointmentServiceImpl(NeondbContext context)
        {
            this.context = context;
            this.userService = new UserServiceImpl(context);
        }

        public Appointment FindById(long id)
        {
            Appointment appointment = context.Appointments.FirstOrDefault(appointment => appointment.AppointmentId == id);
            if (appointment == null)
            {
                throw new ApplicationException("Appointment with id: " + id + " does not exist!");
            }
            return appointment;
        }
        public void AddNew(AppointmentDTO appointment)
        {
            Appointment newAppointment = new Appointment();
            newAppointment.Status = "active";
            newAppointment.DoctorRefNavigation = userService.FindById(appointment.DoctorRef);
            newAppointment.PatientRefNavigation = userService.FindById(appointment.PatientRef);
            newAppointment.Message = appointment.Message;
            newAppointment.DateAndTime = appointment.DateAndTime;
            context.Appointments.Add(newAppointment);
            context.SaveChanges();
        }

        public void DeleteById(long id)
        {
            Appointment appointmentToDelete = FindById(id);
            context.Appointments.Remove(appointmentToDelete);
            context.SaveChanges();
        }
        public void ArchiveById(long id)
        {
            Appointment appointmentToArchive = FindById(id);
            appointmentToArchive.Status = "archive";
            context.SaveChanges();
        }

        public void Update(AppointmentDTO appointmentDTO)
        {
            Appointment appointment = FindById(appointmentDTO.AppointmentId);
            appointment.DoctorRefNavigation = userService.FindById(appointmentDTO.DoctorRef);
            appointment.PatientRefNavigation = userService.FindById(appointmentDTO.PatientRef);
            appointment.Message = appointmentDTO.Message;
            appointment.DateAndTime = appointmentDTO.DateAndTime;
            context.SaveChanges(true);
           
        }

        public List<Appointment> GetArchiveAppointmentsByUserId(long id)
        {
            return context.Appointments
                .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id) && appointment.Status == "archive")
                .ToList();
        }


        public List<Appointment> GetActiveAppointmentsByUserId(long id)
        {
            return context.Appointments
               .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id) && appointment.Status == "active")
               .ToList();
        }

        public List<Appointment> GetAppointmentsByUserId(long id)
        {
            return context.Appointments
               .Where(appointment => (appointment.DoctorRef == id || appointment.PatientRef == id))
               .ToList();
        }

        public long GetNumberOfAppointments()
        {
            return context.Appointments.Count();
        }
    }
}
