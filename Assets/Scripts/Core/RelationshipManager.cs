using System;
using UnityEditor;
using UnityEngine;

public class RelationshipManager : MonoBehaviour
{
    public static RelationshipManager Instance { get; private set; }
    [SerializeField] private int _initialBondingStat = 35;
    [SerializeField] private int _maxBondingStat = 100;
    [SerializeField] private int _minBondingStat = 0;
    [SerializeField] private int _currentBondingStat;
    [SerializeField] private int _positiveAmount = 2;
    [SerializeField] private int _negativeAmount = -5;
    [SerializeField] private int DIFFICULTY_LEVEL = 1;
    public int CurrentBondingStat
    {
        get { return _currentBondingStat; }
        set { _currentBondingStat = value; }
    }

    /// <summary>
    /// Subscribe to update UI
    /// </summary>
    public event Action<int, int> OnLevelChanged; // (prevLevel, newLevel)

    private void Start()
    {
        LoadInitialValues();
    }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
   
    }


    public void LoadInitialValues()
    {
        _currentBondingStat = _initialBondingStat;
    }
    /// <summary>
    /// The way you responde to requests impacts relationship.
    /// It updates your relationship status every time you interact with the family members
    /// </summary>
    /// <param name="impact"></param> it can be positive + or negative -
    public void ApplyInteractionImpact(bool isPositive, int bonus)
    {
        int impact = CalculateImpactAmount(isPositive, bonus);
        Debug.Log($"[ApplyInteractionImpact] Interaction nature positive:{isPositive} impact:{impact}, bonus {bonus}.");
        int prev = _currentBondingStat;
        _currentBondingStat = Mathf.Clamp(_currentBondingStat + impact, _minBondingStat ,_maxBondingStat ); // the function limits the range
        Debug.Log($"[ApplyInteractionImpact] Previous relationship stat:{prev}, new: {_currentBondingStat}.");
        if (_currentBondingStat != prev)
        {
            OnLevelChanged?.Invoke(prev, _currentBondingStat); // let the listener know
            //Save();                               // should we do anything else?
        }
    }

    private int CalculateImpactAmount(bool isPositive, int bonus)
    {
            if (isPositive)
        {
            return (_positiveAmount + bonus) * DIFFICULTY_LEVEL;
        }
        else
        {
            int negativeResult = (_negativeAmount + bonus) * DIFFICULTY_LEVEL;
            Debug.Log($"negativeResult {negativeResult}");
            return  negativeResult;
        }

    }

    public void Save()
    {

    }
}
