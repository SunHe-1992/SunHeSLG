using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UniFramework.Pooling;
using UniFramework.Singleton;



public class MinigameService : ISingleton

{
    public static MinigameService Inst;
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<MinigameService>();
    }

    public void OnCreate(object createParam)
    {
        Inst = this;
    }

    public void OnDestroy()
    {
        Inst = null;
    }

    public void OnUpdate()
    {
    }

    #region module: slot mini game

    public void SetUpSlotGameData()
    {
        slotGameData = new SlotData();
        slotGameData.RandomData();
    }
    public SlotData slotGameData;
    public class SlotData
    {
        public List<List<int>> slotIdList;
        readonly int wheelIconCount = 20;
        public long rewardMoney = 0;
        readonly int rewardSlot = 6;
        readonly int winRate = 50; //win rate percent
        public bool gotReward;
        public void RandomData()
        {
            slotIdList = new List<List<int>>();
            for (int i = 0; i < 3; i++)
            {
                List<int> subList = new List<int>();
                for (int j = 0; j < wheelIconCount; j++)
                {
                    subList.Add(GetRandomSlotNumber());
                }
                slotIdList.Add(subList);
            }
            // reward rate 50%
            if (Random.Range(0, 100) < winRate)
            {
                gotReward = true;
                //add reward random number
                int rewardNumber = GetRandomSlotNumber();
                foreach (var list in slotIdList)
                {
                    //replace the number
                    list[rewardSlot] = rewardNumber;
                }
                Debugger.Log("force reward id = " + rewardNumber);
                rewardMoney = rewardDic[rewardNumber];
            }
            else //make sure no reward this time
            {
                gotReward = false;
                rewardMoney = 1;
                slotIdList[0][rewardSlot] = Random.Range(0, 3);
                slotIdList[1][rewardSlot] = Random.Range(3, 5);
                slotIdList[2][rewardSlot] = Random.Range(3, 5);
                Debugger.Log("no force reward id ");
            }
        }
        //icon [0,5]
        int GetRandomSlotNumber()
        {
            return Random.Range(0, 5);
        }
        Dictionary<int, long> rewardDic = new Dictionary<int, long>()
        {
            {0, 10000 },
            {1, 20000 },
            {2, 40000 },
            {3, 80000 },
            {4, 100000 },
        };
    }
    #endregion

    #region module: bank heist

    public GameBankHeist gameBankHeist { get; set; }
    public void SetUpBankHeistData()
    {
        gameBankHeist = new GameBankHeist();
        gameBankHeist.RandomData();
    }
    public class GameBankHeist
    {
        //each slot has a tokenId
        public List<int> tokenList;
        public Dictionary<int, long> rewardDic;
        public void RandomData()
        {
            tokenList = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                tokenList.Add(GetRandomToken());
            }

            rewardDic = new Dictionary<int, long>() {
                {0, 10000},
                {1, 20000},
                {2, 50000},
            };
        }
        int GetRandomToken()
        {
            return Random.Range(0, 3);
        }



    }
    #endregion

}
