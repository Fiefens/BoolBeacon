using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject camera;
    public static GameObject activeBlock;
    private float currentY = 0.5f;

    public static int Score = 0;
    public TMP_Text ScoreText;

    public static int TotalAllowedStrikes = 3;
    public static int CurrentStrikes = 0;

    private readonly Vector3 centerPositionXZ = new Vector3(3f, 0f, 3f);

    private void Start()
    {
        Score = 0;
    }

    void Update()
    {
        if (activeBlock == null)
        {
            SpawnNewBlock();
        }

        if (activeBlock != null)
        {
            ControlBlock block = activeBlock.GetComponent<ControlBlock>();
            if (block != null)
            {
                bool strikeGiven = false;

                if (Input.GetMouseButtonDown(0))
                {
                    if (block.truthValue == 'T')
                    {
                        PlaceBlock();
                    }
                    else
                    {
                        CurrentStrikes++;
                        strikeGiven = true;
                        Vector3 explosionForce = new Vector3(
                        Random.Range(-10f, 10f),
                        Random.Range(10f, 80f),
                        Random.Range(-10f, 10f)
                        );

                        block.Launch(explosionForce);
                        activeBlock = null;
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (block.truthValue == 'F')
                    {
                        PlaceBlock();

                    }
                    else
                    {
                        CurrentStrikes++;
                        strikeGiven = true;
                        Vector3 explosionForce = new Vector3(
                        Random.Range(-10f, 10f),
                        Random.Range(10f, 80f),
                        Random.Range(-10f, 10f)
                    );

                        block.Launch(explosionForce);
                        activeBlock = null;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (block.truthValue == 'N')
                    {

                    }
                    else
                    {
                        CurrentStrikes++;
                        strikeGiven = true;
                    }

                    Vector3 explosionForce = new Vector3(
                        Random.Range(-10f, 10f),
                        Random.Range(10f, 80f),
                        Random.Range(-10f, 10f)
                    );

                    block.Launch(explosionForce);
                    activeBlock = null;
                }
            }
        }


        float scrollInput = Input.mouseScrollDelta.y;
        if (scrollInput != 0f)
        {
            Vector3 currentCamPos = camera.transform.position;
            float newY = Mathf.Clamp(currentCamPos.y + scrollInput * 2f, 1f, 100f);
            Vector3 targetPos = new Vector3(currentCamPos.x, newY, currentCamPos.z);

            StartCoroutine(MoveCameraY(targetPos, 0.3f));
        }


        ScoreText.text = Score.ToString();

        if (GameManager.CurrentStrikes >= GameManager.TotalAllowedStrikes)
        {
            SceneSwitcher.SwitchEndScene();
        }
    }

    void SpawnNewBlock()
    {
        int direction = Random.Range(0, 4);
        Vector3 startPosition = GetSpawnPositionFromDirection(direction, currentY);
        activeBlock = Instantiate(blockPrefab, startPosition, Quaternion.identity);

        ControlBlock movement = activeBlock.GetComponent<ControlBlock>();
        movement.SetDirection(GetMoveDirection(direction));

        Rigidbody rb = activeBlock.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true; 
    }



    IEnumerator MoveCameraY(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = camera.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);

            camera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        camera.transform.position = targetPosition;
    }


    void PlaceBlock()
    {
        activeBlock.GetComponent<ControlBlock>().enabled = false;
        activeBlock = null;
        currentY += 1f;

        Score++;

        Vector3 newPosition = camera.transform.position + new Vector3(0f, 1f, 0f);
        StartCoroutine(MoveCameraY(newPosition, 0.5f));
    }


    Vector3 GetSpawnPositionFromDirection(int direction, float yLevel)
    {
        float offset = 15f;
        switch (direction)
        {
            case 0: return new Vector3(3f + offset, yLevel, 3f); // East
            case 1: return new Vector3(3f - offset, yLevel, 3f); // West
            case 2: return new Vector3(3f, yLevel, 3f + offset); // North
            case 3: return new Vector3(3f, yLevel, 3f - offset); // South
            default: return new Vector3(3f + offset, yLevel, 3f);
        }
    }

    Vector3 GetMoveDirection(int direction)
    {
        switch (direction)
        {
            case 0: return Vector3.left;   // From East
            case 1: return Vector3.right;  // From West
            case 2: return Vector3.back;   // From North
            case 3: return Vector3.forward; // From South
            default: return Vector3.zero;
        }
    }
}
