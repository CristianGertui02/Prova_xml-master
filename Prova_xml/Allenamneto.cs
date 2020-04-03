using System;
using System.Collections.Generic;
using System.Text;

namespace Prova_xml
{
    public class Allenamneto
    {
        public string Tipo { get; set; }
        public double Durata { get; set; }
        public int Calorie { get; set; }


        public override string ToString()
        {
            return $"-Allenamento: {Tipo}, durata(min): {Durata}, calorie bruciate(cal): {Calorie}";
        }

    }
}
