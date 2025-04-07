using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Grids
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GameGrid : MonoBehaviour
    {
        [SerializeField]
        public Vector2Int gridSize = new Vector2Int(11, 11);
        [SerializeField]
        public GameObject cellPrefab;
        [SerializeField]
        private Grid grid;
        public int InitialVoidNum = 5;
        public List<Cell> VoidCells;
        public List<Cell> UsedCells;
        public List<Cell> EternalCells;
        public List<Cell> EmptyCells;
        private Mesh mesh;

        private List<Vector2> voidCellsPos;
        private List<Vector2> eternalCellsPos;
        private List<Vector2> usedCellsPos;

        public static List<Vector2> Directions = new List<Vector2>
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, -1),
            new Vector2(0, 1)
        };


        private Vector3[] vertices;
        List<int>[] trisWithVertex;
        int[] triangles;
        bool[] trianglesDisabled;

        private int enternalsRange = 2;

        void Start()
        {
            // Set to center
            //float centeringOffset = -gridSize.x * grid.cellSize.x / 4;
            //transform.position = new Vector3(centeringOffset, transform.position.y, centeringOffset);
            VoidCells = new List<Cell>();
            UsedCells = new List<Cell>();
            voidCellsPos = new List<Vector2>();
            usedCellsPos = new List<Vector2>();
            eternalCellsPos = new List<Vector2>();

            // Assign Void Cells
            while (voidCellsPos.Count < InitialVoidNum)
            {
                Vector2 RandomPos = GetRandomCellPos();
                if (voidCellsPos.Contains(RandomPos) == false)
                {
                    voidCellsPos.Add(RandomPos);
                }
            }

            usedCellsPos.Add(new Vector2(0, 0));
            //UsedCells.Add(CreateCell(new Vector2(0, 0), Status.Full));
            
            //Init eternal cells
            for(int i=-enternalsRange; i<enternalsRange; i++)
            {
                for(int j=-enternalsRange; j<enternalsRange; j++)
                {
                    eternalCellsPos.Add(new Vector2(i, j));
                }
            }

            //Init cells
            for (int i = -gridSize.x; i < gridSize.x; i++)
            {
                for (int j = -gridSize.y; j < gridSize.y; j++)
                {
                    Vector2 gridPosition = new Vector2(i, j);
                    if (voidCellsPos.Contains(gridPosition))
                    {
                        VoidCells.Add(CreateCell(gridPosition, Status.Void));
                    }
                    else if (eternalCellsPos.Contains(gridPosition))
                    {
                        EternalCells.Add(CreateCell(gridPosition, Status.Eternal));
                    }
                    else if (usedCellsPos.Contains(gridPosition))
                    {
                        UsedCells.Add(CreateCell(gridPosition, Status.Full));
                    }
                    else
                    {
                        EmptyCells.Add(CreateCell(gridPosition));
                    }
                }
            }

            Debug.Log(EternalCells.Count); 

            // Init Mesh
            mesh = new Mesh();
            mesh.name = "Grid";

            vertices = new Vector3[(gridSize.x + 1) * (gridSize.y + 1)];
            List<int>[] trisWithVertex;
            triangles = new int[gridSize.x * gridSize.y * 6];
            trianglesDisabled = new bool[triangles.Length];

            RefreshMesh();
        }

        public void RefreshMesh()
        {
            Mesh newMesh = new Mesh();
            newMesh.name = mesh?.name;

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
            newMesh.SetVertices(vertices);
            this.vertices = vertices;
            newMesh.SetUVs(0, uv);

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

            trisWithVertex = new List<int>[vertices.Length];
            for (int i = 0; i < vertices.Length; ++i)
            {
                trisWithVertex[i] = IndexOf(triangles, i);
            }

            newMesh.triangles = triangles;

            bool[] vm = getVoidMap(newMesh);
            newMesh.triangles = triangles.RemoveAllSpecifiedIndicesFromArray(vm).ToArray();


            newMesh.vertices = vertices;
            this.vertices = vertices;
            newMesh.uv = uv;
            newMesh.triangles = triangles;
            newMesh.RecalculateNormals();
            mesh = newMesh;
            SerializedObject s = new SerializedObject(mesh);
            s.FindProperty("m_IsReadable").boolValue = true;
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;

            //if(!IsLowerGrid)
            //    transform.position = Vector3.zero;
            //else
            //    transform.position = new Vector3(0, -20, 0);
        }

        private Vector2 GetRandomCellPos()
        {
            int minRange = (int)-gridSize.x;
            int maxRange = (int)gridSize.y;
            int randomX = Random.Range(minRange, maxRange);
            while(randomX > -enternalsRange && randomX < enternalsRange)
            {
                randomX = Random.Range(minRange, maxRange);
            }
            int randomY = Random.Range(minRange, maxRange);
            while (randomY > -enternalsRange && randomX < enternalsRange)
            {
                randomY = Random.Range(minRange, maxRange);
            }
            return new Vector2(randomX, randomY);
        }

        /// <summary>
        /// Returns Triangles in the provided mesh affected at the pos of a cell
        /// </summary>
        /// <param name="gridPos"></param>
        /// <param name="TargetMesh"></param>
        /// <returns></returns>
        public bool[] SelectCellInMeshAt(Vector2 gridPos, Mesh TargetMesh) {
            Cell cell = getCellAt(gridPos);
            Vector3 cellRealPos = grid.CellToWorld(new Vector3Int((int)cell.Position.x, 0, (int)cell.Position.y));
            bool[] tiranglesAffected = new bool[TargetMesh.triangles.Length];
            for (int i = 0; i < vertices.Length; ++i)
            {
                if (vertices[i] == cellRealPos)
                {
                    for (int j = 0; j < trisWithVertex[i].Count; ++j)
                    {
                        int value = trisWithVertex[i][j];
                        int remainder = value % 3;
                        tiranglesAffected[value - remainder] = true;
                        tiranglesAffected[value - remainder + 1] = true;
                        tiranglesAffected[value - remainder + 2] = true;
                    }
                }
                else
                {
                    for (int j = 0; j < trisWithVertex[i].Count; ++j)
                    {
                        int value = trisWithVertex[i][j];
                        int remainder = value % 3;
                        tiranglesAffected[value - remainder] = false;
                        tiranglesAffected[value - remainder + 1] = false;
                        tiranglesAffected[value - remainder + 2] = false;
                    }
                }
            }
            return tiranglesAffected;
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
            //float centeringOffset = -gridSize.x * grid.cellSize.x / 4;
            //transform.position = new Vector3(centeringOffset, transform.position.y, centeringOffset);
            //RefreshMesh();

        }

        public void ActualizeGrid()
        {
            foreach (Cell voidCell in VoidCells)
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


        private bool[] getVoidMap(Mesh newMesh)
        {
            bool[] trianglesAffectedByVoid = new bool[newMesh.triangles.Length];
            // Pour chaque Cell void => get affected cells puis disable
            foreach (Cell voidCell in VoidCells)
            {
                bool[] trianglesAffectedByVoidCell = SelectCellInMeshAt(voidCell.Position, newMesh);
                trianglesAffectedByVoid = trianglesAffectedByVoid.Merge(trianglesAffectedByVoidCell).ToArray();
            }
            return trianglesAffectedByVoid;
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

            coord = new Vector3(coord.x - (gridSize.x * grid.cellSize.x / 2f), 0, coord.z - (gridSize.y * grid.cellSize.z / 2f));

            coord.y = 0;
            return coord;
        }

        public Cell getCellAt(Vector2 pos)
        {
            foreach (Cell voidCell in VoidCells)
            {
                if (voidCell.Position == pos)
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
            return CreateCell(pos);
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
            Cell target = getCellAt(pos);
            if (target.Status != Status.Void)
            {
                target.MakeVoid();
                VoidCells.Add(getCellAt(pos));
            }
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

        public Cell CreateCell(Vector2 pos, Status status = Status.Empty)
        {
            Vector3 WorldPosition = grid.CellToWorld(new Vector3Int((int)pos.x, 0, (int)pos.y));
            GameObject cellInstance = Instantiate(cellPrefab, transform);
            cellInstance.name = "Cell("+pos.x+","+pos.y+")";
            //cellInstance.transform.InverseTransformPoint(WorldPosition);
            cellInstance.transform.position = new Vector3(pos.x * grid.cellSize.x, grid.transform.position.y, pos.y * grid.cellSize.z);
            cellInstance.transform.localScale = new Vector3(grid.cellSize.x, 1f, grid.cellSize.z);
            Cell cell = cellInstance.GetComponent<Cell>();
            cell.Position = pos;
            cell.SetStatus(status);
            return cellInstance.GetComponent<Cell>();
        }
    }

}
    