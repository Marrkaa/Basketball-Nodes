using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace U1_2._Krepšinis_L4
{
    /// <summary>
    /// Container class
    /// </summary>
    internal sealed class PlayersContainer : IEnumerable
    {
        private Node start;     // Array start
        private Node end;       // Array end
        private Node intr;      // Interface reference

        /// <summary>
        /// Constructor without parameters
        /// </summary>
        public PlayersContainer()
        {
            this.start = null;
            this.end = null;
            this.intr = null;
        }

        /// <summary>
        /// Reference is assigned as list start
        /// </summary>
        public void Start()
        {
            intr = start;
        }

        /// <summary>
        /// Reference is assigned with the next list element
        /// </summary>
        public void Next()
        {
            intr = intr.Next;
        }

        /// <summary>
        /// Checks if reference exists
        /// </summary>
        /// <returns>true or false</returns>
        public bool Exists()
        {
            return intr != null;
        }

        /// <summary>
        /// IEnumerator method
        /// </summary>
        /// <returns>enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            for (Node intr = start; intr != null; intr = intr.Next)
            {
                yield return intr.Data;
            }
        }

        /// <summary>
        /// Creates a new list element and adds it to the end of the list
        /// </summary>
        /// <param name="player">new element value</param>
        public void AddForwards(Player player)
        {
            var intr = new Node(player, null);

            if (start != null)
            {
                end.Next = intr;
                end = intr;
            }
            else
            {
                start = intr;
                end = intr;
            }
        }

        /// <summary>
        /// Creates a new list element and adds it to the front of the list
        /// </summary>
        /// <param name="player">new element value</param>
        public void AddBackwards(Player player)
        {
            //var intr = new Node(player, null);
            //intr.Next = start;
            //start = intr;
            start = new Node(player, start);
        }

        /// <summary>
        /// Returns the reference value
        /// </summary>
        /// <returns>element value</returns>
        public Player GetPlayer() { return intr.Data; }

        /// <summary>
        /// Bubble sort method
        /// </summary>
        public void BubbleSort()
        {
            if (start == null) { return; }
            bool change = true;
            while (change)
            {
                change = false;
                Node intr = start;

                while (intr.Next != null)
                {
                    if (intr.Data.CompareTo(intr.Next.Data) == 1)
                    {
                        Player temp = intr.Data;
                        intr.Data = intr.Next.Data;
                        intr.Next.Data = temp;
                        change = true;
                    }
                    intr = intr.Next;
                }
            }
        }

        /// <summary>
        /// A method for removing unneeded elements
        /// </summary>
        public void RemovePlayers(Player duom) 
        {
            Node delete = Place(duom);
            Node move = Before(delete);

            if (delete == start)
            {
                start = start.Next;
                delete = null;
            }
            else if(delete == end)        
            {
                move.Next = null;
                delete = null;
            }
            else
            {
                move.Next = delete.Next;
                delete = null;
            }
        }

        /// <summary>
        /// A method for finding the place which we need to remove
        /// </summary>
        /// <param name="duom">Player class object</param>
        /// <returns>place</returns>
        private Node Place(Player duom)
        {
            Node dd = start;
            if (dd != null) 
            {
                for (dd = start; dd != null; dd = dd.Next) 
                {
                    if (dd.Data == duom) 
                    {
                        return dd;
                    }
                }
            }
            return null;
        }
        
        /// <summary>
        /// A method for finding the place of an element which is
        /// behind the element that we need to remove
        /// </summary>
        /// <param name="duom">a node</param>
        /// <returns>place</returns>
        private Node Before(Node duom)
        {
            if (duom != start)
            {
                Node r;
                for (r = start; r.Next != duom; r = r.Next)
                    ;
                return r;
            }
            else
                return null;
        }
    }
}
