using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.LowLevelPhysics;

namespace Grids
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] 
        public Vector2Int gridSize = new Vector2Int(11,11);
        [SerializeField] 
        public Grid grid;
        public List<Cell> VoidCells;
        public List<Cell> UsedCells;
        public List<Cell> EternalCells; 
        private Mesh mesh;

        
        public static List<Vector2> Directions = new List<Vector2>
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(0, 1)
        };


        private Vector3[] vertices;
        List<int>[] trisWithVertex;
        bool[] trianglesDisabled;


        void Start()
        {
            // Set to center
            float centeringOffset = -gridSize.x * grid.cellSize.x / 4;
            transform.position = new Vector3(centeringOffset, transform.position.y, centeringOffset);
            VoidCells = new List<Cell>();
            UsedCells = new List<Cell>();
            UsedCells.Add(new Cell(new Vector2(0, 0), Status.Full)); 
            for(int i=-2; i<2; i++)
            {
                for(int j=-2; j<2; j++)
                {
                    EternalCells.Add(new Cell(new Vector2(i, j), Status.Eternal));
                }
            }
            Debug.Log(EternalCells.Count); 
            mesh = new Mesh();
            mesh.name = "Grid";
            RefreshMesh();

        }

        public GameGrid(List<Cell> VoidCells, List<Cell> UsedCells, List<Cell> eternalCells)
        {
            this.VoidCells = VoidCells;
            this.UsedCells = UsedCells;
            EternalCells = eternalCells;
        }

        public void Update()
        {
            // Set to center
            float centeringOffset = -gridSize.x * grid.cellSize.x / 4;
            transform.position = new Vector3(centeringOffset, transform.position.y, centeringOffset);
            RefreshMesh();

        }

        

        public void ActualizeGrid()
        {
            foreach(Cell voidCell in VoidCells)
            {
                foreach (Vector2 ouais in Directions) {
                    Cell neighb = getCellAt(voidCell.Position - ouais);
                    if(neighb.Status != Status.Eternal)
                    {
                        if (neighb.Status == Status.Full)
                        {

                            neighb.MakeVoid();
                            VoidCells.Insert(0, neighb);
                            if (getUsedCellAt(neighb.Position))
                            {
                                UsedCells.Remove(getUsedCellAt(neighb.Position));
                            }
                        }
                    }
                    
                }
            }
            RefreshMesh();
        }

        public void RefreshMesh()
        {
            Mesh newMesh = new Mesh();

            // Handle vertices
            Vector3[] vertices = new Vector3[(gridSize.x + 1) * (gridSize.y + 1)];
            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0, y = 0; y <= gridSize.y; y++)
            {
                for (int x = 0; x <= gridSize.x; x++, i++)
                {
                    vertices[i] = GetCellCenterCoordFromGridPos(y, x);
                    uv[i] = new Vector2(x / gridSize.x, y / gridSize.y);
                }
            }
            newMesh.vertices = vertices;
            this.vertices = vertices;
            newMesh.uv = uv;

            // Handle triangles
            int[] triangles = new int[gridSize.x * gridSize.y * 6];
            trianglesDisabled = new bool[triangles.Length];

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

            trisWithVertex = new List<int>[vertices.Length];
            for (int i = 0; i < vertices.Length; ++i)
            {
                trisWithVertex[i] = IndexOf(triangles, i);
            }

            newMesh.triangles = triangles;


            newMesh.RecalculateNormals();
            mesh = newMesh;
            SerializedObject s = new SerializedObject(mesh);
            s.FindProperty("m_IsReadable").boolValue = true;
            GetComponent<MeshFilter>().mesh = mesh;


            SyncMeshCollider();

        }

        public void SyncMeshCollider()
        {
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private List<int> IndexOf(int[] triangles, int i)
        {
            List<int> result = new List<int>();
            for (int t = 0; t < triangles.Length; ++t)
            {
                if (triangles.GetValue(t).Equals(i))
                {
                    result.Add(t);
                }
            }
            result.Sort();
            return result;
        }

        private Vector3 GetCellCenterCoordFromGridPos(int y, int x)
        {
            Vector3Int gridPosition = new Vector3Int(x, 0, y);
            Vector3 coord = grid.CellToWorld(gridPosition);
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

        public Cell getUsedCellAt(Vector2 pos)
        {
            foreach (Cell usedCell in UsedCells)
            {
                if (usedCell.Position == pos)
                {
                    return usedCell;
                }
            }
            return null; 
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
            UsedCells.Add(getCellAt(pos)); 
        }


        public List<Cell> getVoidCells()
        {
            return VoidCells;
        }

        public bool isVectorInGridGame(Vector2 Pos)
        { 
            if (-gridSize.x < Pos.x && Pos.x < gridSize.x && -gridSize.y < Pos.y && Pos.y < gridSize.y)
            {
                return true;
            }
            return false;
        }
    }

}
    