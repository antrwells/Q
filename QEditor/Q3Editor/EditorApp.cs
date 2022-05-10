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
using Q.Quantum.Forms;
using OpenTK.Mathematics;    

namespace Q3Editor
{

    class TestClass
    {
        public Vector3 TestVec1
        {
            get;
            set;
        }

        public Vector3 TestVec2
        {
            get;
            set;
        }

        public float TestFloat
        {
            get;
            set;
        }

        public string TestString
        {
            get;
            set;
        }

        public int TestInt
        {
            get;
            set;
        }

        public Texture2D TestImage
        {
            get;
            set;
        }

    }

    public class EditorApp : Application
    {

        TestClass test;
        UserInterface UI;
        enum Locality
        {
            Local,Global,Smart
        }
        public EditorApp(GameWindowSettings window_settings,NativeWindowSettings native_settings) : base(window_settings,native_settings)
        {
            
        }

        public enum testEnum
        {
            OpenGL=1,DirectX,Vulkan,None
        }

        public override void InitApp()
        {
            UI = new UserInterface();


            
            
            
            var menu = UI.AddMainMenu() as IMainMenu;

            var tb = UI.AddToolBar() as IToolBar;

            tb.AddItem(new Texture2D("Data/UI/moveicon2.png"));
            tb.AddItem(new Texture2D("Data/UI/rotateicon.png"));
            tb.AddSeperator();
            tb.AddItem(new Texture2D("Data/UI/Moveicon2.png"));
            tb.AddEnumSelector(typeof(Locality));


            tb.AddSpace(25);
            tb.AddItem(new Texture2D("Data/UI/Playicon.png"));
            tb.AddItem(new Texture2D("Data/UI/PauseIcon.png"));
            tb.AddItem(new Texture2D("Data/UI/StopIcon.png"));
            
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

            var dock_area = new IDockArea();

            dock_area.Set(0, 0, AppInfo.Width-1, UI.Root.Size.Y-1);

             UI.Root.Add(dock_area);

            var img = new IImage();
            img.Set(0, 80, AppInfo.Width, AppInfo.Height - 100);
            img.SetImage(new Texture2D("Data/test1.jpg"));

           //UI.Root.Add(img);

            var win = new IWindow();
            win.Set(200, 200, 300, 300);
            win.Title.SetText("Scene View");


            var treeview = new ITreeView();

            var obj1 = treeview.RootItem.AddItem("Object1",null);
            obj1.AddItem("Sub1", null);
            obj1.AddItem("Sub2", null);
            var obj2 = treeview.RootItem.AddItem("Object2", null);
            obj2.AddItem("Other1", null);
            obj2.AddItem("Other2", null);
            for(int i = 0; i < 50; i++)
            {
                obj2.AddItem("Testing:"+i.ToString(), null);
            }
            win.Content.Add(treeview);

            var win2 = new IWindow();
            win2.Set(50, 50, 350, 450);
            win2.Title.SetText("Console");

            var browser = new IContentBrowser();
            browser.BrowsePath = "C:\\Vivid3D\\QEditor";

            win2.Content.Add(browser);

            var cb1 = new ICheckBox();
            cb1.Set(20, 50, 80, 30);
            cb1.SetText("Render");
            cb1.OnCheckChanged += Cb1_OnCheckChanged;
            //win2.Content.Add(cb1);

            
            var win3 = new IWindow();
            win3.Set(300, 300, 250, 250);
            win3.Title.SetText("Editor");

            var win4 = new IWindow();
            win4.Set(20, 20, 350, 250);
            win4.Title.SetText("Properties");

            var cls_prop = new IClassProperties();
            test = new TestClass();
            test.TestVec1 = new Vector3(20, 30, 40);
            test.TestVec2 = new Vector3(50, 60, 70);
            test.TestFloat = 99;
            test.TestInt = 66;
            test.TestString = "Testing String 1";
            test.TestImage = new Texture2D("Data/test1.jpg");
          
            

            win4.Content.Add(cls_prop);
            cls_prop.ActiveClass = test;

            var v3 = new IVector3();
            v3.Set(20, 20, 280, 30);
            //win4.Content.Add(v3);

            IEnumSelector esel = new IEnumSelector(typeof(testEnum));

            esel.Set(20, 20, 200, 30);

            esel.OnSelected += Esel_OnSelected;

           // win4.Content.Add(esel);

            UI.Root.Add(win);
            UI.Root.Add(win2);
        //    UI.Root.Add(win3);
            UI.Root.Add(win4);
            UI.Docker = dock_area;
            
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


            //var frame1 = new IFrame().Set(20, 20, 300, 500);
            //var but1 = new IButton().Set(20, 20, 200, 35).SetText("Button 1") as IButton;
            //var win = new IWindow().Set(0,0,AppInfo.Width-100,AppInfo.Height-30).SetText("Test Window") as IWindow;

           // win.TitleOn = false;

            //var img = new IImage().Set(0, 0, 2000,2000) as IImage;
            //img.SetImage(new Texture2D("data/test1.jpg"));
           // var text = new ITextEdit().Set(350, 2800, 200, 35).SetText("Test Text") as ITextEdit;
            //text.NumericOnly = true;
            
           // var tb = new IToolBar();
           //tb.Set(0, 0, win.Size.X, 35);
            //win.Content.ToolBar = tb;
            //var b1 = tb.AddItem(new Texture2D("Data/UI/Theme/DarkFlatTheme/FileIcon1.png"),"Load");
            //tb.AddItem(null,"Save");

            //b1.Button.Click
           // b1.Button.Click += Button_Click;
            
            //b1.Button.Icon = new Texture2D("Data/UI/Theme/DarkFlatTheme/FileIcon1.png", false);
            // win.Content.ScrollPosition = new OpenTK.Mathematics.Vector2i(50, 0);
           // IVector3 tv = new IVector3();
            // 
           // win.Content.Add(tv);
           // tv.OnValueChanged += Tv_OnValueChanged;

           // tv.Set(30, 50, 500, 35);
           

            //win.Content.Add(img);
        //    win.Content.Add(but1);
         //   win.Content.Add(text);
          //  UI.Add(win);
            
            base.InitApp();
            //tex1 = new Texture2D("data/t1.png", false);
            //tex2 = new Texture2D("data/test2.jpg", false);
          
            //draw1 = new BasicDraw2D();


        }

        private void Cb1_OnCheckChanged(bool check)
        {
            //   throw new NotImplementedException();
            Console.WriteLine("Checked:" + check);
        }

        private void Esel_OnSelected(string value)
        {
            Console.WriteLine("Selected:" + value);
            //throw new NotImplementedException();
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
            Console.WriteLine("TestVec:" + test.TestVec1);
            Console.WriteLine("Vec2:" + test.TestVec2.ToString());
            Console.WriteLine("Float:" + test.TestFloat.ToString());
            Console.WriteLine("Int:" + test.TestInt.ToString());
            Console.WriteLine("String:" + test.TestString);
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
