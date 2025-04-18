﻿using System.Collections.Generic;
using GameFrame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GXGame
{
    public class FindPathData : IVersions
    {
        public List<Vector2Int> Path;
        public ushort NextIndex;
        public bool IsFindPath;
        public int Versions { get; set; }
    }

    public class GridDataComponent : ECSComponent
    {
        public GridData Value;
    }

    public class FindPathComponent : ECSComponent
    {
        [ShowInInspector] public FindPathData Value;
    }
}