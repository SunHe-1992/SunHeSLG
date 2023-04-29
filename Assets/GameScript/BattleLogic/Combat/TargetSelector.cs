using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SunHeTBS
{
    public class TargetSelector
    {
        /// <summary>
        /// select healable allies in range
        /// </summary>
        /// <param name="pList"></param>
        /// <param name="speller"></param>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Pawn> SelectHealAllies(List<Pawn> pList, PawnCamp speller, Vector3Int pos, int range)
        {
            var mapInst = TBSMapService.Inst.map;
            List<Pawn> selectPawns = new List<Pawn>();
            foreach (Pawn p in pList)
            {
                if (mapInst.Distance(p.curPosition, pos) <= range)//in range
                {
                    if (PawnCampTool.CampsFriend(speller, p.camp))//is friend
                    {
                        //todo hp not full
                        selectPawns.Add(p);
                    }
                }
            }
            return selectPawns;
        }
        /// <summary>
        /// select foes in range
        /// </summary>
        /// <param name="pList"></param>
        /// <param name="speller"></param>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Pawn> SelectTargetFoes(List<Pawn> pList, PawnCamp speller, Vector3Int pos, int rangeMin, int rangeMax)
        {
            var mapInst = TBSMapService.Inst.map;
            List<Pawn> selectPawns = new List<Pawn>();
            foreach (Pawn p in pList)
            {
                int dist = mapInst.Distance(p.curPosition, pos);
                if (dist <= rangeMax && dist >= rangeMin)//in range
                {
                    if (PawnCampTool.CampsHostile(speller, p.camp))//is friend
                    {
                        selectPawns.Add(p);
                    }
                }
            }
            return selectPawns;
        }
        /// <summary>
        /// select allies in range
        /// </summary>
        /// <param name="pList"></param>
        /// <param name="speller"></param>
        /// <param name="pos"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public List<Pawn> SelectTargetAllies(List<Pawn> pList, PawnCamp speller, Vector3Int pos, int range)
        {
            var mapInst = TBSMapService.Inst.map;
            List<Pawn> selectPawns = new List<Pawn>();
            foreach (Pawn p in pList)
            {
                if (mapInst.Distance(p.curPosition, pos) <= range)//in range
                {
                    if (PawnCampTool.CampsFriend(speller, p.camp))//is friend
                    {
                        selectPawns.Add(p);
                    }
                }
            }
            return selectPawns;
        }
    }
}
