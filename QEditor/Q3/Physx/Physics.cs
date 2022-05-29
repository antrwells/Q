using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using PhysX.VisualDebugger;
namespace Q.Physx
{
    public static class QPhysics
    {

        public static Foundation _Foundation;
		public static Physics _Physics = null;
		public static Pvd _Pvd = null;
		public static ErrorLog _errorLog;
		public static Cooking _Cooking;
		public static PhysX.Scene _Scene;
        public static void InitPhysics()
        {
			_errorLog = new ErrorLog();
			_Foundation = new Foundation(_errorLog);
			_Pvd = new Pvd(_Foundation);
			_Pvd.Connect("127.0.0.1");


			_Physics = new Physics(_Foundation,true, _Pvd);

			CookingParams cook = new CookingParams();

			

			//cook.MidphaseDesc = mpd;

		

			var sceneDesc = new SceneDesc();
			sceneDesc.Gravity = new System.Numerics.Vector3(0.0f, -9.81f, 0.0f);
			sceneDesc.Flags |= SceneFlag.EnableCcd;
			sceneDesc.CCDMaxPasses = 4;
			sceneDesc.SolverType = SolverType.TGS;
			sceneDesc.Flags |= SceneFlag.EnablePcm;

			_Scene = _Physics.CreateScene(sceneDesc);
			//_Pvd.Connect()
			//_Cooking = _Physics.CreateCooking(cook);
			_Cooking = _Physics.CreateCooking();
			//_Pvd.IsConnected;





			//_Physics.CreateScene();

		}

		public static void Simulate(float time)
        {
			/*
			 * if (_Pvd.IsConnected(true))
			{
				Console.WriteLine("PC: Yes");
			}
            else
            {
				Console.WriteLine("PC: No");
            }
			*/
				_Scene.Simulate(time);
			_Scene.FetchResults(true);
			
        }

    }

	public class ErrorLog : PhysX.ErrorCallback
	{
		private List<string> _errors;

		public ErrorLog()
		{
			_errors = new List<string>();
		}

		public override void ReportError(ErrorCode errorCode, string message, string file, int lineNumber)
		{
			string e = string.Format("Code: {0}, Message: {1}", errorCode, message);

			_errors.Add(e);

			Trace.WriteLine(e);
		}

		public override string ToString()
		{
			if (_errors.Count == 0)
				return "No errors";
			else
				return string.Format("{0} errors. Last error: {1}", _errors.Count, _errors[_errors.Count - 1]);
		}

		public int ErrorCount
		{
			get
			{
				return _errors.Count;
			}
		}

		public bool HasErrors
		{
			get
			{
				return _errors.Any();
			}
		}

		public string LastError
		{
			get
			{
				return _errors.LastOrDefault() ?? String.Empty;
			}
		}
	}


}
