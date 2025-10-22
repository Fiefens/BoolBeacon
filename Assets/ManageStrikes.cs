using UnityEngine;
using UnityEngine.UI;

public class StrikeIndicator : MonoBehaviour
{
    public int Number;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("StrikeIndicator requires an Image component.");
            enabled = false;
        }
    }

    void Start()
    {
        UpdateStrikeVisual();
    }

    void Update()
    {
        UpdateStrikeVisual();
    }

    void UpdateStrikeVisual()
    {
        if (Number > GameManager.TotalAllowedStrikes)
        {
            image.enabled = false;
            return;
        }

        image.enabled = true;

        if (Number > GameManager.CurrentStrikes)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.red;
        }
    }
}
