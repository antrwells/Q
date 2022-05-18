using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Anim
{
    public class KeyPosition
    {

        public Vector3 position;
        public float timeStamp;

        
    }

    public class KeyRotation
    {
        public Quaternion rotation;
        public float timeStamp;
    }

    public class KeyScale
    {
        public Vector3 scale;
        public float timeStamp;
    }

}
