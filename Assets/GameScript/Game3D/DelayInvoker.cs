using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerUnit
{
    public bool repeat;
    public float targetTime;
    public float interval;
    public Action action;
}


public class TimerContainer
{
    private List<TimerUnit> timerList;

    public TimerContainer()
    {
        timerList = new List<TimerUnit>();
    }

    public void AddTimer(TimerUnit timer)
    {
        timerList.Add(timer);
    }

    public bool RemoveTimer(Action action)
    {
        if (timerList == null || timerList.Count <= 0)
        {
            return false;
        }

        int timerCount = timerList.Count;
        for (int i = 0; i < timerCount; i++)
        {
            if (timerList[i].action == action)
            {
                timerList.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public void PerformAction(float fixedTime)
    {
        if (timerList == null || timerList.Count <= 0)
        {
            return;
        }
        int lastIndex = timerList.Count - 1;
        for (int i = lastIndex; i >= 0; i--)
        {
            TimerUnit tu = timerList[i];
            if (tu.targetTime <= fixedTime)
            {
                if (tu.action != null)
                {
                    tu.action();
                }

                if (timerList.Count > i)
                    timerList.RemoveAt(i);

                if (tu.repeat)
                {
                    DelayInvoker.Inst.AppendTimerUnit(tu, tu.interval);
                }
            }
        }
    }
}

public class DelayInvoker : MonoBehaviour
{

    private static DelayInvoker instance_;
    public static DelayInvoker Inst
    {
        get
        {
            if (instance_ == null)
                instance_ = new DelayInvoker();
            return instance_;
        }
    }

    private Dictionary<Action, Coroutine> ActionDict;

    void Awake()
    {
        if (instance_ != null)
        {
            Destroy(gameObject);
            return;
        }
        instance_ = this;
        ActionDict = new Dictionary<Action, Coroutine>();
        DontDestroyOnLoad(this);
    }

    public void ClearAllInfos()
    {
        if (ActionDict != null)
            ActionDict.Clear();
    }


    public void InitData()
    {

    }

    public void DelayInvoke(Action action, float delay)
    {
        Coroutine co = null;
        if (ActionDict.TryGetValue(action, out co))
        {
            //Debugger.LogWarning("One Delay Action Already Exist, will reset it's delay time");
            ActionDict.Remove(action);
            StopCoroutine(co);
        }

        co = StartCoroutine(Invoke(action, delay));
        ActionDict.Add(action, co);
    }

    private IEnumerator Invoke(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);


        action();

        ActionDict.Remove(action);

    }

    public void NextFrameInvoke(Action action, int frameCount = 1)
    {
        if (action == null) return;
        StartCoroutine(FrameDelayInvoke(action, frameCount));
    }


    public void NextFrameInvoke(Action<object> action, object param, int frameCount = 1)
    {
        if (action == null) return;
        StartCoroutine(FrameDelayInvoke(action, param, frameCount));
    }

    private IEnumerator FrameDelayInvoke(Action action, int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }

        action();
    }

    private IEnumerator FrameDelayInvoke(Action<object> action, object param, int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }

        action(param);
    }


    //移除一个延迟执行的动作
    public void RemoveDelayAction(Action action)
    {
        if (!ActionDict.ContainsKey(action))
        {
            return;
        }
        Coroutine co = ActionDict[action];
        StopCoroutine(co);
        ActionDict.Remove(action);
    }

    private TimerContainer[] timerArray;
    private int currIndex = 0;
    private const int TIMER_ARRAY_SIZE = 600;

    // 添加周期执行的操作函数，注意，不要在周期函数内部删除自身，会删不掉
    public void AddRepeatAction(Action action, float delay, float interval)
    {
        if (timerArray == null)
        {
            timerArray = new TimerContainer[TIMER_ARRAY_SIZE]; //够一分钟的
            currIndex = 0;
        }

        TimerUnit tu = new TimerUnit();
        tu.repeat = true;
        tu.targetTime = Time.fixedTime + delay;
        tu.interval = interval;
        tu.action = action;

        int idx = (currIndex + (int)(delay / Time.fixedDeltaTime) + 1) % TIMER_ARRAY_SIZE;
        if (timerArray[idx] == null)
        {
            timerArray[idx] = new TimerContainer();
        }
        timerArray[idx].AddTimer(tu);
    }

    public void AppendTimerUnit(TimerUnit tu, float delay)
    {
        if (timerArray == null)
        {
            timerArray = new TimerContainer[TIMER_ARRAY_SIZE];
            currIndex = 0;
        }
        int idx = (currIndex + (int)(delay / Time.fixedDeltaTime) + 1) % TIMER_ARRAY_SIZE;
        if (timerArray[idx] == null)
        {
            timerArray[idx] = new TimerContainer();
        }
        timerArray[idx].AddTimer(tu);
    }

    public void RemoveRepeatAction(Action action)
    {
        if (timerArray == null)
        {
            return;
        }

        for (int i = 0; i < TIMER_ARRAY_SIZE; i++)
        {
            TimerContainer tc = timerArray[i];
            if (tc == null) continue;
            if (tc.RemoveTimer(action))
            {
                break;
            }
        }
    }

    //检查执行周期性执行的动作
    public void FixedUpdate()
    {
        if (timerArray == null) return;

        currIndex = (currIndex + 1) % TIMER_ARRAY_SIZE;

        TimerContainer tc = timerArray[currIndex];

        if (tc == null) return;

        float currTime = Time.fixedTime;
        tc.PerformAction(currTime);
    }
}
