using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Anim
{
    public class Bone
    {

        public List<KeyPosition> m_Positions = new List<KeyPosition>();
        public List<KeyRotation> m_Rotations = new List<KeyRotation>();
        public List<KeyScale> m_Scales = new List<KeyScale>();
        public int m_NumPositions = 0;
        public int m_NumRotations = 0;
        public int m_NumScales = 0;
        public Matrix4 m_LocalTransform;
        public string m_Name;
        public int m_ID;

        public Bone(string name, int id,Assimp.NodeAnimationChannel chan)
        {
            m_Name = name;
            m_ID = id;

            m_LocalTransform = Matrix4.Identity;
            m_NumPositions = chan.PositionKeyCount;
            for (int positionIndex = 0; positionIndex < m_NumPositions; positionIndex++)
            {
                // KeyPosition pos = new KeyPosition(chan.PositionKeys[positionIndex].Time, chan.PositionKeys[positionIndex].Value);
                var apos = chan.PositionKeys[positionIndex].Value;
                float timeStamp = (float)chan.PositionKeys[positionIndex].Time;
                KeyPosition kp = new KeyPosition();
                kp.position = new Vector3(apos.X, apos.Y, apos.Z);
                kp.timeStamp = timeStamp;
                m_Positions.Add(kp);
            }

            m_NumRotations = chan.RotationKeyCount;

            for (int rotationIndex = 0; rotationIndex < m_NumRotations; rotationIndex++)
            {
                // KeyRotation rot = new KeyRotation(chan.RotationKeys[rotationIndex].Time, chan.RotationKeys[rotationIndex].Value);
                var arot = chan.RotationKeys[rotationIndex].Value;
                float timeStamp = (float)chan.RotationKeys[rotationIndex].Time;
                KeyRotation kr = new KeyRotation();
                kr.rotation = new Quaternion(arot.X, arot.Y, arot.Z, arot.W);
                kr.timeStamp = timeStamp;
                m_Rotations.Add(kr);
            }

            m_NumScales = chan.ScalingKeyCount;

            for (int scaleIndex = 0; scaleIndex < m_NumScales; scaleIndex++)
            {
                // KeyScale scale = new KeyScale(chan.ScalingKeys[scaleIndex].Time, chan.ScalingKeys[scaleIndex].Value);
                var ascale = chan.ScalingKeys[scaleIndex].Value;
                float timeStamp = (float)chan.ScalingKeys[scaleIndex].Time;
                KeyScale ks = new KeyScale();
                ks.scale = new Vector3(ascale.X, ascale.Y, ascale.Z);
                ks.timeStamp = timeStamp;
                m_Scales.Add(ks);
            }

            int a = 0;


        }

        public void Update(float animationTime)
        {
            Matrix4 translation = InterpolatePosition(animationTime);
            Matrix4 rotation = InterpolateRotation(animationTime);
            Matrix4 scale = InterpolateScale(animationTime);
            m_LocalTransform =scale*rotation * translation;// * scale;


        }

        public Matrix4 GetLocalTransform()
        {
            return m_LocalTransform;
        }

        public string GetBoneName()
        {
            return m_Name;
        }

        public int GetBoneID()
        {
            return m_ID;
        }


        //get position index
        public int GetPositionIndex(float animationTime)
        {
            for (int index = 0; index < m_NumPositions - 1; index++)
            {
                if (animationTime < m_Positions[index + 1].timeStamp)
                {
                    return index;
                }
            }
            return 0;
        }

        //get rotation index
        public int GetRotationIndex(float animationTime)
        {
            for (int index = 0; index < m_NumRotations - 1; index++)
            {
                if (animationTime < m_Rotations[index + 1].timeStamp)
                {
                    return index;
                }
            }
            return 0;
        }

        //get scale index
        public int GetScaleIndex(float animationTime)
        {
            for (int index = 0; index < m_NumScales - 1; index++)
            {
                if (animationTime < m_Scales[index + 1].timeStamp)
                {
                    return index;
                }
            }
            return 0;
        }

        //get scale factor
        float GetScaleFactor(float lastTimeStamp,float nextTimeStamp,float animationTime)
        {

            float scaleFactor = 0.0f;
            float midWayLength = animationTime - lastTimeStamp;
            float framesDiff = nextTimeStamp - lastTimeStamp;
            scaleFactor = midWayLength / framesDiff;
            return scaleFactor;
        }

        //interpolate position
        Matrix4 InterpolatePosition(float animationTime)
        {
            if (1 == m_NumPositions)
            {
                return Matrix4.CreateTranslation(m_Positions[0].position);
            }
            int p0Index = GetPositionIndex(animationTime);
            int p1Index = p0Index + 1;
            float scaleFactor = GetScaleFactor(m_Positions[p0Index].timeStamp, m_Positions[p1Index].timeStamp, animationTime);
            Vector3 finalPosition = Vector3.Lerp(m_Positions[p0Index].position, m_Positions[p1Index].position, scaleFactor);
            return Matrix4.CreateTranslation(finalPosition);
        }

        Matrix4 InterpolateRotation(float animationTime)
        {
            if (1 == m_NumRotations)
            {
                return Matrix4.CreateFromQuaternion(m_Rotations[0].rotation);
            }
            int r0Index = GetRotationIndex(animationTime);
            int r1Index = r0Index + 1;
            float scaleFactor = GetScaleFactor(m_Rotations[r0Index].timeStamp, m_Rotations[r1Index].timeStamp, animationTime);
            Quaternion finalRotation = Quaternion.Slerp(m_Rotations[r0Index].rotation, m_Rotations[r1Index].rotation, scaleFactor);
            finalRotation.Normalize();
            return Matrix4.CreateFromQuaternion(finalRotation);
        }

        Matrix4 InterpolateScale(float animationTime)
        {
            if (1 == m_NumScales)
            {
                return Matrix4.CreateScale(m_Scales[0].scale);
            }
            int s0Index = GetScaleIndex(animationTime);
            int s1Index = s0Index + 1;
            float scaleFactor = GetScaleFactor(m_Scales[s0Index].timeStamp, m_Scales[s1Index].timeStamp, animationTime);
            Vector3 finalScale = Vector3.Lerp(m_Scales[s0Index].scale, m_Scales[s1Index].scale, scaleFactor);
            return Matrix4.CreateScale(finalScale);
        }


    }
}
