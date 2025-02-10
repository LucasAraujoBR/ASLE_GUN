using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class LerpControlledBob
    {
        public float BobDuration = 0.2f;
        public float BobAmount = 0.1f;
        private float m_Offset = 0f;

        public float Offset()
        {
            return m_Offset;
        }

        public IEnumerator DoBobCycle()
        {
            float t = 0f;
            while (t < BobDuration)
            {
                m_Offset = Mathf.Lerp(0f, BobAmount, t / BobDuration);
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;
            while (t < BobDuration)
            {
                m_Offset = Mathf.Lerp(BobAmount, 0f, t / BobDuration);
                t += Time.deltaTime;
                yield return null;
            }

            m_Offset = 0f;
        }
    }

    [System.Serializable]
    public class CurveControlledBob
    {
        public float HorizontalBobRange = 0.33f;
        public float VerticalBobRange = 0.33f;
        public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
        public float VerticaltoHorizontalRatio = 1f;

        private float m_CyclePositionX;
        private float m_CyclePositionY;
        private float m_BobBaseInterval;
        private Vector3 m_OriginalCameraPosition;

        public void Setup(Camera camera, float bobBaseInterval)
        {
            m_BobBaseInterval = bobBaseInterval;
            m_OriginalCameraPosition = camera.transform.localPosition;
        }

        public Vector3 DoHeadBob(float speed)
        {
            float xPos = m_OriginalCameraPosition.x + (Bobcurve.Evaluate(m_CyclePositionX) * HorizontalBobRange);
            float yPos = m_OriginalCameraPosition.y + (Bobcurve.Evaluate(m_CyclePositionY) * VerticalBobRange);

            m_CyclePositionX += (speed * Time.deltaTime) / m_BobBaseInterval;
            m_CyclePositionY += ((speed * Time.deltaTime) / m_BobBaseInterval) * VerticaltoHorizontalRatio;

            if (m_CyclePositionX > 1f) m_CyclePositionX -= 1f;
            if (m_CyclePositionY > 1f) m_CyclePositionY -= 1f;

            return new Vector3(xPos, yPos, 0f);
        }
    }

    [System.Serializable]
    public class FOVKick
    {
        public Camera Camera;
        public float OriginalFov;
        public float FOVIncrease = 3f;
        public float TimeToIncrease = 0.5f;
        public float TimeToDecrease = 0.5f;

        public void Setup(Camera camera)
        {
            Camera = camera;
            OriginalFov = camera.fieldOfView;
        }

        public IEnumerator FOVKickUp()
        {
            float t = 0f;
            while (t < TimeToIncrease)
            {
                Camera.fieldOfView = Mathf.Lerp(OriginalFov, OriginalFov + FOVIncrease, t / TimeToIncrease);
                t += Time.deltaTime;
                yield return null;
            }
            Camera.fieldOfView = OriginalFov + FOVIncrease;
        }

        public IEnumerator FOVKickDown()
        {
            float t = 0f;
            while (t < TimeToDecrease)
            {
                Camera.fieldOfView = Mathf.Lerp(OriginalFov + FOVIncrease, OriginalFov, t / TimeToDecrease);
                t += Time.deltaTime;
                yield return null;
            }
            Camera.fieldOfView = OriginalFov;
        }
    }

    [System.Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot, smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot, smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }
        }

        public void UpdateCursorLock()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;
            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
            return q;
        }
    }

    public static class CrossPlatformInputManager
    {
        public static float GetAxis(string name)
        {
            return Input.GetAxis(name);
        }

        public static float GetAxisRaw(string name)
        {
            return Input.GetAxisRaw(name);
        }

        public static bool GetButton(string name)
        {
            return Input.GetButton(name);
        }

        public static bool GetButtonDown(string name)
        {
            return Input.GetButtonDown(name);
        }

        public static bool GetButtonUp(string name)
        {
            return Input.GetButtonUp(name);
        }
    }
}
