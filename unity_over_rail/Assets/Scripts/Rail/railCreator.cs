using UnityEngine;

/// <summary>
/// Script to design rails on tiles
/// Using by prefab of simple roads
/// </summary>

public class RailCreator : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;
    private Vector3 gizmosPosition;



    /// <summary>
    /// formula for cubic Bezier curve :
    ///    - the formula creates a curve between p1 and p4
    ///    - p2 linked to p1 and p3 linked to p4 to shape the path
    /// </summary>
    private void OnDrawGizmos()
    {
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 
                              3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 
                              3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                              Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }
        
        // draw the curve
        Gizmos.DrawLine(new Vector3(controlPoints[0].position.x, controlPoints[0].position.y, controlPoints[0].position.z), 
            new Vector3(controlPoints[1].position.x, controlPoints[1].position.y, controlPoints[1].position.z));
        Gizmos.DrawLine(new Vector3(controlPoints[2].position.x, controlPoints[2].position.y, controlPoints[2].position.z), 
            new Vector3(controlPoints[3].position.x, controlPoints[3].position.y, controlPoints[3].position.z));
    }
}
