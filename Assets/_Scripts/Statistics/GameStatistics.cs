using System;
using System.Collections.Generic;
using UnityEngine;

public enum Statistic
{
    TimePlayed,
    DistanceTraveled,
    ObjectsDestroyed,
    PigsExploded,
    PassengersDelivered,
    PassengersStolen,
    TimesFarted,
    TotalAirFlips,
    TimeInAir,
    DistanceDrifted,
    TimesKnockedBack,
    CartsFixed,
    GravesRobbed,
    FencesBreached,
    TreesChopped,

    [HideInInspector] Count
}

/// <summary>
/// How to add statistics:
/// 
/// Step 1. Add stat name to Statistic enum | 
/// 
/// Step 2. Wherever the statistic is altered (e.g. 'objects destroyed' in health script), add value to it (e.g. GameStatistics.GetStat(Statistic.ObjectsDestroyed).Value += 1) |
/// 
/// Step 3. Access it wherever using GameStatistics.GetStat(Statistic.StatisticName).value or get a callback with GameStatistics.GetStat(Statistic.StatisticName).changed += FunctionName (Function will need to take parameters (float oldVal, float newVal)
/// </summary>
public class GameStatistics : MonoBehaviour
{
    public static Dictionary<Statistic, Observable<float>> m_statDict = new();

    private void Awake()
    {
        for (int i = 0; i < (int)Statistic.Count; i++) m_statDict.Add((Statistic)i, new());
    }

    /// <summary>
    /// Returns a reference type observable float. Val.Changed<float 'oldval', float 'newVal'> callback will be called every time value is changed.
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static Observable<float> GetStat(Statistic _type) => m_statDict[_type];

    private void Update()
    {
        UpdateStats();

        Debug.Log("Objects Destroyed: " + GetStat(Statistic.ObjectsDestroyed).Value);
    }

    void UpdateStats()
    {
        GetStat(Statistic.TimePlayed).Value += Time.deltaTime; // Increases TimePlayed statistic
    }
}

#region Observable<T>

#region PropertyDrawer

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(Observable), true)]
public class ObservableDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        // dirty hack for displaying drop down as straight value
        var value = property.FindPropertyRelative("_value");
        UnityEditor.EditorGUI.PropertyField(position, value, label, true);
    }

    public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label)
    {
        var value = property.FindPropertyRelative("_value");

        return UnityEditor.EditorGUI.GetPropertyHeight(value, label, true);
    }
}
#endif
#endregion

public abstract class Observable { }

[Serializable]
public class Observable<T> : Observable
{
    [SerializeField] private T _value;

    public class ChangedEventArgs : EventArgs
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }
    }

    public EventHandler<ChangedEventArgs> Changed;

    public T Value
    {
        get { return _value; }

        set
        {
            if (!value.Equals(_value))
            {
                T oldValue = _value;
                _value = value;

                EventHandler<ChangedEventArgs> handler = Changed;
                if (handler != null)
                    handler(this, new ChangedEventArgs
                    {
                        OldValue = oldValue,
                        NewValue = value
                    });
            }
        }
    }
}
#endregion