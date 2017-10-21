namespace Custom.Control
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.Windows.Forms;
	using System.Runtime.InteropServices;
    using System.Drawing;
#if PERSIST_SORTLISTVIEW
	using OpenNETCF.Win32;
#endif

	#region SortListView class
	/// <summary>
	/// Represents a Windows list view control, 
	/// which displays a collection of sorted items in a details view.
	/// </summary>
	/// <remarks>
	/// The SortListView class extends the existing .Net Compact Framework ListView class, 
	/// when it used in a details view, with ListView.View set to View.Details. In details view, 
	/// the ListView control has a column header. The SortListView class will automatically sort 
	/// the items in the ListView control when a column header is clicked. 
	/// <p/>
	/// When a column header is clicked for the first time, the items in the column are sorted 
	/// in ascending order. The column is now the sorted column; the ordering of items in the 
	/// other columns follow the ordering of the items in the clicked column. Clicking the column header again will sort the items in the column in 
	/// descending order. A sort arrow is shown in the column header to show the direction of
	/// the sort - an upwards pointing arrow for ascending order, and a downwards pointing arrow 
	/// for descending order. By default the sort arrow is shown to the right of any text in the 
	/// column header. If the column header text is aligned to the right then the sort arrow 
	/// will be shown to the left of the text.
	/// <p/>
	/// Each column has its own sort order, and its own comparer for determining how items in 
	/// the column should be sorted. The sort order is defined as one of the values in the enum 
	/// AdrianStanley.Windows.Forms.SortOrder, and the comparer is defined as an instance of a 
	/// class derived from AdrianStanley.Windows.Forms.SortComparer. The AdrianStanley.Windows.Forms
	/// namespace defines some existing comparers for sorting string, int, short, long, 
	/// double and DateTime objects.
	/// <p/>
	/// The state of the SortListView control can be persisted to and from the registry. The persisted state consists of the number of the 
	/// column that is currently sorted, and the last used sort order for each column. The 
	/// persistance mechanism uses the OpenNETCF.Win32 namespace, available from 
	/// http://www.opennetcf.org. To enable the persistance mechanism, define the 
	/// preprocessor constant PERSIST_SORTLISTVIEW, set the PersistKeyName 
	/// property to the key to be used in the registry for persisting values, and then call 
	/// the LoadState and SaveState methods to respectively load the state from the registry, 
	/// and to save the state to the registry. 
	/// <p/>
	/// The SortListView class requires unmanaged code (contained in ControlEx.dll) to show the 
	/// sort arrow within the column header.
	/// </remarks>
	public class SortListView : ListView
	{
		private IntPtr m_handle						= new IntPtr(0);
		private int m_sortColumn					= -1;
		private int m_lastSortColumn				= -1;
		private bool m_disposed						= false;
		//private bool m_sortListViewCreated			= false;
#if PERSIST_SORTLISTVIEW
		private string m_persistKeyName;
#endif

		#region Construction and destruction.
		/// <summary>
		/// Initializes a new instance of the SortListView class.
		/// </summary>
		/// <remarks>
		/// The new SortListView instance has an ColumnClick event handler defined to handle 
		/// the user clicking a column header to sort the column.
		/// </remarks>
		public SortListView()
		{
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);

			// Capture mouse or stylus input to this ListView control, and then
			// use the unmanaged GetCapture() API to get the handle of the window 
			// that has captured the mouse or stylus input. 
			/*bool capture = Capture;
			Capture = true;
			m_handle = GetCapture();
			Capture = capture;*/

			ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(OnColumnClick);
		}
        /// <summary>
        /// Flicker Free Handle
        /// </summary>
        /// <param name="m"></param>
        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

		/// <summary>
		/// Overrides base class Dispose method.
		/// </summary>
		/// <param name="disposing">true if called directly or indirectly by user's code.</param>
		/// <remarks>
		/// See "Implementing a Dispose method" for design pattern for derived class.
		/// </remarks>
		protected override void Dispose(bool disposing)
		{
			if (!m_disposed)
			{
				try
				{
					if (disposing)
					{
						// Called directly or indirectly by user's code.
						// Dispose managed resources.
					}

					// Now release unmanaged resources.
					/*if (SortListView_Destroy(m_handle))
					{
						// Property has no set accessor, so use the underlying field.
						m_handle = new IntPtr(0);
					}*/

					m_disposed = true;
				}
				finally
				{
					base.Dispose(disposing);
				}
			}
		}
		#endregion

		#region Loading and saving state
