using UnityEngine;
using TMPro; // Para exibir o texto na UI
using GPT4All; // Namespace do wrapper que você adicionou
using System.Threading.Tasks;

public class DicaGPT : MonoBehaviour
{
    public TMP_Text dicaUI; // arraste seu TextMeshPro aqui no Inspector
    private GPT4AllModel modelo;

    async void Start()
    {
        // Caminho do modelo no Android
        string caminhoModelo = Application.streamingAssetsPath + "/Models/gpt4all.bin";

        modelo = new GPT4AllModel(caminhoModelo);
        await modelo.LoadAsync();
    }

    public async void GerarDica(string contexto)
    {
        string prompt = $"O jogador está em um jogo point-and-click. Contexto: {contexto}\nDê uma dica curta e objetiva para o próximo passo:";
        string resposta = await modelo.GenerateAsync(prompt, maxTokens: 50);
        dicaUI.text = resposta.Trim();
    }

    void OnDestroy()
    {
        modelo?.Dispose();
    }
}
