using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Quantum.Forms
{
    public class IContent : IForm
    {
        public Texture.Texture2D Icon
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public bool Over
        {
            get;
            set;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Over = true;
        }

        public override void OnLeave()
        {
            base.OnLeave();
            Over = false;
        }
        public IContent()
        {
            CheckBounds = true;
        }
        public override DragInfo GetDragInfo()
        {
            DragInfo info = new DragInfo();
            info.Icon = Icon;
            info.Text = Name;
            info.Form = this;
            info.Path = Path;
            return info;
            //return base.GetDragInfo();
            
        }
    }
    public class IContentFile : IContent
    {


        public IContentFile()
        {
            CanDragAndDrop = true;
        }

        public override void RenderForm()
        {
            base.RenderForm();

            if (Over)
            {
                Draw(Icon, RenderPosition.X - 4, RenderPosition.Y - 4, Size.X + 8, Size.Y + 8, new Vector4(6, 6, 6, 1.0f));
            }
                Draw(Icon);
                
           // if (Over)
            //{
             //   Draw
            //}

            DrawText(Name, RenderPosition.X, RenderPosition.Y + 70, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
        }
    }
    public class IContentFolder : IContent
    {

        public IContentFolder()
        {
            CanDragAndDrop = true;
        }
   
        public override void RenderForm()
        {
            base.RenderForm();
            if (Over)
            {
             //   DrawFrame();
            }
            if (Over)
            {
                Draw(Icon, RenderPosition.X - 4, RenderPosition.Y - 4, Size.X + 8, Size.Y + 8, new Vector4(6, 6, 6, 1.0f));
            }
                Draw(Icon);
            DrawText(Name, RenderPosition.X, RenderPosition.Y + 70, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
        }

        public override void OnDoubleClick(int button)
        {
            //base.OnDoubleClick(button);
            if (button == 0)
            {
                IContentBrowser.Instance.BrowsePath = Path;
            }
        }
    }

    public class IContentBrowser : IActiveContent
    {

        public static IContentBrowser Instance = null;
        public static Texture.Texture2D FolderIcon
        {
            get;
            set;
        }

        public static Texture.Texture2D FileIcon
        {
            get;
            set;
        }

        public string BrowsePath
        {
            get
            {
                return _BrowsePath;
            }
            set
            {
                _BrowsePath = value;
                Scan();
            }
        }
        public string _BrowsePath = "";

        public Stack<string> Paths = new Stack<string>();

        public override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            Console.WriteLine("!!!!");
            
            if (button == 1)
            {

                if (Paths.Count > 1)
                {
                    Paths.Pop();
                    BrowsePath = Paths.Peek();
                }
                   
                
            }
        }

        public IContentBrowser()
        {
            if(FolderIcon == null)
            {
                FolderIcon = new Texture.Texture2D("Data/UI/Browser/Foldericon.png");
                FileIcon = new Texture.Texture2D("Data/UI/Browser/Fileicon.png");
            }
            Instance = this;
        }
        public List<IForm> scan_contents = new List<IForm>();
        public void Scan()
        {
            int dx, dy;

            dx = 20;
            dy = 15;


            bool found = false;
            //foreach(var p in Paths)
            //{
            //  if(p==Paths.con
            //}
            if (!Paths.Contains(_BrowsePath))
            {
                 Paths.Push(_BrowsePath);
            }

            foreach(IForm sc in scan_contents.ToArray())
            {
                Child.Remove(sc);
                
            }

            scan_contents.Clear();


            foreach(var dir in new DirectoryInfo(_BrowsePath).GetDirectories())
            {
                IContentFolder folder = new IContentFolder();
                folder.Icon = FolderIcon;
                folder.Name = dir.Name;
                folder.Set(dx, dy, 64, 64);
                Add(folder);
                dx = dx + 120;
                if(dx>Size.X-120)
                {
                    dx = 20;
                    dy = dy + 92;
                }
                folder.Path = dir.FullName;                
                scan_contents.Add(folder);
            }
            foreach(var file in new DirectoryInfo(_BrowsePath).GetFiles())
            {
                IContentFile cfile = new IContentFile();
                cfile.Icon = FileIcon;
                cfile.Name = file.Name;
                cfile.Set(dx, dy, 52, 64);
                Add(cfile);
                dx = dx + 120;
                if (dx > Size.X - 120)
                {
                    dx = 20;
                    dy = dy + 92;
                }
                cfile.Path = file.FullName;
                scan_contents.Add(cfile);
            }

        }
        public override void OnResized()
        {
            SetSizeUnsafe(Root.Size);
            Scan();
            Console.WriteLine("Resized.......");
            //base.OnResized();
        }

        public override bool InBounds(int x, int y)
        {
            return base.InBounds(x, y);
            
        }
    }
}
