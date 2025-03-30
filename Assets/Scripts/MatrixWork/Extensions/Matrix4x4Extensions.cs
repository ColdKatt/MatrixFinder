using UnityEngine;

namespace MatrixFinder.Matrices.Extensions
{
    public static class Matrix4x4Extensions
    {
        public static Matrix4x4 ToMatrix4x4(this MatrixData matrixData)
        {
            return new Matrix4x4(new(matrixData.m00, matrixData.m10, matrixData.m20, matrixData.m30),
                                 new(matrixData.m01, matrixData.m11, matrixData.m21, matrixData.m31),
                                 new(matrixData.m02, matrixData.m12, matrixData.m22, matrixData.m32),
                                 new(matrixData.m03, matrixData.m13, matrixData.m23, matrixData.m33));
        }

        public static MatrixData ToMatrixData(this Matrix4x4 matrix)
        {
            var matrixData = new MatrixData();

            matrixData.m00 = matrix.m00;
            matrixData.m10 = matrix.m10;
            matrixData.m20 = matrix.m20;
            matrixData.m30 = matrix.m30;

            matrixData.m01 = matrix.m01;
            matrixData.m11 = matrix.m11;
            matrixData.m21 = matrix.m21;
            matrixData.m31 = matrix.m31;

            matrixData.m02 = matrix.m02;
            matrixData.m12 = matrix.m12;
            matrixData.m22 = matrix.m22;
            matrixData.m32 = matrix.m32;

            matrixData.m03 = matrix.m03;
            matrixData.m13 = matrix.m13;
            matrixData.m23 = matrix.m23;
            matrixData.m33 = matrix.m33;

            return matrixData;
        }
    }
}