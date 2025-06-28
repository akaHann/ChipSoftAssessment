using Chipsoft.Assignments.EPDConsole.Interfaces;
using Chipsoft.Assignments.EPDConsole.Models;
using Chipsoft.Assignments.EPDConsole.Repositories;
using Chipsoft.Assignments.EPDConsole.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Program
    {
        //Don't create EF migrations, use the reset db option
        //This deletes and recreates the db, this makes sure all tables exist
        private static IServiceProvider _serviceProvider;

        private static async Task AddPatient()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Patiënt Toevoegen ===");
                
                Console.Write("Voornaam: ");
                string? firstName = Console.ReadLine();
                
                Console.Write("Achternaam: ");
                string? lastName = Console.ReadLine();
                
                Console.Write("Geboortedatum (dd/mm/yyyy): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, 
                    System.Globalization.DateTimeStyles.None, out DateTime dateOfBirth))
                {
                    Console.WriteLine("Ongeldige datum format. Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }
                
                Console.Write("Telefoonnummer: ");
                string? phoneNumber = Console.ReadLine();
                
                Console.Write("Adres: ");
                string? address = Console.ReadLine();
                
                Console.Write("Postcode: ");
                string? postalCode = Console.ReadLine();
                
                Console.Write("Stad: ");
                string? city = Console.ReadLine();
                
                Console.Write("Verzekeringsnummer (optioneel): ");
                string? insuranceNumber = Console.ReadLine();

                Patient patient = new Patient
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    PhoneNumber = phoneNumber,
                    Address = address,
                    PostalCode = postalCode,
                    City = city,
                    InsuranceNumber = string.IsNullOrWhiteSpace(insuranceNumber) ? string.Empty : insuranceNumber
                };

                using IServiceScope scope = _serviceProvider.CreateScope();
                IPatientService patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                
                await patientService.AddPatientAsync(patient);
                Console.WriteLine("Patiënt succesvol toegevoegd!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }

        private static async Task ShowAppointment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Afspraken Inzien ===");
                Console.WriteLine("1 - Alle afspraken");
                Console.WriteLine("2 - Afspraken per patiënt");
                Console.WriteLine("3 - Afspraken per arts");
                Console.Write("Keuze: ");
                
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Ongeldige keuze.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                using IServiceScope scope = _serviceProvider.CreateScope();
                IAppointmentService appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                IPatientService patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                IPhysicianService physicianService = scope.ServiceProvider.GetRequiredService<IPhysicianService>();

                IEnumerable<Appointment> appointments = new List<Appointment>();

                switch (choice)
                {
                    case 1:
                        appointments = await appointmentService.GetAllAppointmentsAsync();
                        break;
                    case 2:
                        await ShowAppointmentsByPatientAsync(appointmentService, patientService);
                        return;
                    case 3:
                        await ShowAppointmentsByPhysicianAsync(appointmentService, physicianService);
                        return;
                    default:
                        Console.WriteLine("Ongeldige keuze.");
                        Console.WriteLine("Druk op een toets om door te gaan...");
                        Console.ReadKey();
                        
                        return;
                }

                DisplayAppointments(appointments);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }

        private static async Task AddAppointment()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Afspraak Toevoegen ===");
                
                using IServiceScope scope = _serviceProvider.CreateScope();
                IAppointmentService appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                IPatientService patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                IPhysicianService physicianService = scope.ServiceProvider.GetRequiredService<IPhysicianService>();
                
                // Show available patients
                IEnumerable<Patient> patients = await patientService.GetAllPatientsAsync();
                if (!patients.Any())
                {
                    Console.WriteLine("Geen patiënten beschikbaar. Voeg eerst een patiënt toe.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                Console.WriteLine("Beschikbare patiënten:");
                foreach (var patient in patients)
                {
                    Console.WriteLine($"{patient.Id}: {patient.FullName} - {patient.DateOfBirth:dd/MM/yyyy}");
                }
                
                Console.Write("Selecteer patiënt ID: ");
                if (!int.TryParse(Console.ReadLine(), out int patientId))
                {
                    Console.WriteLine("Ongeldig patiënt ID.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                // Show available physicians
                IEnumerable<Physician> physicians = await physicianService.GetAllPhysiciansAsync();
                if (!physicians.Any())
                {
                    Console.WriteLine("Geen artsen beschikbaar. Voeg eerst een arts toe.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                Console.WriteLine("\nBeschikbare artsen:");
                foreach (var physician in physicians)
                {
                    Console.WriteLine($"{physician.Id}: {physician.FullName}");
                }
                
                Console.Write("Selecteer arts ID: ");
                if (!int.TryParse(Console.ReadLine(), out int physicianId))
                {
                    Console.WriteLine("Ongeldig arts ID.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                Console.Write("Datum en tijd (dd/mm/yyyy HH:mm): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime appointmentDateTime))
                {
                    Console.WriteLine("Ongeldige datum/tijd format.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }
                
                Console.Write("Opmerkingen (optioneel): ");
                string? notes = Console.ReadLine();

                Appointment appointment = new Appointment
                {
                    PatientId = patientId,
                    PhysicianId = physicianId,
                    DateTime = appointmentDateTime,
                    Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
                };

                await appointmentService.AddAppointmentAsync(appointment);
                Console.WriteLine("Afspraak succesvol toegevoegd!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }

        private static async Task DeletePhysician()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Arts Verwijderen ===");
                
                using IServiceScope scope = _serviceProvider.CreateScope();
                IPhysicianService physicianService = scope.ServiceProvider.GetRequiredService<IPhysicianService>();
                
                IEnumerable<Physician> physicians = await physicianService.GetAllPhysiciansAsync();
                if (!physicians.Any())
                {
                    Console.WriteLine("Geen artsen gevonden.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                Console.WriteLine("Beschikbare artsen:");
                foreach (var physician in physicians)
                {
                    Console.WriteLine($"{physician.Id}: {physician.FullName}");
                }
                
                Console.Write("Selecteer arts ID om te verwijderen: ");
                if (int.TryParse(Console.ReadLine(), out int physicianId))
                {
                    await physicianService.DeletePhysicianAsync(physicianId);
                    Console.WriteLine("Arts succesvol verwijderd!");
                }
                else
                {
                    Console.WriteLine("Ongeldig ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }

        private static async Task AddPhysician()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Arts Toevoegen ===");
                
                Console.Write("Voornaam: ");
                string? firstName = Console.ReadLine();
                
                Console.Write("Achternaam: ");
                string? lastName = Console.ReadLine();
                
                Console.Write("Telefoonnummer: ");
                string? phoneNumber = Console.ReadLine();

                Physician physician = new Physician
                {
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber
                };

                using IServiceScope scope = _serviceProvider.CreateScope();
                IPhysicianService physicianService = scope.ServiceProvider.GetRequiredService<IPhysicianService>();
                
                await physicianService.AddPhysicianAsync(physician);
                Console.WriteLine("Arts succesvol toegevoegd!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }

        private static async Task DeletePatient()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("=== Patiënt Verwijderen ===");
                
                using IServiceScope scope = _serviceProvider.CreateScope();
                IPatientService patientService = scope.ServiceProvider.GetRequiredService<IPatientService>();
                
                IEnumerable<Patient> patients = await patientService.GetAllPatientsAsync();
                if (!patients.Any())
                {
                    Console.WriteLine("Geen patiënten gevonden.");
                    Console.WriteLine("Druk op een toets om door te gaan...");
                    Console.ReadKey();
                    
                    return;
                }

                Console.WriteLine("Beschikbare patiënten:");
                foreach (var patient in patients)
                {
                    Console.WriteLine($"{patient.Id}: {patient.FullName} - {patient.DateOfBirth:dd/MM/yyyy}");
                }
                
                Console.Write("Selecteer patiënt ID om te verwijderen: ");
                if (int.TryParse(Console.ReadLine(), out int patientId))
                {
                    await patientService.DeletePatientAsync(patientId);
                    Console.WriteLine("Patiënt succesvol verwijderd!");
                }
                else
                {
                    Console.WriteLine("Ongeldig ID.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout: {ex.Message}");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }
        
        // extra 
        
        private static async Task ShowAppointmentsByPatientAsync(IAppointmentService appointmentService, IPatientService patientService)
        {
            IEnumerable<Patient> patients = await patientService.GetAllPatientsAsync();
            if (!patients.Any())
            {
                Console.WriteLine("Geen patiënten gevonden.");
                
                return;
            }

            Console.WriteLine("Beschikbare patiënten:");
            foreach (var patient in patients)
            {
                Console.WriteLine($"{patient.Id}: {patient.FullName}");
            }
            
            Console.Write("Selecteer patiënt ID: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                IEnumerable<Appointment> appointments = await appointmentService.GetAppointmentsByPatientAsync(patientId);
                Patient? selectedPatient = await patientService.GetPatientByIdAsync(patientId);
                
                Console.WriteLine($"\nAfspraken voor {selectedPatient?.FullName}:");
                DisplayAppointments(appointments);
            }
            else
            {
                Console.WriteLine("Ongeldig patiënt ID.");
            }
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }
        
        private static async Task ShowAppointmentsByPhysicianAsync(IAppointmentService appointmentService, IPhysicianService physicianService)
        {
            IEnumerable<Physician> physicians = await physicianService.GetAllPhysiciansAsync();
            if (!physicians.Any())
            {
                Console.WriteLine("Geen artsen gevonden.");
                
                return;
            }

            Console.WriteLine("Beschikbare artsen:");
            foreach (var physician in physicians)
            {
                Console.WriteLine($"{physician.Id}: {physician.FullName} ");
            }
            
            Console.Write("Selecteer arts ID: ");
            if (int.TryParse(Console.ReadLine(), out int physicianId))
            {
                IEnumerable<Appointment> appointments = await appointmentService.GetAppointmentsByPhysicianAsync(physicianId);
                Physician? selectedPhysician = await physicianService.GetPhysicianByIdAsync(physicianId);
                
                Console.WriteLine($"\nAfspraken voor {selectedPhysician?.FullName}:");
                DisplayAppointments(appointments);
            }
            else
            {
                Console.WriteLine("Ongeldig arts ID.");
            }
            
            Console.WriteLine("Druk op een toets om door te gaan...");
            Console.ReadKey();
        }
        
        private static void DisplayAppointments(IEnumerable<Appointment> appointments)
        {
            if (!appointments.Any())
            {
                Console.WriteLine("Geen afspraken gevonden.");
                
                return;
            }

            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine($"{"Datum/Tijd",-20} {"Patiënt",-25} {"Arts",-25} {"Opmerkingen",-10}");
            Console.WriteLine(new string('=', 80));

            foreach (var appointment in appointments)
            {
                string notes = string.IsNullOrEmpty(appointment.Notes) ? "-" : (appointment.Notes.Length > 10 ? appointment.Notes.Substring(0, 10) + "..." : appointment.Notes);
                
                Console.WriteLine($"{appointment.DateTime:dd/MM/yyyy HH:mm} " +
                                  $"{appointment.Patient?.FullName ?? "Onbekend",-25} " +
                                  $"{appointment.Physician?.FullName ?? "Onbekend",-25} " +
                                  $"{notes,-10}");
            }
            Console.WriteLine(new string('=', 80));
        }


        #region FreeCodeForAssignment
        static void Main(string[] args)
        {
            // Setup DI container
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // DbContext
                    services.AddDbContext<EPDDbContext>();
                    
                    // Repositories
                    services.AddScoped<IPatientRepository, PatientRepository>();
                    services.AddScoped<IPhysicianRepository, PhysicianRepository>();
                    services.AddScoped<IAppointmentRepository, AppointmentRepository>();
                    
                    // Services
                    services.AddScoped<IPatientService, PatientService>();
                    services.AddScoped<IPhysicianService, PhysicianService>();
                    services.AddScoped<IAppointmentService, AppointmentService>();
                })
                .Build();

            _serviceProvider = host.Services;
            
            // Start application
            while (ShowMenu())
            {
                //Continue
            }
        }

        public static bool ShowMenu()
        {
            Console.Clear();
            foreach (var line in File.ReadAllLines("logo.txt"))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");
            Console.WriteLine("1 - Patient toevoegen");
            Console.WriteLine("2 - Patienten verwijderen");
            Console.WriteLine("3 - Arts toevoegen");
            Console.WriteLine("4 - Arts verwijderen");
            Console.WriteLine("5 - Afspraak toevoegen");
            Console.WriteLine("6 - Afspraken inzien");
            Console.WriteLine("7 - Sluiten");
            Console.WriteLine("8 - Reset db");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        AddPatient();
                        return true;
                    case 2:
                        DeletePatient();
                        return true;
                    case 3:
                        AddPhysician();
                        return true;
                    case 4:
                        DeletePhysician();
                        return true;
                    case 5:
                        AddAppointment();
                        return true;
                    case 6:
                        ShowAppointment();
                        return true;
                    case 7:
                        return false;
                    case 8:
                        EPDDbContext dbContext = new EPDDbContext();
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        return true;
                    default:
                        return true;
                }
            }
            return true;
        }

        #endregion
    }
}