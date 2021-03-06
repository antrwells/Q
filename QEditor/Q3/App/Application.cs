using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using Q.Texture;
using Q.Shader;


/// <summary>
/// The Q.App namespace contains the classes required to implement a new application utilizing Q.
/// </summary>
namespace Q.App
{

    //opengl callback class
    //
    /// <summary>
    /// AppInfo contains app-wide information, such as display width, height and currently bound frame width,height.
    /// </summary>
    public class AppInfo
    {
        /// <summary>
        /// The visible application width, such as the window or fullscreen display
        /// </summary>
        public static int Width
        {
            get;
            set;
        }
        /// <summary>
        /// The visible application height, such as the window or fullscreen display
        /// </summary>
        public static int Height
        {
            get;
            set;
        }
    
        /// <summary>
        /// The width of the currently bound RenderTarget, or display width if none is bound.
        /// </summary>
        public static int FrameWidth
        {
            get;
            set;
        }

        /// <summary>
        /// The height of the currently bound RenderTarget, or display width if none is bound.
        /// </summary>        
        public static int FrameHeight
        {
            get;
            set;
        }   
    }
    
    public class Application : GameWindow
    {

        public static Stack<AppState> States
        {
            get;
            set;
        }


        public static void PushState(AppState state)
        {
            States.Push(state);
            state.InitState();
        }

        public static void PopState()
        {
            if (States.Count == 0) return;
            var state = States.Pop();
            state.StopState();

        }

        /// <summary>
        /// Any application that inherits the application class, must implement this constructor to begin the app.
        /// </summary>
        /// <param name="window_settings"></param>
        /// <param name="native_settings"></param>
        public Application(GameWindowSettings window_settings, NativeWindowSettings native_settings) : base(window_settings,native_settings)
        {
            Console.WriteLine("Application Created");
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
          //  GL.DebugMessageCallback(GLDebugProc.`   , null);
            uint[] ids = new uint[32];
            ids[0] = 0;
            GL.DebugMessageControl(DebugSource.DontCare, DebugType.DontCare, DebugSeverity.DontCare, ids, true);
            AppInfo.Width = native_settings.Size.X;
            AppInfo.Height = native_settings.Size.Y;
            AppInfo.FrameWidth = native_settings.Size.X;
            AppInfo.FrameHeight = native_settings.Size.Y;
            Console.WriteLine("App begun. Initial resolution X:" + AppInfo.Width + " Y:" + AppInfo.Height);
            States = new Stack<AppState>();
        }

        protected override void OnLoad()
        {
            //base.OnLoad();
            this.VSync = VSyncMode.On;
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Disable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha); 
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Disable(EnableCap.ScissorTest);
            Console.WriteLine("Setup OpenGL. Resolution X:" + Size.X + " Resolution Y:" + Size.Y);
            Input.AppInput.MousePosition = new OpenTK.Mathematics.Vector2(0, 0);
            Input.AppInput.MouseButton = new bool[32];
            Font.FontTTF.Init();
            Texture.Texture.StartTextureSubSystem();
            Global.GlobalEffects.Init();
            Q.Physx.QPhysics.InitPhysics();
            InitApp();
            CursorVisible = false;
           


           // CursorVisible = false;
            

            //Texture2D tex = new Texture2D("data/test1.jpg", false);

           // Effect fx = new Effect("engine/shader/basic_draw_vertex.glsl", "engine/shader/basic_draw_frag.glsl");

        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            Input.AppInput.KeyDown(e.Key);

        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            Input.AppInput.KeyUp(e.Key);
            
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            //base.OnMouseMove(e);

            Input.AppInput.MouseDelta = new OpenTK.Mathematics.Vector2(e.X, e.Y) - Input.AppInput.MousePosition;
            Input.AppInput.MousePosition = new OpenTK.Mathematics.Vector2(e.X, e.Y);
             

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            //base.OnMouseDown(e);
            Input.AppInput.MouseButton[(int)e.Button] = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Input.AppInput.MouseButton[(int)e.Button] = false;

        }





        /// <summary>
        /// You can override this method to add your own application initializtion.
        /// </summary>
        public virtual void InitApp()
        {
            
        }

        /// <summary>
        /// You can override this method to add your own application update logic.
        /// </summary>
        public virtual void UpdateApp()
        {
            
        }

        /// <summary>
        /// You can override this method to add your own application render logic.
        /// </summary>
        public virtual void RenderApp()
        {

        }
        int ptick = 0;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Physx.QPhysics.Simulate(0.02f);
            UpdateApp();

            if (ptick == 0)
            {
                ptick = Environment.TickCount;
            }
            int tick = Environment.TickCount;
            int pc = tick - ptick;
            if (pc < 1) pc = 1;

            float delta = (float)((float)pc) / 1000.0f;

            ptick = tick;


            if (States.Count > 0)
            {
                var state = States.Peek();
                state.UpdateState(delta);
            }

            Texture.Texture._DestroyThread();
        }


        int frame = 0;
        int fps = 0;
        int n_frame;
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //base.OnRenderFrame(args);

            int ctick = Environment.TickCount;
            if (ctick > n_frame)
            {
                n_frame = ctick + 1000;
                fps = frame;
                frame = 0;
                Console.WriteLine("Fps:" + fps);
            }
            frame++;
            

            GL.ClearColor(0, 0, 0, 0.5f);
                        //GL.ClearDepthf(1.0f);


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            RenderApp();
            if (States.Count > 0)
            {
                var state = States.Peek();
                state.RenderState();
            }

            SwapBuffers();
            
        }



    }
}
