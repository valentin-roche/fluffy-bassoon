using UnityEngine;

public class ProceduralMeshTest : MonoBehaviour
{
    private void OnEnable()
    {
        var mesh = new Mesh
        {
            name = "Procedural Mesh"
        };

        mesh.vertices = new Vector3[] {
            new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1),
        };
        
        mesh.normals = new Vector3[] {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back
        };

        mesh.triangles = new int[] {
            0, 2, 1, 1, 2, 3
        };

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
