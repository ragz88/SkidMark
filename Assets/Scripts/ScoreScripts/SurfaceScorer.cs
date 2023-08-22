using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceScorer : MonoBehaviour
{
    public float surfaceArea = 0;
    [Tooltip("Percent of the texture's pixels that will actually be analysed when calculating the score.")]
    [SerializeField][Range(1,100)] int accuracyPercent = 10;

    /// <summary>
    /// Every 'increment' rows of our texture will be analysed. In those rows, every 'increment' pixel will be analysed.
    /// Calculated based on the accuracyPercent variable.
    /// </summary>
    int increment;
    /// <summary>
    /// How many pixels will be analysed in each row of our mask.
    /// </summary>
    int pixelsPerRow;

    /// <summary>
    /// How many pixels in each analysed row belong to Team One.
    /// </summary>
    int[] teamOneTallies;
    /// <summary>
    /// How many pixels in each analysed row belong to Team Two.
    /// </summary>
    int[] teamTwoTallies;


    public float TeamOnePercent = 0;
    public float TeamTwoPercent = 0;


    int iteration = 0;
    Color[] maskColours;

    Mesh surfaceMesh;
    Paintable paintable;

    /// <summary>
    /// This is the Render Texture that stores a representation of what has and hasn't been painted.
    /// </summary>
    RenderTexture mask;

    bool initialised = false;

    const float COLOUR_TOLERANCE = 0.1f;

    void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // There are some Getter Functions that rely on the Start function being complete already. 
        // As such, we'll run an initialisation script in the first update frame.
        if (initialised == false)
        {
            DelayedInitialisation();
        }

        mask = paintable.getMask();

        // Here we count the pixels of each team's colour in a single row of pixels from our mask texture.
        AnalyseTextureRow(iteration, increment);

        iteration++;
        iteration = iteration % pixelsPerRow;

        //Here we turn the counts we obtained in our analysis into a working percentage, representing the score of each team on this surface.
        ProcessTallies();

        // This way, we only use the expensive GetPixels function every pixelsPerRow frames.
        // While this means the score technically doesn't update exactly in real time, it'll still update approximately 1 - 3 times a second.
        if (iteration == 0)
        {
            UpdateColorArray();
        }


        //Debug.Log("Team 1: " + TeamOnePercent);
        //Debug.Log("Team 2: " + TeamTwoPercent);
    }


    /// <summary>
    /// Initialises our references and values after all other scripts have had the opportunity to initialise theirs.
    /// </summary>
    void DelayedInitialisation()
    {
        surfaceMesh = GetComponent<MeshCollider>().sharedMesh;
        surfaceArea = CalculateSurfaceArea(surfaceMesh);

        paintable = GetComponent<Paintable>();
        mask = paintable.getMask();

        increment = Mathf.RoundToInt(mask.height / (mask.height * (accuracyPercent / 100f)));
        Debug.Log(increment);
        pixelsPerRow = Mathf.RoundToInt(mask.height * (accuracyPercent / 100f));

        print("Inc - " + increment);
        print("ppr - " + pixelsPerRow);

        teamOneTallies = new int[pixelsPerRow];
        teamTwoTallies = new int[pixelsPerRow];

        UpdateColorArray();

        initialised = true;
    }


    /// <summary>
    /// Analyses the colours in a single row of our Render Texture.
    /// </summary>
    /// <param name="rowNum">Row of pixels to analyse</param>
    /// <param name="increment">Every 'increment' pixels in the row will be analysed. A lower Increment is more accurate, but more expensive.</param>
    void AnalyseTextureRow(int rowNum, int increment)
    {
        int offset = rowNum * mask.width;

        int teamOneCount = 0;
        int teamTwoCount = 0;

        // Loop through specific group of values from our colour array, which represent a specific row in our mask texture.
        for (int i = 0; i <= mask.width; i += increment)
        {
            if (ColourSimilar(TeamManager.instance.colour_TeamOne, maskColours[offset + i], COLOUR_TOLERANCE))
            {
                teamOneCount++;
            }
            else if (ColourSimilar(TeamManager.instance.colour_TeamTwo, maskColours[offset + i], COLOUR_TOLERANCE))
            {
                teamTwoCount++;
            }
        }

        teamOneTallies[rowNum] = teamOneCount; 
        teamTwoTallies[rowNum] = teamTwoCount;
    }


    /// <summary>
    /// Gets the current Pixels of our Mask and stores them in our Color array.
    /// </summary>
    void UpdateColorArray()
    {
        RenderTexture.active = mask;
        Texture2D tempTexture = new Texture2D(mask.width, mask.height);
        tempTexture.ReadPixels(new Rect(0, 0, mask.width, mask.height), 0, 0);
        tempTexture.Apply();

        maskColours = tempTexture.GetPixels();
        

        RenderTexture.active = null;
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


    
    /// <summary>
    /// Compares two colours for similarity.
    /// </summary>
    /// <param name="colour1">First Colour.</param>
    /// <param name="colour2">Second Colour.</param>
    /// <param name="tolerance">How much the individual r, g, and b values can be off by while still being considered similar.</param>
    /// <returns>Whether or not the two colours are similar.</returns>
    bool ColourSimilar(Color colour1, Color colour2, float tolerance)
    {
        if (Mathf.Abs(colour1.r - colour2.r) < tolerance)
        {
            if (Mathf.Abs(colour1.g - colour2.g) < tolerance)
            {
                if (Mathf.Abs(colour1.b - colour2.b) < tolerance)
                {
                    return true;
                }
            }
        }

        return false;

    }


    /// <summary>
    /// Convert our int arrays that represent the number of pixels in each team's colour into a percentage.
    /// </summary>
    void ProcessTallies()
    {
        int teamOneTot = 0;
        int teamTwoTot = 0;

        for (int i = 0; i < pixelsPerRow; i++)
        {
            teamOneTot += teamOneTallies[i];
            teamTwoTot += teamTwoTallies[i];
        }

        TeamOnePercent = teamOneTot / Mathf.Pow(pixelsPerRow, 2);
        TeamTwoPercent = teamTwoTot / Mathf.Pow(pixelsPerRow, 2);
    }
}
