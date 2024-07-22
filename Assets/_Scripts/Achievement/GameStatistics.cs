using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Statistic
{
    public Observable<int> value;
    public StatisticType type;
}

public enum StatisticType
{
    TimePlayed,
    DistanceTraveled,
    ObjectsDestroyed
}

public class GameStatistics : MonoBehaviour
{
    //public List<Observable<int>> statistics = new
    //{
    //    timePlayed = new Observable<int>(),
    //    distanceTraveled = new Observable<int>(),
    //    objectsDestroyed = new Observable<int>(),
    //    pigsExploded = new Observable<int>(),
    //    passengersDelivered = new Observable<int>(),
    //    passengersStolen = new Observable<int>(),
    //    timesFarted = new Observable<int>(),
    //    totalAirFlips = new Observable<int>(),
    //    timeInAir = new Observable<int>(),
    //    distanceDrifted = new Observable<int>(),
    //    timesKnockedBack = new Observable<int>(),
    //    cartsFixed = new Observable<int>(),
    //    gravesRobbed = new Observable<int>(),
    //    fencesBreached = new Observable<int>(),
    //    treesChopped = new Observable<int>(),
    //};
    public List<Statistic> statistics = new();
    private void Awake()
    {
        //statistics.Add(new Statistic() { value = new(), ID = 0 });
    }


    // Probably the better way ------------------------------------------------ below


    //public static Observable<int> timePlayed = new();
    //public static Observable<int> distanceTraveled = new();
    //public static Observable<int> objectsDestroyed = new();
    //public static Observable<int> pigsExploded = new();
    //public static Observable<int> passengersDelivered = new();
    //public static Observable<int> passengersStolen = new();
    //public static Observable<int> timesFarted = new();
    //public static Observable<int> totalAirFlips = new();
    //public static Observable<int> timeInAir = new();
    //public static Observable<int> distanceDrifted = new();
    //public static Observable<int> timesKnockedBack = new();
    //public static Observable<int> cartsFixed = new();
    //public static Observable<int> gravesRobbed = new();
    //public static Observable<int> fencesBreached = new();
    //public static Observable<int> treesChopped = new();
}

#region Observable<T>
public class Observable<T>
{
    private T _value;

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