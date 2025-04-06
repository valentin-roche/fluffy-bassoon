using System.Collections.Generic;
using UnityEngine;

namespace Grids
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] 
        public int gridSize = 24;
        public List<Cell> voidCells;
        private List<Cell> usedCells;

        public static List<Vector2> Directions = new List<Vector2>
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(0, 1)
        };



        void Start()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            voidCells = new List<Cell>();
            usedCells = new List<Cell>();

            
            actualizeGrid(); 
        }

        public GameGrid(List<Cell> VoidCells, List<Cell> UsedCells)
        {
            voidCells = VoidCells;
            usedCells = UsedCells;
        }

        private void actualizeGrid()
        {
            foreach(Cell voidCell in voidCells)
            {
                foreach (Vector2 ouais in Directions) {
                    Cell neighb = getCellAt(voidCell.Position - ouais);
                    if (neighb.Status != Status.Void)
                    {
                        neighb.MakeVoid();
                        voidCells.Insert(0, neighb);
                    }
                }
            }
        }

    public Cell getCellAt(Vector2 pos)
        {
            foreach(Cell voidCell in voidCells)
            {
                if(voidCell.Position == pos)
                {
                    return voidCell; 
                }
            }
            foreach (Cell usedCell in usedCells)
            {
                if (usedCell.Position == pos)
                {
                    return usedCell;
                }
            }
            return new Cell(pos); 
        }
        public List<Cell> getVoidCells()
        {
            return voidCells;
        }
    }

}
    