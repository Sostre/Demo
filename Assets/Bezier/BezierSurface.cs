using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSurface : MonoBehaviour
{
    [SerializeField]public BezierCurve[] curves;
    int mResolution = 100;

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        for(int i = 1; i <= mResolution; i++) {
            //��ȡÿ�������� t ʱ�̵ĵ�
            Vector3[] curvePoints = new Vector3[curves.Length];
            for(int j = 0; j < curvePoints.Length; j++) {
                Vector3[] referencePoint = GetCurveReferencePoint(curves[j]);
                curvePoints[j] = BezierCurve.GetCurvePointDeCasteljau(referencePoint, (float)i / mResolution);
            }

            //���������õĵ��ٻ�����
            Vector3 start = curvePoints[0];
            for(int j = 1; j <= mResolution; j++) {
                Vector3 end = BezierCurve.GetCurvePointDeCasteljau(curvePoints, (float)j / mResolution);
                Gizmos.DrawLine(start, end);
                start = end;
            }

            // ��Щ��ֱ�����ߵĴ���ʾ��
            // Vector3 start = curvePoints[0];
            // for (int j = 1; j < curvePoints.Length; j++)
            // {
            //     Vector3 end = curvePoints[j];
            //     Gizmos.DrawLine(start, end);
            //     start = end;
            // }
        }
    }

    Vector3[] GetCurveReferencePoint(BezierCurve curve) {
        Vector3[] array = new Vector3[curve.referencePointTransforms.Length];
        for(int i = 0; i < array.Length; i++)
            array[i] = curve.referencePointTransforms[i].position;
        return array;
    }
}