#if PERSIST_SORTLISTVIEW
		/// <summary>
		/// Gets or sets the registry key name for the SortListView object.
		/// </summary>
		/// <value>
		/// The value is within the HKEY_CURRENT_USER branch in the registry.
		/// </value>
		/// <remarks>
		/// It is recommended that a value of the form 
		/// "SOFTWARE\<i>CompanyName</i>\<i>ProductName</i>\<i>ListViewName</i>" is used.
		/// </remarks>
		public string PersistKeyName
		{
			get { return m_persistKeyName; }
			set { m_persistKeyName = value; }
		}

		/// <summary>
		/// Loads the SortListView object state from the registry.
		/// </summary>
		/// <remarks>
		/// The state consists of the number of the currently sorted column, and the last 
		/// used sort order for each column. LoadState() does not automatically call
		/// Sort() to sort the SortListView object.
		/// </remarks>
		public void LoadState()
		{
			Debug.Assert(m_persistKeyName.Length > 0, "m_persistKeyName.Length == 0");

			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(m_persistKeyName);

			try
			{
				SortColumn = (int)(uint)registryKey.GetValue("SortColumn", 0U);
				int column = 0;
				foreach (SortColumnHeader sortColumnHeader in Columns)
				{
					switch ((uint)registryKey.GetValue("ColumnSortOrder" + column.ToString(), (uint)SortOrder.None))
					{
						case 0:
							sortColumnHeader.Sorting = SortOrder.Ascending;
							break;
						case 1:
							sortColumnHeader.Sorting = SortOrder.Descending;
							break;
						default:
							sortColumnHeader.Sorting = SortOrder.None;
							break;
					}
					column++;
				}
			}
			finally
			{
				registryKey.Close();
			}
		}

		/// <summary>
		/// Saves the SortListView object state to the registry.
		/// </summary>
		/// <remarks>
		/// The state consists of the number of the currently sorted column, and the last 
		/// used sort order for each column. SaveState() is not automatically called when 
		/// the SortListView object is disposed or finalized.
		/// </remarks>
		public void SaveState()
		{
			Debug.Assert(m_persistKeyName.Length > 0, "m_persistKeyName.Length == 0");

			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(m_persistKeyName);

			try
			{
				registryKey.SetValue("SortColumn", SortColumn);
				int column = 0;
				foreach(SortColumnHeader sortColumnHeader in Columns)
				{
					uint sortOrder = 2;
					switch (sortColumnHeader.Sorting)
					{
						case SortOrder.Ascending:
							sortOrder = 0;
							break;
						case SortOrder.Descending:
							sortOrder = 1;
							break;
						default:
							sortOrder = 2;
							break;
					}
					registryKey.SetValue("ColumnSortOrder" + column.ToString(), sortOrder);
					column++;
				}
			}
			finally
			{
				registryKey.Close();
			}
		}
