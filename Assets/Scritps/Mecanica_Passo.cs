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

    public Button botaoFinal;
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

        // 🔒 Inicia com só o primeiro toggle liberado
        for (int i = 0; i < listaToggles.Length; i++)
        {
            listaToggles[i].isOn = false;
            listaToggles[i].interactable = (i == 0);

            if (imagensConclusao != null && i < imagensConclusao.Length && imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(false);
        }

        AtualizarUI();
    }

    public void MudarEtapa(int indice)
    {
        // ✅ Só permite mudar se for para uma etapa anterior ou atual
        if (indice <= etapaAtual && indice >= 0)
        {
            etapaAtual = indice;
            AtualizarUI();
        }
    }

    public void MarcarEtapaConcluida(int indice)
    {
        if (indice < 0 || indice >= etapasConcluidas.Length)
            return;

        etapasConcluidas[indice] = true;

        // ✅ Libera o próximo toggle, se existir
        if (indice + 1 < listaToggles.Length)
            listaToggles[indice + 1].interactable = true;

        // ✅ Atualiza etapa atual para o próximo automaticamente
        etapaAtual = Mathf.Min(indice + 1, listaToggles.Length - 1);

        AtualizarUI();
    }
    public void ConfigurarUI(int totalEtapas)
    {
        // 🔒 Garante que não vamos além do número de toggles disponíveis
        int etapasValidas = Mathf.Min(totalEtapas, listaToggles.Length);

        etapasConcluidas = new bool[etapasValidas];
        etapaAtual = 0;

        // 🔒 Desativa todos os toggles extras se existirem
        for (int i = 0; i < listaToggles.Length; i++)
        {
            bool dentroDoLimite = i < etapasValidas;
            listaToggles[i].gameObject.SetActive(dentroDoLimite);

            if (dentroDoLimite)
            {
                listaToggles[i].isOn = false;
                listaToggles[i].interactable = (i == 0);
            }

            if (imagensConclusao != null && i < imagensConclusao.Length && imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(false);
        }

        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (listaToggles == null || listaToggles.Length == 0 || etapasConcluidas == null)
            return;

        int total = Mathf.Min(listaToggles.Length, etapasConcluidas.Length);

        for (int i = 0; i < total; i++)
        {
            bool etapaConcluida = etapasConcluidas[i];
            bool etapaAtualAtiva = (i == etapaAtual);

            listaToggles[i].isOn =  etapaAtualAtiva;

            if (i == 0)
                listaToggles[i].interactable = true;
            else if (etapaConcluida || (i > 0 && etapasConcluidas[i - 1]))
                listaToggles[i].interactable = true;
            else
                listaToggles[i].interactable = false;

            if (imagensConclusao != null && i < imagensConclusao.Length && imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(etapaConcluida);
        }
    }
}
