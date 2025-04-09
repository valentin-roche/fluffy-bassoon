using Grids;
using UnityEngine;

public class CellIndicator : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private GridTransition gridTransition;
    [SerializeField]
    private GameObject selectedCellIndicator;

    bool stateCellIndic = false;
    void Update()
    {
        Vector3? pos = inputManager.GetSelectedMapPosition();
        if (pos.HasValue && IsInBound(pos.Value) && gridTransition.upperGrid != null)
        {
            Vector3Int cellPosinGrid = gridTransition.upperGrid.gameObject.GetComponent<Grid>().WorldToCell(pos.Value + new Vector3(3.5f, 0, 3.5f));

            Vector3 cellPosInWorld = gridTransition.upperGrid.gameObject.GetComponent<Grid>().CellToWorld(cellPosinGrid);
            cellPosInWorld.y = 0.55f;
            //cellPosInWorld.x -= 0.65f;
            //cellPosInWorld.z -= 0.65f;
            if (cellPosInWorld != null)
            {
                transform.position = cellPosInWorld;
            }
        }
    }

    private bool IsInBound(Vector3 pos)
    {
        //if (Mathf.Abs(pos.x) >= Mathf.Abs(gridTransition.upperGrid.gridSize.x * gridTransition.upperGrid.gameObject.GetComponent<Grid>().cellSize.x) || Mathf.Abs(pos.z) >= Mathf.Abs(gridTransition.upperGrid.gridSize.y * gridTransition.upperGrid.gameObject.GetComponent<Grid>().cellSize.z))
        //    return false;

        return pos.y > -10f;
    }
    public void Lock()
    {
        selectedCellIndicator.SetActive(true);
        stateCellIndic = true;
    }
    public void Unlock()
    {
        selectedCellIndicator.SetActive(false);
        stateCellIndic = false;
    }
    public bool SetlockedCellPosition(Vector3 Position)
    {
        if(stateCellIndic && selectedCellIndicator.transform.position.x == Position.x && selectedCellIndicator.transform.position.z == Position.z)
        {
            Unlock();
            return false;
        }
        else
        {
            Position.y = 0.5f;
            selectedCellIndicator.transform.position = Position;
            Lock();
            return true;
        }
        
    }
}
