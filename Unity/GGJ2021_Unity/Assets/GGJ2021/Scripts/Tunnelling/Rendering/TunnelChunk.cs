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

    private int _radiusSegmentCountMax;
    private int _lengthSegmentCountMax;

    // Radius lines are the tunnel circles that run perpendicular to the path of the tunnel
    public LineRenderer[] radiusLines;

    // Length lines are the tunnel edges that run alongside the path of the tunnel
    public LineRenderer[] lengthLines;

    private TunnelPoint[] _tunnelPoints;

    // Orientation/heading for the starting point of the circle drawing
    private Vector3 _upDirection = Vector3.left;

    // Amount by which the tunnel gets drawn ahead of the player
    public float forwardOffset = 3f;

    private TunnelPlacer _parent;

    public void Awake()
    {
        this.enabled = false;
        _tunnelPoints = new TunnelPoint[0];
    }

    public void Initialize(TunnelPlacer parent, int radiusSegmentCountMax, int lengthSegmentCountMax)
    {
        this.enabled = true;
        _parent = parent;

        _radiusSegmentCountMax = radiusSegmentCountMax;
        _lengthSegmentCountMax = lengthSegmentCountMax;

        radiusLines = new LineRenderer[_lengthSegmentCountMax];
        for (int i = 0; i < _lengthSegmentCountMax; i++)
        {
            var obj = Instantiate(radiusLine, Vector3.zero, Quaternion.identity, transform);

            var lineRenderer = obj.GetComponent<LineRenderer>();

            if (lineRenderer == null)
            {
                Debug.LogError("radiusLine didn't have a LineRenderer!");
                break;
            }
            lineRenderer.positionCount = radiusSegmentCountMax;
            lineRenderer.loop = true;

            radiusLines[i] = lineRenderer;
        }

        lengthLines = new LineRenderer[_radiusSegmentCountMax];

        for (int i = 0; i < _radiusSegmentCountMax; i++)
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

        _tunnelPoints = new TunnelPoint[_lengthSegmentCountMax];
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

        // Set the line buffer sizes
        for (int i = 0; i < _radiusSegmentCountMax; i++)
        {
            lengthLines[i].positionCount = lineSegmentsToDraw;
        }

        var tunnelTracer = _parent.tunnelData.tunnelTracer;

        for (int i = 0; i < lineSegmentsToDraw; i++)
        {
            TunnelPoint tunnelPoint = _tunnelPoints[i];

            float radius = tunnelPoint.radius;
            Vector3 center = tunnelPoint.position;

            float stepSize = 2f * Mathf.PI / _radiusSegmentCountMax;

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
                if (!_isFinalized)
                {
                    // This all gets overwritten instantly when drawing the foremost tunnel lines ahead of the player
                    var nextDirection = (tunnelPoint.position - tunnelTracer.transform.position).normalized;

                    center = Vector3.Lerp(center, tunnelTracer.transform.position, 0.5f);

                    if (nextDirection.magnitude < .5f)
                    {
                        nextDirection = tunnelTracer.currentDirection;
                    }

                    rotator = Quaternion.LookRotation(nextDirection, _upDirection);

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

            for (int j = 0; j < _radiusSegmentCountMax; j++)
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

        if (_currentLengthSegment < 1)
        {
            return;
        }
        var tunnelTracer = _parent.tunnelData.tunnelTracer;

        var tunnelPoint = _tunnelPoints[_currentLengthSegment - 1];

        float radius = tunnelPoint.radius;
        Vector3 center = tunnelPoint.position;

        float stepSize = 2f * Mathf.PI / _radiusSegmentCountMax;

        var nextPosition = tunnelTracer.transform.position;
        var nextDirection = (tunnelPoint.position - nextPosition).normalized;

        if (nextDirection.magnitude < .5f)
        {
            nextDirection = tunnelTracer.currentDirection;
        }

        center = nextPosition + tunnelTracer.currentDirection * forwardOffset;
        Quaternion rotator = Quaternion.LookRotation(nextDirection, _upDirection);

        for (int j = 0; j < _radiusSegmentCountMax; j++)
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
        // Reserve 2 segments
        // 1 is for drawing a segment ahead of the player
        // 2 is for storing the point after this chunk, which won't be rendered but the last shown segment still needs to point towards it
        return _currentLengthSegment + 2 < _lengthSegmentCountMax;
    }
}
