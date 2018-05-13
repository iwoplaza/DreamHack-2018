
using UnityEngine;

public class ChunkDebugging : MonoBehaviour
{
    public Transform mytransform;
    public Vector3 rotationPlane;
    public Vector4 posGot;
    public Plane camPlane;

    public void Update()
    {
        Camera m_camera = GetComponent<Camera>();
        camPlane = new Plane(m_camera.transform.rotation * Vector3.forward,0);
		Matrix4x4 projectionMatrix = m_camera.projectionMatrix;
		Matrix4x4 worldToLocalMatrix = m_camera.transform.worldToLocalMatrix;
		float clippingDistance = m_camera.farClipPlane;
		camPlane.distance = -camPlane.GetDistanceToPoint(m_camera.transform.position);
		rotationPlane = Quaternion.LookRotation(camPlane.normal).eulerAngles;
        posGot = GetPos(projectionMatrix,worldToLocalMatrix,mytransform.position,camPlane,clippingDistance);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(camPlane.ClosestPointOnPlane(transform.position),camPlane.ClosestPointOnPlane(transform.position) + camPlane.normal * 250);
    }   

    Vector4 GetPos(Matrix4x4 _projectionMatrix, Matrix4x4 _worldToLocalMatrix, Vector3 _position, Plane _camPlane, float _clippingDistance)
    {
        Matrix4x4 VP = _projectionMatrix * _worldToLocalMatrix;
        Vector4 v = VP * new Vector4( _position.x, _position.y, _position.z, 1 );        
        Vector4 ViewportPoint = v / -v.w;
        ViewportPoint.y = 0.5f + ViewportPoint.y/2f;
        ViewportPoint.x = 0.5f + ViewportPoint.x/2f;
        ViewportPoint.w = v.z;
		Vector3 CamRotation = Quaternion.LookRotation(_camPlane.normal).eulerAngles;
		if(CamRotation.y > 137.5f && CamRotation.y < 313.5f){
			ViewportPoint.z = _camPlane.GetDistanceToPoint(_position)/_clippingDistance;
		}else
			ViewportPoint.z = _camPlane.GetDistanceToPoint(_position)/_clippingDistance;
        return ViewportPoint;
    }
}
