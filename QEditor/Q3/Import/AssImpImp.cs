using Assimp;

using OpenTK.Mathematics;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Q.Mesh;
using Q;
using Q.Scene;
using Q.Scene.Nodes;
using Q.Anim;
//using Vivid.Scene;


using Q.Scene.Modules;
namespace Q.Import
{
    public class AssImpImport
    {
        public static string IPath = "";
        public static Q.Texture.Texture2D NormBlank = null, DiffBlank, SpecBlank, MetalBlank;
        public static float ScaleX = 1, ScaleY = 1, ScaleZ = 1;
        //private Animation.Animator _ta;

        public static Vector3 CTV(Color4D col)
        {
            return new Vector3(col.R, col.G, col.B);
        }

        public Vector3 Cv(Assimp.Vector3D o)
        {
            return new Vector3(o.X, o.Y, o.Z);
        }

        public Vector2 Cv2(Assimp.Vector3D o)
        {
            return new Vector2(o.X, o.Y);
        }

   

        private void ProcessNode(SceneNode root, Assimp.Node s, List<Mesh3D> ml)
        {
            SceneNode r1 = new SceneNode();

            root.Add(r1);
            r1.Root = root;
            r1.Name = s.Name;
            if (s.Name.ToLower().Contains("root"))
            {
                r1.Name = r1.Name + "*";
             //   r1.BreakTop = true;
            }

            //r1.LocalTurn = new OpenTK.Matrix4(s.Transform.A1, s.Transform.A2, s.Transform.A3, s.Transform.A4, s.Transform.B1, s.Transform.B2, s.Transform.B3, s.Transform.B4, s.Transform.C1, s.Transform.C2, s.Transform.C3, s.Transform.C4, s.Transform.D1, s.Transform.D2, s.Transform.D3, s.Transform.D4);
            r1.LocalRotation = new Matrix4(s.Transform.A1, s.Transform.B1, s.Transform.C1, s.Transform.D1, s.Transform.A2, s.Transform.B2, s.Transform.C2, s.Transform.D2, s.Transform.A3, s.Transform.B3, s.Transform.C3, s.Transform.D3, s.Transform.A4, s.Transform.B4, s.Transform.C4, s.Transform.D4);
            Matrix4 lt = r1.LocalRotation;

            r1.LocalRotation = lt.ClearTranslation();
            r1.LocalRotation = r1.LocalRotation.ClearScale();
            r1.LocalPosition = new Vector3(1, 1, 1);// lt.ExtractTranslation();

            r1.LocalScale = new Vector3(1, 1, 1);// lt.ExtractScale();
            // r1.LocalPos = new OpenTK.Vector3(r1.LocalPos.X + 100, 0, 0);
            ModuleMesh3D mm = new ModuleMesh3D();
            r1.AddModule(mm);
            for (int i = 0; i < s.MeshCount; i++)
            {

                //r1.AddMesh(ml[s.MeshIndices[i]]);
                //mm.Meshes.Add(ml[s.MeshIndices[i]]);
                r1.AddMesh(ml[s.MeshIndices[i]]);

                //r1.AddModule(mm);

                
            }
            if (s.HasChildren)
            {
                foreach (Node pn in s.Children)
                {
                    ProcessNode(r1, pn, ml);
                }
            }
        }

