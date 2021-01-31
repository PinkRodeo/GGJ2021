using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBehavior : MonoBehaviour
{
    public static readonly Vector3 BASE_NORMAL = new Vector3(0, 0, 1);

    private Vector3 center = new Vector3();
    public Vector3 normal = BASE_NORMAL;

    public float radius = 10.0f;
    public float speed = 1.0f;


    private float currentAngle = 0.0f;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        center = transform.position;
        rotation = Quaternion.FromToRotation(BASE_NORMAL, normal);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentAngle();
        UpdatePositionOnOrbit();
    }

    void UpdateCurrentAngle()
    {
        float angularSpeed = speed / (radius);
        currentAngle += Time.deltaTime * angularSpeed;
    }

    void UpdatePositionOnOrbit()
    {
        Vector3 offset = new Vector3(
            Mathf.Cos(currentAngle) * radius,
            Mathf.Sin(currentAngle) * radius,
            0
        );

        offset = rotation * offset;

        gameObject.transform.position = center + offset;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (UnityEditor.EditorApplication.isPlaying)
            return;

        center = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, center + normal * 3.5f);

        Quaternion rotation = Quaternion.FromToRotation(new Vector3(0, 0, 1), normal);

        const int STEPS = 16;

        for (int ii = 1; ii <= STEPS; ii++)
        {
            Vector3 current = center + rotation * new Vector3(
                Mathf.Cos((Mathf.PI * 2 / STEPS) * ii) * radius,
                Mathf.Sin((Mathf.PI * 2 / STEPS) * ii) * radius,
                0
            );

            Vector3 prev = center + rotation * new Vector3(
               Mathf.Cos((Mathf.PI * 2 / STEPS) * (ii - 1)) * radius,
               Mathf.Sin((Mathf.PI * 2 / STEPS) * (ii - 1)) * radius,
               0
            );


            Gizmos.DrawLine(prev, current);
        }
    }
#endif

}
