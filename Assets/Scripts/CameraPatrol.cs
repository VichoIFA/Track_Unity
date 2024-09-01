using UnityEngine;
using System.Collections.Generic;

public class CameraPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypoint = 0;
    private float targetProgress = 0f;
    public float dampeningRotation = 10f;
    public float moveSpeed = 1f;
    public int resolution = 20;

    private List<Vector3> splinePoints = new List<Vector3>();

    void Start()
    {
        // Establecer la posición inicial de la cámara en el primer waypoint
        if (waypoints.Length > 0)
            transform.position = waypoints[currentWaypoint].position;

        CalculateSplinePoints();
    }

    void Update()
    {
        // Mover la cámara suavemente a lo largo de la spline de Catmull-Rom
        if (splinePoints.Count > 1)
        {
            targetProgress += moveSpeed * Time.deltaTime;
            if (targetProgress >= 1f)
            {
                targetProgress = 0f;
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                    currentWaypoint = 0;

                CalculateSplinePoints();
            }

            int currentIndex = Mathf.FloorToInt(targetProgress * (splinePoints.Count - 1));
            int nextIndex = (currentIndex + 1) % splinePoints.Count;
            float lerp = targetProgress * (splinePoints.Count - 1) - currentIndex;

            Vector3 nextPosition = Vector3.Lerp(splinePoints[currentIndex], splinePoints[nextIndex], lerp);
            transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * dampeningRotation);
            transform.LookAt(nextPosition);
        }
    }

    void CalculateSplinePoints()
    {
        splinePoints = CalculateCatmullRomSplinePoints();
    }

    List<Vector3> CalculateCatmullRomSplinePoints()
    {
        List<Vector3> curvePoints = new List<Vector3>();

        for (int pointIndex = 0; pointIndex < waypoints.Length; pointIndex++)
        {
            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                int p0Index = (pointIndex - 1 + waypoints.Length) % waypoints.Length;
                int p1Index = pointIndex;
                int p2Index = (pointIndex + 1) % waypoints.Length;
                int p3Index = (pointIndex + 2) % waypoints.Length;

                Vector3 p0 = waypoints[p0Index].position;
                Vector3 p1 = waypoints[p1Index].position;
                Vector3 p2 = waypoints[p2Index].position;
                Vector3 p3 = waypoints[p3Index].position;

                Vector3 position = CalculateCatmullRomPosition(t, p0, p1, p2, p3);
                curvePoints.Add(position);
            }
        }

        return curvePoints;
    }

    Vector3 CalculateCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 part1 = -p0 + 3f * p1 - 3f * p2 + p3;
        Vector3 part2 = 2f * p0 - 5f * p1 + 4f * p2 - p3;
        Vector3 part3 = -p0 + p2;
        Vector3 part4 = 2f * p1;

        return 0.5f * (part1 * t3 + part2 * t2 + part3 * t + part4);
    }

    void OnDrawGizmos()
    {
        DrawSplineGizmo(false);
    }

    void OnDrawGizmosSelected()
    {
        DrawSplineGizmo(true);
    }

    void DrawSplineGizmo(bool selected)
    {
        if (waypoints == null || waypoints.Length < 2)
            return;

        Gizmos.color = selected ? Color.yellow : Color.gray;

        for (int i = 0; i < splinePoints.Count - 1; i++)
        {
            Gizmos.DrawLine(splinePoints[i], splinePoints[i + 1]);
        }
    }

    void OnValidate()
    {
        CalculateSplinePoints();
    }
}