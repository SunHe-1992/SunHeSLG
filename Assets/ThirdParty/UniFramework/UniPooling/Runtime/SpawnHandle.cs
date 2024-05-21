using UnityEngine;
using YooAsset;

namespace UniFramework.Pooling
{
    public sealed class SpawnHandle : GameAsyncOperation
    {
        private enum ESteps
        {
            None,
            Waiting,
            Done,
        }

        private readonly GameObjectPool _pool;
        private InstantiateOperation _operation;
        private readonly Transform _parent;
        private readonly Vector3 _position;
        private readonly Quaternion _rotation;
        private ESteps _steps = ESteps.None;

        private bool selfDestroy = false;
        private float timer = 0f;
        /// <summary>
        /// 实例化的游戏对象
        /// </summary>
        public GameObject GameObj
        {
            get
            {
                if (_operation == null)
                {
                    UniLogger.Warning("The spawn handle is invalid !");
                    return null;
                }

                return _operation.Result;
            }
        }

        /// <summary>
        /// 用户自定义数据集
        /// </summary>
        public System.Object[] UserDatas { private set; get; }

        private SpawnHandle()
        {
        }
        internal SpawnHandle(GameObjectPool pool, InstantiateOperation operation, Transform parent, Vector3 position, Quaternion rotation, params System.Object[] userDatas)
        {
            _pool = pool;
            _operation = operation;
            _parent = parent;
            _position = position;
            _rotation = rotation;
            UserDatas = userDatas;
        }
        protected override void OnStart()
        {
            _steps = ESteps.Waiting;
        }
        protected override void OnUpdate()
        {
            if (selfDestroy && _steps == ESteps.Done)
            {
                timer -= Time.deltaTime;
                if (timer < 0f)
                {
                    this.Restore();
                }
            }
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if (_steps == ESteps.Waiting)
            {
                if (_operation.IsDone == false)
                    return;

                if (_operation.Status != EOperationStatus.Succeed)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _operation.Error;
                    return;
                }

                if (_operation.Result == null)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Clone game object is null.";
                    return;
                }

                // 设置参数	
                _operation.Result.transform.SetParent(_parent);
                _operation.Result.transform.SetPositionAndRotation(_position, _rotation);
                _operation.Result.SetActive(true);

                _steps = ESteps.Done;
                Status = EOperationStatus.Succeed;
            }
        }
        protected override void OnAbort()
        {
        }

        /// <summary>
        /// 回收
        /// </summary>
        public void Restore()
        {
            if (_operation != null)
            {
                ClearCompletedCallback();
                CancelHandle();
                _pool.Restore(_operation);
                _operation = null;
                selfDestroy = false;
                timer = 0;
            }
        }

        /// <summary>
        /// 丢弃
        /// </summary>
        public void Discard()
        {
            if (_operation != null)
            {
                ClearCompletedCallback();
                CancelHandle();
                _pool.Discard(_operation);
                _operation = null;
                selfDestroy = false;
                timer = 0;
            }
        }

        /// <summary>
        /// 等待异步实例化结束
        /// </summary>
        public void WaitForAsyncComplete()
        {
            if (_operation != null)
            {
                if (_steps == ESteps.Done)
                    return;
                _operation.WaitForAsyncComplete();
                OnUpdate();
            }
        }

        private void CancelHandle()
        {
            if (IsDone == false)
            {
                _steps = ESteps.Done;
                Status = EOperationStatus.Failed;
                Error = $"User cancelled !";
            }
        }
        public void SetSelfDestroy(float _timer)
        {
            this.selfDestroy = true;
            this.timer = _timer;
        }
        public void SetNoSelfDestroy()
        {
            this.selfDestroy = false;
            this.timer = 0;
        }
    }
}
