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

    // REMOVEMOS A REFERÊNCIA AO SCRIPT DE ANIMAÇÃO DAQUI

    public void IniciarPasso()
    {
        foreach (var obj in objetosParaAtivarNoInicio)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objetosParaDesativarNoInicio)
            if (obj != null) obj.SetActive(false);
    }

    public void FinalizarPasso()
    {
        foreach (var obj in objetosParaAtivarNoFim)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objetosParaDesativarNoFim)
            if (obj != null) obj.SetActive(false);
    }

    // NOVA FUNÇÃO: Retorna qual é o GameObject principal que o indicador deve seguir
    public GameObject GetAlvoPrincipal()
    {
        switch (tipo)
        {
            case TipoPasso.Clique:
                return objetoClique;
            case TipoPasso.Colisao:
                return objetoA; // Em colisões, vamos seguir o Objeto A por padrão.
            default:
                return null;
        }
    }
}