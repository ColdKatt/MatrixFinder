using System.Collections.Generic;
using UnityEngine;

namespace MatrixFinder.Matrices
{
    /// <summary>
    /// Special comparer for matrices
    /// </summary>
    public class MatrixComparer : IEqualityComparer<Matrix4x4>
    {
        private const double INACCURACY = 1e-4;

        public bool Equals(Matrix4x4 x, Matrix4x4 y)
        {
            return Mathf.Abs(x.m00 - y.m00) < INACCURACY
                 && Mathf.Abs(x.m10 - y.m10) < INACCURACY
                 && Mathf.Abs(x.m20 - y.m20) < INACCURACY
                 && Mathf.Abs(x.m30 - y.m30) < INACCURACY
                 && Mathf.Abs(x.m01 - y.m01) < INACCURACY
                 && Mathf.Abs(x.m11 - y.m11) < INACCURACY
                 && Mathf.Abs(x.m21 - y.m21) < INACCURACY
                 && Mathf.Abs(x.m31 - y.m31) < INACCURACY
                 && Mathf.Abs(x.m02 - y.m02) < INACCURACY
                 && Mathf.Abs(x.m12 - y.m12) < INACCURACY
                 && Mathf.Abs(x.m22 - y.m22) < INACCURACY
                 && Mathf.Abs(x.m32 - y.m32) < INACCURACY
                 && Mathf.Abs(x.m03 - y.m03) < INACCURACY
                 && Mathf.Abs(x.m13 - y.m13) < INACCURACY
                 && Mathf.Abs(x.m23 - y.m23) < INACCURACY
                 && Mathf.Abs(x.m33 - y.m33) < INACCURACY;
        }

        public int GetHashCode(Matrix4x4 obj)
        {
            return obj.GetHashCode();
        }
    }
}