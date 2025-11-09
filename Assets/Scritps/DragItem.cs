using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class DragItem : MonoBehaviour
{
    private bool podeArrastar = false;
    private bool concluido = false;
    private Vector3 offset;
    private Camera cam;
    private Vector3 posicaoInicial;

    public Rigidbody2D rb2;
    private GameObject destinoEsperado;
    private bool dentroDoDestino = false;

    [Tooltip("Se verdadeiro, o objeto será fixado como filho do destino ao encaixar")]
    public bool fixarNoDestino = true;

    public static event Action<GameObject> OnObjetoArrastadoCorretamente;

    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        // Mantém a gravidade normal no início
    }

    void Start()
    {
        cam = Camera.main;
        posicaoInicial = transform.position;
    }

    public void HabilitarArraste(bool estado, GameObject destino)
    {
        rb2.simulated = true;
        rb2.isKinematic = false;
        rb2.gravityScale = 1f; // volta a ter gravidade
        podeArrastar = estado;
        destinoEsperado = destino;
        concluido = false;
    }

    void OnMouseDown()
    {
        if (!podeArrastar || concluido) return;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePos.x, mousePos.y, 0);
    }

    void OnMouseDrag()
    {
        if (!podeArrastar || concluido) return;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, 0) + offset;
    }

    void OnMouseUp()
    {
        if (!podeArrastar || concluido) return;

        if (dentroDoDestino && destinoEsperado != null)
        {
            // Desliga a física imediatamente
            rb2.velocity = Vector2.zero;
            rb2.angularVelocity = 0f;
            rb2.gravityScale = 0f;   // desliga gravidade já no início
            rb2.isKinematic = true;  // impede qualquer força

            // Agora move suavemente até o centro
            StartCoroutine(MoverAteCentro(destinoEsperado.transform.position, () =>
            {
                concluido = true;

                if (fixarNoDestino)
                    transform.SetParent(destinoEsperado.transform);

                OnObjetoArrastadoCorretamente?.Invoke(gameObject);
            }));
        }
        else
        {
            StartCoroutine(MoverAteCentro(posicaoInicial));
        }
    }



    IEnumerator MoverAteCentro(Vector3 destino, Action onComplete = null)
    {
        Vector3 inicio = transform.position;
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * 6f;
            transform.position = Vector3.Lerp(inicio, destino, t);
            yield return null;
        }
        transform.position = destino;
        onComplete?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (destinoEsperado != null && other.gameObject == destinoEsperado)
            dentroDoDestino = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (destinoEsperado != null && other.gameObject == destinoEsperado)
            dentroDoDestino = false;
    }
}
