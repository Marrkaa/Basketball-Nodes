using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace U1_2._Krepšinis_L4
{
    /// <summary>
    /// Class to describe the data of one player
    /// </summary>
    internal class Player : IComparable<Player>
    {
        /// <summary>
        /// Player name and surname
        /// </summary>
        public string NameSurname { get; set; }

        /// <summary>
        /// Player age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Player height
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="nameSur">player name</param>
        /// <param name="years">player age</param>
        /// <param name="size">player height</param>
        public Player(string nameSur, int years, double size)
        {
            NameSurname = nameSur;
            Age = years;
            Height = size;
        }

        /// <summary>
        /// Overriden Object class method
        /// </summary>
        /// <returns>concatenated string (all class properties)</returns>
        public override string ToString()
        {
            string line;
            line = string.Format("{0, -20}   {1, 2}    {2, 3:f2}", 
                NameSurname, Age, Height);
            return line;
        }

        /// <summary>
        /// Object class method for comparison by age and names
        /// </summary>
        /// <param name="diff">different class object</param>
        /// <returns>1 if true and -1 if false</returns>
        public int CompareTo(Player diff)
        {
            int comp = string.Compare(this.NameSurname, diff.NameSurname, 
                StringComparison.CurrentCulture);

            if ((this.Age > diff.Age || this.Age == diff.Age && comp > 0))
                return 1;
            else
                return -1;
        }
    }
}