using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Shapes;
using System;

public class TunnelDrawer : MonoBehaviour
{

    private TunnelContainer _tunnelContainer;

    private PolylinePath[] _cylinderCircleLines = new PolylinePath[0];
    private PolylinePath[] _cylinderEdgeLines = new PolylinePath[0];

    public int cylinderFaceCount = 6;

    public float cylinderLineThickness = 1f;

    // Start is called before the first frame update
    void Start()
    {
        RenderPipelineManager.endFrameRendering += OnEndFrameRendering;
        _tunnelContainer = GetComponent<TunnelContainer>();

        if (_tunnelContainer == null)
        {
            Debug.LogError("Couldn't find a TunnelContainer to draw");
            return;
        }


        CleanupShapes();

        _tunnelContainer.onTunnelUpdated += OnTunnelUpdated;
    }

    private void OnTunnelUpdated()
    {
        CleanupShapes();

        var tunnelPoints = _tunnelContainer.tunnelPoints;

        PolylinePath[] cylinderCircleLines = new PolylinePath[tunnelPoints.Count - 1];

        Vector3 upDirection = Vector3.up;

        int faceCount = cylinderFaceCount;

        Vector3[] previousCylinder = new Vector3[faceCount];

        var cylinderEdgeLines = new PolylinePath[faceCount];
        for (int i = 0; i < faceCount; i++)
        {
            cylinderEdgeLines[i] = new PolylinePath();
        }

        for (int i = 0; i < tunnelPoints.Count - 1; i++)
        {
            TunnelPoint tunnelPoint = tunnelPoints[i];
            var circleLine = new PolylinePath();

            float radius = tunnelPoint.radius;
            Vector3 center = tunnelPoint.position;

            float stepSize = 2f * Mathf.PI / faceCount;

            // Vector3 direction = Vector3.forward;
            Quaternion rotator;

            if (i < tunnelPoints.Count - 1)
            {
                // project to next point
                var nextDirection = (tunnelPoint.position - tunnelPoints[i + 1].position).normalized;

                center = Vector3.Lerp(center, tunnelPoints[i + 1].position, 0.5f);

                rotator = Quaternion.LookRotation(nextDirection, upDirection);
            }
            else
            {
                Vector3 previousDirection;
                if (i > 0)
                {
                    previousDirection = (tunnelPoint.position - tunnelPoints[i - 1].position).normalized;
                }
                else
                {
                    previousDirection = Vector3.forward;
                }

                rotator = Quaternion.LookRotation(previousDirection, upDirection);
            }

            for (int j = 0; j < faceCount; j++)
            {
                Vector3 vertex = new Vector3(
                    radius * Mathf.Cos(stepSize * j),
                    radius * Mathf.Sin(stepSize * j),
                    0f
                );

                vertex = rotator * vertex;
                vertex += center;

                previousCylinder[j] = vertex;

                circleLine.AddPoint(vertex);

                cylinderEdgeLines[j].AddPoint(vertex);
            }

            cylinderCircleLines[i] = circleLine;
        }

        _cylinderCircleLines = cylinderCircleLines;
        _cylinderEdgeLines = cylinderEdgeLines;
    }

    private void CleanupShapes()
    {
        foreach (var p in _cylinderCircleLines)
        {
            p.Dispose();
        }
        _cylinderCircleLines = new PolylinePath[0];

        foreach (var p in _cylinderEdgeLines)
        {
            p.Dispose();
        }

        _cylinderEdgeLines = new PolylinePath[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEndFrameRendering(ScriptableRenderContext arg1, Camera[] arg2)
    {
        float thickness = cylinderLineThickness;

        foreach (var p in _cylinderCircleLines)
        {
            Draw.Polyline(ShapesBlendMode.Transparent, p, true, PolylineGeometry.Billboard, PolylineJoins.Round, thickness, ThicknessSpace.Pixels, Color.white);
        }

        for (int i = 0; i < _cylinderEdgeLines.Length; i++)
        {
            Draw.Polyline(ShapesBlendMode.Transparent, _cylinderEdgeLines[i], false, PolylineGeometry.Billboard, PolylineJoins.Round, thickness * (i + 1), ThicknessSpace.Pixels, Color.white);
        }
    }


    private void OnDestroy()
    {
        CleanupShapes();
    }
}
