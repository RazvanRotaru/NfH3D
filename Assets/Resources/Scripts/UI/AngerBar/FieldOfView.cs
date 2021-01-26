using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public static FieldOfView instance;

    private void Awake()
    {
        instance = this;
    }


    Mesh mesh;
    public LayerMask layerMask;

    public float fov;
    public float fovY = 60;
    public float viewDistance;
    public int rayCount;
    public int layerCount;

    float startingAngle;
    Vector3 origin;

    public delegate void FieldOfViewEvent(GameObject instance);
    public event FieldOfViewEvent OnPrankDetected;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        origin = Vector3.zero;
    }

    void LateUpdate()
    {
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[layerCount * (rayCount + 1) + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3 
                                    + (layerCount - 1) * (rayCount * 3 * 3)];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;

        for (int layer = 0; layer < layerCount; layer++)
        {
            angle = startingAngle;
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit hit;

                if (Physics.Raycast(origin, AngleToVector(angle, layer), out hit,
                                                            viewDistance, layerMask))
                {
                    vertex = hit.point;
                    if (hit.collider.CompareTag("Prank") ||
                                hit.collider.CompareTag("Player"))
                        OnPrankDetected(hit.collider.gameObject);
                }
                else
                    vertex = origin + AngleToVector(angle, layer) * viewDistance;

                vertices[vertexIndex] = vertex;

                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                if (layer > 0 && i > 0)
                {
                    triangles[triangleIndex + 0] = vertexIndex - 1
                                                       - (rayCount + 1);
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex
                                                       - (rayCount + 1);
                 
                    triangleIndex += 3;

                    triangles[triangleIndex + 0] = vertexIndex
                                                       - (rayCount + 1);
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;

            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    Vector3 AngleToVector(float angle, int layer)
    {
        float radians = angle * Mathf.PI / 180;
        float alpha = (-fovY / 2 + layer * fovY / layerCount) * Mathf.PI / 180;
        return new Vector3(Mathf.Cos(radians), Mathf.Sin(alpha), Mathf.Sin(radians));
    }

    float VectorToAngle(Vector3 dir)
    {
        dir = dir.normalized;

        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        return (n + 360) % 360;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        Vector3 aimDir = aimDirection;
        aimDir.y = 0;
        startingAngle = VectorToAngle(aimDir) + fov / 2f;
    }
}
