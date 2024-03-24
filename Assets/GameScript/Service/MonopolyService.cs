using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UniFramework.Pooling;
using UniFramework.Singleton;



public class MonopolyService : ISingleton

{
    public static MonopolyService Inst;
    public static void Init()
    {
        Inst = UniSingleton.CreateSingleton<MonopolyService>();
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
        }
        //icon [0,5]
        int GetRandomSlotNumber()
        {
            return Random.Range(0, 5);
        }
    }
    #endregion
}
