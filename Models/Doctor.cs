using System.Collections.Generic;

namespace VaccOps.Models
{
    public class Doctor
    {
        public Doctor(string name, int popularity)
        {
            this.Name = name;
            this.Popularity = popularity;
            this.Patients=new HashSet<Patient>();
        }

        public string Name { get; set; }
        public int Popularity { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}
