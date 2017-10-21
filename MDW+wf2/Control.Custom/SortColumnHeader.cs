namespace Custom.Control
{
	using System;
	using System.Windows.Forms;

	/// <summary>
	/// Displays a single column in a SortListView control.
	/// </summary>
	/// <remarks>
	/// The SortColumnHeader represents a column header in a AdrianStanley.Windows.Forms.SortListView control.
	/// <p/>
	/// A column header is an item in a SortListView control that contains heading text,
	/// and a sort order. SortColumnHeader objects can be added to a SortListView using the 
	/// Add method of the base ListView.ColumnHeaderCollection class. 
	/// <p/>
	/// SortColumnHeader provides the Sorting property to set the sort order for the column,
	/// and the SortComparer property to set the comparer to be used to sort items in the 
	/// column.
	/// </remarks>
	public class SortColumnHeader : ColumnHeader
	{
		private SortOrder m_sortOrder				= SortOrder.None;
		private SortComparer m_columnHeaderSorter	= null;

		/// <summary>
		/// Initializes a new default instance of the SortColumnHeader class.
		/// </summary>
		/// <remarks>
		/// The new SortColumnHeader instance has a default sort order of SortOrder.None, 
		/// and a default sort comparer of System.Windows.Forms.CompareString.
		/// </remarks>
		public SortColumnHeader()
		{
			m_sortOrder				= SortOrder.None;
			m_columnHeaderSorter	= new ComparerString();
		}

		/// <summary>
		/// Initializes a new instance of the SortColumnHeader class.
		/// </summary>
		/// <param name="sortOrder">The sort order</param>
		/// <param name="columnHeaderSorter">The sort comparer</param>
		/// <remarks>
		/// The new SortColumnHeader instance has a sort order <paramref name="sortOrder"/>, 
		/// and a sort comparer of <paramref name="columnHeaderSorter"/>.
		/// </remarks>
		public SortColumnHeader(SortOrder sortOrder, SortComparer columnHeaderSorter)
		{
			m_sortOrder				= sortOrder;
			m_columnHeaderSorter	= columnHeaderSorter;
		}

		/// <summary>
		/// Gets or sets the sort order for the column.
		/// </summary>
		/// <value>
		/// One of the AdrianStanley.Windows.Forms.SortOrder values. 
		/// The default is AdrianStanley.Windows.Forms.SortOrder.None.
		/// </value>
		/// <remarks>
		/// The Sorting property allows you to specify whether or not items are sorted 
		/// for this column. By default, no sorting is performed. 
		/// When the Sorting property is set to SortOrder.Ascending or SortOrder.Descending, 
		/// the column automatically sorts in ascending order (when the property is 
		/// set to SortOrder.Ascending) or descending order (when the property is set to 
		/// SortOrder.Descending). 
		/// </remarks>
		public SortOrder Sorting 
		{
			get { return m_sortOrder; }
			set 
			{ 
				m_sortOrder						= value; 
				m_columnHeaderSorter.Sorting	= m_sortOrder;
			}
		}

		/// <summary>
		/// Gets or sets the sort comparer for the column.
		/// </summary>
		/// <value>
		/// An object of a class derived from AdrianStanley.Windows.Forms.SortComparer. 
		/// The default is AdrianStanley.Windows.Forms.ComparerStrings.
		/// </value>
		/// <remarks>
		/// The SortComparer property is used in conjunction with the 
		/// AdrianStanley.Windows.Forms.SortListView.Sort method. 
		/// It provides a way to customize the sort order of items in a SortListView.
		/// </remarks>
		public SortComparer ColumnHeaderSorter
		{
			get { return m_columnHeaderSorter; }
			set { m_columnHeaderSorter = value; }
		}
	}
}
