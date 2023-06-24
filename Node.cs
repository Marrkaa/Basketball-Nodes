using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace U1_2._Krepšinis_L4
{
    /// <summary>
    /// Node class
    /// </summary>
    internal sealed class Node
    {
        /// <summary>
        /// Player data
        /// </summary>
        public Player Data { get; set; }

        /// <summary>
        /// Next object data
        /// </summary>
        public Node Next { get; set; }

        /// <summary>
        /// Constructor without parameters
        /// </summary>
        public Node()
        {
        }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="data">Player class object</param>
        /// <param name="next">Node class object</param>
        public Node(Player data, Node next)
        {
            Data = data;
            Next = next;
        }
    }
}
