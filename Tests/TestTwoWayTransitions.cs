using UnityEngine;
using NUnit.Framework;

namespace FSM.Tests
{
	public class TesTwoWayTransitions
	{
		private Recorder<int> recorder;
		private StateMachine<int> fsm;

		[SetUp]
		public void Setup()
		{
			recorder = new Recorder<int>();
			fsm = new StateMachine<int>(0);
		}

		[Test]
		public void Test_two_way_transitions_work_both_ways()
		{
			bool shouldBeInB = false;

			fsm.AddState("A", recorder.TrackedState);
			fsm.AddState("B", recorder.TrackedState);
			fsm.AddTwoWayTransition("A", "B", t => shouldBeInB);

			fsm.Init();
			fsm.OnLogic();
			Assert.AreEqual("A", fsm.ActiveStateName);

			shouldBeInB = true;
			fsm.OnLogic();
			Assert.AreEqual("B", fsm.ActiveStateName);

			shouldBeInB = false;
			fsm.OnLogic();
			Assert.AreEqual("A", fsm.ActiveStateName);
		}
	}
}