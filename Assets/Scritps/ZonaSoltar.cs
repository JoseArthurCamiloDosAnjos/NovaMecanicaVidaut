using UnityEngine;

public class ZonaSoltar : MonoBehaviour
{
    [Tooltip("Tipo de item aceito (opcional)")]
    public string tipoAceito = "Padrao";

    private void OnDrawGizmos()
    {
        // Mostra a área no editor
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}
