using UnityEngine;

public class Paintable : MonoBehaviour {
    const int TEXTURE_SIZE = 1024;

    public float extendsIslandOffset = 1;

    [Tooltip("Only use on planes. Spawns a grid of nodesPerRow x nodesPerColumn score nodes evenly over the surface of a flat plane.")]
    [SerializeField] bool autoSpawnNodes = false;
    [SerializeField] private int nodesPerRow = 1;
    [SerializeField] private int nodesPerColumn = 1;

    public ScoreNode[] scoreNodes;

    private Mesh planeMesh;

    RenderTexture extendIslandsRenderTexture;
    RenderTexture uvIslandsRenderTexture;
    RenderTexture maskRenderTexture;
    RenderTexture supportTexture;
    
    Renderer rend;

    int maskTextureID = Shader.PropertyToID("_MaskTexture");


    float teamOneLocalScore = 0;
    float teamTwoLocalScore = 0;

    public float surfaceArea = 0;

    public RenderTexture getMask() => maskRenderTexture;
    public RenderTexture getUVIslands() => uvIslandsRenderTexture;
    public RenderTexture getExtend() => extendIslandsRenderTexture;
    public RenderTexture getSupport() => supportTexture;
    public Renderer getRenderer() => rend;
    public float getSurfaceArea() => surfaceArea;

    /// <summary>
    /// Returns the current percent of this paintable surface that is controlled by the indicated team.
    /// </summary>
    /// <param name="teamNum">1 or 2. Score of this team will be returned.</param>
    /// <returns></returns>
    public float GetScore(int teamNum)
    {
        if (teamNum == 1)
        {
            return teamOneLocalScore;
        }
        else if (teamNum == 2)
        {
            return teamTwoLocalScore;
        }

        return 0;
    }



    void Start() {
        maskRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        maskRenderTexture.filterMode = FilterMode.Bilinear;

        extendIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        extendIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        uvIslandsRenderTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        uvIslandsRenderTexture.filterMode = FilterMode.Bilinear;

        supportTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        supportTexture.filterMode =  FilterMode.Bilinear;

        rend = GetComponent<Renderer>();
        rend.material.SetTexture(maskTextureID, extendIslandsRenderTexture);

        PaintManager.instance.initTextures(this);

        planeMesh = GetComponent<MeshCollider>().sharedMesh;

        // Will spawn the indicated number of nodes evenly across a plane.
        if (autoSpawnNodes)
        {
            scoreNodes = new ScoreNode[nodesPerRow * nodesPerColumn];

            
            GameObject nodePrefab = TeamManager.instance.scoreNodePrefab;

            Vector3 startPos = planeMesh.vertices[0];
            Vector3 endPos = planeMesh.vertices[planeMesh.vertices.Length - 1];

            int nodeCount = 0;

            for (int i = 0; i < nodesPerRow; i ++)
            {
                for (int j = 0; j < nodesPerColumn; j++)
                {
                    float currentX = Mathf.Lerp(startPos.x, endPos.x, (float)i/(nodesPerRow - 1));
                    float currentZ = Mathf.Lerp(startPos.z, endPos.z, (float)j/(nodesPerColumn - 1));

                    GameObject currentNode = Instantiate(nodePrefab, transform.TransformPoint(new Vector3(currentX, 0, currentZ)), Quaternion.identity) as GameObject;

                    scoreNodes[nodeCount] = currentNode.GetComponent<ScoreNode>();
                    nodeCount++;
                }
            }
        }

        surfaceArea = CalculateSurfaceArea(planeMesh);
    }

    void OnDisable(){
        maskRenderTexture.Release();
        uvIslandsRenderTexture.Release();
        extendIslandsRenderTexture.Release();
        supportTexture.Release();
    }


    /// <summary>
    /// Checks how many nodes belong to TeamOne and TeamTwo respectively, updating their score variables accordingly.
    /// </summary>
    public void CalculateScore()
    {
        int teamOneCount = 0;
        int teamTwoCount = 0;

        for (int i = 0; i < scoreNodes.Length; i++)
        {
            if (scoreNodes[i].ControllingTeam == 1)
            {
                teamOneCount++;
            }
            else if (scoreNodes[i].ControllingTeam == 2)
            {
                teamTwoCount++;
            }
        }

        teamOneLocalScore = ((float)teamOneCount / scoreNodes.Length);
        teamTwoLocalScore = ((float)teamTwoCount / scoreNodes.Length);
    }


    /// <summary>
    /// Calculates Surface Area of Mesh. NOTE: Mesh sclae must be unifor across all axes!
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns>Surface Area in Units Squared</returns>
    float CalculateSurfaceArea(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        double totalArea = 0.0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 corner = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 a = transform.TransformPoint(vertices[triangles[i + 1]]) - corner;
            Vector3 b = transform.TransformPoint(vertices[triangles[i + 2]]) - corner;

            totalArea += Vector3.Cross(a, b).magnitude;
        }

        return (float)(totalArea / 2.0);
    }



}
