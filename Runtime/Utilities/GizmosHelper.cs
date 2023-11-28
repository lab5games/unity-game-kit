using UnityEngine;

#if UNITY_EDITOR
using Handles = UnityEditor.Handles;
using HandleUtility = UnityEditor.HandleUtility;
#endif

namespace Lab5Games
{
    public static class GizmosHelper
    {
        #region Gizmos
        public struct ColorScope : System.IDisposable
        {
            Color _oldColor;

            public ColorScope(Color newColor)
            {
                _oldColor = Gizmos.color;
                Gizmos.color = newColor == default(Color) ? _oldColor : newColor;
            }

            public void Dispose()
            {
                Gizmos.color = _oldColor;
            }
        }

        public struct Box
        {
            public Vector3 localFrontTopLeft { get; private set; }
            public Vector3 localFrontTopRight { get; private set; }
            public Vector3 localFrontBottomLeft { get; private set; }
            public Vector3 localFrontBottomRight { get; private set; }
            public Vector3 localBackTopLeft { get { return -localFrontBottomRight; } }
            public Vector3 localBackTopRight { get { return -localFrontBottomLeft; } }
            public Vector3 localBackBottomLeft { get { return -localFrontTopRight; } }
            public Vector3 localBackBottomRight { get { return -localFrontTopLeft; } }

            public Vector3 frontTopLeft { get { return localFrontTopLeft + origin; } }
            public Vector3 frontTopRight { get { return localFrontTopRight + origin; } }
            public Vector3 frontBottomLeft { get { return localFrontBottomLeft + origin; } }
            public Vector3 frontBottomRight { get { return localFrontBottomRight + origin; } }
            public Vector3 backTopLeft { get { return localBackTopLeft + origin; } }
            public Vector3 backTopRight { get { return localBackTopRight + origin; } }
            public Vector3 backBottomLeft { get { return localBackBottomLeft + origin; } }
            public Vector3 backBottomRight { get { return localBackBottomRight + origin; } }

            public Vector3 origin { get; private set; }
            public Quaternion orientation { get; private set; }
            public Vector3 size { get; private set; }

            public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
            {
                this.orientation = orientation;

                Rotate(orientation);
            }

            public Box(Vector3 origin, Vector3 halfExtents) : this()
            {
                this.origin = origin;
                this.orientation = Quaternion.identity;
                this.size = halfExtents * 2;

                this.localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
                this.localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
                this.localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
            }

            public void Rotate(Quaternion orientation)
            {
                localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
                localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
                localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
                localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
            }

            static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
            {
                Vector3 direction = point - pivot;
                return pivot + rotation * direction;
            }
        }

        public static void DrawPoint(Vector3 position, float scale = 1.0f, Color color = default(Color))
        {
            using (new ColorScope(color))
            {
                Gizmos.DrawRay(position + (Vector3.up * (scale * 0.5f)), -Vector3.up * scale);
                Gizmos.DrawRay(position + (Vector3.right * (scale * 0.5f)), -Vector3.right * scale);
                Gizmos.DrawRay(position + (Vector3.forward * (scale * 0.5f)), -Vector3.forward * scale);
            }
        }

        public static void DrawRay(Vector3 position, Vector3 direction, Color color = default(Color))
        {
            using (new ColorScope(color))
            {
                Gizmos.DrawRay(position, direction);
            }
        }

        public static void DrawLine(Vector3 from, Vector3 to, Color color = default(Color))
        {
            using (new ColorScope(color))
            {
                Gizmos.DrawLine(from, to);
            }
        }

