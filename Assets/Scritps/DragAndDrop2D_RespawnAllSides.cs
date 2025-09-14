using UnityEngine;

public class DragAndDrop2D_RespawnAllSides : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private Camera mainCamera;
    private Vector3 initialPosition;
    private Mecanica_Passo MP;
    public Animator animator;
    void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position; // posição inicial do objeto
    }
    // Quando clica no objeto, inicia o arrasto
    void OnMouseDown()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePos.x, mousePos.y, transform.position.z);
        isDragging = true;
    }

    // Arrasta o objeto junto com o mouse
    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z) + offset;
        }
    }

    // Quando solta o mouse, verifica se está fora da tela
    void OnMouseUp()
    {
        isDragging = false;

        // Converte posição do objeto para coordenadas da tela (viewport)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        bool outOfBounds = false;

        // Verifica cada lado da tela
        if (viewportPos.x < 0f) outOfBounds = true;
        if (viewportPos.x > 1f) outOfBounds = true;
        if (viewportPos.y < 0f) outOfBounds = true;
        if (viewportPos.y > 1f) outOfBounds = true;

        // Se estiver fora, respawna na posição inicial
        if (outOfBounds)
        {
            transform.position = initialPosition;
        }
    }
}
