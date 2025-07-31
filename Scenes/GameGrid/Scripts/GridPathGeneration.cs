using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GridPathGeneration
{

        List<List<Tile>> Grid;
        int GridRows
        {
            get
            {
              return Grid[0].Count;
            }
        }
        int GridCols
        {
            get
            {
            
                return Grid.Count;
            }
        }

        public GridPathGeneration(List<List<Tile>> grid)
        {
            Grid = grid;
        }

        public List<Tile> FindPath(Tile start, Tile end)
        {   GD.PrintErr("Grid: R("+GridRows+") C("+GridCols+")");
            GD.PrintErr("Start:"+start.GridPosition);
            GD.PrintErr("End:"+start.GridPosition);
          

            List<Tile> Path = new List<Tile>();
            List<Tile> OpenList = new List<Tile>();
            HashSet<Tile> ClosedList = new HashSet<Tile>();
            List<Tile> adjacencies;
            Tile current = start;
            var gScore = new Dictionary<Vector2, double> { [start.GridPosition] = 0 };
            var hScore = new Dictionary<Vector2, double> { [start.GridPosition] =start.Position.DistanceTo(end.Position)};
            
            var parentMap = new Dictionary<Vector2, Tile>();

        // add start node to Open List
            OpenList.Add(start);

        while (OpenList.Count > 0)
        {
            current = OpenList.OrderBy(node => gScore[node.GridPosition] + hScore[node.GridPosition]).First();
            ClosedList.Add(current);
            adjacencies = GetAdjacentNodes(current);
             if (current.GridPosition == end.GridPosition)
            { GD.PrintErr("YIPPIE!");
             return ReconstructPath(parentMap, current);
            }
            OpenList.Remove(current);
            ClosedList.Add(current);


            foreach (Tile t in adjacencies)
            {
                double tentativeGScore = gScore[current.GridPosition] + current.Position.DistanceTo(t.Position);


                if (!gScore.ContainsKey(t.GridPosition) || tentativeGScore < gScore[t.GridPosition])
                {
                    // Update gScore and hScore
                    gScore[t.GridPosition] = tentativeGScore;
                    hScore[t.GridPosition] = t.Position.DistanceTo(end.Position);

                    // Set the current node as the parent of the neighbor
                    parentMap[t.GridPosition] = current;
                    if (!OpenList.Contains(t))
                    {
                        OpenList.Add(t);
                    }
                }

            }
             
        }
            GD.PrintErr("DOH!");
            return null;
   
        }
        
    List<Tile> ReconstructPath(Dictionary<Vector2, Tile> parentMap, Tile current)
    {
        var path = new List<Tile> { current };
        
        while (parentMap.ContainsKey(current.GridPosition))
        {
            current = parentMap[current.GridPosition];
            path.Add(current);
        }
        
        path.Reverse();
        return path;
    }


		
    private List<Tile> GetAdjacentNodes(Tile n)
    {
        List<Tile> temp = new List<Tile>();

        int row = (int)n.GridPosition.Y;
        int col = (int)n.GridPosition.X;
        //west
        if (row + 1 < GridRows)
        {
           GD.PrintErr("--:Col("+Grid[0].Count+")Row X("+Grid.Count()+")");
           GD.PrintErr("--:Col("+col+")Row X("+(row+1)+")");

            try
            {
                var This = Grid[col][row + 1];
                temp.Add(This);

            }
            catch
            {

            }

            
        }
        //east
        if (row - 1 >= 0)
        {
            temp.Add(Grid[col][row-1]);
        }
        //south
        /*    if(col - 1 >= 0)
           {
               GD.PrintErr(col-1);
               temp.Add(Grid[col - 1][row]);
           } */
        //north
        if (col + 1 < GridCols)
        {
            temp.Add(Grid[col+1][row]);
        }

        return temp;
    }
    }













