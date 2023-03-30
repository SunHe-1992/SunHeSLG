using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedBjorn.Utils
{
    public class BehaviourEnabled : MonoBehaviour
    {
        public enum Operator
        {
            ANY,
            ALL
        }

        public Operator Logic;
        public Behaviour Target;
        public List<Behaviour> Sources;

        void Update()
        {
            switch (Logic)
            {
                case Operator.ANY: Target.enabled = Sources.Any(s => s.enabled); break;
                case Operator.ALL: Target.enabled = Sources.All(s => s.enabled); break;
            }

        }
    }
}