        public static void DrawBounds(Bounds bounds, Color color = default(Color))
        {
            Vector3
                ruf = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z),
                rub = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z),
                luf = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z),
                lub = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z),
                rdf = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z),
                rdb = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z),
                lfd = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z),
                lbd = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);

            using (new ColorScope(color))
            {
                Gizmos.DrawLine(ruf, luf);
                Gizmos.DrawLine(ruf, rub);
                Gizmos.DrawLine(luf, lub);
                Gizmos.DrawLine(rub, lub);

                Gizmos.DrawLine(ruf, rdf);
                Gizmos.DrawLine(rub, rdb);
                Gizmos.DrawLine(luf, lfd);
                Gizmos.DrawLine(lub, lbd);

                Gizmos.DrawLine(rdf, lfd);
                Gizmos.DrawLine(rdf, rdb);
                Gizmos.DrawLine(lfd, lbd);
                Gizmos.DrawLine(lbd, rdb);
            }
        }

        public static void DrawCircle(Vector3 position, Vector3 up, float radius = 1.0f, Color color = default(Color))
        {
            up = up.normalized * radius;
            Vector3
                forward = Vector3.Slerp(up, -up, 0.5f),
                right = Vector3.Cross(up, forward).normalized * radius;

            Matrix4x4 matrix = new Matrix4x4()
            {
                m00 = right.x,
                m10 = right.y,
                m20 = right.z,

                m01 = up.x,
                m11 = up.y,
                m21 = up.z,

                m02 = forward.x,
                m12 = forward.y,
                m22 = forward.z
            };

            Vector3
                lastPoint = position + matrix.MultiplyPoint3x4(new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0))),
                nextPoint = Vector3.zero;

            using (new ColorScope(color))
            {
                for (int i = 0; i <= 90; i++)
                {
                    nextPoint = position + matrix.MultiplyPoint3x4(
                        new Vector3(
                            Mathf.Cos((i * 4) * Mathf.Deg2Rad),
                            0f,
                            Mathf.Sin((i * 4) * Mathf.Deg2Rad)
                            )
                        );
                    Gizmos.DrawLine(lastPoint, nextPoint);
                    lastPoint = nextPoint;
                }
            }
        }

        public static void DrawCylinder(Vector3 start, Vector3 end, float radius = 1.0f, Color color = default(Color))
        {
            Vector3
                up = (end - start).normalized * radius,
                forward = Vector3.Slerp(up, -up, 0.5f),
                right = Vector3.Cross(up, forward).normalized * radius;

            //Radial circles
            DrawCircle(start, up, radius, color);
            DrawCircle(end, -up, radius, color);
            DrawCircle((start + end) * 0.5f, up, radius, color);

            using (new ColorScope(color))
            {
                //Side lines
                Gizmos.DrawLine(start + right, end + right);
                Gizmos.DrawLine(start - right, end - right);

                Gizmos.DrawLine(start + forward, end + forward);
                Gizmos.DrawLine(start - forward, end - forward);

                //Start endcap
                Gizmos.DrawLine(start - right, start + right);
                Gizmos.DrawLine(start - forward, start + forward);

                //End endcap
                Gizmos.DrawLine(end - right, end + right);
                Gizmos.DrawLine(end - forward, end + forward);
            }
        }

        public static void DrawCone(Vector3 position, Vector3 direction, float angle = 45f, Color color = default(Color))
        {
            float length = direction.magnitude;
            angle = Mathf.Clamp(angle, 0f, 90f);

            Vector3
                forward = direction,
                up = Vector3.Slerp(forward, -forward, 0.5f),
                right = Vector3.Cross(forward, up).normalized * length,
                slerpedVector = Vector3.Slerp(forward, up, angle / 90.0f);

            Plane farPlane = new Plane(-direction, position + forward);
            Ray distRay = new Ray(position, slerpedVector);

            float dist;
            farPlane.Raycast(distRay, out dist);

            using (new ColorScope(color))
            {
                Gizmos.DrawRay(position, slerpedVector.normalized * dist);
                Gizmos.DrawRay(position, Vector3.Slerp(forward, -up, angle / 90.0f).normalized * dist);
                Gizmos.DrawRay(position, Vector3.Slerp(forward, right, angle / 90.0f).normalized * dist);
                Gizmos.DrawRay(position, Vector3.Slerp(forward, -right, angle / 90.0f).normalized * dist);

            }
            DrawCircle(position + forward, direction, (forward - (slerpedVector.normalized * dist)).magnitude, color);
            DrawCircle(position + (forward * 0.5f), direction, ((forward * 0.5f) - (slerpedVector.normalized * (dist * 0.5f))).magnitude, color);
        }

        public static void DrawArrow(Vector3 position, Vector3 direction, float angle = 15f, float headLength = 0.3f, Color color = default(Color))
        {
            if (direction == Vector3.zero)
                return; // can't draw a thing
            if (angle < 0f)
                angle = Mathf.Abs(angle);
            if (angle > 0f)
            {
                float length = direction.magnitude;
                float arrowLength = length * Mathf.Clamp01(headLength);
                Vector3 headDir = direction.normalized * -arrowLength;
                DrawCone(position + direction, headDir, angle, color);
            }
            using (new ColorScope(color))
            {
                Gizmos.DrawRay(position, direction);
            }
        }

        public static void DrawCapsule(Vector3 point1, Vector3 point2, float radius = 1f, Color color = default(Color))
        {
            if (point1 == point2)
            {
                using (new ColorScope(color))
                {
                    Gizmos.DrawWireSphere(point1, radius);
                }
            }
            else
            {
                float
                    height = (point1 - point2).magnitude,
                    sideLength = Mathf.Max(0, (height * 0.5f));

                Vector3
                    up = (point2 - point1).normalized * radius,
                    forward = Vector3.Slerp(up, -up, 0.5f),
                    right = Vector3.Cross(up, forward).normalized * radius,
                    middle = (point2 + point1) * 0.5f;

                point1 = middle + ((point1 - middle).normalized * sideLength);
                point2 = middle + ((point2 - middle).normalized * sideLength);

                //Radial circles
                DrawCircle(point1, up, radius, color);
                DrawCircle(point2, -up, radius, color);

                using (new ColorScope(color))
                {
                    //Side lines
                    Gizmos.DrawLine(point1 + right, point2 + right);
                    Gizmos.DrawLine(point1 - right, point2 - right);

                    Gizmos.DrawLine(point1 + forward, point2 + forward);
                    Gizmos.DrawLine(point1 - forward, point2 - forward);

                    for (int i = 1; i < 26; i++)
                    {
                        //Start endcap
                        Gizmos.DrawLine(Vector3.Slerp(right, -up, i / 25.0f) + point1, Vector3.Slerp(right, -up, (i - 1) / 25.0f) + point1);
                        Gizmos.DrawLine(Vector3.Slerp(-right, -up, i / 25.0f) + point1, Vector3.Slerp(-right, -up, (i - 1) / 25.0f) + point1);
                        Gizmos.DrawLine(Vector3.Slerp(forward, -up, i / 25.0f) + point1, Vector3.Slerp(forward, -up, (i - 1) / 25.0f) + point1);
                        Gizmos.DrawLine(Vector3.Slerp(-forward, -up, i / 25.0f) + point1, Vector3.Slerp(-forward, -up, (i - 1) / 25.0f) + point1);

                        //End endcap
                        Gizmos.DrawLine(Vector3.Slerp(right, up, i / 25.0f) + point2, Vector3.Slerp(right, up, (i - 1) / 25.0f) + point2);
                        Gizmos.DrawLine(Vector3.Slerp(-right, up, i / 25.0f) + point2, Vector3.Slerp(-right, up, (i - 1) / 25.0f) + point2);
                        Gizmos.DrawLine(Vector3.Slerp(forward, up, i / 25.0f) + point2, Vector3.Slerp(forward, up, (i - 1) / 25.0f) + point2);
                        Gizmos.DrawLine(Vector3.Slerp(-forward, up, i / 25.0f) + point2, Vector3.Slerp(-forward, up, (i - 1) / 25.0f) + point2);
                    }
                }
            }
        }

        public static void DrawFrustum(Camera camera, Color color = default(Color))
        {
            if(camera == null)
                camera = Camera.main;

            using (new ColorScope(color))
            {
                Gizmos.matrix = Matrix4x4.TRS(camera.transform.position, camera.transform.rotation, Vector3.one);
                Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
                Gizmos.matrix = Matrix4x4.identity;
            }
        }

        public static void DrawPlane(Vector3 start, Vector3 end, Vector3 upward, float height = 1f, Color color = default(Color))
        {
            float width = Vector3.Distance(start, end);
            if (Mathf.Approximately(width, 0f))
                return;

            using (new ColorScope(color))
            {
                Quaternion rotation =
                    Quaternion.LookRotation(end - start, upward) *
                    Quaternion.Euler(0f, -90f, 0f);
                Gizmos.matrix = Matrix4x4.TRS(start, rotation, Vector3.one);
                Gizmos.DrawCube(
                    new Vector3(width * 0.5f, height * 0.5f, 0f),
                    new Vector3(width, height, float.Epsilon));
                Gizmos.matrix = Matrix4x4.identity;
            }
        }

        public static void DrawSphere(Vector3 center, float radius, Color color = default(Color))
        {
            using (new ColorScope(color))
            {
                Gizmos.DrawSphere(center, radius);
            }
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, bool isWired = true, Color color = default(Color))
        {
            DrawBox(new Box(origin, halfExtents, orientation), isWired, color);
        }

        public static void DrawBox(Box box, bool isWired = true, Color color = default(Color))
        {
            using (new ColorScope(color))
            {
                if (isWired)
                {
                    Gizmos.DrawLine(box.frontTopLeft, box.frontTopRight);
                    Gizmos.DrawLine(box.frontTopRight, box.frontBottomRight);
                    Gizmos.DrawLine(box.frontBottomRight, box.frontBottomLeft);
                    Gizmos.DrawLine(box.frontBottomLeft, box.frontTopLeft);

                    Gizmos.DrawLine(box.backTopLeft, box.backTopRight);
                    Gizmos.DrawLine(box.backTopRight, box.backBottomRight);
                    Gizmos.DrawLine(box.backBottomRight, box.backBottomLeft);
                    Gizmos.DrawLine(box.backBottomLeft, box.backTopLeft);

                    Gizmos.DrawLine(box.frontTopLeft, box.backTopLeft);
                    Gizmos.DrawLine(box.frontTopRight, box.backTopRight);
                    Gizmos.DrawLine(box.frontBottomRight, box.backBottomRight);
                    Gizmos.DrawLine(box.frontBottomLeft, box.backBottomLeft);
                }
                else
                {
                    Gizmos.matrix = Matrix4x4.TRS(box.origin, box.orientation, Vector3.one);
                    Gizmos.DrawCube(box.origin, box.size);
                    Gizmos.matrix = Matrix4x4.identity;
                }
            }
        }
        #endregion

        #region Handles
