using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
namespace SunHeSLG
{
    public class PawnBase
    {
        public Vector3Int curPosition;
        public PawnCamp camp = PawnCamp.Default;
        public bool actionEnd = false;
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
    }
}
