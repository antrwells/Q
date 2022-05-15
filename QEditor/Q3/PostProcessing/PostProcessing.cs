using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.PostProcessing
{
    public class PostProcessing
    {
        public List<PostProcess> Processes = new List<PostProcess>();

        public void Add(PostProcess process)
        {
            Processes.Add(process);
        }

    }
}
