namespace poharnok_client_application
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PictureBox pictureBox1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            dgvOrders = new DataGridView();
            button2 = new Button();
            textBoxFilter = new TextBox();
            label1 = new Label();
            button3 = new Button();
            label2 = new Label();
            textBoxPrice = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            dateTimePicker2 = new DateTimePicker();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            checkBox1 = new CheckBox();
            dgvGiftCards = new DataGridView();
            button4 = new Button();
            label6 = new Label();
            label7 = new Label();
            dateTimePicker3 = new DateTimePicker();
            dateTimePicker4 = new DateTimePicker();
            cmbAmount = new ComboBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvGiftCards).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, -1);
            pictureBox1.Margin = new Padding(3, 4, 3, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(28, 35);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(10, 34);
            button1.Name = "button1";
            button1.Size = new Size(123, 48);
            button1.TabIndex = 0;
            button1.Text = "Betöltés";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dgvOrders
            // 
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(10, 150);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.RowHeadersWidth = 51;
            dgvOrders.Size = new Size(889, 236);
            dgvOrders.TabIndex = 1;
            dgvOrders.CellValueChanged += dgvOrders_CellValueChanged;
            dgvOrders.CurrentCellDirtyStateChanged += dgvOrders_CurrentCellDirtyStateChanged;
            // 
            // button2
            // 
            button2.Location = new Point(10, 88);
            button2.Name = "button2";
            button2.Size = new Size(123, 48);
            button2.TabIndex = 2;
            button2.Text = "Küldés";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBoxFilter
            // 
            textBoxFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBoxFilter.Font = new Font("Segoe UI", 13F);
            textBoxFilter.Location = new Point(141, 37);
            textBoxFilter.Margin = new Padding(3, 4, 3, 4);
            textBoxFilter.Name = "textBoxFilter";
            textBoxFilter.Size = new Size(233, 36);
            textBoxFilter.TabIndex = 3;
            textBoxFilter.TextChanged += textBoxFilter_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 9);
            label1.Name = "label1";
            label1.Size = new Size(89, 20);
            label1.TabIndex = 4;
            label1.Text = "POHÁRNOK";
            // 
            // button3
            // 
            button3.Location = new Point(141, 88);
            button3.Name = "button3";
            button3.Size = new Size(123, 48);
            button3.TabIndex = 5;
            button3.Text = "Mind kijelölése";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(141, 9);
            label2.Name = "label2";
            label2.Size = new Size(46, 20);
            label2.TabIndex = 6;
            label2.Text = "Email";
            // 
            // textBoxPrice
            // 
            textBoxPrice.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxPrice.Font = new Font("Segoe UI", 13F);
            textBoxPrice.Location = new Point(381, 37);
            textBoxPrice.Margin = new Padding(3, 4, 3, 4);
            textBoxPrice.Name = "textBoxPrice";
            textBoxPrice.Size = new Size(127, 36);
            textBoxPrice.TabIndex = 7;
            textBoxPrice.TextChanged += textBoxPrice_TextChanged;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateTimePicker1.Location = new Point(516, 41);
            dateTimePicker1.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(188, 27);
            dateTimePicker1.TabIndex = 8;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateTimePicker2.Location = new Point(711, 41);
            dateTimePicker2.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(188, 27);
            dateTimePicker2.TabIndex = 9;
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(381, 13);
            label3.Name = "label3";
            label3.Size = new Size(121, 20);
            label3.TabIndex = 10;
            label3.Text = "Minimum összeg";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(516, 13);
            label4.Name = "label4";
            label4.Size = new Size(98, 20);
            label4.TabIndex = 11;
            label4.Text = "Kezdő dátum";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(711, 13);
            label5.Name = "label5";
            label5.Size = new Size(96, 20);
            label5.TabIndex = 12;
            label5.Text = "Végső dátum";
            // 
            // checkBox1
            // 
            checkBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(687, 112);
            checkBox1.Margin = new Padding(3, 4, 3, 4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(212, 24);
            checkBox1.TabIndex = 14;
            checkBox1.Text = "Több kupon engedélyezése";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // dgvGiftCards
            // 
            dgvGiftCards.AccessibleDescription = "";
            dgvGiftCards.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvGiftCards.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGiftCards.Location = new Point(10, 467);
            dgvGiftCards.Name = "dgvGiftCards";
            dgvGiftCards.RowHeadersWidth = 51;
            dgvGiftCards.Size = new Size(889, 317);
            dgvGiftCards.TabIndex = 15;
            dgvGiftCards.CellFormatting += dgvGiftCards_CellFormatting;
            // 
            // button4
            // 
            button4.Location = new Point(7, 405);
            button4.Name = "button4";
            button4.Size = new Size(123, 48);
            button4.TabIndex = 16;
            button4.Text = "Frissites";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(711, 405);
            label6.Name = "label6";
            label6.Size = new Size(96, 20);
            label6.TabIndex = 20;
            label6.Text = "Végső dátum";
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(516, 405);
            label7.Name = "label7";
            label7.Size = new Size(98, 20);
            label7.TabIndex = 19;
            label7.Text = "Kezdő dátum";
            // 
            // dateTimePicker3
            // 
            dateTimePicker3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateTimePicker3.Location = new Point(516, 433);
            dateTimePicker3.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker3.Name = "dateTimePicker3";
            dateTimePicker3.Size = new Size(188, 27);
            dateTimePicker3.TabIndex = 18;
            // 
            // dateTimePicker4
            // 
            dateTimePicker4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dateTimePicker4.Location = new Point(711, 433);
            dateTimePicker4.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker4.Name = "dateTimePicker4";
            dateTimePicker4.Size = new Size(188, 27);
            dateTimePicker4.TabIndex = 17;
            // 
            // cmbAmount
            // 
            cmbAmount.FormattingEnabled = true;
            cmbAmount.Location = new Point(270, 88);
            cmbAmount.Name = "cmbAmount";
            cmbAmount.Size = new Size(104, 28);
            cmbAmount.TabIndex = 21;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(909, 796);
            Controls.Add(cmbAmount);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(dateTimePicker3);
            Controls.Add(dateTimePicker4);
            Controls.Add(button4);
            Controls.Add(dgvGiftCards);
            Controls.Add(checkBox1);
            Controls.Add(pictureBox1);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(dateTimePicker2);
            Controls.Add(dateTimePicker1);
            Controls.Add(textBoxPrice);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(label1);
            Controls.Add(textBoxFilter);
            Controls.Add(button2);
            Controls.Add(dgvOrders);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Pohárnok kliensalkalmazás";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvGiftCards).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private DataGridView dgvOrders;
        private Button button2;
        private TextBox textBoxFilter;
        private Label label1;
        private Button button3;
        private Label label2;
        private TextBox textBoxPrice;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private Label label3;
        private Label label4;
        private Label label5;
        private PictureBox pictureBox1;
        private CheckBox checkBox1;
        private DataGridView dgvGiftCards;
        private Button button4;
        private Label label6;
        private Label label7;
        private DateTimePicker dateTimePicker3;
        private DateTimePicker dateTimePicker4;
        private ComboBox cmbAmount;
    }
}
