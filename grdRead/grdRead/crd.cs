using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace grdRead
{

    /**
    * Размерность измерения углов.
    */
    enum AngleDim
    {
        RAD, //Радианы 
        DEG  //Градусы
    };

    /**
     * Допустимые поляризации волн для которых рассчитываются ДН.
     * Также используется для указания типа поляризации - кополярная или кроссполярная.
     */
    enum Polarisation
    {
        LHP,    // Круговая левая либо кополярная поляризация.
        RHP     // Круговая правая либо кроссполярная поляризация.
    };

    class Direct
    {
  
        public double az; //Азимутальный угол, рад
        public double el; //Угол места, рад

        /**
       * Конструктор с параметрами.
       * @param az азимутальный угол.
       * @param el угол места.
       */
        public Direct(double az, double el,  AngleDim dim)
        {
            setAz(az, dim);
            setEl(el, dim);
        }

        /**
      * Задать значение азимутального угла.
      * @param val задаваемое значение.
      * @param dim используемая размерность.
      */
        public void setAz(double val, AngleDim dim)
        {

            if (dim == grdRead.AngleDim.RAD) az = val;
            else az = Math.PI * val / 180;
        }
        /**
       * Получить значение угла места.
       * @param val задаваемое значение.
       * @param dim используемая размерность.
       */
        public void setEl(double val, AngleDim dim)
        {

            if (dim == grdRead.AngleDim.RAD) el = val;
            else el = Math.PI * val / 180;
        }
    }
}
