

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ScoreChangeEvent
{
    public int NewScore;
    public int OldScore;
    public int Delta;
}

public delegate void ScoreEvent(ScoreChangeEvent e);

public delegate void BaseEvent<T>(T e);

public class ScoreManager
{
    private static int score = 0;

    public static int MaxScore = 2100;
    public static int Score { get => score; }

    public static event BaseEvent<ScoreChangeEvent> OnScoreChanged;
    public static event BaseEvent<ScoreChangeEvent> OnScoreIncreased;
    public static event BaseEvent<ScoreChangeEvent> OnScoreDecreased;

    public static void Add(int delta)
    {
        int oldScore = score;
        score += delta;

        if (score == oldScore)
        {
            //nothing changed, early exit
            return;
        }

        ScoreChangeEvent eventPayload = new ScoreChangeEvent
        {
            NewScore = score,
            OldScore = oldScore,
            Delta = delta
        };

        // fire score changed event
        OnScoreChanged?.Invoke(eventPayload);

        if (score > oldScore)
        {
            // fire score increased event
            OnScoreIncreased?.Invoke(eventPayload);
        }
        else
        {
            // fire score decreased event
            OnScoreDecreased?.Invoke(eventPayload);
        }
    }

    public static void Set(int value)
    {
        int delta = value - score;
        Add(delta);
    }
}
