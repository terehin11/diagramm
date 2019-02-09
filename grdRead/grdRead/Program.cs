 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

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



        public static Dictionary<Polarisation, DirectionPattern> Read(string fileaname, uint satelliteId, uint antennaId)
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
////////////////////////////////////////////////////////////////////////////////////////////////////
            DirectionPattern lhpDP = new DirectionPattern();
            List<Beam> lhpBeams = new List<Beam>();
            DirectionPattern rhpDP = new DirectionPattern();
            List<Beam> rhpBeams = new List<Beam>();

            Beam lhpBeam = new Beam();
            Beam rhpBeam = new Beam();

            for (int i =0; i < beemNumber;i++)
            {
                double xs, ys, xe, ye;
                lineBuf = readFile.ReadLine();
                parse(lineBuf);
                xs = Double.Parse(words[0]);
                ys = Double.Parse(words[1]);
                xe = Double.Parse(words[2]);
                ye = Double.Parse(words[3]);

                lineBuf = readFile.ReadLine();
                uint nx, ny, kLimit;
                parse(lineBuf);
                nx = UInt32.Parse(words[0]);
                ny = UInt32.Parse(words[1]);
                kLimit = UInt32.Parse(words[2]);

                double dx = (xe - xs) / (nx - 1);
                double dy = (ye - ys) / (ny - 1);


                Direct mainDirect = new Direct(dx * center.Keys.ElementAtOrDefault(i),  dx*center.Values.ElementAtOrDefault(i) ,AngleDim.DEG);
                Direct leftBottom = new Direct(xs, ys, AngleDim.DEG);
                Direct rightTop = new Direct(xe, ye, AngleDim.DEG);

                
                rhpBeam.setCenterAxis(ref mainDirect);
                rhpBeam.setElPointNumber(ny);
                rhpBeam.setAzPointNumber(nx);
                rhpBeam.setLeftBottom(ref leftBottom);
                rhpBeam.setRightTop(ref rightTop);

               
                lhpBeam.setCenterAxis(ref mainDirect);
                lhpBeam.setElPointNumber(ny);
                lhpBeam.setAzPointNumber(nx);
                lhpBeam.setLeftBottom(ref leftBottom);
                lhpBeam.setRightTop(ref rightTop);

                var rhpSamples = rhpBeam.getSamples();
                var lhpSamples = lhpBeam.getSamples();

                for(var y = 0; y < ny;y++)
                {
                    for(var x = 0; x < nx;x++)
                    {

                        double rhpRe, rhpIm, lhpRe, lhpIm;
                        lineBuf = readFile.ReadLine();
                        parse(lineBuf);
                        rhpRe = Double.Parse(words[0]);
                        rhpIm = Double.Parse(words[1]);
                        lhpRe = Double.Parse(words[2]);
                        lhpIm = Double.Parse(words[3]);
                        Complex rhpBuf = new Complex(rhpRe, rhpIm);
                        Complex lhpBuf = new Complex(lhpRe, lhpIm);
                        rhpSamples.Add(rhpBuf);
                        lhpSamples.Add(lhpBuf);
                    }
                    rhpBeams.Add(rhpBeam);
                    lhpBeams.Add(lhpBeam);
                }
                
            }
            file.Close();

            
            lhpDP.setBeams(lhpBeams);
            lhpDP.setFrequency(frequency);
            lhpDP.setSatelliteId(satelliteId);
            lhpDP.setAntennaId(antennaId);
            lhpDP.setPolarisation(Polarisation.LHP);

            rhpDP.setBeams(rhpBeams);
            rhpDP.setFrequency(frequency);
            rhpDP.setSatelliteId(satelliteId);
            rhpDP.setAntennaId(antennaId); 
            rhpDP.setPolarisation(Polarisation.RHP);

            

            Dictionary<Polarisation, DirectionPattern> res = new Dictionary<Polarisation, DirectionPattern>();
            res[Polarisation.LHP] = lhpDP;
            res[Polarisation.RHP] = rhpDP;

            Console.Write("frequency: " + frequency + "\n");
            Console.Write("coordinatesType: " +  coordinatesType + "\n");
            Console.Write("beemNumber: " + beemNumber + "\n");
            Console.Write("iComp: " + iComp + "\n");
            Console.Write("nComp: " +  nComp + "\n");
            Console.Write("gridType: " +gridType + "\n");
            Console.Write("center: " + center.Keys.ElementAtOrDefault(0) + " " + center.Values.ElementAtOrDefault(0) + "\n");
            

            return res;
        }

        

        static void Main(string[] args)
        {
            string filename = @"C:\repos\grdRead\beam1.grd";
            Read(filename, 0, 0);
           
        }
    }
}
