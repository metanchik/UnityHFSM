using System;

namespace FSM
{
	/// <summary>
	/// A class that allows you to run additional functions (companion code)
	/// before and after the wrapped state's code.
	/// </summary>
	public class TransitionWrapper<TData, TStateId>
	{
		public class WrappedTransition : TransitionBase<TData, TStateId>
		{
			private Action<TransitionBase<TData, TStateId>>
				beforeOnEnter,
				afterOnEnter,

				beforeShouldTransition,
				afterShouldTransition;

			private TransitionBase<TData, TStateId> transition;

			public WrappedTransition(
					TransitionBase<TData, TStateId> transition,

					Action<TransitionBase<TData, TStateId>> beforeOnEnter = null,
					Action<TransitionBase<TData, TStateId>> afterOnEnter = null,

					Action<TransitionBase<TData, TStateId>> beforeShouldTransition = null,
					Action<TransitionBase<TData, TStateId>> afterShouldTransition = null) : base(
					transition.from, transition.to, forceInstantly: transition.forceInstantly)
			{
				this.transition = transition;

				this.beforeOnEnter = beforeOnEnter;
				this.afterOnEnter = afterOnEnter;

				this.beforeShouldTransition = beforeShouldTransition;
				this.afterShouldTransition = afterShouldTransition;
			}

			public override void Init()
			{
				transition.fsm = this.fsm;
			}

			public override void OnEnter()
			{
				beforeOnEnter?.Invoke(transition);
				transition.OnEnter();
				afterOnEnter?.Invoke(transition);
			}

			public override bool ShouldTransition()
			{
				beforeShouldTransition?.Invoke(transition);
				bool shouldTransition = transition.ShouldTransition();
				afterShouldTransition?.Invoke(transition);
				return shouldTransition;
			}
		}

		private Action<TransitionBase<TData, TStateId>>
			beforeOnEnter,
			afterOnEnter,

			beforeShouldTransition,
			afterShouldTransition;

		public TransitionWrapper(
				Action<TransitionBase<TData, TStateId>> beforeOnEnter = null,
				Action<TransitionBase<TData, TStateId>> afterOnEnter = null,

				Action<TransitionBase<TData, TStateId>> beforeShouldTransition = null,
				Action<TransitionBase<TData, TStateId>> afterShouldTransition = null)
		{
			this.beforeOnEnter = beforeOnEnter;
			this.afterOnEnter = afterOnEnter;

			this.beforeShouldTransition = beforeShouldTransition;
			this.afterShouldTransition = afterShouldTransition;
		}

		public WrappedTransition Wrap(TransitionBase<TData, TStateId> transition)
		{
			return new WrappedTransition(
				transition,
				beforeOnEnter,
				afterOnEnter,
				beforeShouldTransition,
				afterShouldTransition
			);
		}
	}

	public class TransitionWrapper<TData> : TransitionWrapper<TData, string>
	{
		public TransitionWrapper(
			Action<TransitionBase<TData, string>> beforeOnEnter = null,
			Action<TransitionBase<TData, string>> afterOnEnter = null,

			Action<TransitionBase<TData, string>> beforeShouldTransition = null,
			Action<TransitionBase<TData, string>> afterShouldTransition = null) : base(
			beforeOnEnter, afterOnEnter,
			beforeShouldTransition, afterShouldTransition)
		{
		}
	}
}
