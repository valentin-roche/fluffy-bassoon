using UnityEngine;

namespace Grids
{
    public class GridTransition : MonoBehaviour
    {
        private GameGrid upperGrid;
        private GameGrid lowerGrid;


        private void InitalizeLowerGrid()
        {
            foreach (Grids.Cell voidCellUpper in upperGrid.voidCells)
            {
                int minRange = (int)-lowerGrid.gridSize / 2;
                int maxRange = (int)lowerGrid.gridSize / 2;
                /*Vector2 newPos = new Vector2(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange));
                Grids.Cell newCell = new Grids.Cell(newPos, Grids.Status.void);
                lowerGrid.voidCells.Add(newCell); */
            }
        }
    }
}

