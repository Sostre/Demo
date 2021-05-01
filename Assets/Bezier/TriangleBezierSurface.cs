using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TriangleReferencePoint
{
    public string ijk;
    public Transform trans;

    public bool isEqual(uint i, uint j, uint k) {
        uint num = uint.Parse(ijk);
        if(100 * i + 10 * j + k == num)
            return true;
        return false;
    }
}

public class TriangleBezierSurface : MonoBehaviour
{
    public uint n = 3;
    public TriangleReferencePoint[] referencePoints;
    int mResolution = 50;

    Vector3[,,] PointsToVector3s() {
        Vector3[,,] array = new Vector3[n + 1, n + 1, n + 1];
        for(uint i = 0; i <= n; i++) {
            for(uint j = 0; j <= n - i; j++) {
                uint k = n - i - j;
                array[i, j, k] = FindReferencePoint(i, j, k).trans.position;
            }
        }

        return array;
    }

    TriangleReferencePoint FindReferencePoint(uint i, uint j, uint k) {
        foreach(var point in referencePoints) {
            if(point.isEqual(i, j, k))
                return point;
        }
        Debug.LogError("û�ҵ���Ӧ���Ƶ㣬����Ŀ��Ƶ��±�");
        return new TriangleReferencePoint();
    }

    void OnDrawGizmos() {
        if((n + 1) * (n + 2) / 2 != referencePoints.Length) {
            Debug.LogError("��������ƥ��");
            return;
        }
        Gizmos.color = Color.green;
        Vector3[,,] referencePointPositions = PointsToVector3s();
        float u, v, w;
        uint i, j, k;
        float step = 1.0f / mResolution;
        //ȡ��������
        for(u = 0; u <= 1.0f; u += step) {
            for(v = 0; v <= 1.0f - u; v += step) {
                w = 1.0f - u - v;

                //de Casteljau
                currentN = n;
                Vector3 p = GetSurfacePointDeCasteljau(referencePointPositions, u, v, w);

                //����������Ĳ���˹̹����ʽ������ÿ��(u,v,w)��Ӧ�������ϵĵ�P(u,v,w)
                //Vector3 p = Vector3.zero;
                //for(i = 0; i <= n; i++) {
                //    for(j = 0; j <= n - i; j++) {
                //        k = n - i - j;
                //        p += TriangleBernsteinNum(i, j, k, n, u, v, w) * referencePointPositions[i, j, k];
                //    }
                //}

                Gizmos.DrawSphere(p, 0.02f);
            }
        }
    }

    //��¼��ǰ�Ľ���
    uint currentN;
    //de Casteljau �㷨��������һ��
    Vector3 GetSurfacePointDeCasteljau(Vector3[,,] referencePoint, float u, float v, float w) {
        currentN--;
        Vector3[,,] newReferencePoint = new Vector3[currentN + 1, currentN + 1, currentN + 1];
        for(uint i = 0; i <= currentN; i++) {
            for(uint j = 0; j <= currentN - i; j++) {
                uint k = currentN - i - j;
                newReferencePoint[i, j, k] = u * referencePoint[i + 1, j, k] + v * referencePoint[i, j + 1, k] +
                                             w * referencePoint[i, j, k + 1];
            }
        }
        if(currentN == 0)
            return newReferencePoint[0, 0, 0];
        else
            return GetSurfacePointDeCasteljau(newReferencePoint, u, v, w);
    }

    //������Ĳ���˹̹����ʽ
    float TriangleBernsteinNum(uint i, uint j, uint k, uint n, float u, float v, float w) {
        return TriangleCombinatorialNum(i, j, k, n) * Mathf.Pow(u, i) * Mathf.Pow(v, j) * Mathf.Pow(w, k);
    }

    //������������
    ulong TriangleCombinatorialNum(uint i, uint j, uint k, uint n) {
        if(i + j + k != n)
            return 0;
        return Factorial(n) / (Factorial(i) * Factorial(j) * Factorial(k));
    }

    //�׳�
    ulong Factorial(uint n) {
        if(n == 0)
            return 1;
        ulong result = n;
        for(n--; n > 0; n--)
            result *= n;
        return result;
    }
}
