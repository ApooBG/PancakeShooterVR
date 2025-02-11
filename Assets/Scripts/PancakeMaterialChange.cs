using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PancakeMaterialChange : MonoBehaviour
{
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material normalMaterial;
    [SerializeField] float MaximumCookingTime;
    [SerializeField] float PerfectCookingTime;
    [SerializeField] float TimeToStayPerfect;
    [SerializeField] TextMeshProUGUI timerUI;

    [SerializeField] Color almostReadyColor;
    [SerializeField] Color burntColor;
    float cookingTime = 0f;
    private Renderer renderer;
    private MaterialPropertyBlock propertyBlock;

    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }
    public void CookPancake()
    {
        if (cookingTime < MaximumCookingTime)
        {
            cookingTime += Time.deltaTime;

            // Update the material color based on cooking time
            if (cookingTime <= PerfectCookingTime)
            {
                float lerpFactor = cookingTime / PerfectCookingTime;
                Color color = Color.Lerp(whiteMaterial.color, almostReadyColor, lerpFactor);
                propertyBlock.SetColor("_Color", color);
                renderer.SetPropertyBlock(propertyBlock);
                UpdateUI(Color.yellow);
            }
            else if (cookingTime > PerfectCookingTime && cookingTime <= PerfectCookingTime + TimeToStayPerfect)
            {
                // Switch to normal material after reaching perfect cooking time
                renderer.material = normalMaterial;
                UpdateUI(Color.green);
            }
            else if (cookingTime > PerfectCookingTime + TimeToStayPerfect && cookingTime <= MaximumCookingTime)
            {
                float lerpFactor = (cookingTime - (PerfectCookingTime + TimeToStayPerfect)) / (MaximumCookingTime - (PerfectCookingTime + TimeToStayPerfect));
                Color color = Color.Lerp(normalMaterial.color, burntColor, lerpFactor);
                propertyBlock.SetColor("_Color", color);
                renderer.SetPropertyBlock(propertyBlock);
                UpdateUI(Color.red);
            }
        }
        else
        {
            // Pancake is overcooked
            Debug.Log("Pancake Overcooked");
            UpdateUI(Color.black);
        }
    }

    void UpdateUI(Color color)
    {
        timerUI.text = ((int)cookingTime).ToString() + "s";
        timerUI.color = color;
    }

    public bool IsEdible()
    {
        if (cookingTime > PerfectCookingTime && cookingTime <= PerfectCookingTime + TimeToStayPerfect + 1)
        {
            return true;
        }

        if (cookingTime < PerfectCookingTime && cookingTime >= PerfectCookingTime - 1)
        {
            return true;
        }

        return false;
    }
}
