using UnityEngine;
using UnityEngine.UI;

public class Mecanica_Passo : MonoBehaviour
{
    [Header("UI de Progresso")]
    public Toggle[] listaToggles;
    public GameObject[] imagensConclusao;
    public ToggleGroup grupo;

    [Header("Objetos da cena na ordem dos passos")]
    public GameObject[] objetosPorPasso;

    private bool[] passosConcluidos;
    private int passoAtual = 0;

    void Start()
    {
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
                    passoAtual = index;
                }
            });
        }

        AtualizarUI();
    }

    // ✅ Marca um passo como concluído
    public void DefinirPasso(int index, bool concluido)
    {
        if (index < 0 || index >= passosConcluidos.Length) return;

        passosConcluidos[index] = concluido;

        if (imagensConclusao[index] != null)
            imagensConclusao[index].SetActive(concluido);

        AtualizarUI();
    }

    // ✅ Sincroniza o passo atual
    public void MudarPasso(int index)
    {
        if (index < 0 || index >= listaToggles.Length) return;

        passoAtual = index;
        listaToggles[passoAtual].isOn = true;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        for (int i = 0; i < listaToggles.Length; i++)
        {
            listaToggles[i].isOn = (i == passoAtual);
            listaToggles[i].interactable = (i <= passoAtual);

            if (imagensConclusao[i] != null)
                imagensConclusao[i].SetActive(passosConcluidos[i]);
        }
    }

    public bool ObjetoCorreto(GameObject objeto)
    {
        return passoAtual < objetosPorPasso.Length && objetosPorPasso[passoAtual] == objeto;
    }
}
