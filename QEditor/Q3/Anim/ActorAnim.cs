using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q.Anim
{

    public enum AnimType
    {
        Forward, Backward, PingPong, Once
    }
    public class ActorAnim
    {

        public float StartTime = 0;
        public float EndTime = 0;
        public float CurTime = 0;
        public float Speed = 0;
        public string Name = "";
        public AnimType mType;

        public ActorAnim(string name, float start, float end, float speed, AnimType type)
        {
            StartTime = start;
            EndTime = end;
            CurTime = 0;
            mType = type;
            Name = name;
            Speed = speed;
        }

        public void Begin()
        {
            CurTime = StartTime;
        }

        public void Update()
        {

            CurTime = CurTime + Speed;
            if (CurTime >= EndTime)
            {

                switch (mType)
                {

                    case AnimType.Forward:
                        CurTime = StartTime;

                        break;
                    case AnimType.Once:

                        CurTime = EndTime;

                        break;
                }

            }

        }
    }
}