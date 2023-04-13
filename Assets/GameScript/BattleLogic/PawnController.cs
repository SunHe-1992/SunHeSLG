using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedBjorn.ProtoTiles;
namespace SunHeTBS
{
    public class PawnController : MonoBehaviour
    {
        private Pawn m_Pawn;
        Queue<INode> pathQueue;
        INode nextTile;
        Vector3 destPos;

        public float moveSpeed = 10f;
        const float nearDist = 0.02f;
        // Start is called before the first frame update
        void Start()
        {

        }

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
                    var rotateTo = Quaternion.Euler(0, GetMoveRotation(), 0);
                    this.transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, 0.3f);
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
            SetPosition();
        }
        void SetPosition()
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
            this.transform.rotation = Quaternion.Euler(0, GetMoveRotation(), 0);
            nextTile = null;
            pathQueue = null;
        }
    }
}
