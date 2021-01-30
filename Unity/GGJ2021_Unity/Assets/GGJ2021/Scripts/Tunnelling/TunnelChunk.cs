using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelChunk : MonoBehaviour
{

    public GameObject radiusLine;
    public GameObject lengthLine;

    public int currentLengthSegment
    {
        get
        {
            return _currentLengthSegment;
        }
    }

    private bool _isFinalized = false;

    private int _currentLengthSegment = 0;

    private int _radialSegmentCount;
    private int _lengthSegmentCount;

    public LineRenderer[] radiusLines;
    public LineRenderer[] lengthLines;

    private TunnelPoint[] _tunnelPoints;

    // Orientation/heading for the starting point of the circle drawing
    private Vector3 _upDirection = Vector3.left;

    public float forwardOffset = 3f;

    public void Initialize(int radialSegmentCount, int lengthSegmentCount)
    {
        _radialSegmentCount = radialSegmentCount;
        _lengthSegmentCount = lengthSegmentCount;

        radiusLines = new LineRenderer[_lengthSegmentCount];
        for (int i = 0; i < _lengthSegmentCount; i++)
        {
            var obj = Instantiate(radiusLine, Vector3.zero, Quaternion.identity, transform);

            var lineRenderer = obj.GetComponent<LineRenderer>();

            if (lineRenderer == null)
            {
                Debug.LogError("radiusLine didn't have a LineRenderer!");
                break;
            }
            lineRenderer.positionCount = radialSegmentCount;
            lineRenderer.loop = true;

            radiusLines[i] = lineRenderer;
        }

        lengthLines = new LineRenderer[_radialSegmentCount];

        for (int i = 0; i < _radialSegmentCount; i++)
        {
            var obj = Instantiate(lengthLine, Vector3.zero, Quaternion.identity, transform);

            var lineRenderer = obj.GetComponent<LineRenderer>();

            if (lineRenderer == null)
            {
                Debug.LogError("lengthLine didn't have a LineRenderer!");
                break;
            }
            lineRenderer.positionCount = 0;

            lengthLines[i] = lineRenderer;
        }

        _tunnelPoints = new TunnelPoint[_lengthSegmentCount];
    }


    private void UpdateLineRenderers()
    {
        int lineSegmentsToDraw = _currentLengthSegment;

        if (_isFinalized)
        {
            // Remove budget because we're not drawing to the "final" point, that final point is only used to calculate the
            // direction of the last segment you see
            lineSegmentsToDraw -= 1;
        }
        else
        {
            // Budget to draw to the "active" circle around the player
            // lineSegmentsToDraw += 1;
        }


        for (int i = 0; i < _radialSegmentCount; i++)
        {


            lengthLines[i].positionCount = lineSegmentsToDraw;
        }

        for (int i = 0; i < lineSegmentsToDraw; i++)
        {
            TunnelPoint tunnelPoint = _tunnelPoints[i];

            float radius = tunnelPoint.radius;
            Vector3 center = tunnelPoint.position;

            float stepSize = 2f * Mathf.PI / _radialSegmentCount;

            // Vector3 direction = Vector3.forward;
            Quaternion rotator;

            if (i < _currentLengthSegment - 1)
            {

                // project to next point
                var nextDirection = (tunnelPoint.position - _tunnelPoints[i + 1].position).normalized;

                center = Vector3.Lerp(center, _tunnelPoints[i + 1].position, 0.5f);

                rotator = Quaternion.LookRotation(nextDirection, _upDirection);
            }
            else
            {
                // TODO: Get heading of player

                if (!_isFinalized)
                {

                    var nextDirection = (tunnelPoint.position - TunnelTracer.activeTunnelTracer.transform.position).normalized;

                    center = Vector3.Lerp(center, TunnelTracer.activeTunnelTracer.transform.position, 0.5f);

                    if (nextDirection.magnitude < .5f)
                    {
                        nextDirection = TunnelTracer.activeTunnelTracer.currentDirection;
                    }

                    rotator = Quaternion.LookRotation(nextDirection, _upDirection);

                    // var nextPosition = TunnelTracer.activeTunnelTracer.transform.position;

                    // var nextDirection = (tunnelPoint.position - nextPosition).normalized;
                    // center = Vector3.Lerp(center, nextPosition, 0.5f);
                    // rotator = Quaternion.LookRotation(nextDirection, _upDirection);
                    rotator = Quaternion.identity;

                }
                else
                {
                    Debug.LogWarning("This code should never run, pls ask Steff");
                    Vector3 previousDirection;
                    if (i > 0)
                    {
                        previousDirection = (tunnelPoint.position - _tunnelPoints[i - 1].position).normalized;
                    }
                    else
                    {
                        previousDirection = Vector3.forward;
                    }

                    rotator = Quaternion.LookRotation(previousDirection, _upDirection);
                }

            }

            for (int j = 0; j < _radialSegmentCount; j++)
            {
                Vector3 vertex = new Vector3(
                    radius * Mathf.Cos(stepSize * j),
                    radius * Mathf.Sin(stepSize * j),
                    0f
                );

                vertex = rotator * vertex;
                vertex += center;

                radiusLines[i].SetPosition(j, vertex);
                lengthLines[j].SetPosition(i, vertex);
            }
        }

        Update();
    }

    private void Update()
    {
        if (_isFinalized)
        {
            return;
        }

        var tunnelPoint = _tunnelPoints[_currentLengthSegment - 1];

        float radius = tunnelPoint.radius;
        Vector3 center = tunnelPoint.position;

        float stepSize = 2f * Mathf.PI / _radialSegmentCount;

        var nextPosition = TunnelTracer.activeTunnelTracer.transform.position;
        var nextDirection = (tunnelPoint.position - nextPosition).normalized;

        if (nextDirection.magnitude < .5f)
        {
            nextDirection = TunnelTracer.activeTunnelTracer.currentDirection;
        }

        center = nextPosition + TunnelTracer.activeTunnelTracer.currentDirection * forwardOffset;
        Quaternion rotator = Quaternion.LookRotation(nextDirection, _upDirection);

        for (int j = 0; j < _radialSegmentCount; j++)
        {
            Vector3 vertex = new Vector3(
                radius * Mathf.Cos(stepSize * j),
                radius * Mathf.Sin(stepSize * j),
                0f
            );

            vertex = rotator * vertex;
            vertex += center;

            radiusLines[_currentLengthSegment - 1].SetPosition(j, vertex);
            lengthLines[j].SetPosition(_currentLengthSegment - 1, vertex);
        }

    }

    public void AddPoint(TunnelPoint tunnelPoint)
    {
        if (!CanAddMorePoints())
        {
            Debug.LogError("Tried adding point while chunk is full");
            return;
        }

        _tunnelPoints[_currentLengthSegment] = tunnelPoint;

        _currentLengthSegment += 1;

        UpdateLineRenderers();
    }

    public void AddCapPoint(TunnelPoint tunnelPoint)
    {
        _tunnelPoints[_currentLengthSegment] = tunnelPoint;

        _currentLengthSegment += 1;

        UpdateLineRenderers();
    }

    public void AddFirstPointOfNextChunk(TunnelPoint tunnelPoint)
    {
        _tunnelPoints[_currentLengthSegment] = tunnelPoint;

        _currentLengthSegment += 1;
        _isFinalized = true;

        UpdateLineRenderers();
    }


    public bool CanAddMorePoints()
    {
        return _currentLengthSegment + 2 < _lengthSegmentCount;
    }

}
