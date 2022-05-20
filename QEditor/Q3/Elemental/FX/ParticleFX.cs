using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace Q.Elemental.FX
{
    public class Particle
    {
        
        public Vector3 Position
        {
            get;
            set;
        }

        public Vector3 Inertia
        {
            get;
            set;
        }

        public float Rotation
        {
            get;
            set;
        }

        public float RotationI
        {
            get;
            set;
        }

        public Vector2 Size
        {
            get;
            set;
        }

        public Vector2 SizeI
        {
            get;
            set;
        }

        public float Alpha
        {
            get;
            set;
        }

        public float AlphaI
        {
            get;
            set;
        }
        public float Life
        {
            get;
            set;
        }

        public float LifeI
        {
            get;
            set;
        }

        public Q.Texture.Texture2D Image
        {
            get;
            set;
        }

        public Scene.Nodes.SceneNode Node
        {
            get;
            set;
        }

        public virtual Particle Clone()
        {
            var cloned = new Particle();
            cloned.Image = Image;
            return cloned;
        }

        public virtual void Update()
        {

            Life = Life * LifeI;
            Alpha = Alpha * AlphaI;
            Rotation = Rotation + RotationI;
            Position = Position + Inertia;
            Size = Size * SizeI;

        }

    }
    public class ParticleFX : EFX
    {

        public static Random RR = null;

        public List<Particle> Particles
        {
            get;
            set;
        }

        public Vector3 SpawnPosMin
        {
            get;
            set;
        }

        public Vector3 SpawnPosMax
        {
            get;
            set;
        }

        public Vector2 SpawnSizeMin
        {
            get;
            set;
        }

        public Vector2 SpawnSizeMax
        {
            get;
            set;
        }

        public float SpawnAlphaMin
        {
            get;
            set;
        }

        public float SpawnAlphaMax
        {
            get;
            set;
        }

        public float SpawnAlphaIMin
        {
            get;
            set;
        }

        public float SpawnAlphaIMax
        {
            get;
            set;
        }

        public float SpawnRotationMin
        {
            get;
            set;
        }

        public float SpawnRotationMax
        {
            get;
            set;
        }

        public float SpawnRotationIMin
        {
            get;
            set;
        }

        public float SpawnRotationIMax
        {
            get;
            set;
        }

        public Vector2 SpawnSizeIMin
        {
            get;
            set;
        }

        public Vector2 SpawnSizeIMax
        {
            get;
            set;
        }

        public Vector3 SpawnInertiaMin
        {
            get;
            set;
        }

        public Vector3 SpawnInertiaMax
        {
            get;
            set;
        }

        public float SpawnLifeMin
        {
            get;
            set;
        }

        public float SpawnLifeMax
        {
            get;
            set;
        }

        public float SpawnLifeIMin
        {
            get;
            set;
        }

        public float SpawnLifeIMax
        {
            get;
            set;
        }


        private Mesh.Mesh3D BasicBase = null;


        public ParticleFX()
        {
            Particles = new List<Particle>();
            if (RR == null)
            {
                RR = new Random(Environment.TickCount);
            }
            SpawnAlphaMin = 0.7f;
            SpawnAlphaMax = 1.0f;
            SpawnAlphaIMin = 0.7f;
            SpawnAlphaIMax = 0.95f;
            SpawnRotationMin = 0;
            SpawnRotationMax = 360.0f;
            SpawnRotationIMin = -1f;
            SpawnRotationIMax = 1.0f;
            SpawnLifeMin = 0.8f;
            SpawnLifeMax = 1.0f;
            SpawnLifeIMin = 0.85f;
            SpawnLifeIMax = 0.98f;
            SpawnSizeMin = new Vector2(0.02f, 0.02f); 
            SpawnSizeMax = new Vector2(0.2f, 0.2f);
            SpawnSizeIMin = new Vector2(1f, 1f);
            SpawnSizeIMax = new Vector2(1.05f, 1.05f);
          
            BasicBase = new Mesh.Mesh3D();

            Mesh.Vertex v1, v2, v3, v4;

            v1 = new Mesh.Vertex();
            v2 = new Mesh.Vertex();
            v3 = new Mesh.Vertex();
            v4 = new Mesh.Vertex();

            v1.Pos = new Vector3(-1, -1, 0);
            v2.Pos = new Vector3(1, -1, 0);
            v3.Pos = new Vector3(1, 1, 0);
            v4.Pos = new Vector3(-1, 1, 0);

            v1.UV = new Vector3(0, 0, 0);
            v2.UV = new Vector3(1, 0, 0);
            v3.UV = new Vector3(1, 1, 0);
            v4.UV = new Vector3(0, 1, 0);

            v1.Norm = new Vector3(0, 0, 1);
            v2.Norm = v1.Norm;
            v3.Norm = v2.Norm;
            v4.Norm = v3.Norm;

            BasicBase.AddVertex(v1);
            BasicBase.AddVertex(v2);
            BasicBase.AddVertex(v3);
            BasicBase.AddVertex(v4);

            Mesh.Tri t1, t2, t3, t4;

            t1 = new Mesh.Tri(0, 1, 2);
            t2 = new Mesh.Tri(2, 3, 0);
            t3 = new Mesh.Tri(0, 2, 1);
            t4 = new Mesh.Tri(2, 0, 3);

            BasicBase.AddTri(t1);
            BasicBase.AddTri(t2);
            BasicBase.AddTri(t3);
            BasicBase.AddTri(t4);

            BasicBase.Finalize();

        }


        public void Spawn(Particle particle,int number)
        {
            for (int i = 0; i < number;i++) {
                Spawn(particle);
            }
        }

        public void Spawn(int number,params Particle[] particles)
        {

            for(int i = 0; i < number; i++)
            {

                int pnum = RR.Next(0, particles.Length - 1);
                Spawn(particles[pnum]);

            }

        }

        private Vector2 RndVector(Vector2 min,Vector2 max)
        {

            Vector2 res = new Vector2();

            res.X = min.X + ((max.X - min.X) * RR.NextSingle());
            res.Y = min.Y + ((max.Y - min.Y) * RR.NextSingle());

            return res;

        }

        private Vector3 RndVector(Vector3 min,Vector3 max)
        {

            Vector3 res = new Vector3();
            res.X = min.X + ((max.X - min.X) * RR.NextSingle());
            res.Y = min.Y + ((max.Y - min.Y) * RR.NextSingle());
            res.Z = min.Z + ((max.Z - min.Z) * RR.NextSingle());
            return res;

        }

        public float RndFloat(float min,float max)
        {

            return min + ((max-min) * RR.NextSingle());

        }

        public void Spawn(Particle particle)
        {

            Particle new_particle = particle.Clone();

            var pos = RndVector(SpawnPosMin, SpawnPosMax);
            var size = RndVector(SpawnSizeMin, SpawnSizeMax);
            size.X = size.Y;
            float alpha = RndFloat(SpawnAlphaMin, SpawnAlphaMax);
            float alphaI = RndFloat(SpawnAlphaIMin, SpawnAlphaMax);
            float life = RndFloat(SpawnLifeMin, SpawnLifeMax);
            float lifeI = RndFloat(SpawnLifeIMin, SpawnLifeIMax);
            float rotation = RndFloat(SpawnRotationMin, SpawnRotationMax);
            float rotationI = RndFloat(SpawnRotationIMin, SpawnRotationIMax);
            var inertia = RndVector(SpawnInertiaMin, SpawnInertiaMax);
            var sizeI = RndVector(SpawnSizeIMin, SpawnSizeIMax);
            sizeI.X = sizeI.Y;

            new_particle.Position = Position+pos;
            new_particle.Size = size;
            new_particle.Alpha = alpha;
            new_particle.AlphaI = alphaI;
            new_particle.Life = life;
            new_particle.LifeI = lifeI;
            new_particle.Rotation = rotation;
            new_particle.RotationI = rotationI;
            new_particle.Inertia = inertia;
            new_particle.SizeI = sizeI;


       //     Console.WriteLine("POS:" + pos.ToString());

            Particles.Add(new_particle);

            Scene.Nodes.SceneNode node = new Scene.Nodes.SceneNode();

            node.Meshes.Add(BasicBase.Clone());

            node.LocalPosition = Position+pos;

            node.Type = Scene.Nodes.NodeType.Particle;

            node.AlwaysFaceCamera = true;

            node.Meshes[0].Material = new Material.Material3D();
            node.Meshes[0].Material.ColorMap = particle.Image;

            new_particle.Node = node;

            Owner.CurrentScene.Add(node);

        }

        public override void Update()
        {
            //base.Update();
            foreach (var particle in Particles.ToArray())
            {
                particle.Update();
                particle.Node.LocalPosition = particle.Position;
                //particle.Node.Color = new Vector4(1, 1, 1, particle.Alpha);
                particle.Node.Color = new Vector4(particle.Alpha, particle.Alpha, particle.Alpha, particle.Alpha);
                particle.Node.Spin = particle.Rotation;
                particle.Node.LocalScale = new Vector3(particle.Size.X, particle.Size.Y, 1.0f);
                if (particle.Life < 0.05f)
                {
                    Particles.Remove(particle);
                    Owner.CurrentScene.Remove(particle.Node);
                  //  Console.WriteLine("Particle removed.");
                }
                    
            }
        }


    }
}
