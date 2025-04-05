using UnityEngine;

namespace GameState
{
    public class GameManager : MonoBehaviour
    {
        private int roundNumber;
        // 20 max
        private int layerNumber;

        [SerializeField]
        public TerrainManager terrainManager;
        [SerializeField]
        public WaveManager waveManager;

        public bool IsPlaying = false;
        public bool IsDead = false;
        public bool IsWin = false;

        public GameObject LayerParent;

        private void Start()
        {
            GetComponent<CoreManager>().Core.CoreDestroyed += GameLost;
        }

        private void OnDestroy()
        {
            GetComponent<CoreManager>().Core.CoreDestroyed -= GameLost;
        }

        void GameLost()
        {
            IsDead = true;
        }

        private void Update()
        {
            if (IsPlaying)
            {
                if (GetComponent<SpawnManager>().SpawnParent.childCount == 0) IsWin = true;
            }
            if (IsWin)
            {
                // Detruire et recommencer
                Debug.Log("Appel a TerrainManager.GoToNextLayer");
                terrainManager.GoToNextLayer();
            }
            if (IsDead)
            {

            }
        }

        public void StartGame()
        {
            waveManager.StartWave();
            IsPlaying = true;
        }
    }
}
