using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q.Quantum;
using OpenTK.Mathematics;
namespace Q.Quantum.Forms
{
    public delegate void ValueChanged(Vector3 value);

    public class IVector3 : IForm
    {


        public event ValueChanged OnValueChanged;

        public Vector3 Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                
            }
        }
        private Vector3 _Value;
        
        private ITextEdit VX, VY, VZ;
        private ILabel LX, LY, LZ;
        
        public IVector3()
        {

            VX = new ITextEdit();
            VY = new ITextEdit();
            VZ = new ITextEdit();
            Add(VX, VY, VZ);
            LX = new ILabel().SetText("X") as ILabel;
            LY = new ILabel().SetText("Y") as ILabel;
            LZ = new ILabel().SetText("Z") as ILabel;
            Add(LX, LY, LZ);
            VX.NumericOnly = true;
            VY.NumericOnly = true;
            VZ.NumericOnly = true;
            VX.EditText = "0";
            VY.EditText = "0";
            VZ.EditText = "0";
            VX.OnNumberChanged += VX_OnNumberChanged;
            VY.OnNumberChanged += VY_OnNumberChanged;
            VZ.OnNumberChanged += VZ_OnNumberChanged;
            

        }

        private void VZ_OnNumberChanged(float num)
        {
            //throw new NotImplementedException();
            _Value = new Vector3(_Value.X, _Value.Y,num);
            OnValueChanged?.Invoke(_Value);
        }

        private void VY_OnNumberChanged(float num)
        {
            //   throw new NotImplementedException();
            _Value = new Vector3(_Value.X,num, _Value.Z);
            OnValueChanged?.Invoke(_Value);            
        }

        private void VX_OnNumberChanged(float num)
        {
            Console.WriteLine("VX:" + num);
            _Value = new Vector3(num, _Value.Y, _Value.Z);
            OnValueChanged?.Invoke(_Value);
            //throw new NotImplementedException();



        }

        public override void Resized()
        {

            float w3 = Size.X / 3.0f;
            w3 = w3 - 25;
            int sx, sy;
            sx = Position.X;
            sy = Position.Y;
            sx = 0;
            sy = 0;

            VX.Set(25, sy, (int)w3, 30);
            VY.Set(25+sx+(int)w3 + 25, sy, (int)w3, 30);
            VZ.Set(25+sx+(int)w3 * 2 + 50,sy, (int)w3, 30);
            LX.Set(5, 12, 5, 5);
            LY.Set(25 + (int)w3+8,12, 5, 5);
            LZ.Set(25 + sx + (int)w3 * 2 + 32,12, 5, 5);
          

        }

    }
}
