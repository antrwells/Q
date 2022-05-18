using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Anim
{
    public class Animation
    {
        public float m_Duration = 0;
        int m_TicksPerSecond = 0;
        public List<Bone> m_Bones = new List<Bone>();
        public Dictionary<string, BoneInfo> m_BoneInfoMap = new Dictionary<string, BoneInfo>();
        


        public Animation(Assimp.Scene scene,Q.Scene.Nodes.SceneNode model)
        {
            if (scene == null || scene.RootNode == null)
            {
                Console.WriteLine("Error: scene is null");
                int a = 0;
                
            }
            m_RootNode = new AssimpNodeData();
            var animation = scene.Animations[0];
            m_Duration = (float)animation.DurationInTicks;
            m_TicksPerSecond = (int)animation.TicksPerSecond;
            ReadHeirarchyData(m_RootNode, scene.RootNode);
            ReadMissingBones(animation, model);
            
            
            
        }

        public Bone FindBone(string name)
        {

            foreach(var b in m_Bones)
            {
                if(b.m_Name == name)
                {
                    return b;
                }
            }

            return null;
            
            //return m_Bones.Find(x => x.m_Name == name);


        }

        public float GetTicksPerSecond()
        {
            return m_TicksPerSecond;
        }

        public float GetDuration()
        {
            return m_Duration;
        }

        public AssimpNodeData GetRootNode()
        {
            return m_RootNode;
        }

        public Dictionary<string,BoneInfo> GetBoneIDMap()
        {
            return m_BoneInfoMap;
        }

        public void ReadHeirarchyData(AssimpNodeData dst, Assimp.Node src)
        {
            dst.name = src.Name;
            dst.transformation = ToTK(src.Transform);

            dst.childCount = src.ChildCount;
            for (int i = 0; i < src.ChildCount; i++)
            {
                AssimpNodeData newData = new AssimpNodeData();
                ReadHeirarchyData(newData, src.Children[i]);
                dst.children.Add(newData);
            }
            
        }

        public void ReadMissingBones(Assimp.Animation animation,Q.Scene.Nodes.SceneNode model)
        {
            int size = animation.NodeAnimationChannelCount;

            var boneInfomap = model.m_BoneInfoMap;
            int boneCount = model.m_BoneCount;

            for(int i = 0; i < size; i++)
            {

                var channel = animation.NodeAnimationChannels[i];
                string boneName = channel.NodeName;

                if(!boneInfomap.ContainsKey(boneName))
                {
                    //boneInfomap
                    // boneInfomap[boneName].ID = boneCount;
                    BoneInfo info = new BoneInfo();
                    info.ID = boneCount;
                    boneInfomap.Add(boneName,info);
                    boneCount++;
                }
                //
                m_Bones.Add(new Bone(channel.NodeName, boneInfomap[channel.NodeName].ID, channel));
                

            }

            m_BoneInfoMap = boneInfomap;
            model.m_BoneInfoMap = boneInfomap;
            model.m_BoneCount = boneCount;

        }
           private Matrix4 ToTK(Assimp.Matrix4x4 mat)
        {
            Matrix4 result = Matrix4.Identity;

            result.Row0 = new Vector4(mat.A1, mat.B1, mat.C1, mat.D1);
            result.Row1 = new Vector4(mat.A2, mat.B2, mat.C2, mat.D2);
            result.Row2 = new Vector4(mat.A3, mat.B3, mat.C3, mat.D3);
            result.Row3 = new Vector4(mat.A4, mat.B4, mat.C4, mat.D4);

            return result;

        }

        public void Update()
        {
            
        }

        AssimpNodeData m_RootNode;

    }
}
