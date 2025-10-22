using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static List<(string statement, char truthValue)> Statements = new List<(string, char)>();
    public static int nextLetterIndex = 0;
    public static Dictionary<char, bool> AssignedTruths = new Dictionary<char, bool>();

    void Awake()
    {
        LoadStatementsFromFile();
    }

    public static void RegisterBlock(char letter, bool truthValue)
    {
        AssignedTruths.Remove(letter);
        AssignedTruths.Add(letter, truthValue);
    }

    void LoadStatementsFromFile()
    {
        TextAsset textFile = Resources.Load<TextAsset>("Statements");
        if (textFile == null)
        {
            Debug.LogError("Resources.txt not found in Assets/Resources/");
            return;
        }

        string[] lines = textFile.text.Split('\n');
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Trim().Split(',');
            if (parts.Length == 2)
            {
                char truthValue = parts[0].Trim()[0];
                string statement = parts[1].Trim();

                if (truthValue == 'T' || truthValue == 'F' || truthValue == 'N')
                {
                    Statements.Add((statement, truthValue));
                }
            }
        }
    }

    public static char GetNextLetter()
    {
        char letter = (char)('A' + nextLetterIndex);
        nextLetterIndex = (nextLetterIndex + 1) % 26;
        return letter;
    }
}
