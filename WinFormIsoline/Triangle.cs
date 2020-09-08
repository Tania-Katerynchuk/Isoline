using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static System.Math;

namespace WinFormIsoline
{
    class Triangle
    {
        public Vector3[] p = new Vector3[3];

        public Triangle(Vector3 P1, Vector3 P2, Vector3 P3)
        {
            this.p[0] = P1;
            this.p[1] = P2;
            this.p[2] = P3;
        }
        public int IndexOfZ(float z)
        {
            for(int i = 0; i < 3; i++)
            {
                if (p[i].Z == z)
                {
                    return i;
                }
            }
            return 0;
        }
        public int MidIndexZ(int min, int max)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i != min && i != max)
                {
                    return i;
                }
            }
            return 0;
        }

        //public float MinPointZ()
        //{
        //    return Min(Min(p[0].Z, p[1].Z), p[2].Z);
        //}
        //public float MaxPointZ()
        //{
        //    return Max(Max(p[0].Z, p[1].Z), p[2].Z);
        //}
    }
}
