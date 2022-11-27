﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateThesis.Common.ShortUID
{
    internal static class RandomUtils
    {
        private static readonly Random Random = new();
        private static readonly object ThreadLock = new();

        public static int GenerateNumberInRange(int min, int max)
        {
            lock (ThreadLock)
            {
                return Random.Next(min, max);
            }
        }
    }
}