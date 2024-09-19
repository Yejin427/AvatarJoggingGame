using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParseJson{
    [System.Serializable]
public class StateData
{
    public string State;
    public string Time;
    public string Speed;

    public StateData(string state, string time, string speed)
    {
        State = state;
        Time = time;
        Speed = speed;
    }
}

[System.Serializable]
public class StateDataList
{
    public List<StateData> stateDataList;

    public StateDataList(List<StateData> list)
    {
        stateDataList = list;
    }
}
}
