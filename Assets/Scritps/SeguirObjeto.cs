using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SeguirObjeto : MonoBehaviour
{
    public Transform alvoInicio;
    public Transform alvoFim;

    public bool usarArrastar = false;
    public bool loopArraste = false;
    public float tempoArrastar = 1f;
    public float pausaEmB = 0.25f;
    public float tempoRetornoParaA = 0.03f;

    public float amplitude = 0.5f;
    public float velocidadeSobeDesce = 1f;
    public Vector3 offset = new Vector3(0, 20f, 0);
    public float suavizacaoPosicao = 10f;

    private Coroutine arrasteCoroutine;
    private bool bloqueadoParaDesativar = false;

    // Pendências quando StartArrasteForce é chamado enquanto o componente está desativado
    private bool pendingStartArraste = false;
    private bool pendingLoop = false;

    void OnEnable()
    {
        if (alvoInicio != null)
            transform.position = alvoInicio.position + offset;

        if (pendingStartArraste)
        {
            pendingStartArraste = false;
            StartArrasteForce(pendingLoop);
        }
    }

    void Update()
    {
        if (alvoInicio == null) return;

        if (usarArrastar && alvoFim != null)
        {
            if (arrasteCoroutine == null)
            {
                Debug.Log("[SeguirObjeto] Iniciando coroutine de arraste A->B");
                arrasteCoroutine = StartCoroutine(LoopAparaB(alvoInicio, alvoFim));
            }
            return;
        }

        Vector3 alvoPos = alvoInicio.position + offset;
        float yOffset = Mathf.Sin(Time.time * velocidadeSobeDesce) * amplitude;
        Vector3 destino = alvoPos + new Vector3(0, yOffset, 0);
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * suavizacaoPosicao);
    }

    IEnumerator LoopAparaB(Transform inicioT, Transform fimT)
    {
        Debug.Log("[SeguirObjeto] LoopAparaB started");
        while (usarArrastar && inicioT != null && fimT != null)
        {
            Vector3 posA = inicioT.position + offset;
            if (Vector3.Distance(transform.position, posA) > 0.001f)
                transform.position = Vector3.Lerp(transform.position, posA, 0.5f);

            yield return StartCoroutine(AnimarDePara(inicioT, fimT, tempoArrastar));

            float tAcum = 0f;
            while (tAcum < pausaEmB)
            {
                if (fimT == null) break;
                tAcum += Time.deltaTime;
                Vector3 alvoPos = fimT.position + offset;
                float yOffset = Mathf.Sin(Time.time * velocidadeSobeDesce) * amplitude;
                transform.position = Vector3.Lerp(transform.position, alvoPos + new Vector3(0, yOffset, 0), Time.deltaTime * suavizacaoPosicao);
                yield return null;
            }

            if (!loopArraste) break;

            Vector3 destinoA = inicioT.position + offset;
            if (tempoRetornoParaA <= 0f)
            {
                transform.position = destinoA;
            }
            else
            {
                float t = 0f;
                Vector3 start = transform.position;
                while (t < tempoRetornoParaA)
                {
                    t += Time.deltaTime;
                    float p = Mathf.Clamp01(t / tempoRetornoParaA);
                    transform.position = Vector3.Lerp(start, destinoA, Mathf.SmoothStep(0f, 1f, p));
                    yield return null;
                }
                transform.position = destinoA;
            }

            yield return null;
        }

        arrasteCoroutine = null;
        Debug.Log("[SeguirObjeto] LoopAparaB ended");

        if (fimT != null && inicioT != null)
        {
            alvoInicio = fimT;
            alvoFim = null;
            usarArrastar = false;
        }
    }

    IEnumerator AnimarDePara(Transform fromT, Transform toT, float duracao)
    {
        if (fromT == null || toT == null) yield break;
        Vector3 start = fromT.position + offset;
        Vector3 end = toT.position + offset;
        float t = 0f;
        while (t < duracao)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duracao);
            float eased = Mathf.SmoothStep(0f, 1f, p);
            transform.position = Vector3.Lerp(start, end, eased);
            yield return null;
        }
        transform.position = end;
    }

    // DefinirAlvos com forceRestart
    public void DefinirAlvos(Transform inicio, Transform fim = null, bool arrastar = false, bool loop = false, bool forceRestart = false)
    {
        bool mesmaSituacao = (alvoInicio == inicio) && (alvoFim == fim) && (usarArrastar == arrastar) && (loopArraste == loop);

        if (mesmaSituacao && arrasteCoroutine != null && !forceRestart) return;

        bloqueadoParaDesativar = true;

        if (arrasteCoroutine != null)
        {
            StopCoroutine(arrasteCoroutine);
            arrasteCoroutine = null;
        }

        alvoInicio = inicio;
        alvoFim = fim;
        usarArrastar = arrastar && fim != null;
        loopArraste = loop;

        if (alvoInicio != null)
            transform.position = Vector3.Lerp(transform.position, alvoInicio.position + offset, 0.5f);

        StartCoroutine(LiberarBloqueioDesativacao());
    }

    // StartArrasteForce agora robusto: enfileira se componente estiver inativo; aceita loop
    public void StartArrasteForce(bool loop = false)
    {
        if (!this.isActiveAndEnabled)
        {
            pendingStartArraste = true;
            pendingLoop = loop;
            Debug.Log("[SeguirObjeto] StartArrasteForce enfileirado porque componente inativo (loop=" + loop + ")");
            return;
        }

        if (alvoInicio == null || alvoFim == null)
        {
            Debug.LogWarning("[SeguirObjeto] StartArrasteForce abortado: alvoInicio ou alvoFim nulos");
            return;
        }

        if (arrasteCoroutine != null)
        {
            StopCoroutine(arrasteCoroutine);
            arrasteCoroutine = null;
        }

        usarArrastar = true;
        loopArraste = loop;
        arrasteCoroutine = StartCoroutine(LoopAparaB(alvoInicio, alvoFim));
        Debug.Log("[SeguirObjeto] StartArrasteForce: coroutine iniciada (loop=" + loop + ")");
    }

    IEnumerator LiberarBloqueioDesativacao()
    {
        yield return null;
        bloqueadoParaDesativar = false;
    }

    public void PararEEsconder()
    {
        StopAllCoroutines();
        arrasteCoroutine = null;
        alvoInicio = null;
        alvoFim = null;
        usarArrastar = false;
        loopArraste = false;
        pendingStartArraste = false;
        pendingLoop = false;
    }

    public bool PodeSerDesativada()
    {
        return !bloqueadoParaDesativar && arrasteCoroutine == null;
    }
}
