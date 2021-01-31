
#if UNITY_EDITOR
#define DEBUGDRAW
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(BoxCollider))]
public class PickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    private BoxCollider _boxCollider;
    private Bounds _bounds;
    private List<GameObject> _activePickups = new List<GameObject>();

    public int waveBudget = 50;

    public int clusterMin = 3;
    public int clusterMax = 9;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();

        _bounds = _boxCollider.bounds;
        // _bounds.center = _bounds.center;

        Destroy(_boxCollider);
    }

    private bool SpawnPickup(Vector3 position)
    {
        if (!_bounds.Contains(position))
            return false;

        var overlappers = Physics.OverlapSphere(position, 4f);

        if (overlappers.Length > 1)
        {
            // There's something in the way, pick a new point
            return false;
        }

        var newPickup = Instantiate(pickupPrefab, position, Quaternion.identity, transform);

        _activePickups.Add(newPickup);

        return true;
    }

    private void RemovePickup(Pickup pickupToRemove)
    {
        _activePickups.Remove(pickupToRemove.gameObject);

        Destroy(pickupToRemove.gameObject);

        if (_activePickups.Count < 1)
        {
            SpawnPickupWave();
        }
    }

    private void SpawnPickupWave()
    {
        int timeOut = 200;

        while ((_activePickups.Count < waveBudget) && timeOut > 0)
        {
            timeOut -= 1;

            var startPoint = PickValidSpawnPoint();
            SpawnPickup(startPoint);

            int clusterSize = Random.Range(clusterMin, clusterMax);

            for (int i = 0; i < clusterSize; i++)
            {
                var previousPoint = startPoint;

                startPoint += Random.insideUnitSphere.normalized * 5f;

                int maxTries = 20;
                while (!SpawnPickup(startPoint) && maxTries > 0)
                {
                    maxTries--;
                    startPoint = previousPoint + Random.insideUnitSphere.normalized * 5f;
                }
            }
        }
    }

    private Vector3 PickValidSpawnPoint()
    {
        var bounds = _boxCollider.bounds;

        for (int i = 0; i < 10000; i++)
        {
            var potentialSpawnPoint = RandomPointInBounds(bounds);
            // DebugExtension.DebugPoint(potentialSpawnPoint, Color.green, 10f, 5f);

            var overlappers = Physics.OverlapSphere(potentialSpawnPoint, 4f);

            if (overlappers.Length > 1)
                continue;

            return potentialSpawnPoint;
        }

        Debug.LogError("Tried a 100 times to find a valid spawn point but couldn't");

        return Vector3.zero;
    }

    void Start()
    {
        SpawnPickupWave();
    }

    // Update is called once per frame
    void Update()
    {
        DebugExtension.DebugBounds(_bounds, Color.white, Time.deltaTime, false);

    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

}
