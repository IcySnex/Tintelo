using SkeleKit;

namespace Tintelo.iOS.Views;

public class SquareRowPanel : Panel
{
	public double Spacing { get; set; }

	public double MaxItemSize { get; set; } = double.PositiveInfinity;

	
	int VisibleCount()
	{
		int count = 0;
		foreach (View child in Children)
		{
			if (child.IsVisible.Value)
				count++;
		}

		return count;
	}
	

	protected override Size MeasureOverride(
		Size availableSize)
	{
		Thickness insets = ContentInsets;
		Size inner = availableSize.Deflate(insets);

		int count = VisibleCount();
		if (count == 0)
			return Size.Zero.Inflate(insets);

		double gaps = Spacing * (count - 1);
		double side;

		if (double.IsFinite(inner.Width))
		{
			side = Math.Min(Math.Max(0, (inner.Width - gaps) / count), MaxItemSize);
		}
		else
		{
			side = 0;

			foreach (View child in Children)
			{
				if (!child.IsVisible.Value)
					continue;

				child.Measure(Size.Infinity);
				side = Math.Max(side, child.DesiredSize.Width);
			}

			side = Math.Min(side, MaxItemSize);
		}

		foreach (View child in Children)
		{
			if (child.IsVisible.Value)
				child.Measure(new(side, side));
		}

		double width = double.IsFinite(inner.Width)
			? Math.Min(inner.Width, side * count + gaps)
			: side * count + gaps;

		return new Size(width, side).Inflate(insets);
	}
	
	protected override Size ArrangeOverride(
		Size finalSize)
	{
		Thickness insets = ContentInsets;
		Size inner = finalSize.Deflate(insets);

		int count = VisibleCount();
		if (count == 0)
			return finalSize;

		double gaps = Spacing * (count - 1);
		double side = Math.Min(
			Math.Min(Math.Max(0, (inner.Width - gaps) / count), inner.Height),
			MaxItemSize);
		double row = side * count + gaps;
		double x = insets.Left + Math.Max(0, (inner.Width - row) / 2);
		double y = insets.Top + Math.Max(0, (inner.Height - side) / 2);

		foreach (View child in Children)
		{
			if (!child.IsVisible.Value)
				continue;

			child.Arrange(new(x, y, side, side));
			x += side + Spacing;
		}

		return finalSize;
	}
}