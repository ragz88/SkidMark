using UnityEngine;
public class DriftPainter : MonoBehaviour
{
    [Tooltip("Where the painter raycast will fire down from, relative to car body centre.")]
    [SerializeField] Vector3 offset = new Vector3(0,0,0);
    [Tooltip("How far from the ground can we paint from?")]
    [SerializeField] float paintDistance = 1f;

    [Space]


    [Header("Brush Settings")]

    [Tooltip("Colour of this car's paint.")]
    [SerializeField] Color paintColor;
    [SerializeField] float strength = 1;
    [SerializeField] float hardness = 1;

    [Tooltip("Width of brush ellipse.")]
    [SerializeField] float width = 1;
    [Tooltip("Height of brush ellipse.")]
    [SerializeField] float height = 1;


    private float radius = 1;
    // ROtation is in radians, with Pi being 180 degrees.
    private float rotation = 0;

    void Update()
    {
        if (Input.GetButton("Jump"))  // Replace with "if Drifting"
        {
            Ray ray = new Ray((transform.position + offset), Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, paintDistance))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                Paintable p = hit.collider.GetComponent<Paintable>();
                if (p != null)
                {
                    rotation = transform.rotation.eulerAngles.y*Mathf.Deg2Rad;
                    //Debug.Log(rotation);
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, width, height, rotation, paintColor);
                }
            }
        }

    }

}
