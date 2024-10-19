using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class AnimationJoin
    {
        public static string GetAnimation(params string[] tags)
        {
            return string.Join("_", tags);
        }
    }
}
