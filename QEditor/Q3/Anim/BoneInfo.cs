using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Anim
{
    public class BoneInfo
    {
        public int ID
        {
            get;
            set;
        }
        
        public Matrix4 Offset
        {
            get;
            set;
        }

        
    }
}
