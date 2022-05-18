using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Anim
{
    public class Animator
    {

        public List<Matrix4> m_FinalBoneMatrices = new List<Matrix4>();
        //public Animation m_CurrentAnimation;
        public float m_CurrentTime = 0;
        public float m_DeltaTime = 0;
        public Animation m_CurrentAnimation = null;

        public Animator(Animation anim)
        {
            m_CurrentTime = 0;
            m_CurrentAnimation = anim;
            for(int i = 0; i < 100; i++)
            {
                m_FinalBoneMatrices.Add(Matrix4.Identity);
            }    

            

        }

        public void SetTime(float time)
        {
            m_CurrentTime = time;
            CalculateBoneTransform(m_CurrentAnimation.GetRootNode(), Matrix4.Identity);

            
        }

        public void PlayAnimation(Animation anim)
        {

            m_CurrentAnimation = anim;
            m_CurrentTime = 0;

        }

        public void UpdateAnimation(float dt)
        {

            m_DeltaTime = dt;
            if (m_CurrentAnimation != null)
            {

                m_CurrentTime += m_CurrentAnimation.GetTicksPerSecond() * dt;
                m_CurrentTime = m_CurrentTime % m_CurrentAnimation.GetDuration();
                if(m_CurrentTime>=(m_CurrentAnimation.GetDuration()-1.0f))
                {

                }
                else
                {
                    CalculateBoneTransform(m_CurrentAnimation.GetRootNode(), Matrix4.Identity);
                }
            }
            
        }

        public void CalculateBoneTransform(AssimpNodeData node,Matrix4 parentTransform)
        {
            string nodeName = node.name;
            Matrix4 nodeTransform = node.transformation;
            Bone bone = m_CurrentAnimation.FindBone(nodeName);
            if (bone != null)
            {
                bone.Update(m_CurrentTime);
                nodeTransform = bone.GetLocalTransform();
            }

            Matrix4 globalTransform = nodeTransform * parentTransform;

            var boneInfoMap = m_CurrentAnimation.GetBoneIDMap();

            if (boneInfoMap.ContainsKey(nodeName))
            {
                int boneID = boneInfoMap[nodeName].ID;
                Matrix4 offset = boneInfoMap[nodeName].Offset;
                m_FinalBoneMatrices[boneID] = offset* globalTransform;// *Q.Scene.SceneGlobal.GlobalInverse;
                
                
            }

            for(int i = 0; i < node.childCount; i++)
            {
                CalculateBoneTransform(node.children[i], globalTransform);
            }

        }
        
        public List<Matrix4> GetFinalBoneMatrices()
        {
            return m_FinalBoneMatrices;
        }

    }
}
