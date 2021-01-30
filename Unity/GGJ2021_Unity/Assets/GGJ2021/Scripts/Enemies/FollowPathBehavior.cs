using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathBehavior : MonoBehaviour
{
    public Vector3[] Points = new Vector3[0];
    public float speed = 1.0f;

    private int lastIndex = 0;
    private int nextIndex = 0;
    private Vector3 diff;
    private float progress;


    // Start is called before the first frame update
    void Start()
    {
        lastIndex = Random.Range(0, Points.Length);
        nextIndex = (lastIndex + 1) % Points.Length;
        diff = Points[nextIndex] - Points[lastIndex];

        progress = Random.value * 0.999f * progress; //Random.value is including 1.0, and we don't want that.

        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Points.Length <= 1) return;

        UpdateProgress();
        UpdatePosition();
    }

    void UpdateProgress()
    {
        progress += speed * Time.deltaTime;
        float factor = progress / diff.magnitude;

        if (factor < 1) return;

        progress -= diff.magnitude;

        lastIndex = (lastIndex + 1) % Points.Length;
        nextIndex = (lastIndex + 1) % Points.Length;

        diff = Points[nextIndex] - Points[lastIndex];
    }

    void UpdatePosition()
    {
        float factor = progress / diff.magnitude;
        gameObject.transform.position = Points[lastIndex] + diff * factor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int ii = 0; ii < Points.Length; ii++)
        {
            Gizmos.DrawSphere(Points[ii], 0.3f);
        }

        for (int ii = 0; ii < Points.Length; ii++)
        {
            Vector3 current = Points[ii];
            Vector3 next = Points[(ii + 1) % Points.Length];

            Gizmos.DrawLine(current, next);

        }
    }
}
