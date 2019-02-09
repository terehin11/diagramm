using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace grdRead
{

    
    class Beam
    {
        /** Направление, относительно которого рассчитана ДН луча. */
        Direct centerAxis;
        /** Количество отсчетов по углу места. */
        ulong elPointNumber = 0;
        /** Количество отсчетов по азимутальному углу. */
        ulong azPointNumber = 0;
        /** Направление на левый нижний угол области определения ДН луча. */
        Direct leftBottom;
        /** Направление на правый верхний угол области определения ДН луча. */
        Direct rightTop;

        /** Отсчеты ДН - комплексные коэффициенты усиления. */
         List<Complex> samples = new List<Complex>();

        //Dictionary<double, double> samples = new Dictionary<double, double>();
        
      
        /**
      * Задать направление, относительно которого рассчитана ДН луча.
      * @param centerAxis направление, относительно которого рассчитана ДН луча.
      */
        public void setCenterAxis(ref Direct centerAxis)
        {
            this.centerAxis = centerAxis;
            
        }

        /**
    * Задать количество отсчетов в ДН луча углу места.
    * @param elPointNumber количество отсчетов в ДН луча углу места.
    */
        public void setElPointNumber(ulong elPointNumber)
        {
            this.elPointNumber = elPointNumber;
        }

        /**
       * Задать количество отсчетов в ДН луча по азимутальному углу.
       * @param azPointNumber количество отсчетов в ДН луча по азимутальному углу.
       */
        public void setAzPointNumber(ulong azPointNumber)
        {
            this.azPointNumber = azPointNumber;
        }

        /**
    * Задать направление на левый нижний угол области определения ДН.
    * @param leftTop направление на левый нижний угол области определения ДН.
    */
        unsafe public void setLeftBottom(ref Direct leftTop)
        {
            this.leftBottom = leftTop;
        }

        /**
         * Задать направление на верхний правый угол области определения ДН.
         * @param rightBottom направление на верхний правый угол области определения ДН.
         */
        public void setRightTop(ref Direct rightBottom)
        {
            this.rightTop = rightBottom;
        }


        /**
 * Получить отсчеты ДН - комплексные коэффициенты усиления (для изменения).
 * @return отсчеты ДН - комплексные коэффициенты усиления.
 */


        unsafe public List<Complex> getSamples()
        {
            return samples;
        }
    


        /**
        * Получить отсчет ДН - комплексный коэффициент усиления.
        * @param azIdx - индекс по азимутальному углу.
        * @param elIdx - индекс угла места.
        * @return отсчет ДН - комплексный коэффициент усиления.
        */
        public Complex getSample(ulong azIdx, ulong elIdx)
        {
            int el = Convert.ToInt32(elIdx);
            int azPN = Convert.ToInt32(azPointNumber);
            int az = Convert.ToInt32(azIdx);
            return samples[(el * azPN) + az];
        }
    }
}
