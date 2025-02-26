using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public struct FindPathData
    {
        public List<Vector2Int> Path;
        public ushort NextIndex;
    }
    
    public class GridDataComponent : ECSComponent
    {
        public GridData Value;
    }

    public class FindPathComponent : ECSComponent
    {
        public FindPathData Value;
    }
}