using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
namespace SunHeTBS
{
    public class PawnBase
    {
        public Vector3Int curPosition;
        public PawnCamp camp = PawnCamp.Default;
        public bool actionEnd = false;

        public int TilePosId()
        {
            return TBSMapService.Inst.GetTileId(curPosition);
        }
    }
    public class Pawn : PawnBase
    {
        public PawnController controller;
        /// <summary>
        /// 3d model res name
        /// </summary>
        public string modelName;

        public void Init()
        {
            this.LoadModel();
        }
        public override string ToString()
        {
            return $"Pawn camp={camp} pos={curPosition} posId={TilePosId()} model={modelName}";
        }

        public void LoadModel()
        {
            if (this.controller == null)
            {
                string resPath = $"Pawns/{modelName}";
                AssetOperationHandle handler = YooAssets.LoadAssetAsync<GameObject>(resPath);
                handler.Completed += ModelLoadDone;

            }
        }
        void ModelLoadDone(AssetOperationHandle handle)
        {
            if (handle.AssetObject != null)
            {
                var obj = GameObject.Instantiate(handle.AssetObject as GameObject);
                obj.name = GetObjName();
                this.controller = obj.AddComponent<PawnController>();
                this.controller.Initialize(this);
            }
        }
        string GetObjName()
        {
            return this.modelName;
        }

        #region Attribute for test
        public PawnMoveType moveType = PawnMoveType.Ground;
        public int move_points = 0;
        public int atk_range_max = 8;
        public int atk_range_min = 3;

        /// <summary>
        /// pawn will cost extra move points in certain tiles
        /// </summary>
        /// <returns></returns>
        public bool IsExtraMoveCost()
        {
            if (this.moveType == PawnMoveType.Flier)
                return false;
            //any else skill or buff
            return true;
        }
        /// <summary>
        /// foes do not block this unit's movement 
        /// </summary>
        /// <returns></returns>
        public bool IsPassFoe()
        {
            //todo check skill
            return false;
        }
        public int GetAtkRangeMax()
        {
            //todo check this pawn has attack ability 
            return atk_range_max;
        }
        public int GetAtkRangeMin()
        {
            return atk_range_min;
        }
        #endregion
    }
}
