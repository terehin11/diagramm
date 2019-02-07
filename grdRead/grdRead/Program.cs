using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace grdRead
{
    class Program
    {
       public Dictionary<Polarisation, DirectionPattern> read(ref string fileaname, uint satelliteId, uint antennaId)
        {
            //  StreamReader file = new StreamReader(fileaname);
            FileStream file = File.OpenRead(fileaname);
            /*придумать проверку(файл не открылся)*/



            // Буфер для хранения считанных из файла строк.
            StreamReader readFile = new StreamReader(file);
            // Первой строкой лежит версия ticra data.
            const string TICRA_VERSION_STR = "VERSION: TICRA-EM-FIELD-V0.1";
            readFile.ReadLine();
            if (readFile.ReadLine() != TICRA_VERSION_STR)  Console.WriteLine("Incorrect version of TICRA!");
            // Заголовок.
            readFile.ReadLine();
            // Заголовок частоты.
            readFile.ReadLine();
            // Заголовок источника поля.
            readFile.ReadLine();

            // Строка с частотой
            string line = readFile.ToString();
            string[] words = line.Split(' ');
            
            /*разобрать что тут происходит */
            double frequency;
            frequency = double.Parse(words[1]);
            //analogi4no
            readFile.ReadLine();

            // Строка с плюсами.
            const string PLUS_STR = "++++";
            readFile.ReadLine();
            if (readFile.ReadLine() != PLUS_STR) Console.WriteLine("Incorrect plus line!");

            // Строка с единицей (указание типа сетки - двухмерная, в ticra это 1).
            int coordinatesType = 0;
            const int SPHERICAL_GRID = 1;
            string lineWith1 = readFile.ToString();
            string[] words1 = lineWith1.Split(' ');
            coordinatesType = int.Parse(words1[0]);//затестить что будет происходить если много пробелов
            readFile.ReadLine();
            if (coordinatesType != SPHERICAL_GRID) Console.WriteLine("Incorrect grid type line!");





            Dictionary<Polarisation, DirectionPattern> res;
            return res;
        }

        

        static void Main(string[] args)
        {

        }
    }
}
