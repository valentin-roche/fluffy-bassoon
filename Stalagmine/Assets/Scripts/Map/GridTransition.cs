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
        [SerializeField]
        public int VoidPas = 2;
        [SerializeField]
        public int minNbOfCase = 2;
       
        private int minRange;
        private int maxRange;

        private int layerLevel = 1;

        private void Start()
        {
            GameObject upperGo = Instantiate(GameGridPrefab, transform);
            upperGrid = upperGo.GetComponent<GameGrid>();
            upperGrid.InitialVoidNum = 3;
            upperGrid.gameObject.SetActive(true);
            GameObject lowerGo = Instantiate(GameGridPrefab, transform);
            lowerGo.transform.position = upperGrid.transform.position - lowerGridOffset;
            lowerGrid = lowerGo.GetComponent<GameGrid>();
            lowerGrid.InitialVoidNum = 5;
            //PrepareNextGrid(lowerGrid);
            lowerGrid.gameObject.SetActive(true);

            EventDispatcher.Instance.LayerDestroyed(layerLevel);

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

        public void DestroyBoard()
        {
            Destroy(upperGrid.gameObject);
            

            upperGrid = lowerGrid;

            upperGrid.transform.position = new Vector3(upperGrid.transform.position.x, 0f, upperGrid.transform.position.z);

            GameObject lowerGo = Instantiate(GameGridPrefab, transform);
            lowerGo.transform.position = upperGrid.transform.position - lowerGridOffset;

            lowerGrid = lowerGo.GetComponent<GameGrid>();

            if (InitialVoidNum + VoidPas <= (lowerGrid.gridSize.x * lowerGrid.gridSize.y - 9 - minNbOfCase))
            {
                lowerGrid.InitialVoidNum = upperGrid.InitialVoidNum + VoidPas;
            }
            else
            {
                lowerGrid.InitialVoidNum = upperGrid.InitialVoidNum;
            }

            lowerGrid.gameObject.SetActive(true);

            layerLevel++;
            EventDispatcher.Instance.LayerDestroyed(layerLevel);
        }

        //tmp
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                DestroyBoard();
            }
        }
    }
}

