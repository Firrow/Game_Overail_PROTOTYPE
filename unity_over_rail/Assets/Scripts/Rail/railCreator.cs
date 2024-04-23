using UnityEngine;

public class RailCreator : MonoBehaviour
{
    [SerializeField]
    private Transform[] controlPoints;

    private Vector3 gizmosPosition;

    //UTILE UNIQUEMENT POUR AFFICHER LES COURBES LORS DE LEUR PLACEMENT SUR LES TUILES
    private void OnDrawGizmos()
    {
        /* formule pour courbe de Bezier cubique :
        - la formule cr�er une courbe entre les p1 et p4
        - p2 li� � p1 et p3 li� � p4 permettent de modeler la forme du chemin*/
        for (float t = 0; t <= 1; t += 0.05f)
        {
            gizmosPosition = Mathf.Pow(1 - t, 3) * controlPoints[0].position + 
                              3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1].position + 
                              3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2].position +
                              Mathf.Pow(t, 3) * controlPoints[3].position;

            Gizmos.DrawSphere(gizmosPosition, 0.25f);
        }
        
        // dessin de la courbe
        Gizmos.DrawLine(new Vector3(controlPoints[0].position.x, controlPoints[0].position.y, controlPoints[0].position.z), 
            new Vector3(controlPoints[1].position.x, controlPoints[1].position.y, controlPoints[1].position.z));
        Gizmos.DrawLine(new Vector3(controlPoints[2].position.x, controlPoints[2].position.y, controlPoints[2].position.z), 
            new Vector3(controlPoints[3].position.x, controlPoints[3].position.y, controlPoints[3].position.z));
    }
}
