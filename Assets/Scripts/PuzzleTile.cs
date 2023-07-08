using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTile : MonoBehaviour
{
    public TextMeshProUGUI NumberText;

    public Vector2Int CurrectPosition { get; set; }

    public void SetNumber(int value)
    {
        NumberText.text = value.ToString();
    }

    public void SetPosition(Vector2 position)
    {
        transform.localPosition = position;
    }

    public Vector2 GetPosition()
    {
        return transform.localPosition;
    }

    public bool IsInCorrectPosition()
    {
        return transform.localPosition.x == CurrectPosition.x && transform.localPosition.y == CurrectPosition.y;
    }

    // public void OnMouseDown()
    // {
    //     PuzzleManager.TryMoveTile(correctIndex);
    // }

    public void ResetPosition()
    {
        transform.localPosition = Vector2.zero;
    }
}
