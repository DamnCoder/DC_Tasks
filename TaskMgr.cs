/// TaskManager.cs
/// 
/// By Jorge López, based on the TaskManager by Ken Rockot <k-e-n-@-REMOVE-CAPS-AND-HYPHENS-oz.gs>
/// 
using DC.Subsystem;
using System.Collections.Generic;
using System.Collections;

namespace DC.Task
{
	public class TaskMgr: SubsystemBehavior
	{
		//--------------------------------------------------
		// Special fields
		// In order: (Static/Enums/Constants)
		//--------------------------------------------------
		
		#region STATIC_ENUM_CONSTANTS
		public static TaskMgr 	Instance
		{ get { return taskMgrInstance; } }
		
		private static TaskMgr	taskMgrInstance;

		public static CoroutineTask Create(IEnumerator coroutine)
		{
			if(taskMgrInstance != null)
			{
				return new CoroutineTask(coroutine);
			}
			return null;
		}
		
		public static CoroutineTask Launch(IEnumerator coroutine)
		{
			CoroutineTask task = Create(coroutine);
			if(task != null)
			{
				task.Start();
			}
			return task;
		}
		
		public static CoroutineTask LaunchDelayed(System.Action DelayedAction, float delayTime)
		{
			return Launch(DC.TimeHelp.DelayAction(DelayedAction, delayTime));
		}

		public static CoroutineTask LaunchDelayed(System.Action<float> CountdownAction, System.Action DelayedAction, float delayTime)
		{
			return Launch(DC.TimeHelp.DelayAction(CountdownAction, DelayedAction, delayTime));
		}
		
		public static CoroutineTask LaunchRepeated(System.Action RepeatedAction)
		{
			return Launch (DC.TimeHelp.RepeatAction (RepeatedAction));
		}
		
		public static CoroutineTask LaunchRepeated(System.Action RepeatedAction, float maxTime, System.Action EndTimeAction = null)
		{
			return Launch (DC.TimeHelp.RepeatAction (RepeatedAction, maxTime, EndTimeAction));
		}
		
		public static CoroutineTask LaunchRepeated(System.Action<float> RepeatedAction, float maxTime, System.Action EndTimeAction = null)
		{
			return Launch (DC.TimeHelp.RepeatAction (RepeatedAction, maxTime, EndTimeAction));
		}
		#endregion
		
		//--------------------------------------------------
		// Fields 
		// In order: (Public/Protected/Private)
		//--------------------------------------------------
		
		#region FIELDS
		private List<CoroutineTask> tasksPool;
		#endregion
		
		//--------------------------------------------------
		// Accessors
		//--------------------------------------------------
		
		#region ACCESSORS
		#endregion
		
		//--------------------------------------------------
		// Overrided/Overloaded methods
		//--------------------------------------------------
		
		#region OVERRIDED_METHODS
		#endregion
		
		//--------------------------------------------------
		// Methods
		// In order: (Public/Protected/Private)
		//--------------------------------------------------
		
		#region METHODS
		public override void Init()
		{
			if(taskMgrInstance == null)
			{
				UnityEngine.Debug.Log ("[TaskMgr INIT]");
				tasksPool = new List<CoroutineTask>();
				
				DontDestroyOnLoad(gameObject);
				
				taskMgrInstance = this;
			}
		}
		
		public override void End()
		{
			if(taskMgrInstance != null)
			{
				tasksPool.Clear();
				tasksPool = null;
				
				taskMgrInstance = null;
				Destroy(gameObject);
			}
		}
		
		public void Add(CoroutineTask task)
		{
			if(task != null)
			{
				tasksPool.Add(task);
			}
		}
		
		public void Remove(CoroutineTask task)
		{
			if(task != null)
			{
				tasksPool.Remove(task);
			}
		}
		#endregion
	}
}