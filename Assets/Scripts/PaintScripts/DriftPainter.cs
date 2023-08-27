using UnityEngine;
public class DriftPainter : MonoBehaviour
{
    [SerializeField] private DriftManager driftManager;
    [SerializeField] private int teamNumber = 1;

    [Space]

    [Tooltip("Where the painter raycast will fire down from, relative to car body centre.")]
    [SerializeField] Vector3 offset = new Vector3(0,0,0);
    [Tooltip("How far from the ground can we paint from?")]
    [SerializeField] float paintDistance = 1f;

    [Space]


    [Header("Brush Settings")]

    //[Tooltip("Colour of this car's paint.")]
    [HideInInspector] public Color paintColor;
    [SerializeField] float strength = 1;
    [SerializeField] float hardness = 1;

    [Tooltip("Width of brush ellipse.")]
    [SerializeField] float width = 1;
    [Tooltip("Height of brush ellipse.")]
    [SerializeField] float height = 1;

    [Space]

    [Tooltip("Child capsule collider that will collide with score nodes in secret")]
    [SerializeField] private GameObject scoreCapsule;


    private float radius = 1;
    // Rotation is in radians, with Pi being 180 degrees.
    private float rotation = 0;

    private Vector3 scoreCapsuleInitialScale;


    private void Start()
    {
        scoreCapsuleInitialScale = scoreCapsule.transform.localScale;

        // Fix - assign this from GameModeManager
        if (teamNumber == 1)
        {
            paintColor = GameModeManager.instance.colour_TeamOne;
        }
        else if (teamNumber == 2)
        {
            paintColor = GameModeManager.instance.colour_TeamTwo;
        }
        
    }


    void Update()
    {
        if (driftManager.isDrifting)  
        {
            Ray ray = new Ray((transform.position + offset), Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();                 //ERIK UPDATE THIS TO CHECK THE WHOLE CAR BOTTOM!
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y*Mathf.Deg2Rad;
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, width, height, rotation, paintColor);

                    //Update Score
                    p.CalculateScore();
                    GameModeManager.instance.ScoreUpdatedThisFrame = true;
                }
            }

            scoreCapsule.SetActive(true);
        }
        else
        {
            scoreCapsule.SetActive(false);
            scoreCapsule.transform.localScale = scoreCapsuleInitialScale;
        }

    }

}
