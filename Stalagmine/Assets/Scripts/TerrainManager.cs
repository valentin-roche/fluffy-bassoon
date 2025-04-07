using Grids;
using UnityEngine;

namespace GameState
{
    public class TerrainManager : MonoBehaviour
    {
        [SerializeField]
        public GameState GameState; 
        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        private GridTransition gridTransition;
        [SerializeField]
        private GameObject turretManager;

        private Vector3Int? selectedCellPos;
        [SerializeField]
        private GameObject cellIndicator;

        public bool OnClickLeftMouseToBuild()
        {
            if (gridTransition != null && gridTransition.upperGrid != null)
            {
                Vector3? mousePos = inputManager.GetSelectedMapPosition();
                if(mousePos != null)
                {
                    Vector3Int gridPos = gridTransition.upperGrid.gameObject.GetComponent<Grid>().WorldToCell(mousePos.Value);
                    Vector2 gridPos2d = new Vector2(gridPos.x, gridPos.z);
                    if (gridTransition.upperGrid.isVectorInGridGame(gridPos2d))
                    {
                        if (gridTransition.upperGrid.isCellEmpty(gridPos2d))
                        {
                            selectedCellPos = gridPos;
                            //HighlightSelectedCellFromUpperGrid(); 
                            return true;
                        }
                    }
                }
            }
            return false;  
        }

        /*public void HighlightSelectedCellFromUpperGrid()
        {
            Vector3 selectCellGridCenter = SnapToGrid(gridTransition.upperGrid.grid,selectedCellPos);
            Vector3 selectCellGridToWorld = gridTransition.upperGrid.grid.CellToWorld(new Vector3Int((int)selectCellGridCenter.x,(int)selectCellGridCenter.y, (int)selectCellGridCenter.z)); 
            cellIndicator.transform.position = selectCellGridToWorld;
        }*/


        public void BuildTurret(TurretSO turretSo)
        {
            if (selectedCellPos != null)
            {
                Vector2 cellPos2d = new Vector2(selectedCellPos.Value.x, selectedCellPos.Value.z);
                
                GameObject turretGo = Instantiate(turretSo.Prefab, SnapToGrid(gridTransition.upperGrid.gameObject.GetComponent<Grid>(), selectedCellPos.Value), Quaternion.identity, turretManager.transform);
                turretGo.name = selectedCellPos.ToString(); 
                gridTransition.upperGrid.SetContentAt(cellPos2d, turretGo);
                Debug.Log(gridTransition.upperGrid.UsedCells.Count); 
            }
        }
        public Vector3 SnapToGrid(Grid grid, Vector3 pos)
        {
            if (selectedCellPos != null)
            {
                pos = grid.GetCellCenterWorld(selectedCellPos.Value);
                pos.y = 0;
            }

            return pos;
        }

        public void CancelSelection()
        {
            selectedCellPos = null;
        }
    }

}
