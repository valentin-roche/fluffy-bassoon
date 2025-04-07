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
    public bool lockCellIndicator = false;
    public Vector3 lockedCellPosition; 
    // Update is called once per frame
    void Update()
    {
        Vector3? pos = inputManager.GetSelectedMapPosition();
        if (pos.HasValue)
        {
            Vector3Int cellPosinGrid = gridTransition.upperGrid.gameObject.GetComponent<Grid>().WorldToCell(new Vector3Int((int)pos.Value.x, (int)pos.Value.y, (int)pos.Value.z));
            Vector3 cellPosInWorld = gridTransition.upperGrid.gameObject.GetComponent<Grid>().GetCellCenterWorld(cellPosinGrid);
            //transform.localScale = gridTransition.upperGrid.gameObject.GetComponent<Grid>().cellSize; 
            cellPosInWorld.y = 0.1f;
            if (cellPosInWorld != null)
            {
                transform.position = cellPosInWorld;
            }
        }
    }
    public void Lock()
    {
        lockCellIndicator = true;
    }
    public void Unlock()
    {
        selectedCellIndicator.SetActive(false);
    }
    public bool SetlockedCellPosition(Vector3 Position)
    {
        if(lockedCellPosition.x == Position.x && lockedCellPosition.z == Position.z)
        {
            selectedCellIndicator.SetActive(false);
            lockedCellPosition = Vector3.zero;

            return false;
        }
        else
        {
            Position.y = 0.05f;
            selectedCellIndicator.SetActive(true);
            selectedCellIndicator.transform.position = Position; 
            lockedCellPosition = Position;
            return true;
        }
        
    }
}
