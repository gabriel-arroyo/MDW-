namespace Custom.Control
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.Windows.Forms;

	/// <summary>
	/// Specifies how items in a SortListView are sorted.
	/// </summary>
	/// <remarks>
	/// Use the members of this enumeration to set the value of the Sorting property of the SortListView control.
	/// </remarks>
	public enum SortOrder 
	{ 
		/// <summary>
		/// Sort items in ascending order.
		/// </summary>
		Ascending, 
		/// <summary>
		/// Sort items in descending order.
		/// </summary>
		Descending, 
		/// <summary>
		/// Do not sort items.
		/// </summary>
		None 
	}

	/// <summary>
	/// Exposes a method that compares two objects using a AdrianStanley.Windows.Forms.SortOrder.
	/// </summary>
	/// <remarks>
	/// The SortComparer class is used in conjunction with the 
	/// AdrianStanley.Windows.Forms.SortListView.Sort method. 
	/// It provides a way to customize the sort order of items in a SortListView.
	/// The class is marked as abstract, as it acts as a base class for derived classes
	/// which implement the Compare method.
	/// </remarks>
	public abstract class SortComparer : IComparer
	{
		private SortOrder m_sortOrder	= SortOrder.None;
		private int m_sortColumn		= -1;

		/// <summary>
		/// Initializes a new default instance of the SortComparer class.
		/// </summary>
		/// <remarks>
		/// The new SortComparer instance has a default sort order of AdrianStanley.Windows.Forms.SortOrder.None, 
		/// and a default sort column of -1 (none).
		/// </remarks>
		public SortComparer()
		{
			m_sortOrder		= SortOrder.None;
			m_sortColumn	= -1;
		}

		/// <summary>
		/// Initializes a new instance of the SortComparer class.
		/// </summary>
		/// <param name="sortOrder">The sort order</param>
		/// <param name="sortColumn">The sort column</param>
		/// <remarks>
		/// The new SortComparer instance has a sort order of <paramref name="sortOrder"/> 
		/// and a sort column of <paramref name="sortColumn"/>.
		/// </remarks>
		public SortComparer(SortOrder sortOrder, int sortColumn)
		{
			m_sortOrder		= sortOrder;
			m_sortColumn	= sortColumn;
		}
		
		/// <summary>
		/// Gets or sets the sort order for the SortComparer.
		/// </summary>
		/// <value>
		/// One of the AdrianStanley.Windows.Forms.SortOrder values. 
		/// The default is AdrianStanley.Windows.Forms.SortOrder.None.
		/// </value>
		/// <remarks>
		/// The Sorting property allows you to specify whether or not items are sorted 
		/// by the SortComparer. By default, no sorting is performed. 
		/// When the Sorting property is set to SortOrder.Ascending or SortOrder.Descending, 
		/// the SortComparer automatically sorts in ascending order (when the property is 
		/// set to SortOrder.Ascending) or descending order (when the property is set to 
		/// SortOrder.Descending). 
		/// </remarks>
		public SortOrder Sorting 
		{
			get { return m_sortOrder; }
			set { m_sortOrder = value; }
		}

		/// <summary>
		/// Gets or sets the sort column for the SortComparer.
		/// </summary>
		/// <value>
		/// The column to be sorted. The default is -1.
		/// </value>
		/// <remarks>
		/// The SortColumn property allows you to specify the column to be sorted in the
		/// SortListView, numbered from 0 for the left most column, to 
		/// SortListView.Items.Count - 1 for the right most column. -1 indicates no column
		/// is to be sorted. .
		/// </remarks>
		public int SortColumn
		{
			get { return m_sortColumn; }
			set { m_sortColumn = value; }
		}

		/// <summary>
		/// Compares two specified objects.
		/// </summary>
		/// <param name="x">The first object.</param>
		/// <param name="y">The second object.</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="x"/> is less than <paramref name="y"/>. 
		/// Zero: <paramref name="x"/> equals <paramref name="y"/>. 
		/// Greater than zero: <paramref name="x"/> equals <paramref name="y"/>. 
		/// </returns>
		/// <remarks>
		/// The comparison of the two objects takes into account the current sort order.
		/// </remarks>
		public int Compare(object x, object y)
		{
			ListViewItem lvItemX = (ListViewItem)x;
			ListViewItem lvItemY = (ListViewItem)y;

			return 
				Sorting == SortOrder.Ascending ?	
				Compare(lvItemX, lvItemY) :
				Compare(lvItemY, lvItemX);
		}

		/// <summary>
		/// Compares two specified ListViewItem objects.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="x"/> is less than <paramref name="y"/>. 
		/// Zero: <paramref name="x"/> equals <paramref name="y"/>. 
		/// Greater than zero: <paramref name="x"/> equals <paramref name="y"/>. 
		/// </returns>
		/// <remarks>
		/// This method is abstract; override it in derive classes to implement type specific
		/// comparisons.
		/// </remarks>
		protected abstract int Compare(ListViewItem lvItemA, ListViewItem lvItemB);
	}

	/// <summary>
	/// SortComparer for sorting strings.
	/// </summary>
	/// <remarks>
	/// Use the ComparerString class to sort strings in a SortListView column.
	/// </remarks>
	public class ComparerString : SortComparer
	{
		/// <summary>
		/// Compares two specified ListViewItem objects as strings.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="lvItemA"/> is less than <paramref name="lvItemB"/>. 
		/// Zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// Greater than zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// </returns>
		/// <remarks>
		/// The value of the SortColumn property in the base SortComparer class is used to
		/// determine which column will be sorted. The Text property of the 
		/// ListViewSubItem that is <paramref name="lvItemA"/>.SubItem[SortColumn], is compared
		/// to the Text property of the ListViewSubItem that is 
		/// <paramref name="lvItemB"/>.SubItem[SortColumn].
		/// </remarks>
		protected override int Compare(ListViewItem lvItemA, ListViewItem lvItemB)
		{
			return lvItemA.SubItems[SortColumn].Text.CompareTo(lvItemB.SubItems[SortColumn].Text);
		}
	}

	/// <summary>
	/// SortComparer for sorting short integers represented as strings.
	/// </summary>
	/// <remarks>
	/// Use the ComparerStringAsShort class to sort strings in a SortListView column, 
	/// where each string represents a short.
	/// </remarks>
	public class ComparerStringAsShort : SortComparer
	{
		/// <summary>
		/// Compares two specified ListViewItem objects as shorts.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="lvItemA"/> is less than <paramref name="lvItemB"/>. 
		/// Zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// Greater than zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// </returns>
		/// <remarks>
		/// The value of the SortColumn property in the base SortComparer class is used to
		/// determine which column will be sorted. The short value of the Text property 
		/// of the ListViewSubItem that is <paramref name="lvItemA"/>.SubItem[SortColumn], 
		/// is compared to the short value of the Text property of the ListViewSubItem 
		/// that is <paramref name="lvItemB"/>.SubItem[SortColumn].
		/// </remarks>
		protected override int Compare(ListViewItem lvItemA, ListViewItem lvItemB)
		{
			return Int16.Parse(lvItemA.SubItems[SortColumn].Text).CompareTo(Int16.Parse(lvItemB.SubItems[SortColumn].Text));
		}
	}

	/// <summary>
	/// SortComparer for sorting ints represented as strings.
	/// </summary>
	/// <remarks>
	/// Use the ComparerStringAsInt class to sort strings in a SortListView column, 
	/// where each string represents an int.
	/// </remarks>
	public class ComparerStringAsInt : SortComparer
	{
		/// <summary>
		/// Compares two specified ListViewItem objects as ints.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="lvItemA"/> is less than <paramref name="lvItemB"/>. 
		/// Zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// Greater than zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// </returns>
		/// <remarks>
		/// The value of the SortColumn property in the base SortComparer class is used to
		/// determine which column will be sorted. The int value of the Text property 
		/// of the ListViewSubItem that is <paramref name="lvItemA"/>.SubItem[SortColumn], 
		/// is compared to the int value of the Text property of the ListViewSubItem 
		/// that is <paramref name="lvItemB"/>.SubItem[SortColumn].
		/// </remarks>
		protected override int Compare(ListViewItem lvItemA, ListViewItem lvItemB)
		{
			return Int32.Parse(lvItemA.SubItems[SortColumn].Text).CompareTo(Int32.Parse(lvItemB.SubItems[SortColumn].Text));
		}
	}

	/// <summary>
	/// SortComparer for sorting longs represented as strings.
	/// </summary>
	/// <remarks>
	/// Use the ComparerStringAsLong class to sort strings in a SortListView column, 
	/// where each string represents a long.
	/// </remarks>
	public class ComparerStringAsLong : SortComparer
	{
		/// <summary>
		/// Compares two specified ListViewItem objects as longs.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="lvItemA"/> is less than <paramref name="lvItemB"/>. 
		/// Zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// Greater than zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// </returns>
		/// <remarks>
		/// The value of the SortColumn property in the base SortComparer class is used to
		/// determine which column will be sorted. The long value of the Text property 
		/// of the ListViewSubItem that is <paramref name="lvItemA"/>.SubItem[SortColumn], 
		/// is compared to the long value of the Text property of the ListViewSubItem 
		/// that is <paramref name="lvItemB"/>.SubItem[SortColumn].
		/// </remarks>
		protected override int Compare(ListViewItem lvItemA, ListViewItem lvItemB)
		{
			return Int64.Parse(lvItemA.SubItems[SortColumn].Text).CompareTo(Int64.Parse(lvItemB.SubItems[SortColumn].Text));
		}
	}

	/// <summary>
	/// SortComparer for sorting doubles represented as strings.
	/// </summary>
	/// <remarks>
	/// Use the ComparerStringAsDouble class to sort strings in a SortListView column, 
	/// where each string represents a double.
	/// </remarks>
	public class ComparerStringAsDouble : SortComparer
	{
		/// <summary>
		/// Compares two specified ListViewItem objects as doubles.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="lvItemA"/> is less than <paramref name="lvItemB"/>. 
		/// Zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// Greater than zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// </returns>
		/// <remarks>
		/// The value of the SortColumn property in the base SortComparer class is used to
		/// determine which column will be sorted. The double value of the Text property 
		/// of the ListViewSubItem that is <paramref name="lvItemA"/>.SubItem[SortColumn], 
		/// is compared to the double value of the Text property of the ListViewSubItem 
		/// that is <paramref name="lvItemB"/>.SubItem[SortColumn].
		/// </remarks>
		protected override int Compare(ListViewItem lvItemA, ListViewItem lvItemB)
		{
			return Double.Parse(lvItemA.SubItems[SortColumn].Text).CompareTo(Double.Parse(lvItemB.SubItems[SortColumn].Text));
		}
	}

	/// <summary>
	/// SortComparer for sorting dates represented as strings.
	/// </summary>
	/// <remarks>
	/// Use the ComparerStringAsShort class to sort strings in a SortListView column, 
	/// where each string represents a date.
	/// </remarks>
	public class ComparerStringAsDateTime : SortComparer
	{
		/// <summary>
		/// Compares two specified ListViewItem objects as shorts.
		/// </summary>
		/// <param name="lvItemA">The first ListViewItem object</param>
		/// <param name="lvItemB">The second ListViewItem object</param>
		/// <returns>
		/// A 32-bit signed integer indicating the lexical relationship between the two comparands.
		/// Less than zero: <paramref name="lvItemA"/> is less than <paramref name="lvItemB"/>. 
		/// Zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// Greater than zero: <paramref name="lvItemA"/> equals <paramref name="lvItemB"/>. 
		/// </returns>
		/// <remarks>
		/// The value of the SortColumn property in the base SortComparer class is used to
		/// determine which column will be sorted. The DateTime value of the Text property 
		/// of the ListViewSubItem that is <paramref name="lvItemA"/>.SubItem[SortColumn], 
		/// is compared to the DateTime value of the Text property of the ListViewSubItem 
		/// that is <paramref name="lvItemB"/>.SubItem[SortColumn].
		/// </remarks>
		protected override int Compare(ListViewItem lvItemA, ListViewItem lvItemB)
		{
			return DateTime.Parse(lvItemA.SubItems[SortColumn].Text).CompareTo(DateTime.Parse(lvItemB.SubItems[SortColumn].Text));
		}
	}
}
