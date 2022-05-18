using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
namespace Q.Anim
{
    public class AssimpNodeData
    {

        public Matrix4 transformation;
        public string name = "";
        public int childCount = 0;
        public List<AssimpNodeData> children = new List<AssimpNodeData>();

    }
}
