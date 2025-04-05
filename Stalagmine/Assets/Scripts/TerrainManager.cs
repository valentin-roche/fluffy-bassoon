using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameState
{
    public class TerrainManager : MonoBehaviour
    {
        [SerializeField]
        public GameState GameState;
        [SerializeField]
        private GameObject mouseIndicator, cellIndicator;
        [SerializeField]
        private InputManager inputManager;

        private Grids.GridFactory gridFactory;
        // Active grid is always 0
        private List<Grids.GameGrid> layers;


        private void Update()
        {
            Vector3 mousePos = inputManager.GetSelectedMapPosition();
            Vector3Int gridPos = layers.ElementAt(0).WorldToCell(mousePos);
            mouseIndicator.transform.position = mousePos;
            cellIndicator.transform.position = layers.ElementAt(0).CellToWorld(gridPos);
        }
    }

}
