using UnityEngine;
using UnityEngine.UI;

public class Mecanica_Passo : MonoBehaviour
{
    [Header("UI de Progresso")]
    public Toggle[] listaToggles;         // Arraste todos os Toggles (checkboxes)
    public GameObject[] imagensConclusao; // Arraste os ícones de concluído
    public ToggleGroup grupo;             // ToggleGroup

    [Header("Referências")]
    public Banheiro_Minigame bMinigame;     // Arraste o objeto com o script Banheiro_Minigame aqui

 

    private bool[] passosConcluidos;
    private int passoAtual = 0;

    void Start()
    {
        // Tenta encontrar a referência do minigame se não foi arrastada no Inspector
        if (bMinigame == null)
        {
            bMinigame = FindObjectOfType<Banheiro_Minigame>();
        }

        int quantidade = listaToggles.Length;
        passosConcluidos = new bool[quantidade];

        for (int i = 0; i < quantidade; i++)
        {
            int index = i;

            if (imagensConclusao[index] != null)
                imagensConclusao[index].SetActive(false);

            listaToggles[i].onValueChanged.AddListener((bool ligado) =>
            {
                if (ligado)
                {
                    // Lógica para pular entre os passos (se necessário)
                    // Esta parte pode ser ajustada dependendo do comportamento desejado
                }
            });
        }
        AtualizarUI();
    }

    public void ProximoPasso()
    {
        if (passoAtual < passosConcluidos.Length)
        {
            passosConcluidos[passoAtual] = true;
        }

        if (passoAtual + 1 < listaToggles.Length)
        {
            passoAtual++;
        }
        AtualizarUI();
    }

    // ATUALIZAÇÃO: Este método é chamado por um botão na UI para INICIAR o processo de voltar
    public void PassoAnterior()
    {
        // Ele apenas chama o método principal no script do minigame.
        if (bMinigame != null)
        {
            bMinigame.VoltarPasso();
        }
    }

    // ATUALIZAÇÃO: Novo método para ser controlado pelo Banheiro_Minigame
    // Sincroniza a UI com o estado atual do jogo quando um passo é voltado.
    public void AtualizarParaPasso(int novoPasso)
    {
        passoAtual = novoPasso;
        passosConcluidos[passoAtual] = false; // Desmarca o passo como concluído
        AtualizarUI();
    }

    void AtualizarUI()
    {
        for (int i = 0; i < listaToggles.Length; i++)
        {
            listaToggles[i].isOn = (i == passoAtual);

            // Permite clicar nos passos já concluídos para revisitá-los (opcional)
            listaToggles[i].interactable = passosConcluidos[i] || (i == passoAtual);

            if (imagensConclusao.Length > i && imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(passosConcluidos[i]);
        }
    }


}