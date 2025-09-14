using UnityEngine;

[System.Serializable]
public class Passo
{
    public enum TipoPasso { Clique, Colisao }
    public TipoPasso tipo;

    [Header("Para Clique")]
    public GameObject objetoClique;

    [Header("Para Colisão")]
    public GameObject objetoA;
    public GameObject objetoB;

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

    public void IniciarPasso()
    {
        foreach (var obj in objetosParaAtivarNoInicio)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objetosParaDesativarNoInicio)
            if (obj != null) obj.SetActive(false);

        if (objetoDestaque != null)
            objetoDestaque.SetActive(true);
    }

    public void FinalizarPasso()
    {
        foreach (var obj in objetosParaAtivarNoFim)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objetosParaDesativarNoFim)
            if (obj != null) obj.SetActive(false);

        if (objetoDestaque != null)
            objetoDestaque.SetActive(false);
    }
}
