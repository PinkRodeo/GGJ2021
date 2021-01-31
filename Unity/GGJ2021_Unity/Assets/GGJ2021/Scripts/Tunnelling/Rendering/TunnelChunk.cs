using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelChunk : MonoBehaviour
{

    public GameObject radiusLine;
    public GameObject lengthLine;



    private bool _isFinalized = false;

    private int _currentLengthSegment = 0;

    public int currentLengthSegment
    {
        get
        {
            return _currentLengthSegment;
        }
    }

    private int _radiusSegmentCount;
    private int _lengthSegmentCountMax;

    // Radius lines are the tunnel circles that run perpendicular to the path of the tunnel
    public LineRenderer[] radiusLines;
    private Vector3[][] _radiusLineVertices;

    // Length lines are the tunnel edges that run alongside the path of the tunnel
    public LineRenderer[] lengthLines;
    private Vector3[][] _lengthLineVertices;

    private TunnelPoint[] _tunnelPoints;

    public TunnelChunk previousChunk;
    public TunnelChunk nextChunk;

    public TunnelPoint[] tunnelPoints
    {
        get
        {
            return _tunnelPoints;
        }
    }

    // Orientation/heading for the starting point of the circle drawing
    private Vector3 _upDirection = Vector3.left;

    // Amount by which the tunnel gets drawn ahead of the player
    public float forwardOffset = 3f;

    private TunnelPlacer _parent;

    public TunnelPlacer parent
    {
        get
        {
            return _parent;
        }
    }

    public void Awake()
    {
        this.enabled = false;
        _tunnelPoints = new TunnelPoint[0];
    }

    public void Initialize(TunnelPlacer parent, int radiusSegmentCount, int lengthSegmentCountMax)
    {
        this.enabled = true;
        _parent = parent;

        _radiusSegmentCount = radiusSegmentCount;
        _lengthSegmentCountMax = lengthSegmentCountMax;

        radiusLines = new LineRenderer[_lengthSegmentCountMax];
        _radiusLineVertices = new Vector3[_lengthSegmentCountMax][];
        for (int i = 0; i < _lengthSegmentCountMax; i++)
        {
            var obj = Instantiate(radiusLine, Vector3.zero, Quaternion.identity, transform);

            var lineRenderer = obj.GetComponent<LineRenderer>();

            if (lineRenderer == null)
            {
                Debug.LogError("radiusLine didn't have a LineRenderer!");
                break;
            }
            lineRenderer.positionCount = radiusSegmentCount;
            _radiusLineVertices[i] = new Vector3[_radiusSegmentCount];

            lineRenderer.loop = true;

            radiusLines[i] = lineRenderer;
        }

        lengthLines = new LineRenderer[_radiusSegmentCount];
        _lengthLineVertices = new Vector3[_radiusSegmentCount][];

        for (int i = 0; i < _radiusSegmentCount; i++)
        {
            var obj = Instantiate(lengthLine, Vector3.zero, Quaternion.identity, transform);

            var lineRenderer = obj.GetComponent<LineRenderer>();

            if (lineRenderer == null)
            {
                Debug.LogError("lengthLine didn't have a LineRenderer!");
                break;
            }
            lineRenderer.positionCount = 0;
            _lengthLineVertices[i] = new Vector3[_radiusSegmentCount];

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
        for (int i = 0; i < _radiusSegmentCount; i++)
        {
            lengthLines[i].positionCount = lineSegmentsToDraw;
            _lengthLineVertices[i] = new Vector3[lineSegmentsToDraw];
        }

        var tunnelTracer = _parent.tunnelData.tunnelTracer;

        float stepSize = 2f * Mathf.PI / _radiusSegmentCount;

        for (int lineCounter = 0; lineCounter < lineSegmentsToDraw; lineCounter++)
        {
            TunnelPoint tunnelPoint = _tunnelPoints[lineCounter];

            float radius = tunnelPoint.radius;
            Vector3 center = tunnelPoint.position;

            Quaternion rotator;

            if (lineCounter < _currentLengthSegment - 1)
            {
                // project to next point
                var previousPosition = _tunnelPoints[lineCounter + 1].position;

                var nextDirection = (tunnelPoint.position - previousPosition).normalized;

                center = Vector3.Lerp(center, previousPosition, 0.5f);

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
                    if (lineCounter > 0)
                    {
                        previousDirection = (tunnelPoint.position - _tunnelPoints[lineCounter - 1].position).normalized;
                    }
                    else
                    {
                        previousDirection = Vector3.forward;
                    }

                    rotator = Quaternion.LookRotation(previousDirection, _upDirection);
                }

            }

            for (int radiusCounter = 0; radiusCounter < _radiusSegmentCount; radiusCounter++)
            {
                Vector3 vertex = new Vector3(
                    radius * Mathf.Cos(stepSize * radiusCounter),
                    radius * Mathf.Sin(stepSize * radiusCounter),
                    0f
                );

                vertex = (rotator * vertex) + center;

                _radiusLineVertices[lineCounter][radiusCounter] = vertex;
                _lengthLineVertices[radiusCounter][lineCounter] = vertex;
            }

            for (int radiusCounter = 0; radiusCounter < _radiusSegmentCount; radiusCounter++)
            {
                radiusLines[lineCounter].SetPositions(_radiusLineVertices[lineCounter]);
            }
        }

        for (int radiusCounter = 0; radiusCounter < _radiusSegmentCount; radiusCounter++)
        {
            lengthLines[radiusCounter].SetPositions(_lengthLineVertices[radiusCounter]);
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

        float stepSize = 2f * Mathf.PI / _radiusSegmentCount;

        var nextPosition = tunnelTracer.transform.position;
        var nextDirection = (tunnelPoint.position - nextPosition).normalized;

        if (nextDirection.magnitude < .5f)
        {
            nextDirection = tunnelTracer.currentDirection;
        }

        if (nextDirection.magnitude < .5f)
        {
            nextDirection = Vector3.up;
        }

        center = nextPosition + tunnelTracer.currentDirection * forwardOffset;
        Quaternion rotator = Quaternion.LookRotation(nextDirection, _upDirection);

        for (int j = 0; j < _radiusSegmentCount; j++)
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

    public void FinishChunk()
    {
        TunnelWorld.instance.AddChunk(this);

        _radiusLineVertices = new Vector3[0][];
        _lengthLineVertices = new Vector3[0][];

        this.enabled = false;
    }

    public Bounds GetBounds()
    {
        var returnValue = new Bounds(_tunnelPoints[0].position, Vector3.one);

        for (int i = 0; i < _currentLengthSegment; i++)
        {
            returnValue.Encapsulate(_tunnelPoints[i].position);
        }

        return returnValue;
    }

    public void DoDebugDraw(float Duration)
    {
        Vector3 previousPosition = Vector3.zero;

        for (int i = 0; i < _currentLengthSegment; i++)
        {
            if (i == 0)
            {
                previousPosition = _tunnelPoints[i].position;
                continue;
            }

            Debug.DrawLine(_tunnelPoints[i].position, previousPosition, Color.green, Duration);

            previousPosition = _tunnelPoints[i].position;
        }
    }
}
