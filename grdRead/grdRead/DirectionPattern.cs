using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grdRead
{
    struct IDirectionPattern
    {
       


    }




    class DirectionPattern
    {
        List<Beam> beams;
        /** ИД антенны. */
        public uint antennaID = 0;

        /** Частота для которой рассчитана ДН, ГГц. */
        public double frequency = 0.0;

        /** ИД КА для которого рассчитана ДН. */
        uint satelliteId = 0;

        /** Поляризация для которой рассчитана ДН. */
        Polarisation polarisation;
        public DirectionPattern()
        {
            
        }

        /**
        * Задать лучи ДН.
        * @param beams лучи ДН.
        */
        unsafe public void setBeams(List<Beam> beams)
            {
            this.beams = beams;
            }


        /**
        * Задать ИД антенны.
        * @param antennaId ИД антенны.
        */
        public void setAntennaId(uint antennaID)
        {
            this.antennaID = antennaID;
        }

        /**
        * Задать частоту для которой рассчитана ДН.
        * @param frequency частота для которой рассчитана ДН, ГГц.
        */
        public double setFrequency(double frequency)
        {
            return this.frequency = frequency;
        }

        /**
       * Задать поляризацию для которой рассчитана ДН.
       * @param polarisation поляризация для которой рассчитана ДН.
       */
        public void setPolarisation(Polarisation polarisation)
        {
            this.polarisation = polarisation;
        }

        /**
         * Задать ИД спутника на котором расположена антенна.
         * @param satelliteId ИД спутника.
         */
        public void setSatelliteId(uint satelliteId)
        {
            this.satelliteId = satelliteId;
        }
    }
}
