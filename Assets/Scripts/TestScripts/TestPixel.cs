using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPixel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down, Color.red);

        if (Physics.Raycast(ray, out hit, 5f))
        {
            Renderer rend = hit.transform.GetComponent<Renderer>();
            MeshCollider meshCollider = hit.collider as MeshCollider;

            if (rend == null /*|| rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null */ || meshCollider == null)
            {
                return;
            }
            else
            {
                Texture2D tex = rend.material.mainTexture as Texture2D;
                Vector2 pixelUV = hit.textureCoord;

                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;

                Debug.Log(tex.GetPixel((int)pixelUV.x, (int)pixelUV.y).r);
            }

        }
    }
}
