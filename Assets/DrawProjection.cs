using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawProjection : MonoBehaviour
{
    GranadeController granadeController;
    LineRenderer lineRenderer;

    // Number of points on the line
    public int numPoints = 50;

    // distance between those points on the line
    public float timeBetweenPoints = 0.01f;

    // The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    void Start()
    {
        granadeController = GetComponent<GranadeController>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    

    public void Update()
    {
        if(ActiveWeapon.playerState == ActiveWeapon.PlayerState.throwingGranade)
        {
            lineRenderer.positionCount = (int)numPoints;
            List<Vector3> points = new List<Vector3>();
            Vector3 startingPosition = granadeController.ShotPoint.position;
            Vector3 startingVelocity = granadeController.ShotPoint.up * granadeController.BlastPower;
            for (float t = 0; t < numPoints; t += timeBetweenPoints)
            {
                Vector3 newPoint = startingPosition + t * startingVelocity;
                newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 2, CollidableLayers).Length > 0)
                {
                    lineRenderer.positionCount = points.Count;
                    break;
                }
            }

            lineRenderer.SetPositions(points.ToArray());
        }

    }
}