        public SceneNode ImportActor(string path)
        {


            List<Q.Material.Material3D> materials = new List<Material.Material3D>();
            List<Mesh3D> meshes = new List<Mesh3D>();
            if (NormBlank == null)
            {
                NormBlank = new Q.Texture.Texture2D("data/tex/normblank.png");
                DiffBlank = new Q.Texture.Texture2D("data/tex/diffblank.png");
                SpecBlank = new Q.Texture.Texture2D("data/tex/specblank.png");
            }
            string ip = path;
            int ic = ip.LastIndexOf("/");
            if (ic < 1) ic = ip.LastIndexOf("\\");
            if (ic > 0)
            {
                IPath = ip.Substring(0, ic);
            }

            //Entity3D root = new Entity3D();
            SceneNode root = new SceneNode();
            string file = path;
            root.Type = NodeType.Actor;

            AssimpContext e = new Assimp.AssimpContext();
            Assimp.Configs.NormalSmoothingAngleConfig c1 = new Assimp.Configs.NormalSmoothingAngleConfig(75);
            //e.SetConfig(c1);

            Console.WriteLine("Impporting:" + file);
            Assimp.Scene s = null;
            if (Optimize)
            {
                s = e.ImportFile(file, PostProcessSteps.OptimizeGraph | PostProcessSteps.OptimizeMeshes | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateSmoothNormals);
            }
            else
            {

                //s = e.ImportFile(file, PostProcessSteps.Triangulate | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateNormals);
            }

            if (s == null)
            {
                Console.WriteLine("Error importing:" + file);
                return null;
            }
            Matrix4x4 tf = s.RootNode.Transform;

            tf.Inverse();


            
            SceneGlobal.GlobalInverse = ToTK(tf);

            for (int i = 0; i < s.MaterialCount; i++)
            {

                Q.Material.Material3D new_mat = new Material.Material3D();
                var amat = s.Materials[i];
                string mat_name = amat.Name;

                int color_texc = amat.GetMaterialTextureCount(TextureType.Unknown);

                materials.Add(new_mat);
                

            }

            for (int i = 0; i < s.MeshCount; i++)
            {

                var mesh = s.Meshes[i];

                Q.Mesh.Mesh3D new_mesh = new Mesh3D();

                new_mesh.Material = materials[mesh.MaterialIndex];

                meshes.Add(new_mesh);

                for (int index = 0; index < mesh.VertexCount; index++)
                {

                    var vert = mesh.Vertices[index];
                    var normal = mesh.Normals[index];
                    Vector3D bi_normal = new Vector3D(0, 0, 0);

                    Vector3D tang = new Vector3D(0, 0, 0);

                    if (mesh.HasTangentBasis)
                    {
                        bi_normal = mesh.BiTangents[index];
                        tang = mesh.Tangents[index];
                    }

                    var uv = mesh.TextureCoordinateChannels[0][index];

                    Vertex new_vert = new Vertex();

                    new_vert.Pos = new Vector3(vert.X, vert.Y, vert.Z);
                    new_vert.Norm = new Vector3(normal.X, normal.Y, normal.Z);
                    new_vert.BiNorm = new Vector3(bi_normal.X, bi_normal.Y, bi_normal.Z);
                    new_vert.Tan = new Vector3(tang.X, tang.Y, tang.Z);
                    new_vert.UV = new Vector3(uv.X, uv.Y, uv.Z);
                    new_vert.Col = new Vector3(1, 1, 1);

                    new_mesh.AddVertex(new_vert);


                }

                Console.WriteLine("Mesh VertexCount:" + mesh.VertexCount);

                Dictionary<string, BoneInfo> m_BoneInfoMap = new Dictionary<string, BoneInfo>();
                int m_BoneCounter = 0;

                int max_verts = new_mesh.VerticesCount;

                for(int boneIndex=0;boneIndex<mesh.BoneCount;++boneIndex)
                {
                    int boneID = -1;
                    string boneName = mesh.Bones[boneIndex].Name;
                    Console.WriteLine("Bone:"+boneName);
                    if(!m_BoneInfoMap.ContainsKey(boneName))
                    {
                        BoneInfo newBoneInfo = new BoneInfo();
                        newBoneInfo.ID = m_BoneCounter;
                        //boneMath
                        newBoneInfo.Offset = ToTK(mesh.Bones[boneIndex].OffsetMatrix);
                        m_BoneInfoMap.Add(boneName, newBoneInfo);
                        boneID = m_BoneCounter;
                        m_BoneCounter++;
                    }
                    else
                    {
                        boneID = m_BoneInfoMap[boneName].ID;
                    }

                    if(boneID ==-1)
                    {
                        Console.WriteLine("Non-valid bone id.");
                        int b = 0;
                    }

                    var weights = mesh.Bones[boneIndex].VertexWeights;
                    int numWeights = mesh.Bones[boneIndex].VertexWeightCount;

                    for(int weightIndex=0;weightIndex<numWeights;++weightIndex)
                    {
                        
                        int vertexId =  weights[weightIndex].VertexID;
                        float weight = weights[weightIndex].Weight;
                        if(!(vertexId<=max_verts))
                        {
                            Console.WriteLine("Non valid vertexId");
                            int a = 0;
                        }

                        new_mesh.SetBoneData(vertexId, boneID, weight);


                    }

                }

                root.SetBoneInfoMap(m_BoneInfoMap, m_BoneCounter);

                for(int tri = 0; tri < mesh.FaceCount; tri++)
                {
                    var face = mesh.Faces[tri];
                    if(face.IndexCount == 3)
                    {
                        Tri t = new Tri();
                        t.V0 = face.Indices[0];
                        t.V1 = face.Indices[1];
                        t.V2 = face.Indices[2];

                        new_mesh.AddTri(t);

                    }
                }

                new_mesh.Finalize();
                root.AddMesh(new_mesh);

            }

            NodeAnimator node_anim = new NodeAnimator();

         
            

            Q.Anim.Animation anim = new Anim.Animation(s, root);
            Q.Anim.Animator animer = new Animator(anim);

            root.Animator = animer;
            
            
            return root;
           

        }

