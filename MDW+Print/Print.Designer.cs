namespace MDW_Print
{
    partial class Print
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Print));
            this.Tab = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel29 = new System.Windows.Forms.Panel();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lbPrinters = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btQuitar = new System.Windows.Forms.ToolStripMenuItem();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btAddPrinter = new System.Windows.Forms.Button();
            this.lbLabelName = new System.Windows.Forms.Label();
            this.panel28 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tgWebSocketPrint = new System.Windows.Forms.CheckBox();
            this.tgWebServicePrint = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel18 = new System.Windows.Forms.TableLayoutPanel();
            this.tbPrefix = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
            this.label33 = new System.Windows.Forms.Label();
            this.tbPrinterAlias = new System.Windows.Forms.TextBox();
            this.btPrint = new System.Windows.Forms.Button();
            this.btOpenDefaultLabel = new System.Windows.Forms.Button();
            this.btOpenLabel = new System.Windows.Forms.Button();
            this.cbPrinters = new System.Windows.Forms.ComboBox();
            this.stkAjusteEPC = new System.Windows.Forms.TableLayoutPanel();
            this.label35 = new System.Windows.Forms.Label();
            this.txtNum = new System.Windows.Forms.NumericUpDown();
            this.panel30 = new System.Windows.Forms.Panel();
            this.PrintList = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbToolbarUser = new System.Windows.Forms.Label();
            this.lbToolbarIP = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbToolbarIPWifi = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Tab.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel29.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.panel5.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel28.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel18.SuspendLayout();
            this.tableLayoutPanel17.SuspendLayout();
            this.stkAjusteEPC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum)).BeginInit();
            this.panel30.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PrintList)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Tab
            // 
            this.Tab.Controls.Add(this.tabPage3);
            this.Tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tab.Location = new System.Drawing.Point(162, 59);
            this.Tab.Name = "Tab";
            this.Tab.SelectedIndex = 0;
            this.Tab.Size = new System.Drawing.Size(881, 556);
            this.Tab.TabIndex = 5;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel29);
            this.tabPage3.Controls.Add(this.panel28);
            this.tabPage3.Controls.Add(this.panel30);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(873, 530);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "impresión";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel29
            // 
            this.panel29.Controls.Add(this.Picture);
            this.panel29.Controls.Add(this.panel5);
            this.panel29.Controls.Add(this.lbLabelName);
            this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel29.Location = new System.Drawing.Point(180, 0);
            this.panel29.Name = "panel29";
            this.panel29.Size = new System.Drawing.Size(693, 317);
            this.panel29.TabIndex = 1;
            // 
            // Picture
            // 
            this.Picture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Picture.Location = new System.Drawing.Point(150, 0);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(543, 317);
            this.Picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Picture.TabIndex = 0;
            this.Picture.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lbPrinters);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.btAddPrinter);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(150, 317);
            this.panel5.TabIndex = 2;
            // 
            // lbPrinters
            // 
            this.lbPrinters.ContextMenuStrip = this.contextMenuStrip1;
            this.lbPrinters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPrinters.FormattingEnabled = true;
            this.lbPrinters.Location = new System.Drawing.Point(0, 44);
            this.lbPrinters.Margin = new System.Windows.Forms.Padding(2);
            this.lbPrinters.Name = "lbPrinters";
            this.lbPrinters.Size = new System.Drawing.Size(150, 273);
            this.lbPrinters.TabIndex = 4;
            this.lbPrinters.SelectedIndexChanged += new System.EventHandler(this.lbPrinters_SelectedIndexChanged);
            this.lbPrinters.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbPrinters_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btQuitar});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            this.contextMenuStrip1.Text = "Opciones";
            // 
            // btQuitar
            // 
            this.btQuitar.Name = "btQuitar";
            this.btQuitar.Size = new System.Drawing.Size(107, 22);
            this.btQuitar.Text = "Quitar";
            this.btQuitar.Click += new System.EventHandler(this.btQuitar_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 23);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(150, 21);
            this.panel6.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Impresoras Activas";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btAddPrinter
            // 
            this.btAddPrinter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.btAddPrinter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btAddPrinter.Dock = System.Windows.Forms.DockStyle.Top;
            this.btAddPrinter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(68)))), ((int)(((byte)(96)))));
            this.btAddPrinter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btAddPrinter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btAddPrinter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btAddPrinter.ForeColor = System.Drawing.Color.AliceBlue;
            this.btAddPrinter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btAddPrinter.Location = new System.Drawing.Point(0, 0);
            this.btAddPrinter.Name = "btAddPrinter";
            this.btAddPrinter.Size = new System.Drawing.Size(150, 23);
            this.btAddPrinter.TabIndex = 3;
            this.btAddPrinter.Text = "--> Añadir";
            this.btAddPrinter.UseVisualStyleBackColor = false;
            this.btAddPrinter.Click += new System.EventHandler(this.btAddPrinter_Click);
            // 
            // lbLabelName
            // 
            this.lbLabelName.AutoSize = true;
            this.lbLabelName.Location = new System.Drawing.Point(4, 5);
            this.lbLabelName.Name = "lbLabelName";
            this.lbLabelName.Size = new System.Drawing.Size(0, 13);
            this.lbLabelName.TabIndex = 1;
            // 
            // panel28
            // 
            this.panel28.Controls.Add(this.tableLayoutPanel4);
            this.panel28.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel28.Location = new System.Drawing.Point(0, 0);
            this.panel28.Name = "panel28";
            this.panel28.Size = new System.Drawing.Size(180, 317);
            this.panel28.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tgWebSocketPrint, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.tgWebServicePrint, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel18, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel17, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btPrint, 0, 8);
            this.tableLayoutPanel4.Controls.Add(this.btOpenDefaultLabel, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.btOpenLabel, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.cbPrinters, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.stkAjusteEPC, 0, 7);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 9;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(180, 319);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tgWebSocketPrint
            // 
            this.tgWebSocketPrint.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgWebSocketPrint.AutoSize = true;
            this.tgWebSocketPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.tgWebSocketPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tgWebSocketPrint.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(68)))), ((int)(((byte)(96)))));
            this.tgWebSocketPrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.SteelBlue;
            this.tgWebSocketPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.tgWebSocketPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.tgWebSocketPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tgWebSocketPrint.ForeColor = System.Drawing.Color.AliceBlue;
            this.tgWebSocketPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tgWebSocketPrint.Location = new System.Drawing.Point(2, 178);
            this.tgWebSocketPrint.Margin = new System.Windows.Forms.Padding(2);
            this.tgWebSocketPrint.Name = "tgWebSocketPrint";
            this.tgWebSocketPrint.Size = new System.Drawing.Size(176, 34);
            this.tgWebSocketPrint.TabIndex = 12;
            this.tgWebSocketPrint.Text = "Web Socket";
            this.tgWebSocketPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tgWebSocketPrint.UseVisualStyleBackColor = false;
            this.tgWebSocketPrint.CheckedChanged += new System.EventHandler(this.tgWebSocketPrint_CheckedChanged);
            // 
            // tgWebServicePrint
            // 
            this.tgWebServicePrint.Appearance = System.Windows.Forms.Appearance.Button;
            this.tgWebServicePrint.AutoSize = true;
            this.tgWebServicePrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.tgWebServicePrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tgWebServicePrint.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(68)))), ((int)(((byte)(96)))));
            this.tgWebServicePrint.FlatAppearance.CheckedBackColor = System.Drawing.Color.SteelBlue;
            this.tgWebServicePrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.tgWebServicePrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.tgWebServicePrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tgWebServicePrint.ForeColor = System.Drawing.Color.AliceBlue;
            this.tgWebServicePrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tgWebServicePrint.Location = new System.Drawing.Point(2, 140);
            this.tgWebServicePrint.Margin = new System.Windows.Forms.Padding(2);
            this.tgWebServicePrint.Name = "tgWebServicePrint";
            this.tgWebServicePrint.Size = new System.Drawing.Size(176, 34);
            this.tgWebServicePrint.TabIndex = 11;
            this.tgWebServicePrint.Text = "WebService";
            this.tgWebServicePrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.tgWebServicePrint.UseVisualStyleBackColor = false;
            this.tgWebServicePrint.CheckedChanged += new System.EventHandler(this.tgWebServicePrint_CheckedChanged);
            // 
            // tableLayoutPanel18
            // 
            this.tableLayoutPanel18.ColumnCount = 2;
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.86207F));
            this.tableLayoutPanel18.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.13793F));
            this.tableLayoutPanel18.Controls.Add(this.tbPrefix, 0, 0);
            this.tableLayoutPanel18.Controls.Add(this.label34, 0, 0);
            this.tableLayoutPanel18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel18.Location = new System.Drawing.Point(3, 217);
            this.tableLayoutPanel18.Name = "tableLayoutPanel18";
            this.tableLayoutPanel18.RowCount = 1;
            this.tableLayoutPanel18.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel18.Size = new System.Drawing.Size(174, 25);
            this.tableLayoutPanel18.TabIndex = 10;
            // 
            // tbPrefix
            // 
            this.tbPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPrefix.Location = new System.Drawing.Point(48, 3);
            this.tbPrefix.Name = "tbPrefix";
            this.tbPrefix.Size = new System.Drawing.Size(123, 20);
            this.tbPrefix.TabIndex = 9;
            this.tbPrefix.TextChanged += new System.EventHandler(this.tbPrefix_TextChanged);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label34.Location = new System.Drawing.Point(3, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(39, 25);
            this.label34.TabIndex = 7;
            this.label34.Text = "Prefijo";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel17
            // 
            this.tableLayoutPanel17.ColumnCount = 2;
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.56322F));
            this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.43678F));
            this.tableLayoutPanel17.Controls.Add(this.label33, 0, 0);
            this.tableLayoutPanel17.Controls.Add(this.tbPrinterAlias, 1, 0);
            this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel17.Location = new System.Drawing.Point(3, 34);
            this.tableLayoutPanel17.Name = "tableLayoutPanel17";
            this.tableLayoutPanel17.RowCount = 1;
            this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel17.Size = new System.Drawing.Size(174, 25);
            this.tableLayoutPanel17.TabIndex = 9;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Location = new System.Drawing.Point(3, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(35, 25);
            this.label33.TabIndex = 7;
            this.label33.Text = "Alias";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbPrinterAlias
            // 
            this.tbPrinterAlias.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbPrinterAlias.Location = new System.Drawing.Point(44, 3);
            this.tbPrinterAlias.Name = "tbPrinterAlias";
            this.tbPrinterAlias.Size = new System.Drawing.Size(127, 20);
            this.tbPrinterAlias.TabIndex = 8;
            this.tbPrinterAlias.TextChanged += new System.EventHandler(this.tbAlias_TextChanged);
            // 
            // btPrint
            // 
            this.btPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.btPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btPrint.Enabled = false;
            this.btPrint.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(68)))), ((int)(((byte)(96)))));
            this.btPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPrint.ForeColor = System.Drawing.Color.AliceBlue;
            this.btPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btPrint.Location = new System.Drawing.Point(3, 279);
            this.btPrint.Name = "btPrint";
            this.btPrint.Size = new System.Drawing.Size(174, 37);
            this.btPrint.TabIndex = 8;
            this.btPrint.Text = "Imprimir";
            this.btPrint.UseVisualStyleBackColor = false;
            this.btPrint.Click += new System.EventHandler(this.btPrint_Click);
            // 
            // btOpenDefaultLabel
            // 
            this.btOpenDefaultLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.btOpenDefaultLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btOpenDefaultLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOpenDefaultLabel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(68)))), ((int)(((byte)(96)))));
            this.btOpenDefaultLabel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btOpenDefaultLabel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btOpenDefaultLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOpenDefaultLabel.ForeColor = System.Drawing.Color.AliceBlue;
            this.btOpenDefaultLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOpenDefaultLabel.Location = new System.Drawing.Point(3, 103);
            this.btOpenDefaultLabel.Name = "btOpenDefaultLabel";
            this.btOpenDefaultLabel.Size = new System.Drawing.Size(174, 32);
            this.btOpenDefaultLabel.TabIndex = 3;
            this.btOpenDefaultLabel.Text = "Etiqueta por default";
            this.btOpenDefaultLabel.UseVisualStyleBackColor = false;
            this.btOpenDefaultLabel.Click += new System.EventHandler(this.btOpenDefaultLabel_Click);
            // 
            // btOpenLabel
            // 
            this.btOpenLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.btOpenLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btOpenLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btOpenLabel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(68)))), ((int)(((byte)(96)))));
            this.btOpenLabel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SteelBlue;
            this.btOpenLabel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SteelBlue;
            this.btOpenLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btOpenLabel.ForeColor = System.Drawing.Color.AliceBlue;
            this.btOpenLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btOpenLabel.Location = new System.Drawing.Point(3, 65);
            this.btOpenLabel.Name = "btOpenLabel";
            this.btOpenLabel.Size = new System.Drawing.Size(174, 32);
            this.btOpenLabel.TabIndex = 2;
            this.btOpenLabel.Text = "Abrir";
            this.btOpenLabel.UseVisualStyleBackColor = false;
            this.btOpenLabel.Click += new System.EventHandler(this.btOpenLabel_Click);
            // 
            // cbPrinters
            // 
            this.cbPrinters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPrinters.FormattingEnabled = true;
            this.cbPrinters.Location = new System.Drawing.Point(3, 3);
            this.cbPrinters.Name = "cbPrinters";
            this.cbPrinters.Size = new System.Drawing.Size(174, 21);
            this.cbPrinters.TabIndex = 0;
            // 
            // stkAjusteEPC
            // 
            this.stkAjusteEPC.ColumnCount = 2;
            this.stkAjusteEPC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.11494F));
            this.stkAjusteEPC.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.88506F));
            this.stkAjusteEPC.Controls.Add(this.label35, 0, 0);
            this.stkAjusteEPC.Controls.Add(this.txtNum, 1, 0);
            this.stkAjusteEPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stkAjusteEPC.Location = new System.Drawing.Point(3, 248);
            this.stkAjusteEPC.Name = "stkAjusteEPC";
            this.stkAjusteEPC.RowCount = 1;
            this.stkAjusteEPC.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.stkAjusteEPC.Size = new System.Drawing.Size(174, 25);
            this.stkAjusteEPC.TabIndex = 7;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label35.Location = new System.Drawing.Point(3, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(115, 25);
            this.label35.TabIndex = 7;
            this.label35.Text = "Ajuste de EPC =";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNum
            // 
            this.txtNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNum.Location = new System.Drawing.Point(124, 3);
            this.txtNum.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.txtNum.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(47, 20);
            this.txtNum.TabIndex = 8;
            // 
            // panel30
            // 
            this.panel30.Controls.Add(this.PrintList);
            this.panel30.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel30.Location = new System.Drawing.Point(0, 317);
            this.panel30.Name = "panel30";
            this.panel30.Size = new System.Drawing.Size(873, 213);
            this.panel30.TabIndex = 1;
            // 
            // PrintList
            // 
            this.PrintList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PrintList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PrintList.Location = new System.Drawing.Point(0, 0);
            this.PrintList.Margin = new System.Windows.Forms.Padding(5);
            this.PrintList.Name = "PrintList";
            this.PrintList.Size = new System.Drawing.Size(873, 213);
            this.PrintList.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SlateGray;
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(162, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(881, 59);
            this.panel2.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(26, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 15);
            this.label10.TabIndex = 1;
            this.label10.Text = "MIDDLEWARE";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(21, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(97, 31);
            this.label9.TabIndex = 0;
            this.label9.Text = "MDW+";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(50)))), ((int)(((byte)(72)))));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(162, 615);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(63)))), ((int)(((byte)(111)))));
            this.panel3.Controls.Add(this.tableLayoutPanel1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.ForeColor = System.Drawing.Color.AliceBlue;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(162, 165);
            this.panel3.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.12346F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.87654F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbToolbarUser, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbToolbarIP, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbToolbarIPWifi, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 59);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(162, 106);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(37)))), ((int)(((byte)(59)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuario";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // lbToolbarUser
            // 
            this.lbToolbarUser.AutoSize = true;
            this.lbToolbarUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lbToolbarUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbToolbarUser.Location = new System.Drawing.Point(68, 0);
            this.lbToolbarUser.Name = "lbToolbarUser";
            this.lbToolbarUser.Size = new System.Drawing.Size(91, 21);
            this.lbToolbarUser.TabIndex = 4;
            this.lbToolbarUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbToolbarIP
            // 
            this.lbToolbarIP.AutoSize = true;
            this.lbToolbarIP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lbToolbarIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbToolbarIP.Location = new System.Drawing.Point(68, 21);
            this.lbToolbarIP.Name = "lbToolbarIP";
            this.lbToolbarIP.Size = new System.Drawing.Size(91, 21);
            this.lbToolbarIP.TabIndex = 5;
            this.lbToolbarIP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(37)))), ((int)(((byte)(59)))));
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "IP WiFi";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(37)))), ((int)(((byte)(59)))));
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 21);
            this.label6.TabIndex = 1;
            this.label6.Text = "IP Local";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbToolbarIPWifi
            // 
            this.lbToolbarIPWifi.AutoSize = true;
            this.lbToolbarIPWifi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(41)))), ((int)(((byte)(64)))));
            this.lbToolbarIPWifi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbToolbarIPWifi.Location = new System.Drawing.Point(68, 42);
            this.lbToolbarIPWifi.Name = "lbToolbarIPWifi";
            this.lbToolbarIPWifi.Size = new System.Drawing.Size(91, 21);
            this.lbToolbarIPWifi.TabIndex = 5;
            this.lbToolbarIPWifi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(58)))), ((int)(((byte)(106)))));
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(162, 59);
            this.panel4.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(37)))), ((int)(((byte)(59)))));
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(162, 59);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 615);
            this.Controls.Add(this.Tab);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Print";
            this.Text = "Impresión";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Print_FormClosing);
            this.Load += new System.EventHandler(this.Print_Load);
            this.Tab.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel29.ResumeLayout(false);
            this.panel29.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.panel5.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel28.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel18.ResumeLayout(false);
            this.tableLayoutPanel18.PerformLayout();
            this.tableLayoutPanel17.ResumeLayout(false);
            this.tableLayoutPanel17.PerformLayout();
            this.stkAjusteEPC.ResumeLayout(false);
            this.stkAjusteEPC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNum)).EndInit();
            this.panel30.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PrintList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Tab;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel29;
        private System.Windows.Forms.Label lbLabelName;
        private System.Windows.Forms.PictureBox Picture;
        private System.Windows.Forms.Panel panel28;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox tgWebSocketPrint;
        private System.Windows.Forms.CheckBox tgWebServicePrint;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel18;
        private System.Windows.Forms.TextBox tbPrefix;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox tbPrinterAlias;
        private System.Windows.Forms.Button btPrint;
        private System.Windows.Forms.Button btOpenDefaultLabel;
        private System.Windows.Forms.Button btOpenLabel;
        private System.Windows.Forms.ComboBox cbPrinters;
        private System.Windows.Forms.TableLayoutPanel stkAjusteEPC;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.NumericUpDown txtNum;
        private System.Windows.Forms.Panel panel30;
        private System.Windows.Forms.DataGridView PrintList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbToolbarUser;
        private System.Windows.Forms.Label lbToolbarIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbToolbarIPWifi;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ListBox lbPrinters;
        private System.Windows.Forms.Button btAddPrinter;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btQuitar;
    }
}

