using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormIsoline
{
    class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
