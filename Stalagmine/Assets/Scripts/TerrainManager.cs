using Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
        [SerializeField]
        public TurretSO[] turretSOs;

        private Vector3Int selectedCellPos; 

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClickLeftMouseToBuild();
                BuildTurret(turretSOs[0], selectedCellPos); 
            }
            
        }
        public bool OnClickLeftMouseToBuild()
        {
            if (gridTransition != null && gridTransition.upperGrid != null)
            {
                Vector3 mousePos = inputManager.GetSelectedMapPosition();
                Vector3Int gridPos = gridTransition.upperGrid.gameObject.GetComponent<Grid>().WorldToCell(mousePos);
                Vector2 gridPos2d = new Vector2(gridPos.x, gridPos.z);

                if (gridTransition.upperGrid.isCellEmpty(gridPos2d))
                {
                    selectedCellPos = gridPos; 
                    return true;
                }
                

            }
            return false;  
        }
        public void BuildTurret(TurretSO turretSo, Vector3Int cellPos)
        {
            if (selectedCellPos != null)
            {
                Vector2 cellPos2d = new Vector2(cellPos.x, cellPos.z);
                GameObject turretGo = Instantiate(turretSo.Prefab, SnapToGrid(gridTransition.upperGrid.gameObject.GetComponent<Grid>(), cellPos), Quaternion.identity, turretManager.transform);
                gridTransition.upperGrid.SetContentAt(cellPos2d, turretGo);
            }
        }
        public Vector3 SnapToGrid(Grid grid, Vector3 pos)
        {
            Vector3Int cellPos = grid.WorldToCell(pos);
            pos = grid.GetCellCenterWorld(cellPos);
            pos.y = 0; 
            return pos;
        }
    }

}
