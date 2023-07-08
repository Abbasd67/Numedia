using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    public GameObject PuzzleTilePrefab;
    public Vector2Int PuzzleSize;
    public Transform PuzzleContainer;
    public Text MoveCountText { get; set; }

    private PuzzleTile[][] PuzzleTiles { get; set; }
    private Vector2Int emptyTilePosition { get; set; }
    private int MoveCount { get; set; }

    private void Start()
    {
        MoveCount = 0;
        GeneratePuzzle();
        ShufflePuzzle();
    }

    private void GeneratePuzzle()
    {
        PuzzleTiles = new PuzzleTile[(int)PuzzleSize.x][];

        for (int i = 0; i < PuzzleSize.x; i++)
        {
            PuzzleTiles[i] = new PuzzleTile[(int)PuzzleSize.y];
            for (int j = 0; j < PuzzleSize.y; j++)
            {
                if (i != PuzzleSize.x || j != PuzzleSize.y)
                {
                    GameObject tileObject = Instantiate(PuzzleTilePrefab, PuzzleContainer);
                    PuzzleTile puzzleTile = tileObject.GetComponent<PuzzleTile>();
                    puzzleTile.CurrectPosition = new Vector2Int(i, j);
                    puzzleTile.SetNumber(i * (int)PuzzleSize.y + j + 1);
                    puzzleTile.SetPosition(new Vector2(i, j));
                    PuzzleTiles[i][j] = puzzleTile;
                }
                else
                {
                    emptyTilePosition = new Vector2Int(i, j);
                }
            }
        }
    }

    private void ShufflePuzzle()
    {
        for (int i = 0; i < PuzzleSize.x; i++)
        {
            for (int j = 0; j < PuzzleSize.y; j++)
            {
                int randomI = Random.Range(0, (int)PuzzleSize.x);
                int randomJ = Random.Range(0, (int)PuzzleSize.y);
                SwapTiles(i, j, randomI, randomJ);
            }
        }
    }

    private void SwapTiles(int tileAx, int tileAy, int tileBx, int tileBy)
    {
        if (tileAx == tileBx && tileAy == tileBy)
            return;

        PuzzleTile tempTile = PuzzleTiles[tileAx][tileAy];
        PuzzleTiles[tileAx][tileAy] = PuzzleTiles[tileBx][tileBy];
        PuzzleTiles[tileBx][tileBy] = tempTile;

        if (PuzzleTiles[tileAx][tileAy] != null)
            PuzzleTiles[tileAx][tileAy].SetPosition(new Vector2(tileBx, tileBy));
        if (PuzzleTiles[tileBx][tileBy] != null)
            PuzzleTiles[tileBx][tileBy].SetPosition(new Vector2(tileAx, tileAy));

    }

    private bool CheckPuzzleSolved()
    {
        for (int i = 0; i < PuzzleSize.x; i++)
            for (int j = 0; j < PuzzleSize.y; j++)
                if (!PuzzleTiles[i][j].IsInCorrectPosition())
                    return false;

        return true;
    }

    private void UpdateMoveCountText()
    {
        MoveCountText.text = "Moves: " + MoveCount.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            TryMoveTile(new Vector2Int(emptyTilePosition.x, emptyTilePosition.y + 1));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            TryMoveTile(new Vector2Int(emptyTilePosition.x, emptyTilePosition.y - 1));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TryMoveTile(new Vector2Int(emptyTilePosition.x - 1, emptyTilePosition.y));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TryMoveTile(new Vector2Int(emptyTilePosition.x + 1, emptyTilePosition.y));
        }
    }

    public void TryMoveTile(Vector2Int tilePosition)
    {
        if (tilePosition.x < 0 || tilePosition.y < 0 || tilePosition.x >= PuzzleSize.x || tilePosition.y >= PuzzleSize.y)
            return;

        SwapTiles(tilePosition.x, tilePosition.y, emptyTilePosition.x, emptyTilePosition.y);
        emptyTilePosition = tilePosition;
        MoveCount++;
        UpdateMoveCountText();

        if (CheckPuzzleSolved())
        {
            Debug.Log("Puzzle solved!");
        }
    }


    public void ResetPuzzle()
    {
        MoveCount = 0;
        UpdateMoveCountText();

        for (int i = 0; i < PuzzleSize.x; i++)
            for (int j = 0; j < PuzzleSize.y; j++)
            {
                PuzzleTiles[i][j].ResetPosition();
                PuzzleTiles[i][j].CurrectPosition = new Vector2Int(i, j);
            }

        ShufflePuzzle();
    }
}
