using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBjorn.ProtoTiles;
using YooAsset;

namespace SunHeTBS
{
    public class PawnController : MonoBehaviour
    {
        private Pawn m_Pawn;
        Queue<INode> pathQueue;
        INode nextTile;
        Vector3 destPos;

        HPGaugeController gaugeController;
        Transform pawnModelTrans;
        public float moveSpeed = 10f;
        const float nearDist = 0.02f;

        // Update is called once per frame
        void Update()
        {
            if (m_Pawn.curState == PawnState.Moving)
            {
                FindNextTile();
                UpdateMovingState();
            }
        }
        void FindNextTile()
        {
            if (nextTile == null)
            {
                if (pathQueue != null && pathQueue.Count > 0)
                {
                    nextTile = pathQueue.Dequeue();
                    MapEntity mapEntity = TBSMapService.Inst.map;
                    destPos = mapEntity.WorldPosition(nextTile as TileEntity);
                }
            }
        }
        void UpdateMovingState()
        {
            if (nextTile != null)
            {
                float stepDist = Time.deltaTime / m_Pawn.moveTileTime;
                if (Vector3.Distance(destPos, this.transform.position) < stepDist)
                {
                    nextTile = null;
                }
                else
                {
                    Vector3 distOffset = destPos - transform.position;
                    transform.position += distOffset.normalized * stepDist;

                    //2d don't change rotation
                    //var rotateTo = Quaternion.Euler(0, GetMoveRotation(), 0);
                    //pawnModelTrans.transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, 0.3f);
                }
            }
        }
        //calculate rotate angle
        float GetMoveRotation()
        {
            Vector3 distOffset = destPos - transform.position;
            return Vector3.Angle(Vector3.forward, distOffset);
        }
        public void Initialize(Pawn _pawn)
        {
            m_Pawn = _pawn;
            this.pawnModelTrans = GameObject.Find("model").transform;
            SetPosition();
            LoadHpGuageObj();
        }
        public void SetPosition()
        {
            //set obj to cur position instantly
            MapEntity mapEntity = TBSMapService.Inst.map;
            var curTile = mapEntity.Tile(this.m_Pawn.curPosition);
            Vector3 wPos = mapEntity.WorldPosition(curTile);
            this.transform.position = wPos;
        }
        public void SetPaths(List<INode> tileList)
        {
            this.pathQueue = new Queue<INode>(tileList);
        }
        public void StartMove()
        {
            FindNextTile();
        }
        public void StopMove()
        {
            //pawnModelTrans.transform.rotation = Quaternion.Euler(0, GetMoveRotation(), 0);
            nextTile = null;
            pathQueue = null;
        }

        #region hp gauge

        void LoadHpGuageObj()
        {
            if (this.gaugeController == null)
            {
                string resPath = $"UIPanel/HPGauge";
                AssetHandle handler = YooAssets.LoadAssetAsync<GameObject>(resPath);
                handler.Completed += ModelLoadDone;
            }
        }
        void ModelLoadDone(AssetHandle handle)
        {
            if (handle.AssetObject != null)
            {
                var obj = GameObject.Instantiate(handle.AssetObject as GameObject);
                obj.name = $"HPGauge{m_Pawn.sequenceId}";
                obj.transform.SetParent(this.transform, false);
                gaugeController = obj.AddComponent<HPGaugeController>();
                gaugeController.InitGauge(m_Pawn.camp);
                UpdateHPGauge();
                UpdateWeaponGauge();
            }
        }
        public void UpdateHPGauge()
        {
            if (gaugeController != null)
            {
                gaugeController.SetHpValue(m_Pawn.HP, m_Pawn.GetAttribute().HPMax);
            }
        }
        public void UpdateWeaponGauge()
        {
            gaugeController.SetWeaponIcons(m_Pawn.GetHoldingWeaponType());
        }
        #endregion
    }
}
