using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents; // Namespace do ML-Agents

public class GameManager : MonoBehaviour
{
    // Enum para tornar os passos mais legíveis
    public enum EscovaSteps
    {
        PegarEscova,
        PegarPasta,
        AbrirPasta,
        ColocarPastaNaEscova,
        MolharEscova,
        EscovarDentes,
        EnxaguarBoca,
        LimparEscova,
        Finalizado
    }

    public EscovaSteps currentStep;
    public Text hintTextUI;
    public HintGenerator hintGenerator;
    public HintAgent hintAgent; // Referência ao nosso agente inteligente

    // --- Variáveis observáveis pelo Agente ---
    public float timeSinceLastAction = 0f;
    public int wrongClicks = 0;

    void Start()
    {
        ResetEpisode();
    }

    void Update()
    {
        // Apenas acumula o tempo para o agente observar
        timeSinceLastAction += Time.deltaTime;

        // Penaliza o agente por deixar o tempo passar (incentiva a ação)
        hintAgent.AddReward(-0.001f);
    }

    public void OnObjectClicked(EscovaSteps objectStep)
    {
        if (objectStep == currentStep)
        {
            // Ação correta! Recompensa o agente e avança.
            Debug.Log("Passo correto! Recompensando o agente.");
            hintAgent.AddReward(1.0f); // Grande recompensa por sucesso
            hintAgent.EndEpisode(); // Termina o "ciclo de aprendizado" para este passo
        }
        else
        {
            // Ação errada! Penaliza o agente.
            wrongClicks++;
            Debug.Log("Clique errado! Penalizando o agente.");
            hintAgent.AddReward(-0.1f);
        }
    }

    // O agente chama este método quando decide que uma dica é necessária
    public void RequestHintFromAgent(int hintType) // 0 = sutil, 1 = direta
    {
        string typeString = (hintType == 0) ? "dica sutil" : "dica direta";
        Debug.Log($"Agente solicitou uma {typeString}");

        // Penaliza o agente por usar uma dica (para que ele não use o tempo todo)
        float penalty = (hintType == 0) ? -0.2f : -0.4f;
        hintAgent.AddReward(penalty);

        string prompt = $"Estou em um jogo para uma criança autista aprender a escovar os dentes. A criança está no passo '{currentStep}' e precisa de uma {typeString}. Gere uma frase curta, positiva e muito simples para ajudar.";

        hintGenerator.GetHint(prompt, (generatedHint) => {
            UpdateHintText(generatedHint);
        });

        // Reseta o tempo para dar um fôlego após a dica
        timeSinceLastAction = 0f;
    }

    void UpdateTutorialState()
    {
        string instruction = "";
        switch (currentStep)
        {
            case EscovaSteps.PegarPasta:
                instruction = "Ótimo! Agora, vamos pegar a pasta de dente.";
                break;
            // ... adicione as instruções para todos os outros passos aqui ...
            case EscovaSteps.Finalizado:
                instruction = "Parabéns! Você escovou os dentes direitinho!";
                // Lógica para reiniciar ou terminar o jogo
                break;
        }
        UpdateHintText(instruction);
    }

    void ResetEpisode()
    {
        currentStep++;
        if (currentStep > EscovaSteps.LimparEscova)
        {
            currentStep = EscovaSteps.PegarEscova; // Reinicia do começo
        }

        UpdateTutorialState();
        timeSinceLastAction = 0f;
        wrongClicks = 0;
    }

    void UpdateHintText(string message)
    {
        if (hintTextUI != null)
        {
            hintTextUI.text = message;
        }
    }
}