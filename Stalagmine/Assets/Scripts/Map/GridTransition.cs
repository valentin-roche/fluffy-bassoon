using UnityEngine;

namespace Grids
{
    public class GridTransition : MonoBehaviour
    {
        public GameGrid upperGrid { get; private set; }
        public  GameGrid lowerGrid { get; private set; }
        private Vector3 lowerGridOffset = new Vector3(0, 40 , 0);
        [SerializeField]
        public GameGrid GameGridPrefab;
        [SerializeField]
        public int InitialVoidNum = 5;

        private void Start()
        {
            upperGrid = Instantiate<GameGrid>(GameGridPrefab, GameGridPrefab.transform.position, Quaternion.identity, this.transform);
            upperGrid.InitialVoidNum = 5;
            upperGrid.gameObject.SetActive(true);
            lowerGrid = Instantiate(GameGridPrefab, upperGrid.transform.position - lowerGridOffset, Quaternion.identity, this.transform);
            upperGrid.InitialVoidNum = 5;
            PrepareNextGrid(lowerGrid);
            lowerGrid.gameObject.SetActive(true);
        }

        private void PrepareNextGrid(GameGrid nextGrid)
        {
            for (int i = 0; i <= upperGrid.VoidCells.Count / 4; i++)
            {
                Cell newCell = new Cell(GetRandomCellPos(), Status.Void);
                nextGrid.VoidCells.Add(newCell);
            }
            // nextGrid.ActualizeGrid();
        }

        private void PushLowerGrid()
        {
            
            foreach (var usedCell in upperGrid.UsedCells)
            {
                lowerGrid.SetContentAt(usedCell.Position, usedCell.Content);
            }
            upperGrid = lowerGrid;
            lowerGrid = Instantiate(GameGridPrefab, upperGrid.transform.position - lowerGridOffset, Quaternion.identity);
        }

        private Vector2 GetRandomCellPos()
        {
            int minRange = (int)-GameGridPrefab.gridSize.x;
            int maxRange = (int)GameGridPrefab.gridSize.y;
            return new Vector2(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange));
        }
    }
}

