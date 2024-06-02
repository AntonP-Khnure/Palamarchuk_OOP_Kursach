using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// Представляє хворобу.
    /// </summary>
    public class Disease
    {
        /// <summary>
        /// Назва хвороби.
        /// </summary>
        public string name;

        /// <summary>
        /// Показник наявності ризику захворювання.
        /// </summary>
        public bool risk;

        /// <summary>
        /// Показник наявності захворювання.
        /// </summary>
        public bool ill;

        /// <summary>
        /// Показник наявності рецесивної хвороби.
        /// </summary>
        public bool recessive;

        /// <summary>
        /// Показник наявності ризику рецесивної хвороби.
        /// </summary>
        public bool recessive_risk;

        /// <summary>
        /// Обчислює хворобу на основі хвороби матері, хвороби батька та статі.
        /// </summary>
        /// <param name="mot_dis">Хвороба матері.</param>
        /// <param name="fat_dis">Хвороба батька.</param>
        /// <param name="sex">Стать особи.</param>
        /// <returns>Хвороба, якщо вона присутня або може бути успадкована, в іншому випадку - null.</returns>
        /// <remarks>
        /// X - здорова хромосома,\ - уражена хромосома
        /// X\ - рецесивна жінка, 
        /// \\ - хвора жінка, 
        /// XX - здорова жінка, 
        /// \Y - хворий чоловік, 
        /// XY - здоровий чоловік, 
        /// 1. XX + \Y -> здоровий хлопчик || рецесивна дівчинка 
        /// 2. X\ + XY -> можливо хворий хлопчик || можливо рецесивна дівчинка 
        /// 3. \\ + XY -> хворий хлопчик || рецесивна дівчинка 
        /// 4. \\ + \Y -> хворий хлопчик || хвора дівчинка 
        /// 5. X\ + \Y -> ожливо хворий хлопчик || можливо хвора, рецесивна дівчинка 
        /// 
        /// </remarks>
        public static Disease? Calculate(Disease mot_dis, Disease fat_dis, Sex sex)
        {
            if (mot_dis == null || fat_dis == null)
            {
                if (mot_dis == null && fat_dis != null && fat_dis.ill)
                {
                    if (sex == Sex.M)
                    {
                        return null;
                    }
                    else
                    {
                        return new Disease() { name = fat_dis.name, recessive = true };
                    }
                }
                else if (mot_dis != null && mot_dis.recessive && fat_dis == null)
                {
                    if (sex == Sex.M)
                    {
                        return new Disease() { name = mot_dis.name, risk = true };
                    }
                    else
                    {
                        return new Disease() { name = mot_dis.name, recessive_risk = true };
                    }
                }
                else if (mot_dis != null && mot_dis.ill && fat_dis == null)
                {
                    if (sex == Sex.M)
                    {
                        return new Disease() { name = mot_dis.name, ill = true };
                    }
                    else
                    {
                        return new Disease() { name = mot_dis.name, recessive = true };
                    }
                }
            }


            else if (mot_dis.ill && fat_dis.ill)
            {
                return new Disease() { name = fat_dis.name, ill = true };
            }
            else if (mot_dis.recessive && fat_dis.ill)
            {
                if (sex == Sex.M)
                {
                    return new Disease() { name = fat_dis.name, risk = true };
                }
                else
                {
                    return new Disease() { name = fat_dis.name, risk = true, recessive = true };
                }
            }
            return null;
        }
    }

}
