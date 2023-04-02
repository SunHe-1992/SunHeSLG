using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBjorn.ProtoTiles;
namespace SunHeSLG
{
    public class PawnController : MonoBehaviour
    {
        private Pawn m_Pawn;
        Queue<TileEntity> pathQueue;
        TileEntity nextTile;
        Vector3 destPos;
        bool moving = false;

        float moveSpeed = 10f;
        const float nearDist = 0.02f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (moving)
            {
                if (nextTile == null)
                {
                    if (pathQueue != null && pathQueue.Count > 0)
                    {
                        nextTile = pathQueue.Dequeue();
                        MapEntity mapEntity = BattleDriver.Instance.GetMapEntity();

                        destPos = mapEntity.WorldPosition(nextTile);
                    }
                    else
                        this.moving = false;
                }
                float stepDist = moveSpeed * Time.deltaTime;
                if (Vector3.Distance(destPos, this.transform.position) < stepDist)
                {
                    nextTile = null;
                }
                else
                {
                    Vector3 distOffset = destPos - transform.position;
                    transform.position += distOffset.normalized * stepDist;
                }
            }
        }
        public void Initialize(Pawn _pawn)
        {
            m_Pawn = _pawn;
            SetPosition();
        }
        void SetPosition()
        {
            //set obj to cur position instantly
            MapEntity mapEntity = BattleDriver.Instance.GetMapEntity();
            var curTile = mapEntity.Tile(this.m_Pawn.curPosition);
            Vector3 wPos = mapEntity.WorldPosition(curTile);
            this.transform.position = wPos;
        }
        public void SetPaths(List<TileEntity> tileList)
        {
            this.pathQueue = new Queue<TileEntity>(tileList);
        }
        public void StartMove()
        {

        }
    }
}
