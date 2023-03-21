using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using tes;


namespace BFS
{ 
    public class BFSMazeSolver
    {
        private int[,] maze; // a 2D array representing the maze
        private bool[,] visited; // a 2D array keeping track of visited cells
        private int numRows; // number of rows in the maze
        private int numCols; // number of columns in the maze
        private List<Tuple<int, int>> goalStates; // list of goal states in the maze   
        private int numGoalsVisited; // number of goal states visited in current path
        private int pathCount;

        public BFSMazeSolver(int[,] maze, List<Tuple<int, int>> goalStates)
        {
            this.maze = maze;
            this.numRows = maze.GetLength(0);
            this.numCols = maze.GetLength(1);
            this.visited = new bool[numRows, numCols];
            this.goalStates = goalStates;
            this.numGoalsVisited = 0;
            this.pathCount = 0;
        }

        public void Solve(Tuple<int, int> startNode, ref string path, ref string pathPlan, ref List<Tuple<int, int>> simpulHidup, bool tsp)
        {
            int startRow = simpulHidup[0].Item1;
            int startCol = simpulHidup[0].Item2;
            if (pathPlan != ""){
                path += pathPlan[0];
                pathPlan = pathPlan.Remove(0,1);
            }
            simpulHidup.RemoveAt(0);

            // if (!(tsp)){
            if (pathCount == 1)
            {
                return;
            }
            // }

            // Check if current cell is a goal state
            if (IsGoalState(startRow, startCol))
            {
                // Increment number of goals visited and add path to current goal state to paths list
                numGoalsVisited++;
            }

            // Mark current cell as visited
            visited[startRow, startCol] = true;

            // Check if all goal states have been visited
            if (numGoalsVisited == goalStates.Count)
            {
                Console.WriteLine("Path that visited all goal states: " + path);
                if (tsp) {
                    this.goalStates.Clear();
                    goalStates.Add(startNode);
                    numGoalsVisited = 0;

                    // Mengubah semua node menjadi belum dikunjungi
                    for (int i = 0; i < numRows; i++)
                    {
                        for (int j = 0; j < numCols; j++)
                        {
                            visited[i,j] = false;
                        }
                    }

                    pathCount = 0;
                    pathPlan = "";
                    simpulHidup.Clear();
                    simpulHidup.Add(new Tuple<int, int>(startRow,startCol));
                    Solve(startNode, ref path, ref pathPlan, ref simpulHidup, false);

                } else {
                    pathCount += 1;
                    return;
                }
            }

            // Check all possible moves from current cell
            if (CanMove(startRow, startCol - 1)) // move left
            {
                // Console.WriteLine("L");
                pathPlan += "L";
                simpulHidup.Add(new Tuple<int, int>(startRow, startCol-1));
            }
            if (CanMove(startRow, startCol + 1)) // move right
            {
                // Console.WriteLine("R");
                pathPlan += "R";
                simpulHidup.Add(new Tuple<int, int>(startRow, startCol+1));
            }
            if (CanMove(startRow - 1, startCol)) // move up
            {
                // Console.WriteLine("U");
                pathPlan += "U";
                simpulHidup.Add(new Tuple<int, int>(startRow-1, startCol));
            }
            if (CanMove(startRow + 1, startCol)) // move down
            {
                // Console.WriteLine("D");
                pathPlan += "D";
                simpulHidup.Add(new Tuple<int, int>(startRow+1, startCol));
            }

            Solve(startNode, ref path, ref pathPlan, ref simpulHidup, tsp);
            // BACKTRACKING

            // Mark current cell as unvisited 
            visited[startRow, startCol] = false;

            // Decrement number of goals visited (if necessary)
            if (IsGoalState(startRow, startCol))
            {
                numGoalsVisited--;
            }

        }

        private bool IsGoalState(int row, int col)
        {
            foreach (Tuple<int, int> goalState in goalStates)
            {
                if (row == goalState.Item1 && col == goalState.Item2)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CanMove(int row, int col)
        {
            // Check if cell is within maze boundaries
            if (row < 0 || row >= numRows || col < 0 || col >= numCols)
            {
                return false;
            }
            // Check if cell is not a wall and has not been visited
            // if (maze[row, col] == 1)
            if (maze[row, col] == 1 || visited[row, col])
            {
                return false;
            }
            return true;
        }
    }
}