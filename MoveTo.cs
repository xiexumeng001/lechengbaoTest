using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public Transform beginTran;
    public Transform endTran;
    public float time;
    public bool isPingpong;
    public MoveTypeEnum moveType;

    public enum MoveTypeEnum
    {
        Constant,    //匀速
        EaseIn,
        EaseOut,
        EaseInOut,
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(move(gameObject, beginTran.transform.position, endTran.transform.position, time, isPingpong));
    }

    // Update is called once per frame
    void Update()
    {

    }


    //IEnumerator move(GameObject gameObj, Vector3 begin, Vector3 end, float time, bool pingpong)
    //{
    //    Vector3 speed = (end - begin) / time;
    //    float runTime = 0;
    //    while (true)
    //    {
    //        runTime += Time.deltaTime;
    //        bool isArrive = (runTime > time);
    //        if (isArrive)
    //        {
    //            runTime = time;
    //        }

    //        //更新位置
    //        Vector3 curVec = begin + speed * runTime;
    //        gameObj.transform.position = curVec;


    //        //若到达,则复原
    //        if (isArrive)
    //        {
    //            if (!pingpong) { break; }

    //            Vector3 temp = begin;
    //            begin = end;
    //            end = temp;
    //            speed = (end - begin) / time;
    //            runTime = 0;
    //        }

    //        yield return null;
    //    }
    //}


    IEnumerator move(GameObject gameObj, Vector3 begin, Vector3 end, float time, bool pingpong)
    {
        float runTime = 0;
        while (true)
        {
            runTime += Time.deltaTime;
            bool isArrive = (runTime > time);
            if (isArrive)
            {
                runTime = time;
            }

            //更新位置
            Vector3 pos = Vector3.zero;
            switch (moveType)
            {
                case MoveTypeEnum.Constant:
                    pos = GetPos_Constant(begin, end, runTime, time);
                    break;
                case MoveTypeEnum.EaseIn:
                    pos = GetPos_EaseIn(begin, end, runTime, time);
                    break;
                case MoveTypeEnum.EaseOut:
                    pos = GetPos_EaseOut(begin, end, runTime, time);
                    break;
                case MoveTypeEnum.EaseInOut:
                    pos = GetPos_EaseInOut(begin, end, runTime, time);
                    break;
            }
            gameObj.transform.position = pos;


            //若到达,则复原
            if (isArrive)
            {
                if (!pingpong) { break; }

                Vector3 temp = begin;
                begin = end;
                end = temp;
                runTime = 0;
            }

            yield return null;
        }
    }


    Vector3 GetPos_Constant(Vector3 begin, Vector3 end, float curTime, float allTime)
    {
        Vector3 length = end - begin;
        float progress = curTime / allTime;
        return begin + length * progress;
    }

    Vector3 GetPos_EaseIn(Vector3 begin, Vector3 end, float curTime, float allTime)
    {
        Vector3 length = end - begin;
        float progress = curTime / allTime;
        progress = progress * progress;
        return begin + length * progress;
    }

    Vector3 GetPos_EaseOut(Vector3 begin, Vector3 end, float curTime, float allTime)
    {
        Vector3 length = end - begin;
        float progress = curTime / allTime;
        progress = (2 - progress) * progress;
        return begin + length * progress;
    }

    Vector3 GetPos_EaseInOut(Vector3 begin, Vector3 end, float curTime, float allTime)
    {
        float halfTime = allTime / 2;
        Vector3 centerPos = (begin + end) / 2;
        if (curTime < halfTime)
        {
            return GetPos_EaseIn(begin, centerPos, curTime, halfTime);
        }
        else
        {
            curTime = curTime - halfTime;
            return GetPos_EaseOut(centerPos, end, curTime, halfTime);
        }
    }
}
