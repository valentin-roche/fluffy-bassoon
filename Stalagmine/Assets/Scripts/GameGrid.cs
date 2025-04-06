using System.Collections.Generic;
using UnityEngine;

namespace Grids
{
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] 
        public int gridSize = 24;
        public List<Cell> VoidCells;
        public List<Cell> UsedCells;

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
            VoidCells = new List<Cell>();
            UsedCells = new List<Cell>();           

            ActualizeGrid(); 
        }

        public GameGrid(List<Cell> VoidCells, List<Cell> UsedCells)
        {
            this.VoidCells = VoidCells;
            this.UsedCells = UsedCells;
        }

        public void ActualizeGrid()
        {
            foreach(Cell voidCell in VoidCells)
            {
                foreach (Vector2 ouais in Directions) {
                    Cell neighb = getCellAt(voidCell.Position - ouais);
                    if (neighb.Status != Status.Void)
                    {
                        neighb.MakeVoid();
                        VoidCells.Insert(0, neighb);
                    }
                }
            }
        }

        public Cell getCellAt(Vector2 pos)
        {
            foreach(Cell voidCell in VoidCells)
            {
                if(voidCell.Position == pos)
                {
                    return voidCell; 
                }
            }
            foreach (Cell usedCell in UsedCells)
            {
                if (usedCell.Position == pos)
                {
                    return usedCell;
                }
            }
            return new Cell(pos); 
        }

        public void MakeVoidAt(Vector2 pos)
        {
            getCellAt(pos).MakeVoid();
        }

        public void SetContentAt(Vector2 pos, GameObject go)
        {
            getCellAt(pos).SetContent(go);
        }

        public List<Cell> getVoidCells()
        {
            return VoidCells;
        }
    }

}
    