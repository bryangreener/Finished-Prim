using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimMazeGenerate
{
    class Program
    {
        // Lists used between multiple methods

        // At most 4 values adjacent to current visited cell
        static List<int> adjacent = new List<int>();
        // Self explanatory; will end with .Count of 1000
        static List<int> visited = new List<int>();
        // Cells that are adjacent to visited cells
        static List<int> frontier = new List<int>();
        // Stores weight values for 1000 indexes
        static List<int> weight = new List<int>();

        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;

            Random rnd = new Random();

            // Generate weight values for each index in weight array.
            // This is used to determine the location to move to later on.
            //
            // !! - CHANGE THE VALUE IN "i<####" TO ANY NUMBER WHERE # mod ( #/10) == 0
            // Runtime on i7 4790k, 32GB RAM, SSD:
            // 1,000     = 12ms
            // 100,000   = 65s
            // 1,000,000 = 131m

            for (int i = 0; i < 100000; i++)
            {
                weight.Add(rnd.Next(1, 10));
            }
            // Call method Prim()
            Prim();

            TimeSpan ts = DateTime.Now - dt;
            Console.WriteLine(ts.TotalMilliseconds.ToString());

            // Prevent console from closing automatically.
            Console.ReadLine();
        }
        /// <summary>
        /// This Method runs through a loop where each time through it will calculate new surrounding cells
        /// and add one visited cell.
        /// Two temp lists are created here and cleared at end of each loop.
        /// adjacent list also cleared at end of each step through.
        /// Each time through also calls the AdjacentTile() method.
        /// </summary>
        private static void Prim()
        {
            // First Position manual add
            // Adds value 0 (0,0 in grid) to visited
            visited.Add(0);
            // Check adjacent tiles to (0,0)
            AdjacentTile(0);
            // For adjacent tiles...
            for (int i = 3; i >= 0; i--)
            {
                if (adjacent.ElementAtOrDefault(i) != 0)
                {
                    // Add adjacent tiles to frontier
                    frontier.Add(adjacent[i]);
                }
            }
            // Clean up adjacent list to prevent overlapping
            adjacent.Clear();

            // While the frontier list isn't empty and the visited list hasn't maxed
            while (visited.Count != weight.Count && frontier.Count != 0)
            {
                List<int> tempWeights = new List<int>();
                List<int> weightLink = new List<int>();

                // Move to lowest weight value.
                for (int l = 0; l < frontier.Count; l++)
                {
                    // The following lines are used to form a link between the frontier and weight lists.
                    // This makes it so that I can take the min weight value and associate it with an index
                    // in the frontier list which is the same value as the position.

                    // Add weight value at current frontier position to tempweights.
                    tempWeights.Add(weight[frontier[l]]);
                    // Add frontier value to weightlink
                    weightLink.Add(frontier[l]);
                }
                // Lowest is the value, not index, of lowest value in tempWeights
                int lowest = tempWeights.Min();
                // Add the value of the index of the lowest value in tempWeights to Visited.
                // This is the same as saying to add the cell with the lowest value.
                visited.Add(weightLink[tempWeights.IndexOf(lowest)]);
                // Remove this same value from Frontier as it is a new visited location.
                frontier.Remove(weightLink[tempWeights.IndexOf(lowest)]);
                // Check for adjacent tiles around the newly visited location.
                AdjacentTile(visited[visited.Count - 1]);

                // Clear the temp lists to prevent overlap.
                tempWeights.Clear();
                weightLink.Clear();

                // Check surrounding 4 tiles to see if they are available to move to.
                for (int i = 3; i >= 0; i--)
                {
                    if (adjacent.ElementAtOrDefault(i) != 0)
                    {
                        // Add location of adjacent[i][ to frontier.
                        frontier.Add(adjacent[i]);
                    }
                }
                // Clear Adjacent list to prevent overlap.
                adjacent.Clear();
            }
        }
        /// <summary>
        /// This method takes in the currentPos int which is from 0-999.
        /// With this, it calculates adjacent cells, excluding previously visited cells.
        /// </summary>
        /// <param name="currentPos"></param>
        public static void AdjacentTile(int currentPos)
        {
            /* Basic setup is each of these checks if the desired location to move to is valid
            * if so, then it makes the move.
            * Cell is valid if it is within bounds and hadn't been visited and isn't already in frontier list.
            * The grid is laid out using a single line of values 0-999.
            * First row goes from 0-99, second from 100-199, and so on.
            * Therefore, if we have the selected cell 550, the adjacent cells are
            * 549 (West), 551 (East), 650 (North), and 450 (South).
            * So using the following IF statements we are simulating a 2d grid while keeping
            * functionality of the List<>s
            */

            //SOUTH
            if (currentPos >= (weight.Count / 10) &&
                !visited.Contains(currentPos - Convert.ToInt32(Math.Sqrt(weight.Count))) &&
                !frontier.Contains(currentPos - Convert.ToInt32(Math.Sqrt(weight.Count))))
            {
                adjacent.Add(currentPos - Convert.ToInt32(Math.Sqrt(weight.Count)));
            }
            //NORTH
            if (currentPos < (weight.Count - (weight.Count / 10)) &&
                !visited.Contains(currentPos + Convert.ToInt32(Math.Sqrt(weight.Count))) &&
                !frontier.Contains(currentPos + Convert.ToInt32(Math.Sqrt(weight.Count))))
            {
                adjacent.Add(currentPos + Convert.ToInt32(Math.Sqrt(weight.Count)));
            }
            //WEST
            if ((currentPos % (weight.Count / 10) != 0) &&
                !visited.Contains(currentPos - 1) && !frontier.Contains(currentPos - 1))
            {
                adjacent.Add(currentPos - 1);
            }
            //EAST
            if (currentPos % (weight.Count / 10) != (weight.Count / 10) - 1 &&
                !visited.Contains(currentPos + 1) &&
                !frontier.Contains(currentPos + 1))
            {
                adjacent.Add(currentPos + 1);
            }
        }
    }
}