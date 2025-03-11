using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files_3___PuntenBestand
{
    public class PuntenAdmin
    {
        private float result = 0.0f;
        // ===  Auto-implemented properties  ===  (snippet: prop)
        public string Firstname { get; set; }
        public string Familyname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Score { get; set; }      // punten
        public int TotalScore { get; set; } // puntentotaal

        // === Methods ===
        public float Percent() // behaald percentage  -> vb. 75% = 0.75f
        {
            result = (float)Score / TotalScore;
            return result;
        }

        public string Grade() // beoordeling -> geslaagd / niet geslaagd
        {
            return (Percent() >= 0.8) ? "Geslaagd" : "Niet Geslaagd";
        }

        //  === Constructors  === (snippet: ctor)
        public PuntenAdmin() : this("", "", "", "onzijdig", 10, 20)
        {
        }

        public PuntenAdmin(string firstname, string familyname, string email,
            string gender, int score, int totalScore)
        {
            Firstname = firstname;
            Familyname = familyname;
            Email = email;
            Gender = gender;
            Score = score;
            TotalScore = totalScore;
        }

    }
}
