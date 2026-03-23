using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Отступ вокруг доски в клетках")]
    public float padding = 1.5f;

    public void FitToBoard(int boardWidth, int boardHeight)
    {
        // Центрируем камеру по центру доски
        float centerX = (boardWidth - 1) / 2f;
        float centerY = (boardHeight - 1) / 2f;
        transform.position = new Vector3(centerX, centerY, transform.position.z);

        // Считаем нужный orthographicSize
        // orthographicSize = половина высоты видимой области
        float aspectRatio = (float)Screen.width / Screen.height;

        float sizeByHeight = (boardHeight / 2f) + padding;
        float sizeByWidth = (boardWidth / 2f) + padding / aspectRatio;

        // Берём максимум — чтобы доска точно влезла
        Camera.main.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth / aspectRatio);
    }
}