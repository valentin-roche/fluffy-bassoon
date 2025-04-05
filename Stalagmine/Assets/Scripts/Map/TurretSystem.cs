using UnityEngine;
using UnityEngine.Tilemaps;

public class TurretSystem : MonoBehaviour
{
    public static TurretSystem current;

    public GridLayout gLayout;
    private Grid grid;
    [SerializeField] private Tilemap MainTilemap;
    [SerializeField] private Tile basicTile;

    public GameObject turretPrefab;

    // private PlaceableObject toPlace;

    void Awake()
    {
        // One turret system per layer -> easier to disable inputs per layer
        current = this;
        grid = gLayout.gameObject.GetComponent<Grid>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)){
            InitWithTurret(turretPrefab);
        }
    }

    public static Vector3 GetMouseworldPos() {
        Ray billyCyrus = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(billyCyrus, out RaycastHit raycastHit)) {
            return raycastHit.point;
        }
        else {
            return Vector3.zero;
        }
    }

    public Vector3 SnapToGrid(Vector3 pos) {
        Vector3Int cellPos = gLayout.WorldToCell(pos);
        pos = grid.GetCellCenterWorld(cellPos);
        return pos;
    }

    public void InitWithTurret(GameObject turret){
        Vector3 position = SnapToGrid(GetMouseworldPos());
        GameObject newTurret = Instantiate(turret, position, Quaternion.identity);
        newTurret.AddComponent<ObjectPlace>();

    }
}
