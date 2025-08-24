using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

public class HintGenerator : MonoBehaviour
{
    private string apiUrl = "http://localhost:4891/v1/chat/completions";

    [Serializable] private class ApiMessage { public string role; public string content; }
    [Serializable] private class ApiRequest { public ApiMessage[] messages; public float temperature = 0.7f; public int max_tokens = 50; public bool stream = false; }
    [Serializable] private class ApiResponseChoice { public ApiMessage message; }
    [Serializable] private class ApiResponse { public ApiResponseChoice[] choices; }

    public void GetHint(string prompt, Action<string> callback)
    {
        StartCoroutine(SendRequest(prompt, callback));
    }

    private IEnumerator SendRequest(string prompt, Action<string> callback)
    {
        // 1. Montar o corpo da requisição em formato JSON
        ApiRequest requestBody = new ApiRequest { messages = new ApiMessage[] { new ApiMessage { role = "system", content = "Você é um assistente amigável e paciente." }, new ApiMessage { role = "user", content = prompt } } };
        string jsonBody = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        // 2. Criar e configurar a requisição web (MÉTODO CORRIGIDO)
        var uploadHandler = new UploadHandlerRaw(bodyRaw);
        var downloadHandler = new DownloadHandlerBuffer();
        // Usar este construtor é mais robusto para garantir que seja um POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST", downloadHandler, uploadHandler);
        request.SetRequestHeader("Content-Type", "application/json");

        // 3. Enviar a requisição e esperar pela resposta
        yield return request.SendWebRequest();

        // 4. Processar a resposta
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro na API: " + request.error + " | Resposta: " + request.downloadHandler.text);
            callback?.Invoke("Ops, tive um probleminha. Vamos tentar de novo!");
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            ApiResponse response = JsonUtility.FromJson<ApiResponse>(jsonResponse);
            string generatedHint = response.choices[0].message.content.Trim();
            Debug.Log("IA respondeu: " + generatedHint);
            callback?.Invoke(generatedHint);
        }
    }
}