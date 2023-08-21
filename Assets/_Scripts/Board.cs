using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPORTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B,KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z,
    };

    private Row[] rows;

    private string[] solutions;
    private string[] validWords;
    private string word;

    private int rowIndex;
    private int columIndex;

    [Header("States")]
    public Tile.State emptyState;
    public Tile.State occupiedState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
    }

    private void Start()
    {
        LoadData();
        SetRandomWord();
    }

    private void LoadData()
    {
        TextAsset textFile = Resources.Load("Official_Words_All") as TextAsset;
        validWords = textFile.text.Split('\n');

        textFile = Resources.Load("Official_Words_Common") as TextAsset;
        solutions = textFile.text.Split('\n');
    }

    private void SetRandomWord()
    {
        word = solutions[Random.Range(0, solutions.Length)];
        word = word.ToLower().Trim();
    }

    private void Update()
    {
        Row currentRow = rows[rowIndex];

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            columIndex = Mathf.Max(columIndex - 1, 0);

            currentRow.tiles[columIndex].SetLetter('\0');
            currentRow.tiles[columIndex].SetState(emptyState);
        }
        else if (columIndex >= rows[rowIndex].tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPORTED_KEYS.Length; i++)
            {
                if (Input.GetKeyDown(SUPPORTED_KEYS[i]))
                {
                    currentRow.tiles[columIndex].SetLetter((char)SUPPORTED_KEYS[i]);
                    currentRow.tiles[columIndex].SetState(occupiedState);
                    columIndex++;
                    break;
                }
            }
        } 
    }

    private void SubmitRow(Row row)
    {
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];

            if(tile.letter == word[i])
            {
                tile.SetState(correctState);
                // Correct state
            }else if(word.Contains(tile.letter))
            {
                tile.SetState(wrongSpotState);
                // Wrong spot
            }
            else
            {
                tile.SetState(incorrectState);
                // incorrect
            }
        }

        rowIndex++;
        columIndex = 0;

        if (rowIndex >= rows.Length)
        {
            enabled = false;
        }
    }
}
