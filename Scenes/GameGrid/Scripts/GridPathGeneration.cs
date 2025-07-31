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
        {
          

            List<Tile> Path = new List<Tile>();
            PriorityQueue<Tile, float> OpenList = new PriorityQueue<Tile,float>();
            List<Tile> ClosedList = new List<Tile>();
            List<Tile> adjacencies;
            Tile current = start;
           
            // add start node to Open List
            OpenList.Enqueue(start, start.F);

            while(OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
            {
                current = OpenList.Dequeue();
                ClosedList.Add(current);
                adjacencies = GetAdjacentNodes(current);

                foreach(Tile t in adjacencies)
                {
                    if (!ClosedList.Contains(t) && t.Tile_Type==TileType.Empty)
                    {
                        bool isFound = false;
                        foreach (var oLNode in OpenList.UnorderedItems)
                        {
                            if (oLNode.Element == t)
                            {
                                isFound = true;
                            }
                        }
                        if (!isFound)
                        {
                            t.LastTile = current;
                            t.Cost = t.Weight + t.LastTile.Cost;
                            OpenList.Enqueue(t, t.F);
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.Exists(x => x.Position == end.Position))
            {
                return null;
            }

            // if all good, return path
            Tile temp = ClosedList[ClosedList.IndexOf(current)];
            if (temp == null) return null;
            do
            {
                Path.Add(temp);
                temp.LastTile.NextTile = temp;
                temp = temp.LastTile;
            } while (temp != start && temp != null) ;
            return Path;
        }
		
        private List<Tile> GetAdjacentNodes(Tile n)
        {
            List<Tile> temp = new List<Tile>();

            int row = (int)n.GridPosition.Y;
            int col = (int)n.GridPosition.X;
            //west
            if(row + 1 < GridRows)
            {
                temp.Add(Grid[col][row + 1]);
            }
            //east
            if (row - 1 >= 0)
            {
            temp.Add(Grid[col][row - 1]);
            }
            //south
            if(col - 1 >= 0)
            {
                temp.Add(Grid[col - 1][row]);
            }
            //north
            if(col + 1 < GridCols)
            {
                temp.Add(Grid[col + 1][row]);
            }

            return temp;
        }
    }













