using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridInstancer : MonoBehaviour
{

    public GameObject objectToInstance;

    public int gridSize = 50;
    public float gridSpacing = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        UpdateDrawlist();
    }

    private void UpdateDrawlist()
    {
        float heightOffset = transform.position.y;

        float x_orig = transform.position.x;
        float z_orig = transform.position.z;

        var neutralRotation = Quaternion.identity;
        var neutralScale = Vector3.one;
        var currentTransform = transform;

        float offset = -gridSize * gridSpacing * 0.5f;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(
                    x_orig + offset + x * gridSpacing,
                    heightOffset,
                    z_orig + offset + z * gridSpacing);

                Instantiate(objectToInstance, position, neutralRotation, currentTransform);
            }
        }
    }
}