#endif
		#endregion

		#region Public properties
		/// <summary>
		/// Gets the window handle that the control is bound to.
		/// </summary>
		/// <value>
		/// An IntPtr that contains the window handle (HWND) of the control.
		/// </value>
		/// <remarks>
		/// The value of the Handle property is a Windows HWND.
		/// </remarks>
		public IntPtr Handle
		{
			get { return m_handle; }
		}

		/// <summary>
		/// Gets or sets the SortComparer object for the current sort column.
		/// </summary>
		/// <value>
		/// An object derived from AdrianStanley.Windows.Forms.SortComparer, 
		/// that will be used for sorting the items in the control.
		/// </value>
		/// <remarks>
		/// The value of the ListViewItemSorter property is an object derived from
		/// AdrianStanley.Windows.Forms.SortComparer, that will be used for sorting 
		/// the items in the control when Sort is called.
		/// </remarks>
		public SortComparer ListViewItemSorter
		{
			get 
			{ 
				SortComparer sortComparer = null;
				if (SortColumnHeader != null)
				{
					sortComparer = SortColumnHeader.ColumnHeaderSorter;
				}
				return sortComparer;
			}
			set 
			{ 
				if (SortColumnHeader != null)
				{
					SortColumnHeader.ColumnHeaderSorter = value;
					Sort(); 
				}
			}
		}

		/// <summary>
		/// Gets or sets the sort order for the control.
		/// </summary>
		/// <value>
		/// One of the AdrianStanley.Windows.Forms.SortOrder values. 
		/// The default is AdrianStanley.Windows.Forms.SortOrder.None.
		/// </value>
		/// <remarks>
		/// The Sorting property allows you to specify whether or not items are sorted 
		/// in the control. By default, no sorting is performed. 
		/// When the Sorting property is set to SortOrder.Ascending or SortOrder.Descending, 
		/// the control automatically sorts in ascending order (when the property is 
		/// set to SortOrder.Ascending) or descending order (when the property is set to 
		/// SortOrder.Descending). 
		/// </remarks>
		public SortOrder Sorting 
		{
			get 
			{ 
				SortOrder sortOrder = SortOrder.None;
				if (SortColumnHeader != null)
				{
					sortOrder = SortColumnHeader.Sorting;
				}
				return sortOrder;
			}
			set 
			{ 
				if (SortColumnHeader != null)
				{
					SortColumnHeader.Sorting = value;
					Sort();
				}
			}
		}

		/// <summary>
		/// Gets or sets the SortColumnHeader object for the current sort column.
		/// </summary>
		/// <value>
		/// An AdrianStanley.Windows.Forms.SortColumnHeader object which represents the 
		/// column header for the current sort column.
		/// </value>
		/// <remarks>
		/// The value of the SortColumnHeader property is an instance of the 
		/// AdrianStanley.Windows.Forms.SortColumnHeader class.
		/// </remarks>
		public SortColumnHeader SortColumnHeader
		{
			get
			{
				Debug.Assert(m_sortColumn > -1, "m_sortColumn < 0");
				Debug.Assert(m_sortColumn < Columns.Count, "m_sortColumn >= Columns.Count");

				SortColumnHeader sortColumnHeader = null;
				if (m_sortColumn > -1 && m_sortColumn < Columns.Count)
				{
					sortColumnHeader = (SortColumnHeader)Columns[m_sortColumn];
				}
				else
				{
					throw new ArgumentException("Invalid sort column");
				}

				return sortColumnHeader;
			}
		}

		/// <summary>
		/// Gets or sets the current sort column for the control.
		/// </summary>
		/// <value>
		/// An int representing the sort column, where 0 is the left most column in the 
		/// control, and the right most column is ListView.Columns.Count - 1. The value -1
		/// means that there is no current sort column.
		/// </value>
		/// <remarks>
		/// The SortColumn property allows you to specify the sort column in which items are 
		/// to be sorted. The ordering of the items in the other columns follow the sort column.
		/// </remarks>
		public int SortColumn
		{
			get { return m_sortColumn; }

			set 
			{ 
				Debug.Assert(value >= -1, "value < -1");
				Debug.Assert(value < Columns.Count, "value >= Columns.Count");

				if (value >= -1 && value < Columns.Count)
				{
					m_lastSortColumn	= m_sortColumn;
					m_sortColumn		= value;

					if (SortColumnHeader != null)
					{
						SortColumnHeader.ColumnHeaderSorter.SortColumn = m_sortColumn;
					}
				}
				else
				{
					throw new ArgumentException("Invalid sort column");
				}
			}
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Sorts the items of the list view.
		/// </summary>
		/// <remarks>
		/// The Sort method sorts the current sort column using the current sort order for that
		/// column, and the current sort comparer for that column.
		/// </remarks>
		public void Sort()
		{
			Debug.Assert(View == View.Details);

			// If Sorting == SortOrder.None, then ShowSortArrow will remove the sort arrow.
			//ShowSortArrow();

			if (ListViewItemSorter.Sorting != SortOrder.None)
			{
				// Need to sort the column.

				// Copy all the items in the ListView control to an array.
				ListViewItem[] arrayItems = new ListViewItem[Items.Count];
				Items.CopyTo(arrayItems, 0);

				// Sort the copied items.
				Array.Sort(arrayItems, 0, arrayItems.Length, ListViewItemSorter);

				// Replace the existing items in the ListView control with the sorted items.
				BeginUpdate();

				// Before clearing the existing items in the control, save which ones are selected.
				int[] arraySelectedIndices = new int[SelectedIndices.Count];
				SelectedIndices.CopyTo(arraySelectedIndices, 0);

				Items.Clear();

				foreach (ListViewItem lvItem in arrayItems)
				{
					Items.Add(lvItem);
				}

				// Restore selection of items.
				foreach (int selectedIndex in arraySelectedIndices)
				{
					Items[selectedIndex].Selected = true;
				}

				EndUpdate();
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Handler for the ColumnClick event.
		/// </summary>
		/// <param name="sender">The SortListView control that sent the event</param>
		/// <param name="e">A ColumnClickEventArgs that contains the event data</param>
		/// <remarks>
		/// The OnColumnClick event is fired when a user clicks a column header.
		/// </remarks>
		private void OnColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
			Debug.Assert(sender.GetType() == typeof(SortListView), "sender.GetType() != typeof(SortListView)");

			if (sender.GetType() == typeof(SortListView))
			{
				SortOrder sortOrder = SortOrder.None;
				if (e.Column == SortColumn)
				{
					// Clicked on column that currently has sort arrow, so flip the state.
					if (Sorting == SortOrder.Ascending)
					{
						sortOrder = SortOrder.Descending;
					}
					else
					{
						sortOrder = SortOrder.Ascending;
					}

					SortColumn = e.Column;
				}
				else
				{
					// This changes the Sorting property to point to the new column.
					SortColumn = e.Column;

					if (Sorting == SortOrder.None)
					{
						// Default to ascending sort order.
						sortOrder = SortOrder.Ascending;
					}
					else
					{
						// Use existing sort order.
						sortOrder = Sorting;
					}
				}

				// This will execute the sort.
				Sorting = sortOrder;
			}
			else
			{
				throw new ArgumentException("Invalid sender");
			}
		}
		#endregion

		#region Private methods
		/// <summary>
		/// Shows the current sort arrow for the current sort colum.
		/// </summary>
		/// <returns>
		/// Returns true if the sort arrow is shown successfully.
		/// </returns>
		/// <remarks>
		/// The ShowSortArrow method uses unmanaged methods defined in ControlEx.dll to 
		/// display the sort arrow. The sort arrow will be displayed either to the left or 
		/// the right of any text in the column header, depending on the horizontal alignment 
		/// of the column header. If there is no current sort column, then any existing 
		/// sort arrow will be removed.
		/// </remarks>
		private bool ShowSortArrow()
		{
			bool success = false;
            /*
                        Debug.Assert(m_handle != new IntPtr(0), "m_handle == new IntPtr(0)");

                        if (m_handle != new IntPtr(0))
                        {
                             It is possible for the m_sortColumn to be -1, which removes any sort arrow.
                            if (!m_sortListViewCreated)
                            {
                                if ((m_sortListViewCreated = SortListView_Create(m_handle)) != true)
                                {
                                    throw new ExternalException("SortListView_Create error = 0x" + Marshal.GetLastWin32Error().ToString("x"));
                                }
                            }

                            success = 
                                SortListView_SetSortColumn(
                                    m_handle, 
                                    m_sortColumn, 
                                    ref m_lastSortColumn,
                                    m_sortColumn > -1 ? SortColumnHeader.Sorting : SortOrder.None, 
                                    m_sortColumn > -1 ? SortColumnHeader.TextAlign == HorizontalAlignment.Right : false);

                            if (!success)
                            {
                                throw new ExternalException("SortListView_SetSortColumn error = 0x" + Marshal.GetLastWin32Error().ToString("x"));
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid handle");
                        }
                        */
            return success;
		}
		#endregion


		#region Unmanaged methods
//#if WindowsCE
        //[DllImport("coredll.dll")]
//#elif WIN32
        //[DllImport("user32.dll")]
//#endif
        [DllImport("user32.dll")]
		private static extern IntPtr GetCapture();

		[DllImport("ControlEx.dll", SetLastError=true)]
		private static extern bool SortListView_Create(IntPtr hwndLV);

		[DllImport("ControlEx.dll", SetLastError=true)]
		private static extern bool SortListView_Destroy(IntPtr hwndLV);

		[DllImport("ControlEx.dll", SetLastError=true)]
		private static extern bool SortListView_SetSortColumn(IntPtr hwndLV, int iColumn, ref int iLastColumn, SortOrder sortOrder, bool bTextAlignmentRight);
		
        #endregion
	}
	#endregion

	#region SortListViewItem class
	/// <summary>
	/// Represents an item in a Windows list view control. 
	/// </summary>
	/// <remarks>
	/// A ListViewItem object contains the information for a single item 
	/// in a ListView control. When the ListView control is in details view, then the 
	/// ListViewItem object contains the data for a single row in the control. 
	/// <p/>
	/// The SortListViewItem class extends the base ListViewItem class with a Tag property. 
	/// The Tag property can be used for storing an object associated with the SortListViewItem 
	/// object, i.e., an object associated with a specific row in the ListView control. 
	/// The Tag property mimics the ListViewItem.Tag property available in the .Net Framework, but 
	/// not implemented in the .Net Compact Framework.
	/// <p/>
	/// It is not necessary to use the SortListViewItem class to enable sorting in the 
	/// SortListView class, as the SortListView class sorts items based on the text displayed 
	/// in the ListView control, and does not use the value of the Tag property for sorting.
	/// </remarks>
	public class SortListViewItem : ListViewItem
	{
		private object m_Tag;

		/// <summary>
		/// Initializes a new instance of the SortListViewItem class with an array of strings 
		/// representing subitems, and a tag object.
		/// </summary>
		/// <param name="items">An array of strings that represent the subitems of the new item</param>
		/// <param name="tag">A tag object to be associated with the SortListViewItem object.</param>
		public SortListViewItem(string[] items, object tag) : base(items)
		{
			m_Tag = tag;
		}

		/// <summary>
		/// Gets or sets the tag object for this SortListViewItem object.
		/// </summary>
		/// <value>
		/// Any reference type.
		/// </value>
		/// <remarks>
		/// The Tag property can be used for storing an object associated with the SortListViewItem 
		/// object, i.e., an object associated with a specific row in the ListView control. 
		/// The Tag property mimics the ListViewItem.Tag property available in the .Net Framework, but 
		/// not implemented in the .Net Compact Framework.
		/// </remarks>
		public object Tag
		{
			get { return m_Tag; }
			set { m_Tag = value; }
		}
	}
	#endregion
}
