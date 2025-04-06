using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Grids
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] 
        public Vector2Int gridSize = new Vector2Int(11,11);
        [SerializeField] 
        private Grid grid;
        public List<Cell> VoidCells;
        public List<Cell> UsedCells;
        private Mesh mesh;

        public static List<Vector2> Directions = new List<Vector2>
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(0, 1)
        };
        private Vector3[] vertices;

        void Start()
        {
            VoidCells = new List<Cell>();
            UsedCells = new List<Cell>();
            mesh = new Mesh();
            mesh.name = "Grid";
            RefreshMesh();

        }

        public GameGrid(List<Cell> VoidCells, List<Cell> UsedCells)
        {
            this.VoidCells = VoidCells;
            this.UsedCells = UsedCells;
        }

        public void ActualizeGrid()
        {
            foreach(Cell voidCell in VoidCells)
            {
                foreach (Vector2 ouais in Directions) {
                    Cell neighb = getCellAt(voidCell.Position - ouais);
                    if (neighb.Status != Status.Void)
                    {
                        neighb.MakeVoid();
                        VoidCells.Insert(0, neighb);
                    }
                }
            }
            RefreshMesh();
        }

        public void RefreshMesh()
        {
            Mesh newMesh = new Mesh();

            // Handle vertices
            vertices = new Vector3[(gridSize.x + 1) * (gridSize.y + 1)];
            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0, y = 0; y <= gridSize.y; y++)
            {
                for (int x = 0; x <= gridSize.x; x++, i++)
                {
                    Vector3 cellCoordInGrid = GetCellCenterCoordInGrid(y, x);
                    vertices[i] = new Vector3(x , 0, y) + cellCoordInGrid;
                    uv[i] = new Vector2(x / gridSize.x, y / gridSize.y);
                }
            }
            newMesh.vertices = vertices;
            newMesh.uv = uv;

            // Handle triangles
            int[] triangles = new int[gridSize.x * gridSize.y * 6];

            for (int ti = 0, vi = 0, y = 0; y < gridSize.y; y++, vi++)
            {
                for (int x = 0; x < gridSize.x; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + gridSize.x + 1;
                    triangles[ti + 5] = vi + gridSize.x + 2;
                }
            }
            newMesh.triangles = triangles;
            mesh.RecalculateNormals();

            GetComponent<MeshFilter>().mesh = newMesh;
        }

        private Vector3 GetCellCenterCoordInGrid(int y, int x)
        {
            Vector3Int cellPosition = new Vector3Int(x, 0, y);
            Vector3 coord = grid.GetCellCenterWorld(cellPosition);
            return coord;
        }

        private void OnDrawGizmos()
        {
            if (vertices == null)
            {
                return;
            }
            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }

        public Cell getCellAt(Vector2 pos)
        {
            foreach(Cell voidCell in VoidCells)
            {
                if(voidCell.Position == pos)
                {
                    return voidCell; 
                }
            }
            foreach (Cell usedCell in UsedCells)
            {
                if (usedCell.Position == pos)
                {
                    return usedCell;
                }
            }
            return new Cell(pos); 
        }

        public bool isCellEmpty(Vector2 pos)
        {
            foreach (Cell voidCell in VoidCells)
            {
                if (voidCell.Position == pos)
                {
                    return false;
                }
            }
            foreach (Cell usedCell in UsedCells)
            {
                if (usedCell.Position == pos)
                {
                    return false;
                }
            }
            return true; 
        }

        public void MakeVoidAt(Vector2 pos)
        {
            getCellAt(pos).MakeVoid();
        }



        public void SetContentAt(Vector2 pos, GameObject go)
        {
            getCellAt(pos).SetContent(go);
        }

        //private Mesh GetCellMeshAt(Vector2 pos)
        //{
        //    return getCellAt(pos).getMesh();
        //}

        public List<Cell> getVoidCells()
        {
            return VoidCells;
        }
    }

}
    