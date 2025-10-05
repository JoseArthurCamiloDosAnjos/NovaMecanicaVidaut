using UnityEngine;
using UnityEngine.UI;

public class Mecanica_Passo : MonoBehaviour
{
    [Header("UI de Etapas")]
    public Toggle[] listaToggles;         // Cada toggle representa uma ETAPA
    public GameObject[] imagensConclusao; // Ícone de "etapa concluída"

    [Header("Referências")]
    public Banheiro_Minigame bMinigame;   // Referência ao controlador principal

    private bool[] etapasConcluidas;
    private int etapaAtual = 0;

    void Start()
    {
        if (bMinigame == null)
            bMinigame = FindObjectOfType<Banheiro_Minigame>();

        if (listaToggles == null || listaToggles.Length == 0)
        {
            Debug.LogWarning("Nenhum Toggle configurado para etapas.");
            return;
        }

        etapasConcluidas = new bool[listaToggles.Length];

        for (int i = 0; i < listaToggles.Length; i++)
        {
            int index = i;
            listaToggles[i].isOn = false;
            listaToggles[i].interactable = false;

            if (imagensConclusao.Length > i && imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(false);
        }

        AtualizarUI();
    }

    public void ConfigurarUI(int totalEtapas)
    {
        if (listaToggles.Length < totalEtapas)
            Debug.LogWarning("Existem mais etapas do que toggles configurados na UI!");

        etapasConcluidas = new bool[totalEtapas];
        etapaAtual = 0;
        AtualizarUI();
    }

    public void MudarEtapa(int indice)
    {
        etapaAtual = Mathf.Clamp(indice, 0, listaToggles.Length - 1);
        AtualizarUI();
    }

    public void MarcarEtapaConcluida(int indice)
    {
        if (indice < 0 || indice >= etapasConcluidas.Length) return;

        etapasConcluidas[indice] = true;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        for (int i = 0; i < listaToggles.Length; i++)
        {
            bool ativa = (i == etapaAtual);
            listaToggles[i].isOn = ativa;
            listaToggles[i].interactable = ativa;

            if (imagensConclusao.Length > i && imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(i < etapasConcluidas.Length && etapasConcluidas[i]);
        }
    }
}
