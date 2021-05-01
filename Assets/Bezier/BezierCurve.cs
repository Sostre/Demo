using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [SerializeField] public Transform[] referencePointTransforms;
    [SerializeField] public Transform curvePointTransform;
    Vector3[] mReferencePoints;
    int mSign = 1;
    float t = 0;
    int mResolution = 100;

    void Update() {
        TransformsToVectors();
        t = Mathf.Clamp(t, 0, 1);
        // curvePointTransform.position = GetCurvePointDeCasteljau(mReferencePoints, t);
        curvePointTransform.position = GetCurvePointByBernstein(mReferencePoints, t);
        t += Time.deltaTime * 0.2f * mSign;
        if(t > 1 || t < 0)
            mSign *= -1;
    }

    void OnDrawGizmos() {
        TransformsToVectors();
        if(mReferencePoints.Length == 0)
            return;

        //�����Ƶ�����
        Gizmos.color = Color.red;
        for(int i = 0; i < mReferencePoints.Length - 1; i++)
            Gizmos.DrawLine(mReferencePoints[i], mReferencePoints[i + 1]);

        //������
        Gizmos.color = Color.green;
        Vector3 start = mReferencePoints[0];
        for(int i = 1; i <= mResolution; i++) {
            Vector3 end = GetCurvePointDeCasteljau(mReferencePoints, (float)i / mResolution);
            Gizmos.DrawLine(start, end);
            start = end;
        }
    }

    void TransformsToVectors() {
        int referencePointCount = referencePointTransforms.Length;
        mReferencePoints = new Vector3[referencePointCount];
        for(int i = 0; i < referencePointCount; i++)
            mReferencePoints[i] = referencePointTransforms[i].position;
    }

    //de Casteljau �㷨��������һ��
    public static Vector3 GetCurvePointDeCasteljau(Vector3[] referencePoint, float t) {
        int len = referencePoint.Length;
        if(len < 2)
            Debug.LogError("���Ƶ�������Ҫ����");

        Vector3[] newPoint = new Vector3[len - 1];
        for(int i = 0; i < len - 1; i++)
            newPoint[i] = Vector3.Lerp(referencePoint[i], referencePoint[i + 1], t);
        if(len == 2)
            return newPoint[0];
        else
            return GetCurvePointDeCasteljau(newPoint, t);
    }

    //����˹̹����ʽ������������һ��
    Vector3 GetCurvePointByBernstein(Vector3[] referencePoint, float t) {
        uint i = 0;
        int n = referencePoint.Length - 1;// n ���� n+1 ������
        Vector3 point = Vector3.zero;
        for(; i <= n; i++)
            point += BernsteinNum(i, (uint)n, t) * referencePoint[i];
        return point;
    }

    //����˹̹����ʽ
    float BernsteinNum(uint i, uint n, float t) {
        return CombinatorialNum(i, n) * Mathf.Pow(t, i) * Mathf.Pow(1.0f - t, n - i);
    }

    //�����
    ulong CombinatorialNum(uint i, uint n) {
        if(i > n)
            return 0;
        return Factorial(n) / (Factorial(i) * Factorial(n - i));
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