#if UNITY_EDITOR
        public struct HandlesColorScope : System.IDisposable
        {
            Color _oldColor;

            public HandlesColorScope(Color newColor)
            {
                _oldColor = Handles.color;
                Handles.color = newColor == default(Color) ? Handles.color : newColor;
            }

            public void Dispose()
            {
                Handles.color = _oldColor;
            }
        }

        public static bool IsHandleHackAvailable
        {
            get
            {
                return UnityEditor.SceneView.currentDrawingSceneView != null || 
                    (Application.isPlaying && Camera.main != null);
            }
        }

        public static float GetHandleSize(Vector3 center)
        {
            return HandleUtility.GetHandleSize(center);
        }
#endif

        public static void DrawLabel(Vector3 position, string text, Vector2 offset = default(Vector2), GUIStyle style = default(GUIStyle), Color color = default(Color))
        {
#if UNITY_EDITOR
            if (IsHandleHackAvailable)
            {
                Transform cam = UnityEditor.SceneView.currentDrawingSceneView != null ?
                    UnityEditor.SceneView.currentDrawingSceneView.camera.transform :
                    Camera.main.transform;

                if (Vector3.Dot(cam.forward, position - cam.position) > 0)
                {
                    Vector3 textPos = position;

                    if(offset != default(Vector2))
                    {
                        Vector3 camRightVector = cam.right * offset.x;
                        textPos += camRightVector + new Vector3(0, offset.y, 0);
                    }

                    if (style == default(GUIStyle))
                    {
                        if(color == default(Color))
                        {
                            Handles.Label(textPos, text, GUI.skin.textArea);
                        }
                        else
                        {
                            style = new GUIStyle(GUI.skin.textArea);
                            Color oldTextColor = style.normal.textColor;
                            style.normal.textColor = color;
                            Handles.Label(textPos, text, style);
                            style.normal.textColor = oldTextColor;
                        }
                    }
                    else
                    {
                        if (color == default(Color))
                        {
                            Handles.Label(textPos, text, style);
                        }
                        else
                        {
                            Color oldTextColor = style.normal.textColor;
                            style.normal.textColor = color;
                            Handles.Label(textPos, text, style);
                            style.normal.textColor = oldTextColor;  
                        }
                    }
                }
            }
#endif
        }

        public static void DrawArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, bool constantScreenSize = true, Color color = default(Color))
        {
#if UNITY_EDITOR
            if(IsHandleHackAvailable)
            {
                if (constantScreenSize)
                    radius *= GetHandleSize(center);

                using (new HandlesColorScope(color))
                {
                    Handles.DrawSolidArc(center, normal, from, angle, radius);
                }
            }
#endif
        }

        public static void DrawAngleBetween(Vector3 center, Vector3 from, Vector3 to, Vector3 axis, float radius, bool label, bool constantScreenSize = true, Color color = default(Color))
        {
#if UNITY_EDITOR
            if(IsHandleHackAvailable)
            {
                float angle = Vector3.SignedAngle(from, to, axis);
                DrawArc(center, axis, from, angle, radius, constantScreenSize, color);
                if (label)
                {
                    float factor = constantScreenSize ? HandleUtility.GetHandleSize(center) : 1f;
                    Vector3 labelPos = center + (Vector3.Lerp(from, to, 0.5f) * factor);
                    DrawLabel(labelPos, $"{angle:F2}");
                }
            }
#endif
        }
#endregion
    }
}
