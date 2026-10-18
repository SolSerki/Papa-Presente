using UnityEngine;
using UnityEngine.UI;

public class RelationshipBar : MonoBehaviour
{
    [SerializeField] private Image pinkFill;
    [SerializeField] private RectTransform indicator;
    [SerializeField] private float barWidth;

    private RelationshipManager relationshipManager;


    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        barWidth = ((RectTransform)pinkFill.transform).rect.width;
        Debug.Log($"[RelationshipBar] width:{barWidth}");
        UpdateBar(RelationshipManager.Instance.InitialBondingStat);

        relationshipManager = RelationshipManager.Instance;
        relationshipManager.OnLevelChanged += UpdateBar;
     
        
    }

    public void UpdateBar(int currentRelationshipStat)
    {
        float _fillLevel = (float) currentRelationshipStat / RelationshipManager.Instance.MaxBondingStat;

        pinkFill.fillAmount = _fillLevel;
        Debug.Log($"[RelationshipBar] width:{barWidth}, _fillLevel={_fillLevel}, pinkFill.fillAmount {pinkFill.fillAmount}" +
            $", currentRelationshipStat={currentRelationshipStat},  RelationshipManager.Instance.MaxBondingStat={RelationshipManager.Instance.MaxBondingStat}");
        float xPos = Mathf.Lerp(-barWidth / 2f, barWidth/2f, _fillLevel);
        indicator.anchoredPosition = new Vector2(xPos, indicator.anchoredPosition.y);

    }
}
