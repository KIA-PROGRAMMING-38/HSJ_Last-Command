using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Manager
{
    public interface IManager
    {
        public void Init(GameManager gameManager);
        public void Instantiate();
    }
}