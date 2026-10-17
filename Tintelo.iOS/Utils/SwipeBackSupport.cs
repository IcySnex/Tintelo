using System.Runtime.CompilerServices;

namespace Tintelo.iOS.Utils;

public static class SwipeBackSupport
{
	sealed class Hook : UIGestureRecognizerDelegate
	{
		readonly UINavigationController stack;

		public Hook(
			UINavigationController stack)
		{
			this.stack = stack;

			if (stack.InteractivePopGestureRecognizer is UIGestureRecognizer swipe)
			{
				swipe.AddTarget(Reassert);
				Claim(swipe);
			}

			if (OperatingSystem.IsIOSVersionAtLeast(26)
				&& stack.InteractiveContentPopGestureRecognizer is UIGestureRecognizer contentSwipe)
			{
				contentSwipe.AddTarget(Reassert);
				Claim(contentSwipe);
			}
		}

		
		public override bool ShouldBegin(
			UIGestureRecognizer recognizer) =>
			stack.ViewControllers is { Length: > 1 }
			&& stack.GetTransitionCoordinator() is null;

		
		public void Reassert()
		{
			if (stack.TopViewController?.NavigationItem.HidesBackButton is not true)
				return;

			if (stack.InteractivePopGestureRecognizer is UIGestureRecognizer swipe)
				Claim(swipe);

			if (OperatingSystem.IsIOSVersionAtLeast(26)
				&& stack.InteractiveContentPopGestureRecognizer is UIGestureRecognizer contentSwipe)
				Claim(contentSwipe);
		}

		void Claim(
			UIGestureRecognizer recognizer)
		{
			recognizer.Delegate = this;
			recognizer.Enabled = true;
		}
	}
	
	
	static readonly ConditionalWeakTable<UINavigationController, Hook> Hooks = new();


	public static void Attach(
		UINavigationController stack) =>
		Hooks.GetValue(stack, static stack => new Hook(stack)).Reassert();
}
