namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        private Dictionary<string, Doctor> doctors;
        // private Dictionary<string, List<Patient>> patients;

        public VaccDb()
        {
            this.doctors = new Dictionary<string, Doctor>();
            // this.patients = new Dictionary<string, List<Patient>>();
        }
        public void AddDoctor(Doctor doctor)
        {
            if (this.doctors.ContainsKey(doctor.Name))
            {
                throw new ArgumentException("We have doctor with this name");
            }

            this.doctors.Add(doctor.Name, doctor);

        }

        public void AddPatient(Doctor doctor, Patient patient)
        {

            if (!this.Exist(doctor))
            {
                throw new ArgumentException("We dont have doctor with this name");
            }

            if (this.doctors[doctor.Name].Patients.Contains(patient))
            {
                throw new ArgumentException($"D-r {doctor.Name} has patient with name {patient.Name}");
            }

            doctor.Patients.Add(patient);
            patient.Doctor = doctor;
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!this.Exist(oldDoctor) || !this.Exist(newDoctor) || !this.Exist(patient))
            {
                throw new ArgumentException();

            }
            oldDoctor.Patients.Remove(patient);
            newDoctor.Patients.Add(patient);
            patient.Doctor = newDoctor;

        }

        public bool Exist(Doctor doctor)
        {
            if (doctors.ContainsKey(doctor.Name))
            {
                return true;
            }

            return false;
        }

        public bool Exist(Patient patient)
        {
            if (this.doctors.Values.Any(x => x.Patients.Any(x => x.Name == patient.Name)))
            {
                return true;
            }

            return false;
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return this.doctors.Values.ToArray();
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
        {
            var doctorsByPopularity = new List<Doctor>();

            foreach (var doc in doctors.Values)
            {

                if (doc.Popularity == populariry)
                {

                    doctorsByPopularity.Add(doc);
                }
            }

            return doctorsByPopularity;
        }

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
        {                        

            return this.doctors.Values.OrderByDescending(x => x.Patients.Count)
                .ThenBy(x => x.Name).ToArray(); ;
        }

        public IEnumerable<Patient> GetPatients()
        {

            var resultPatients = new List<Patient>();

            foreach (var doc in doctors.Values)
            {

                resultPatients.AddRange(doc.Patients);

            }
            return resultPatients;
        }

        public IEnumerable<Patient> GetPatientsByTown(string town)
        {
            var patientsByTown = new List<Patient>();

            foreach (var doc in doctors.Values)
            {
                patientsByTown.AddRange(doc.Patients.Where(x => x.Town == town).ToArray());
            }

            return patientsByTown;
        }

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
        {
            var patientsByAge = new List<Patient>();

            foreach (var doc in doctors.Values)
            {

                patientsByAge.AddRange(doc.Patients.Where(x => x.Age >= lo && x.Age <= hi).ToArray());

            }

            return patientsByAge;
        }

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
        {
            var result = this.GetPatients()
                .OrderBy(x => x.Doctor.Popularity)
                .ThenByDescending(x => x.Height)
                .ThenBy(x => x.Age)
                .ToArray();

            return result;
        }

        public Doctor RemoveDoctor(string name)
        {
            if (!doctors.ContainsKey(name))
            {
                throw new ArgumentException();
            }
            var doctor = doctors[name];

            doctors.Remove(name);

            return doctor;
        }
    }
}
