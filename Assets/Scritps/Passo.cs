using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Passo
{
    
    public enum TipoPasso { Clique, Colisao, Arrastar }
    public TipoPasso tipo;
    private Banheiro_Minigame banheiroMinigame;

    [Header("Para Clique")]
    public GameObject objetoClique;

    [Header("Para Colisão")]
    public GameObject objetoA;
    public GameObject objetoB;

    [Header("Para Arrastar")]
    public GameObject objetoArrastavel;
    public GameObject destinoArrastar;

    [Header("Objetos para Ativar ao Iniciar")]
    public GameObject[] objetosParaAtivarNoInicio;

    [Header("Objetos para Desativar ao Iniciar")]
    public GameObject[] objetosParaDesativarNoInicio;

    [Header("Objetos para Ativar ao Finalizar")]
    public GameObject[] objetosParaAtivarNoFim;

    [Header("Objetos para Desativar ao Finalizar")]
    public GameObject[] objetosParaDesativarNoFim;

    [Header("Destaque visual (opcional)")]
    public GameObject objetoDestaque;

    [HideInInspector] public bool concluido = false;

    private Banheiro_Minigame Bg;


    [Header("Falas e Áudio do Passo")]
    public AudioClip[] audiosPasso;
    [TextArea(2, 5)] public string[] textosPasso;
    public float delayEntreFalas = 1f;
    public void IniciarPasso()
    {
        
        NormalizarAlvos();

        if (objetosParaAtivarNoInicio != null)
            foreach (var obj in objetosParaAtivarNoInicio)
                if (obj != null) obj.SetActive(true);

        if (objetosParaDesativarNoInicio != null)
            foreach (var obj in objetosParaDesativarNoInicio)
                if (obj != null) obj.SetActive(false);

        if (objetoDestaque != null) objetoDestaque.SetActive(true);

        if (tipo == TipoPasso.Arrastar && objetoArrastavel != null)
        {
            var drag = objetoArrastavel.GetComponent<DragItem>();
            if (drag != null)
                drag.HabilitarArraste(true, destinoArrastar);

            var seguidor = objetoArrastavel.GetComponent<SeguirObjeto>();
            if (seguidor != null)
                seguidor.SeguirTransform(objetoArrastavel.transform);
        }
 
    }

    public void FinalizarPasso()
    {
        if (objetosParaAtivarNoFim != null)
            foreach (var obj in objetosParaAtivarNoFim)
                if (obj != null) obj.SetActive(true);

        if (objetosParaDesativarNoFim != null)
            foreach (var obj in objetosParaDesativarNoFim)
                if (obj != null) obj.SetActive(false);

        if (objetoDestaque != null)
            objetoDestaque.SetActive(false);

        if (tipo == TipoPasso.Arrastar && objetoArrastavel != null)
        {
            var drag = objetoArrastavel.GetComponent<DragItem>();
            if (drag != null)
                drag.HabilitarArraste(false, null);

            var seguidor = objetoArrastavel.GetComponent<SeguirObjeto>();
            if (seguidor != null)
                seguidor.PararEEsconder();
        }
       
      
        concluido = true;
    }

    public void NormalizarAlvos()
    {
        if (objetoA != null && objetoB != null && objetoA == objetoB)
            objetoB = null;

        if (tipo == TipoPasso.Clique)
        {
            objetoA = null;
            objetoB = null;
            objetoArrastavel = null;
            destinoArrastar = null;
        }
        else if (tipo == TipoPasso.Arrastar)
        {
            objetoA = null;
            objetoB = null;
            objetoClique = null;
        }

    }
    public IEnumerator ReproduzirFalas(AudioSource fonte, Text textoUI)
    {
        if (audiosPasso == null || textosPasso == null)
            yield break;

        int total = Mathf.Min(audiosPasso.Length, textosPasso.Length);
        for (int i = 0; i < total; i++)
        {
            if (textoUI != null)
                textoUI.text = textosPasso[i];

            if (fonte != null && audiosPasso[i] != null)
            {
                fonte.clip = audiosPasso[i];
                fonte.Play();
                yield return new WaitForSeconds(fonte.clip.length + delayEntreFalas);
            }
            else
                yield return new WaitForSeconds(delayEntreFalas);
        }
    }
}
