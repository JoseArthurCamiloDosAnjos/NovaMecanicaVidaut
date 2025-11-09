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
    public Vector3 offset = new Vector3(0, 1f, 0);
    public float suavizacaoPosicao = 10f;

    private Coroutine arrasteCoroutine;

    void Update()
    {
        if (alvoInicio == null) return;

        if (usarArrastar && alvoFim != null)
        {
            if (arrasteCoroutine == null)
                arrasteCoroutine = StartCoroutine(LoopAparaB(alvoInicio, alvoFim));
            return;
        }

        // Movimento normal (seguir alvoInicio com flutuação)
        Vector3 alvoPos = alvoInicio.position + offset;
        float yOffset = Mathf.Sin(Time.time * velocidadeSobeDesce) * amplitude;
        Vector3 destino = alvoPos + new Vector3(0, yOffset, 0);
        transform.position = Vector3.Lerp(transform.position, destino, Time.deltaTime * suavizacaoPosicao);
    }

    IEnumerator LoopAparaB(Transform inicioT, Transform fimT)
    {
        // anima de A até B
        yield return StartCoroutine(AnimarDePara(inicioT, fimT, tempoArrastar));

        // pausa em B
        float tAcum = 0f;
        while (tAcum < pausaEmB)
        {
            tAcum += Time.deltaTime;
            Vector3 alvoPos = fimT.position + offset;
            float yOffset = Mathf.Sin(Time.time * velocidadeSobeDesce) * amplitude;
            transform.position = Vector3.Lerp(transform.position, alvoPos + new Vector3(0, yOffset, 0), Time.deltaTime * suavizacaoPosicao);
            yield return null;
        }

        // se loopar, volta para A
        if (loopArraste)
        {
            yield return StartCoroutine(AnimarDePara(fimT, inicioT, tempoRetornoParaA));
            arrasteCoroutine = StartCoroutine(LoopAparaB(inicioT, fimT));
        }
        else
        {
            arrasteCoroutine = null;
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

    // 🔹 Método compatível com Banheiro_Minigame
    public void DefinirAlvos(Transform inicio, Transform fim = null, bool arrastar = false, bool loop = false, bool forceRestart = false)
    {
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
            transform.position = alvoInicio.position + offset;

        if (usarArrastar && alvoFim != null)
            arrasteCoroutine = StartCoroutine(LoopAparaB(alvoInicio, alvoFim));
    }

    // 🔹 Método compatível com Passo.cs
    public void SeguirTransform(Transform novoAlvo)
    {
        alvoInicio = novoAlvo;
        alvoFim = null;
        usarArrastar = false;
        loopArraste = false;

        if (arrasteCoroutine != null)
        {
            StopCoroutine(arrasteCoroutine);
            arrasteCoroutine = null;
        }

        if (alvoInicio != null)
            transform.position = alvoInicio.position + offset;
    }

    // 🔹 Método compatível com Passo.cs (FinalizarPasso chama isso)
    public void PararEEsconder()
    {
        StopAllCoroutines();
        arrasteCoroutine = null;
        alvoInicio = null;
        alvoFim = null;
        usarArrastar = false;
        loopArraste = false;

        // opcional: esconder o objeto seguidor
        gameObject.SetActive(false);
    }
}
