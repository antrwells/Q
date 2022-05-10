using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Quantum.Forms
{
    public class IContentFile : IForm
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

        public override void RenderForm()
        {
            base.RenderForm();
            Draw(Icon);
            DrawText(Name, RenderPosition.X, RenderPosition.Y + 70, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
        }
    }
    public class IContentFolder : IForm
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

        public override void RenderForm()
        {
            base.RenderForm();
            Draw(Icon);
            DrawText(Name, RenderPosition.X, RenderPosition.Y + 70, new OpenTK.Mathematics.Vector4(1, 1, 1, 1));
        }
    }

    public class IContentBrowser : IActiveContent
    {

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

        public IContentBrowser()
        {
            if(FolderIcon == null)
            {
                FolderIcon = new Texture.Texture2D("Data/UI/Browser/Foldericon.png");
                FileIcon = new Texture.Texture2D("Data/UI/Browser/Fileicon.png");
            }
        }
        public List<IForm> scan_contents = new List<IForm>();
        public void Scan()
        {
            int dx, dy;

            dx = 20;
            dy = 15;
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
                scan_contents.Add(cfile);
            }

        }
        public override void OnResized()
        {
            SetSizeUnsafe(Root.Size);
            Scan();
            //base.OnResized();
        }
    }
}
