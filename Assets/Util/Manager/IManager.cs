using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Manager
{
    public interface IManager
    {
        public void Init(StageManager gameManager);
        public void Instantiate();
    }
}