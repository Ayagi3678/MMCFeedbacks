using System;
using System.Collections;
using System.Collections.Generic;
using MMCFeedbacks.Core;
using UnityEngine;

public class PerformanceTest : MonoBehaviour
{
    [SerializeField] private FeedbackPlayer player;
    [SerializeField] private int count;
    private void Update()
    {
        for (int i = 0; i < count; i++)
        {
            player.PlayFeedbacks();   
        }
    }
}
