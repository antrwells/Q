using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Draw.Simple;
namespace Q.App
{
    public class AppState
    {

        public Draw2D Draw
        {
            get;
            set;
        }

        public AppState()
        {

            Draw = new Draw2D();

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
