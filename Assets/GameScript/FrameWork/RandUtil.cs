using System.Collections.Generic;

static public class RandUtil
{
    static double multiplier = 3125;
    static double moder = 34359738337;
    static double randomSeed = 1.0;



    static public double RandDouble()
    {
        double next_seed = randomSeed * multiplier % moder;
        double ret = next_seed / moder;
        randomSeed = next_seed;

        return ret;
    }

    static public int RandInRange(int min, int max)
    {
        if (min == max) return min;

        if (min > max)
        {
            int temp = min;
            min = max;
            max = temp;
        }

        int range = max - min;
        int rand = (int)(RandDouble() * range) % range;
        return min + rand;
    }

    static public int RandInRange(List<int> arr)
    {
        if (arr == null || arr.Count <= 0) return 0;

        int min = arr[0];
        int max = arr.Count > 1 ? arr[1] : min;

        return RandInRange(min, max);
    }
    static public int PickRandValue(List<int> list)
    {
        if (list == null)
            return 0;
        int idx = RandInRange(0, list.Count - 1);
        return list[idx];
    }


    static public void Srand(int seed)
    {
        if (seed == 0)
        {
            //Debugger.LogError("Error:srand(0)");
            seed = 1;
        }

        randomSeed = seed;
    }

    /// <summary>
    /// 根据权重 随机一个ID
    /// </summary>
    /// <param name="dataSource">key=ID value=weight</param>
    /// <returns></returns>
    public static int RandomByWeight(List<KeyValuePair<int, int>> dataSource)
    {
        if (dataSource.Count == 0)
            return -1;
        if (dataSource.Count == 1)
            return dataSource[0].Key;

        int weightSum = 0;
        foreach (var pair in dataSource)
        {
            weightSum += pair.Value;
        }
        int ran = RandInRange(0, weightSum + 1);

        int countSum = 0;
        foreach (var pair in dataSource)
        {
            countSum += pair.Value;
            if (countSum >= ran)
            {
                return pair.Key;
            }
        }
        return dataSource[0].Key;
    }


}


