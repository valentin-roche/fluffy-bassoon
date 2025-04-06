using System.Collections.Generic;
using UnityEngine;

namespace Grids
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GameGrid : MonoBehaviour
    {
        [SerializeField] 
        public Vector2 gridSize = new Vector2(7,7);
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



        void Start()
        {
            VoidCells = new List<Cell>();
            UsedCells = new List<Cell>();
            mesh = new Mesh();

            Vector3[] vertices = new Vector3[3];
            Vector2[] uv = new Vector2[3];
            int[] triangles = new int[3];

            vertices[0] = new Vector3(0, 0);
            vertices[1] = new Vector3(0, 0, 100);
            vertices[2] = new Vector3(100, 0, 100);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;


            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            GetComponent<MeshFilter>().mesh = mesh;

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
    