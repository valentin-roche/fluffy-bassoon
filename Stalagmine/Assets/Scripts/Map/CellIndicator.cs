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
        if (pos.HasValue && IsInBound(pos.Value))
        {
            Vector3Int cellPosinGrid = gridTransition.upperGrid.gameObject.GetComponent<Grid>().WorldToCell(new Vector3Int((int)pos.Value.x, (int)pos.Value.y, (int)pos.Value.z));

            Vector3 cellPosInWorld = gridTransition.upperGrid.gameObject.GetComponent<Grid>().GetCellCenterWorld(cellPosinGrid);
            cellPosInWorld.y = 0.55f;
            cellPosInWorld.x -= 0.65f;
            cellPosInWorld.z -= 0.65f;
            if (cellPosInWorld != null)
            {
                transform.position = cellPosInWorld;
            }
        }
    }

    private bool IsInBound(Vector3 pos)
    {
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
