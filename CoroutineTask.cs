using System;
using System.Collections;

namespace DC.Task
{
	public class CoroutineTask
	{
		public enum EState
		{
			RUN,
			PAUSE,
			STOP
		}
		
		public event Action OnTaskStarted = null;
		public event Action OnTaskFinished = null;
		
		private IEnumerator coroutine;
		private EState 		state;
		
		private TaskMgr	taskManager;

		public EState State
		{ get { return state; } }

		public bool Active
		{ get { return state == EState.RUN; } }
		
		public CoroutineTask(IEnumerator coroutine)
		{
			this.coroutine = coroutine;
			this.taskManager = TaskMgr.Instance;
			this.taskManager.Add(this);
		}
		
		public void Start()
		{
			state = EState.RUN;
			taskManager.StartCoroutine(Run());
		}
		
		public void Pause()
		{
			state = EState.PAUSE;
		}
		
		public void Unpause()
		{
			state = EState.RUN;
		}
		
		public void Stop()
		{
			state = EState.STOP;
		}
		
		public IEnumerator StartAndWait()
		{
			state = EState.RUN;
			yield return taskManager.StartCoroutine(Run());
		}
		
		/// <summary>
		/// Run process of the task.
		/// There are two events, OnTaskStarted and OnTaskFinished.
		/// 
		/// The life cycle is as follows:
		/// OnTaskStarted - pass a frame - runs the coroutine - pass a frame - OnTaskFinished
		/// </summary>
		private IEnumerator Run()
		{
			if (OnTaskStarted != null)
				OnTaskStarted();
			
			yield return null;
			
			while(state != EState.STOP) 
			{
				if(state == EState.PAUSE)
				{
					yield return null;
				}
				else if(coroutine != null && coroutine.MoveNext())
				{
					yield return coroutine.Current;
				}
				else 
				{
					Stop ();
					yield return null;
				}
			}

			if (OnTaskFinished != null)
				OnTaskFinished();
			
			taskManager.Remove(this);
		}
	}
}