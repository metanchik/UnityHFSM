using System;

namespace FSM
{
	public static class StateMachineShortcuts
	{
		/*
			"Shortcut" methods
			- These are meant to reduce the boilerplate code required by the user for simple
			states and transitions.
			- They do this by creating a new State / Transition instance in the background
			and then setting the desired fields.
			- They can also optimise certain cases for you by choosing the best type,
			such as a StateBase for an empty state instead of a State instance.
		*/

		/// <summary>
		/// Shortcut method for adding a regular state.
		/// It creates a new State() instance under the hood. => See State for more information.
		/// For empty states with no logic it creates a new StateBase for optimal performance.
		/// </summary>
		public static void AddState<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TStateId name,
			Action<State<TData, TStateId, TEvent>> onEnter = null,
			Action<State<TData, TStateId, TEvent>> onLogic = null,
			Action<State<TData, TStateId, TEvent>> onExit = null,
			Func<State<TData, TStateId, TEvent>, bool> canExit = null,
			bool needsExitTime = false,
			bool isGhostState = false)
		{
			// Optimise for empty states
			if (onEnter == null && onLogic == null && onExit == null && canExit == null)
			{
				fsm.AddState(name, new StateBase<TData, TStateId>(needsExitTime, isGhostState));
				return;
			}

			fsm.AddState(
				name,
				new State<TData, TStateId, TEvent>(
					onEnter,
					onLogic,
					onExit,
					canExit,
					needsExitTime: needsExitTime,
					isGhostState: isGhostState
				)
			);
		}

		/// <summary>
		/// Creates the most efficient transition type possible for the given parameters.
		/// It creates a Transition instance when a condition is specified and otherwise
		/// it returns a TransitionBase.
		/// </summary>
		private static TransitionBase<TData, TStateId> CreateOptimizedTransition<TData, TStateId>(
			TStateId from,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition = null,
			bool forceInstantly = false)
		{
			if (condition == null)
				return new TransitionBase<TData, TStateId>(from, to, forceInstantly);

			return new Transition<TData, TStateId>(from, to, condition, forceInstantly);
		}

		/// <summary>
		/// Shortcut method for adding a regular transition.
		/// It creates a new Transition() instance under the hood. => See Transition for more information.
		/// When no condition is required, it creates a TransitionBase for optimal performance.
		/// </summary>
		public static void AddTransition<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TStateId from,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition = null,
			bool forceInstantly = false)
		{
			fsm.AddTransition(CreateOptimizedTransition(from, to, condition, forceInstantly));
		}

		/// <summary>
		/// Shortcut method for adding a regular transition that can happen from any state.
		/// It creates a new Transition() instance under the hood. => See Transition for more information.
		/// When no condition is required, it creates a TransitionBase for optimal performance.
		/// </summary>
		public static void AddTransitionFromAny<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition = null,
			bool forceInstantly = false)
		{
			fsm.AddTransitionFromAny(CreateOptimizedTransition(default, to, condition, forceInstantly));
		}

		/// <summary>
		/// Shortcut method for adding a new trigger transition between two states that is only checked
		/// when the specified trigger is activated.
		/// It creates a new Transition() instance under the hood. => See Transition for more information.
		/// When no condition is required, it creates a TransitionBase for optimal performance.
		/// </summary>
		public static void AddTriggerTransition<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TEvent trigger,
			TStateId from,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition = null,
			bool forceInstantly = false)
		{
			fsm.AddTriggerTransition(trigger, CreateOptimizedTransition(from, to, condition, forceInstantly));
		}

		/// <summary>
		/// Shortcut method for adding a new trigger transition that can happen from any possible state, but is only
		/// checked when the specified trigger is activated.
		/// It creates a new Transition() instance under the hood. => See Transition for more information.
		/// When no condition is required, it creates a TransitionBase for optimal performance.
		/// </summary>
		public static void AddTriggerTransitionFromAny<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TEvent trigger,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition = null,
			bool forceInstantly = false)
		{
			fsm.AddTriggerTransitionFromAny(trigger, CreateOptimizedTransition(default, to, condition, forceInstantly));
		}

		/// <summary>
		/// Shortcut method for adding two transitions:
		/// If the condition function is true, the fsm transitions from the "from"
		/// state to the "to" state. Otherwise it performs a transition in the opposite direction,
		/// i.e. from "to" to "from".
		/// </summary>
		public static void AddTwoWayTransition<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TStateId from,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition,
			bool forceInstantly = false)
		{
			fsm.AddTwoWayTransition(new Transition<TData, TStateId>(from, to, condition, forceInstantly));
		}

		/// <summary>
		/// Shortcut method for adding two transitions that are only checked when the specified trigger is activated:
		/// If the condition function is true, the fsm transitions from the "from"
		/// state to the "to" state. Otherwise it performs a transition in the opposite direction,
		/// i.e. from "to" to "from".
		/// </summary>
		public static void AddTwoWayTriggerTransition<TData, TOwnId, TStateId, TEvent>(
			this StateMachine<TData, TOwnId, TStateId, TEvent> fsm,
			TEvent trigger,
			TStateId from,
			TStateId to,
			Func<Transition<TData, TStateId>, bool> condition,
			bool forceInstantly = false)
		{
			fsm.AddTwoWayTriggerTransition(trigger, new Transition<TData, TStateId>(from, to, condition, forceInstantly));
		}
	}
}