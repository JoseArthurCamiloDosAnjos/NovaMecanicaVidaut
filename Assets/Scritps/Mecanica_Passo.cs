using UnityEngine;
using UnityEngine.UI;

public class Mecanica_Passo : MonoBehaviour
{
    [Header("UI de Progresso")]
    public Toggle[] listaToggles;           // Arraste todos os Toggles (checkboxes)
    public GameObject[] imagensConclusao;   // Arraste os ícones de concluído
    public ToggleGroup grupo;               // ToggleGroup
    Banheiro_Minigame bMinigame;
    [Header("Objetos da cena na ordem dos passos")]
    public GameObject[] objetosPorPasso;    // Arraste os objetos que devem ser clicados na ordem certa

    private bool[] passosConcluidos;
    private int passoAtual = 0;

    void Start()
    {
        int quantidade = listaToggles.Length;
        passosConcluidos = new bool[quantidade];

        for (int i = 0; i < quantidade; i++)
        {
            int index = i;

            // Garante que todas as imagens começam invisíveis
            if (imagensConclusao[index] != null)
                imagensConclusao[index].SetActive(false);

            // Configura evento de seleção do Toggle
            listaToggles[i].onValueChanged.AddListener((bool ligado) =>
            {
                if (ligado)
                {
                    passoAtual = index;
                }
            });
        }

        AtualizarUI();
    }

    public void ProximoPasso()
    {
        // Marca o passo atual como concluído
        passosConcluidos[passoAtual] = true;

        // Ativa a imagem de concluído
        if (imagensConclusao[passoAtual] != null)
            imagensConclusao[passoAtual].SetActive(true);

        // Avança para o próximo se existir
        if (passoAtual + 1 < listaToggles.Length)
        {
            passoAtual++;
            listaToggles[passoAtual].isOn = true; // muda o Toggle ativo
        }

        AtualizarUI();
    }

    public void PassoAnterior()
    {
        if (passoAtual > 0)
        {
            bMinigame.VoltarPasso();
            passoAtual--;
            listaToggles[passoAtual].isOn = true;
        }
    }

    void AtualizarUI()
    {
        for (int i = 0; i < listaToggles.Length; i++)
        {
            // Só o passo atual fica com o Toggle ligado
            listaToggles[i].isOn = (i == passoAtual);

            // Passos anteriores e o atual ficam interativos, futuros bloqueados
            listaToggles[i].interactable = (i <= passoAtual);

            // Mostra a imagem de concluído se o passo já foi feito
            if (imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(passosConcluidos[i]);
        }
    }

    // 🔹 Verifica se o objeto clicado é o esperado no passo atual
    public bool ObjetoCorreto(GameObject objeto)
    {
        return passoAtual < objetosPorPasso.Length && objetosPorPasso[passoAtual] == objeto;
    }
   
}

