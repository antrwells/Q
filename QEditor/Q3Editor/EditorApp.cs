using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.App;
using Q.Shader;
using Q.Texture;
using Q.Shader._2D;
using OpenTK.Windowing.Desktop;
using Q.Draw.Simple;
using Q.Quantum;
using Q.Quantum.Forms;
using Q3.Quantum.Forms;
    
namespace Q3Editor
{
    public class EditorApp : Application
    {
       

        UserInterface UI;

        public EditorApp(GameWindowSettings window_settings,NativeWindowSettings native_settings) : base(window_settings,native_settings)
        {
            
        }

        public override void InitApp()
        {
            UI = new UserInterface();

            var menu = UI.AddMainMenu() as IMainMenu;

            var file = menu.AddItem("File");
            var edit = menu.AddItem("Edit");
            var help = menu.AddItem("Help");
            
            var load_map  = file.AddItem("Load Map");

            load_map.Icon = new Texture2D("Data/UI/Theme/DarkFlatTheme/FileIcon1.png", false);

            var lm_2 = load_map.AddItem("Load original.");
            load_map.AddItem("Load copy.");
            load_map.AddItem("Load other.");

            lm_2.AddItem("Yes it works.");

            var save_map = file.AddItem("Save Map");

            lm_2.CLick = (item) =>
            {

              //  Environment.Exit(1);

            };

            file.AddItem("--------");
            file.AddItem("Exit App");

            load_map.CLick = (button) =>
            {
                //Environment.Exit(1);

            };


            var frame1 = new IFrame().Set(20, 20, 300, 500);
            var but1 = new IButton().Set(20, 20, 200, 35).SetText("Button 1") as IButton;
            var win = new IWindow().Set(0,0,AppInfo.Width-100,AppInfo.Height-30).SetText("Test Window") as IWindow;

            win.TitleOn = false;

            var img = new IImage().Set(0, 0, 2000,2000) as IImage;
            img.SetImage(new Texture2D("data/test1.jpg"));
            var text = new ITextEdit().Set(350, 2800, 200, 35).SetText("Test Text") as ITextEdit;
            text.NumericOnly = true;
            
            var tb = new IToolBar();
           tb.Set(0, 0, win.Size.X, 35);
            //win.Content.ToolBar = tb;
            var b1 = tb.AddItem(new Texture2D("Data/UI/Theme/DarkFlatTheme/FileIcon1.png"),"Load");
            tb.AddItem(null,"Save");

            //b1.Button.Click
            b1.Button.Click += Button_Click;
            
            //b1.Button.Icon = new Texture2D("Data/UI/Theme/DarkFlatTheme/FileIcon1.png", false);
            // win.Content.ScrollPosition = new OpenTK.Mathematics.Vector2i(50, 0);
            IVector3 tv = new IVector3();
            // 
            win.Content.Add(tv);
            tv.OnValueChanged += Tv_OnValueChanged;

            tv.Set(30, 50, 500, 35);
           

            win.Content.Add(img);
            //win.Content.Add(but1);
           // win.Content.Add(text);
            UI.Add(win);
            
            base.InitApp();
            //tex1 = new Texture2D("data/t1.png", false);
            //tex2 = new Texture2D("data/test2.jpg", false);
          
            //draw1 = new BasicDraw2D();


        }

        private void Button_Click(int button)
        {
            Environment.Exit(1);
            //throw new NotImplementedException();
        }

        private void Tv_OnValueChanged(OpenTK.Mathematics.Vector3 value)
        {
            //    throw new NotImplementedException();
            Console.WriteLine("Vec:" + value.ToString());
        }

        public override void UpdateApp()
        {
            base.UpdateApp();
            UI.UpdateUI();
        }

        public override void RenderApp()
        {

            UI.RenderUI();
           

           // draw1.Rect(20, 20, 300, 300, tex1, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
            //draw1.Rect(120, 120, 300, 300, tex1, new OpenTK.Mathematics.Vector4(1, 1, 1, 0.4f));
            return;
         
        }

    }
}
