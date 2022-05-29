using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Draw.Simple;
using Q.Quantum;
using Q.Scene;

namespace Q.App
{
    public class AppState
    {

        public Draw2D Draw
        {
            get;
            set;
        }

        public UserInterface UI
        {
            get;
            set;
        }

        public SceneGraph Graph
        {
            get;
            set;
        }

        public Scene.Nodes.SceneCamera Cam
        {
            get;
            set;
        }

        public List<Scene.Nodes.SceneLight> Lights
        {
            get;
            set;
        }

        public AppState()
        {

            Draw = new Draw2D();
            UI = new UserInterface(true);
            Graph = new SceneGraph();
            Cam = Graph.Camera;
            Lights = new List<Scene.Nodes.SceneLight>();
          

        }


        public virtual void InitState()
        {

        }

        public virtual void UpdateState(float delta)
        {

        }

        public virtual void RenderState()
        {

        }

        public virtual void StopState()
        {

        }

        public virtual void PauseState()
        {
        }

        public virtual void ResumeState()
        {

        }

        public virtual void SaveState(string path)
        {

        }

        public virtual void LoadState(string path)
        {

        }

    }
}
