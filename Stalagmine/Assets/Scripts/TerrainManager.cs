using System;
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

        internal void GoToNextLayer()
        {
            Debug.Log("La ca pete");
        }

        public void Start()
        {
            gridFactory = new Grids.GridFactory();
            //layers.Add(gridFactory.InitMaps());
            return;
        }

        private void Update()
        {
        }
    }

}
