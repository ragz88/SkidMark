using UnityEngine;
public class DriftPainter : MonoBehaviour
{
    [SerializeField] private DriftManager driftManager;

    [Space]

    [Tooltip("Where the central painter raycast will fire down from, relative to car body centre.")]
    [SerializeField] Vector3 offset = new Vector3(0,0,0);
    [Tooltip("How far from the ground can we paint from?")]
    [SerializeField] float paintDistance = 1f;
    const float sideOffset = 1.5f;
    const float frontOffset = 2.75f;
    const float backOffset = 3f;

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
    [SerializeField] public GameObject scoreCapsule;


    private float radius = 1;
    // Rotation is in radians, with Pi being 180 degrees.
    private float rotation = 0;

    private Vector3 scoreCapsuleInitialScale;


    private void Start()
    {
        scoreCapsuleInitialScale = scoreCapsule.transform.localScale;
        
    }


    void Update()
    {
        if (driftManager.isDrifting)  
        {
            // Central Ray --------------------------------------------------------------------------------------
            Ray ray = new Ray((transform.position + offset), Vector3.down);
            RaycastHit hit;
            Vector3 hitPoint = (transform.position + offset);

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();                 
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y*Mathf.Deg2Rad;
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, width, height, rotation, paintColor);

                    hitPoint = hit.point; // THIS METHOD WILL ONLY WORK WHEN THERE"S PAINTABLE GROUND UNDER CAR. if we're near paintable ground, but not on it, we'll get an artifact.

                    //Update Score
                    p.CalculateScore();
                    GameModeManager.instance.ScoreUpdatedThisFrame = true;
                }
            }
            // End Central ----------------------------------------------------------------------------------------


            // Left Ray --------------------------------------------------------------------------------------
            ray = new Ray((transform.position + offset + new Vector3(-sideOffset, 0, 0)), Vector3.down);

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                    PaintManager.instance.paint(p, hitPoint, radius, hardness, strength, width, height, rotation, paintColor);
                }
            }
            // End Left ----------------------------------------------------------------------------------------

            // Right Ray --------------------------------------------------------------------------------------
            ray = new Ray((transform.position + offset + new Vector3(sideOffset, 0, 0)), Vector3.down);

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                    PaintManager.instance.paint(p, hitPoint, radius, hardness, strength, width, height, rotation, paintColor);
                }
            }
            // End Right ----------------------------------------------------------------------------------------

            // Front Ray --------------------------------------------------------------------------------------
            ray = new Ray((transform.position + offset + new Vector3(0, 0, frontOffset)), Vector3.down);

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                    PaintManager.instance.paint(p, hitPoint, radius, hardness, strength, width, height, rotation, paintColor);
                }
            }
            // End Front ----------------------------------------------------------------------------------------

            // Back Ray --------------------------------------------------------------------------------------
            ray = new Ray((transform.position + offset + new Vector3(0, 0, -backOffset)), Vector3.down);

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                //Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
                    PaintManager.instance.paint(p, hitPoint, radius, hardness, strength, width, height, rotation, paintColor);
                }
            }
            // End Back ----------------------------------------------------------------------------------------

            scoreCapsule.SetActive(true);
        }
        else
        {
            scoreCapsule.SetActive(false);
            scoreCapsule.transform.localScale = scoreCapsuleInitialScale;
        }

    }

}
