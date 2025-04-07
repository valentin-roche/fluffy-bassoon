using UnityEngine;

namespace Grids
{
    public class GridTransition : MonoBehaviour
    {
        public GameGrid upperGrid { get; private set; }
        public  GameGrid lowerGrid { get; private set; }
        private Vector3 lowerGridOffset = new Vector3(0, 20 , 0);
        [SerializeField]
        public GameGrid GameGridPrefab;
        [SerializeField]
        public int InitialVoidNum = 5;

        private void Start()
        {
            /*GameObject upperGridParent = new();
            upperGridParent.transform.parent = transform;
            upperGridParent.transform.localPosition = Vector3.zero;*/

            upperGrid = Instantiate<GameGrid>(GameGridPrefab, transform.position, Quaternion.identity, this.transform);
            upperGrid.InitialVoidNum = 5;
            upperGrid.gameObject.SetActive(true);
            var nextLayer = upperGrid.transform.position;

            /*GameObject lowerGridParent = new();
            lowerGridParent.transform.parent = transform;
            lowerGridParent.transform.localPosition = Vector3.zero + new Vector3(0, -20, 0);*/

            lowerGrid = Instantiate<GameGrid>(GameGridPrefab, transform.position - new Vector3(0, 20, 0), Quaternion.identity, this.transform);
            lowerGrid.InitialVoidNum = 5;
            //PrepareNextGrid(lowerGrid);
            lowerGrid.gameObject.SetActive(true);
        }

        private void PrepareNextGrid(GameGrid nextGrid)
        {
            for (int i = 0; i <= upperGrid.VoidCells.Count / 4; i++)
            {
                Cell newCell = new Cell(GetRandomCellPos(), Status.Void);
                nextGrid.VoidCells.Add(newCell);
            }
            nextGrid.ActualizeGrid();
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