        public static bool Optimize = true;
        public SceneNode ImportNode(string path)
        {
            if (NormBlank == null)
            {
                NormBlank = new Q.Texture.Texture2D("data/tex/normblank.png");
                DiffBlank = new Q.Texture.Texture2D("data/tex/diffblank.png");
                SpecBlank = new Q.Texture.Texture2D("data/tex/specblank.png");
            }
            string ip = path;
            int ic = ip.LastIndexOf("/");
            if (ic < 1) ic = ip.LastIndexOf("\\");
            if (ic > 0)
            {
                IPath = ip.Substring(0, ic);
            }

            //Entity3D root = new Entity3D();
            SceneNode root = new SceneNode();
            string file = path;

            AssimpContext e = new Assimp.AssimpContext();
            Assimp.Configs.NormalSmoothingAngleConfig c1 = new Assimp.Configs.NormalSmoothingAngleConfig(75);
            e.SetConfig(c1);

            Console.WriteLine("Impporting:" + file);
            Assimp.Scene s = null;
            try
            {
                if (Optimize)
                {
                    s = e.ImportFile(file, PostProcessSteps.Triangulate | PostProcessSteps.OptimizeGraph | PostProcessSteps.OptimizeMeshes | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateNormals);
                }
                else
                {
                    s = e.ImportFile(file, PostProcessSteps.Triangulate | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateNormals);
                }
                if (s.HasAnimations)
                {
                   // return LoadAnimNode(path);
                }
            }
            catch (AssimpException ae)
            {
                Console.WriteLine(ae);
                Console.WriteLine("Failed to import");
                Environment.Exit(-1);
            }
            Console.WriteLine("Imported.");
            Dictionary<string, Mesh3D> ml = new Dictionary<string, Mesh3D>();
            List<Mesh3D> ml2 = new List<Mesh3D>();
            Console.WriteLine("animCount:" + s.AnimationCount);

            Matrix4x4 tf = s.RootNode.Transform;

            tf.Inverse();

          


            Dictionary<uint, List<VertexWeight>> boneToWeight = new Dictionary<uint, List<VertexWeight>>();

            //root.Animator = new Animation.Animator();

            //s.Animations[0].NodeAnimationChannels[0].
            //s.Animations[0].anim

            // root.Animator.InitAssImp(model);

            foreach (Assimp.Mesh m in s.Meshes)
            {
                Console.WriteLine("M:" + m.Name + " Bones:" + m.BoneCount);
                Console.WriteLine("AA:" + m.HasMeshAnimationAttachments);

                Q.Material.Material3D vm = new Q.Material.Material3D
                {
                    ColorMap = DiffBlank,
                    NormalMap = NormBlank,
                    SpecularMap = SpecBlank
                };
                Mesh3D m2 = new Mesh3D();
                ml2.Add(m2);
                // ml.Add(m.Name, m2);
                for (int b = 0; b < m.BoneCount; b++)
                {
                    string name = m.Bones[b].Name;
                }
                m2.Material = vm;
                // root.AddMesh(m2);
                m2.Name = m.Name;
                Assimp.Material mat = s.Materials[m.MaterialIndex];
                TextureSlot t1;

                if (mat.GetMaterialTextureCount(TextureType.Emissive) > 0)
                {
                    var em = mat.GetMaterialTextures(TextureType.Emissive)[0];
                    vm.EmissiveMap = new Q.Texture.Texture2D(IPath + "/" + em.FilePath);
                    int a = 5;
                }

                int sc = mat.GetMaterialTextureCount(TextureType.Unknown);
                if(sc>0)
                {
                    
                    int a = 1;
                }
                Console.WriteLine("SC:" + sc);
                if (mat.HasColorDiffuse)
                {
                    vm.Diffuse = CTV(mat.ColorDiffuse);
                    Console.WriteLine("Diff:" + vm.Diffuse);
                }
                if (mat.HasColorSpecular)
                {
                    // vm.Spec = CTV ( mat.ColorSpecular );
                    Console.WriteLine("Spec:" + vm.Specular);
                }
                if (mat.HasShininess)
                {
                    //vm.Shine = 0.3f+ mat.Shininess;
                    Console.WriteLine("Shine:");
                }

                Console.WriteLine("Spec:" + vm.Specular);
                //for(int ic = 0; ic < sc; ic++)
                ///{
                if (sc > 0)
                {
                    TextureSlot tex2 = mat.GetMaterialTextures(TextureType.Unknown)[0];
                    // vm.SpecularMap = new Texture.Texture2D ( IPath + "/" + tex2.FilePath, Texture.LoadMethod.Single, false );
                }

                if (mat.GetMaterialTextureCount(TextureType.Normals) > 0)
                {
                    TextureSlot ntt = mat.GetMaterialTextures(TextureType.Normals)[0];
                    Console.WriteLine("Norm:" + ntt.FilePath);

                    var tp = ntt.FilePath;

                    if (tp.Contains("\\"))
                    {
                        tp = tp.Substring(tp.LastIndexOf("\\") + 1);
                    }

                    vm.NormalMap = new Q.Texture.Texture2D(IPath + "/" + tp);
                }
                if (mat.GetMaterialTextureCount(TextureType.Opacity) > 0)
                {
                    int a = 5;
                }
                if (mat.GetMaterialTextureCount(TextureType.Diffuse) > 0)
                {
                    t1 = mat.GetMaterialTextures(TextureType.Diffuse)[0];
                    //   Console.WriteLine("DiffTex:" + t1.FilePath);

                    if (t1.FilePath != null)
                    {
                        //Console.WriteLine ( "Tex:" + t1.FilePath );
                        // Console.Write("t1:" + t1.FilePath);

                        var tp = t1.FilePath;

                        if (tp.Contains("\\"))
                        {
                            tp = tp.Substring(tp.LastIndexOf("\\") + 1);
                        }

                        Console.WriteLine("TPath:" + tp);

                        if (File.Exists(IPath + "/" + tp) == false)
                        {
                            int aa = 0;
                        }

                        vm.ColorMap = new Q.Texture.Texture2D(IPath + "/" + tp);
                        if (File.Exists(IPath + "/" + "norm_" + tp))
                        {
                            vm.NormalMap = new Q.Texture.Texture2D(IPath + "/" + "norm_" + tp);
                        }
                        if (File.Exists(IPath + "/" + "spec_" + tp))
                        {
                            vm.SpecularMap = new Q.Texture.Texture2D(IPath + "/" + "spec_" + tp);
                        }
                    }
                }
                Console.WriteLine("Verts:" + m.VertexCount);
                Console.WriteLine("Tris:" + m.FaceCount);
                for (int i = 0; i < m.VertexCount; i++)
                {
                    Vector3D v = m.Vertices[i];// * new Vector3D(15, 15, 15);

                    //v.Y = -v.Y;

                    Vector3D n = new Vector3D(0, 1, 0);

                    if (m.Normals != null && m.Normals.Count > i)
                    {
                        n = m.Normals[i];
                    }

                    List<Vector3D> t = m.TextureCoordinateChannels[0];
                    Vector3D tan, bi;
                    if (m.Tangents != null && m.Tangents.Count > 0)
                    {
                        tan = m.Tangents[i];
                        bi = m.BiTangents[i];
                    }
                    else
                    {
                        tan = new Vector3D(0, 0, 0);
                        bi = new Vector3D(0, 0, 0);
                    }
                    if (t.Count() == 0)
                    {
                        m2.SetVertex(i, Cv(v), Cv(tan), Cv(bi), Cv(n), Cv(new Vector3D(0, 0, 0)));
                    }
                    else
                    {
                        Vector3D tv = t[i];
                        tv.Y = tv.Y;
                        m2.Vertices.Add(new Vertex());
                        m2.SetVertex(i, Cv(v), Cv(tan), Cv(bi), Cv(n), Cv(tv));
                    }

                    //var v = new PosNormalTexTanSkinned(pos, norm.ToVector3(), texC.ToVector2(), tan.ToVector3(), weights.First(), boneIndices);
                    //verts.Add(v);
                }

                for (int i = 0; i < m.FaceCount; i++)
                {
                    var face = m.Faces[i];
                    if (face.Indices.Count > 2)
                    {
                        m2.AddTri(face.Indices[0], face.Indices[1], face.Indices[2]);

                    }
                }
                /*
                int[] id = m.GetIndices();
                uint[] nd = new uint[id.Length];
                for (int i = 0; i < id.Length; i += 3)
                {
                    //Tri t = new Tri();
                    //t.V0 = (int)nd[i];
                    // t.V1 = (int)nd[i + 1];
                    // t.v2 = (int)nd[i + 2];

                    // nd[i] = (uint)id[i];
                    if (i + 2 < id.Length)
                    {
                        m2.SetTri(i / 3, id[i], id[i + 1], id[i + 2]);
                    }
                }
                */
                // m2.Indices = nd;
                //m2.Scale(AssImpImport.ScaleX, AssImpImport.ScaleY, AssImpImport.ScaleZ);
                //m2.GenerateTangents ( );

                m2.Finalize();
            }

            ProcessNode(root, s.RootNode, ml2);
            //root.SetMultiPass();

            /*
            while (true)
            {
            }
            */
            //  root.Rot(new OpenTK.Vector3(180, 0, 0), Space.Local);
            root.Type = NodeType.Entity;
            return root;
        }

      

        private Matrix4 ToTK(Matrix4x4 mat)
        {
            Matrix4 result = Matrix4.Identity;

            result.Row0 = new Vector4(mat.A1, mat.B1, mat.C1, mat.D1);
            result.Row1 = new Vector4(mat.A2, mat.B2, mat.C2, mat.D2);
            result.Row2 = new Vector4(mat.A3, mat.B3, mat.C3, mat.D3);
            result.Row3 = new Vector4(mat.A4, mat.B4, mat.C4, mat.D4);

            return result;
        }
    }
}