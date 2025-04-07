using UnityEngine;

namespace Grids
{
    public class GridTransition : MonoBehaviour
    {
        public GameGrid upperGrid { get; private set; }
        public  GameGrid lowerGrid { get; private set; }
        private Vector3 lowerGridOffset = new Vector3(0, 40 , 0);

        [SerializeField]
        public GameObject GameGridPrefab;
        [SerializeField]
        public int InitialVoidNum = 5;

        private int minRange;
        private int maxRange;

        private void Start()
        {
            GameObject upperGo = Instantiate(GameGridPrefab, transform);
            upperGrid = upperGo.GetComponent<GameGrid>();
            upperGrid.InitialVoidNum = 5;
            upperGrid.gameObject.SetActive(true);
            GameObject lowerGo = Instantiate(GameGridPrefab, transform);
            lowerGo.transform.position = upperGrid.transform.position - lowerGridOffset;
            lowerGrid = lowerGo.GetComponent<GameGrid>();
            upperGrid.InitialVoidNum = 5;
            PrepareNextGrid(lowerGrid);
            lowerGrid.gameObject.SetActive(true);

            minRange = (int)-upperGrid.gridSize.x;
            maxRange = (int)upperGrid.gridSize.y;
        }

        private void PrepareNextGrid(GameGrid nextGrid)
        {
            for (int i = 0; i <= upperGrid.VoidCells.Count / 4; i++)
            {
                Cell newCell = lowerGrid.CreateCell(GetRandomCellPos(), Status.Void);
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
            GameObject lowerGo = Instantiate(GameGridPrefab);
            lowerGo.transform.position = upperGrid.transform.position - lowerGridOffset;
            lowerGrid = lowerGo.GetComponent<GameGrid>();
        }

        private Vector2 GetRandomCellPos()
        {
            return new Vector2(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange));
        }
    }
}

