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
        static string[] words;
        static public string[] parse(string line)
        {
            line = line.Replace(".", ",");
            words = line.Split(' ');
            words = words.Where(n => !string.IsNullOrEmpty(n)).ToArray();


            return words;
        }


        public Dictionary<Polarisation, DirectionPattern> read(ref string fileaname, uint satelliteId, uint antennaId)
        {
            //  StreamReader file = new StreamReader(fileaname);
            FileStream file = File.OpenRead(fileaname);
            /*придумать проверку(файл не открылся)*/



            // Буфер для хранения считанных из файла строк.
            StreamReader readFile = new StreamReader(file);
            string lineBuf;
            // Первой строкой лежит версия ticra data.
            const string TICRA_VERSION_STR = "VERSION: TICRA-EM-FIELD-V0.1";
            lineBuf = readFile.ReadLine();
            if (lineBuf != TICRA_VERSION_STR) Console.WriteLine("Incorrect version of TICRA!");
            // Заголовок.
            lineBuf = readFile.ReadLine();
            // Заголовок частоты.
            lineBuf = readFile.ReadLine();
            // Заголовок источника поля.
            lineBuf = readFile.ReadLine();

            // Строка с частотой
            lineBuf = readFile.ReadLine();
            parse(lineBuf);

            /*разобрать что тут происходит */
            double frequency;
            frequency = Double.Parse(words[1]);
            //analogi4no
            lineBuf = readFile.ReadLine();

            // Строка с плюсами.
            const string PLUS_STR = "++++";

            if (lineBuf.Substring(0, PLUS_STR.Length) != PLUS_STR) Console.WriteLine("Incorrect plus line!");

            // Строка с единицей (указание типа сетки - двухмерная, в ticra это 1).
            int coordinatesType = 0;
            const int SPHERICAL_GRID = 1;
            lineBuf = readFile.ReadLine();
            parse(lineBuf);
            coordinatesType = Int32.Parse(words[0]);
            if (coordinatesType != SPHERICAL_GRID) Console.WriteLine("Incorrect grid type line!");

            lineBuf = readFile.ReadLine();
            // Контрольный параметр компонент поля. Значит в файле записаны:
            // 1 - угловые компоненты поля в сферической СК.
            // 2 - амплитуды правой и левой циркуляционных компонент.
            // 3 - линейные со и сх компоненты.
            // 4 - основная и побочная оси эллепса поляризации.
            // и т.д. см. документацию ticra.
            // нас интересует 2.
  
            int beemNumber, iComp, nComp, gridType;
            parse(lineBuf);
            beemNumber = Int32.Parse(words[0]);// Количество лепестков.
            iComp = Int32.Parse(words[1]);
            nComp = Int32.Parse(words[2]);     // Количество компонент поля.
            gridType = Int32.Parse(words[3]);  //  Тип сетки.

            lineBuf = readFile.ReadLine();
            parse(lineBuf);
            Dictionary<int, int> center = new Dictionary<int, int>();
            for (int i = 0; i < beemNumber; i++)
            {
                int ix, iy;
                ix = Int32.Parse(words[0]);
                iy = Int32.Parse(words[1]);
                center.Add(ix, iy);
            }

            DirectionPattern lhpDP;
            List<Beam> lhpBeams;
            DirectionPattern rhpDP;
            List<Beam> rhpBeams;




            Dictionary<Polarisation, DirectionPattern> res;
            return res;
        }

        

        static void Main(string[] args)
        {

        }
    }
}
