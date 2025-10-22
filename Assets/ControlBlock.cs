using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ControlBlock : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed = 4f;
    private Vector3 startPosition;
    private float distanceTraveled = 0f;
    private const float maxTravelDistance = 22f;
    private string assignedStatement;
    public char truthValue;
    private char assignedLetter;
    public bool MultiBlock = false;

    private bool launched = false;

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void Launch(Vector3 force)
    {
        launched = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddForce(force, ForceMode.Impulse);

            Vector3 randomTorque = new Vector3(
                Random.Range(-500f, 500f),
                Random.Range(-500f, 500f),
                Random.Range(-500f, 500f)
            );
            rb.AddTorque(randomTorque, ForceMode.Impulse);
        }

        enabled = false; // Stop script-driven movement
    }

    void Start()
    {
        assignedLetter = Database.GetNextLetter();
        startPosition = transform.position;

        bool useCompound = Database.nextLetterIndex >= 7 && Random.value < 0.25f;

        if (!useCompound || Database.AssignedTruths.Count < 1)
        {
            if (Database.Statements.Count > 0)
            {
                int index = Random.Range(0, Database.Statements.Count);
                assignedStatement = Database.Statements[index].statement;
                truthValue = Database.Statements[index].truthValue;
            }
            else
            {
                assignedStatement = "[No Data]";
                truthValue = 'N';
            }
        }
        else
        {
            GenerateCompoundStatement();
            MultiBlock = true;
        }

        if (truthValue == 'T' || truthValue == 'F')
        {
            Database.RegisterBlock(assignedLetter, truthValue == 'T');
        }

        Transform sideText = transform.Find("SideText");
        if (sideText != null)
        {
            var tmp = sideText.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = $"{assignedLetter}: {assignedStatement}";
            }
        }
    }

    void GenerateCompoundStatement()
    {
        string[] operators = { "AND", "OR", "NOT" };
        string selectedOperator = operators[Random.Range(0, operators.Length)];

        bool useRefA = Random.value < 0.5f;
        bool useRefB = selectedOperator != "NOT" && Random.value < 0.5f;

        string partA = "";
        string partB = "";
        char valCharA = 'N';
        char valCharB = 'N';

        if (useRefA && Database.AssignedTruths.Count > 0)
        {
            var keys = new List<char>(Database.AssignedTruths.Keys);
            char refLetter = keys[Random.Range(0, keys.Count)];
            partA = refLetter.ToString();
            valCharA = Database.AssignedTruths[refLetter] ? 'T' : 'F';
        }
        else if (Database.Statements.Count > 0)
        {
            int index = Random.Range(0, Database.Statements.Count);
            partA = Database.Statements[index].statement;
            valCharA = Database.Statements[index].truthValue;
        }

        if (selectedOperator != "NOT")
        {
            if (useRefB && Database.AssignedTruths.Count > 0)
            {
                var keys = new List<char>(Database.AssignedTruths.Keys);
                char refLetter = keys[Random.Range(0, keys.Count)];
                partB = refLetter.ToString();
                valCharB = Database.AssignedTruths[refLetter] ? 'T' : 'F';
            }
            else if (Database.Statements.Count > 0)
            {
                int index = Random.Range(0, Database.Statements.Count);
                partB = Database.Statements[index].statement;
                valCharB = Database.Statements[index].truthValue;
            }
        }

        if (valCharA == 'N' || (selectedOperator != "NOT" && valCharB == 'N'))
        {
            assignedStatement = selectedOperator == "NOT"
                ? $"NOT {partA}"
                : $"{partA} {selectedOperator} {partB}";
            truthValue = 'N';
            return;
        }

        bool valueA = valCharA == 'T';
        bool valueB = valCharB == 'T';

        switch (selectedOperator)
        {
            case "AND":
                assignedStatement = $"{partA} AND {partB}";
                truthValue = (valueA && valueB) ? 'T' : 'F';
                break;

            case "OR":
                assignedStatement = $"{partA} OR {partB}";
                truthValue = (valueA || valueB) ? 'T' : 'F';
                break;

            case "NOT":
                assignedStatement = $"NOT {partA}";
                truthValue = (!valueA) ? 'T' : 'F';
                break;
        }
    }



    void Update()
    {
        if (!launched && gameObject == GameManager.activeBlock)
        {
            Vector3 movement = moveDirection * speed * Time.deltaTime / (MultiBlock ? 2 : 1);
            transform.position += movement;

            distanceTraveled += movement.magnitude;

            if (distanceTraveled >= maxTravelDistance)
            {
                GameManager.CurrentStrikes++;

                Vector3 explosionForce = new Vector3(
                    Random.Range(-10f, 10f),
                    Random.Range(10f, 80f),
                    Random.Range(-10f, 10f)
                );

                Launch(explosionForce);
                GameManager.activeBlock = null;
            }
        }

    }

}
