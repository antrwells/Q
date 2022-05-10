using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Quantum.Forms
{
    public class IClassProperties : IActiveContent
    {
        public static Texture.Texture2D NoImage = null;
        public Object ActiveClass
        {
            get
            {
                return _ActiveClass; 
            }
            set
            {
                _ActiveClass = value;
                Rebuild();
            }
        }
        private Object _ActiveClass = null;

  
        int max_y = 0;

        public IClassProperties()
        {
            if (NoImage == null)
            {
                NoImage = new Texture.Texture2D("Data/UI/NoImage.png");
            }
        }

        public void Rebuild()
        {

           // Child.Clear();

            var label = new ILabel();
            label.SetText("Class:" + _ActiveClass.GetType().Name);
            label.Set(5, 10, 5, 5);
            Add(label);

            int dy = 35;

            var properties = _ActiveClass.GetType().GetProperties();

            foreach (var property in properties)
            {

                //get property name
                var name = property.Name;

                //get property value
                var value = property.GetValue(_ActiveClass);

                //get property type
                var type = property.PropertyType.Name;

                Type enum_type;
                string enum_name = "";
                bool is_enum = false;
                if (property.PropertyType.IsEnum)
                {
                    is_enum = true;
                    enum_name = property.PropertyType.GetEnumName(value);
                    //enum_type = property.PropertyType
                    //enum_type = property.PropertyType.Name;                  

                    

                }
                
                var lab = new ILabel();
                lab.SetText(name);
                lab.Set(5, dy, 5, 5);
                dy = dy + 25;
                Add(lab);

                //switch types
                switch (type)
                {
                    case "Vector3":


                        IVector3 vec3 = new IVector3();
                        vec3.Set(20, dy, 260, 30);
                        vec3.Value = (Vector3)value;
                        Add(vec3);
                        dy = dy + 45;
                        vec3.OnValueChanged += (value) =>
                        {
                            property.SetValue(_ActiveClass, value);
                        };

                        break;
                    case "String":
                    case "string":

                        ITextEdit str = new ITextEdit();
                        str.Set(20, dy, 260, 30);
                        str.EditText = (string)value;
                        str.OnTextChanged += (value) =>
                        {
                            property.SetValue(_ActiveClass, value);
                        };
                        Add(str);
                        dy = dy + 45;
                        break;
                    case "Single":

                        ITextEdit flt = new ITextEdit();
                        flt.Set(20, dy, 260, 30);
                        flt.EditText = value.ToString();
                        flt.OnNumberChanged += (value) =>
                        {
                            property.SetValue(_ActiveClass, value);
                        };
                        Add(flt);
                        dy = dy + 45;
                        flt.NumericOnly = true;

                        break;
                    case "Int32":

                        ITextEdit it = new ITextEdit();
                        it.Set(20, dy, 260, 30);
                        it.EditText = value.ToString();
                        it.OnNumberChanged += (value) =>
                        {
                            property.SetValue(_ActiveClass, (int)value);
                        };
                        Add(it);
                        dy = dy + 45;
                        it.NumericOnly = true;
                        break;

                    case "Texture2D":

                        IImage img = new IImage();
                        img.Set(20, dy, 128, 128);
                        Add(img);
                        if (value == null)
                        {
                            img.SetImage(NoImage);
                        }
                        else
                        {
                            img.SetImage(value as Texture.Texture2D);
                        }
                        dy = dy + 145;

                        break;
                    default:

                        //int a = 5;
                        if (is_enum)
                        {
                            int ii = 0;
                            IEnumSelector esel = new IEnumSelector(property.PropertyType);
                            foreach (var val in property.PropertyType.GetEnumNames())
                            {
                                if (val == enum_name)
                                {
                                    esel.CurrentSelection = ii;
                                    break;
                                }
                                ii++;
                            }
                            esel.Set(20, dy, 260, 30);
                            Add(esel);
                            dy = dy + 45;
                            esel.OnSelected += (value) =>
                            {
                                int v = 0;
                                foreach(var val in property.PropertyType.GetEnumNames())
                                {
                                    if (val == value)
                                    {
                                        break;
                                    }
                                    v++;
                                }
                                property.SetValue(_ActiveClass, v);
                            };
                        };
                

                        break;
                }

            }
            max_y = dy;// - Size.Y;
        }


        public override void OnResized()
        {
            SetSizeUnsafe(Root.Size);
            //base.OnResized();
        }


    }
}
