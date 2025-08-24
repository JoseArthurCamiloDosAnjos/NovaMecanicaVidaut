using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class HintAgent : Agent
{
    public GameManager gameManager; // Refer�ncia ao GameManager

    // O que o agente observa
    public override void CollectObservations(VectorSensor sensor)
    {
        // Normalizamos os valores para facilitar o aprendizado
        sensor.AddObservation((float)gameManager.currentStep / 9f); // Passo atual (0 a 1)
        sensor.AddObservation(gameManager.timeSinceLastAction / 15f); // Tempo de inatividade (0 a 1)
        sensor.AddObservation(gameManager.wrongClicks / 5f); // Cliques errados (0 a 1)
    }

    // O que o agente faz com base no que observou
    public override void OnActionReceived(ActionBuffers actions)
    {
        int decision = actions.DiscreteActions[0];

        switch (decision)
        {
            case 0:
                // A��o 0: Fazer nada.
                break;
            case 1:
                // A��o 1: Pedir dica sutil.
                gameManager.RequestHintFromAgent(0);
                break;
            case 2:
                // A��o 2: Pedir dica direta.
                gameManager.RequestHintFromAgent(1);
                break;
        }
    }

    // Chamado quando um ciclo de aprendizado (um passo do tutorial) termina
    public override void OnEpisodeBegin()
    {
        // O GameManager agora controla o reset do estado do jogo
        // Este m�todo � chamado pelo GameManager.EndEpisode()
        gameManager.SendMessage("ResetEpisode");
    }

    // Para testar sem treinar (voc� pode usar as teclas do teclado)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKeyDown(KeyCode.Alpha0)) discreteActionsOut[0] = 0; // N�o fazer nada
        if (Input.GetKeyDown(KeyCode.Alpha1)) discreteActionsOut[0] = 1; // Dica sutil
        if (Input.GetKeyDown(KeyCode.Alpha2)) discreteActionsOut[0] = 2; // Dica direta
    }
}