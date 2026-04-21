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
            button1 = new Button();
            dgvOrders = new DataGridView();
            button2 = new Button();
            textBoxFilter = new TextBox();
            label1 = new Label();
            button3 = new Button();
            label2 = new Label();
            textBoxPrice = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(9, 33);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(108, 36);
            button1.TabIndex = 0;
            button1.Text = "Load";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // dgvOrders
            // 
            dgvOrders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(9, 120);
            dgvOrders.Margin = new Padding(3, 2, 3, 2);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.RowHeadersWidth = 51;
            dgvOrders.Size = new Size(736, 326);
            dgvOrders.TabIndex = 1;
            dgvOrders.CellValueChanged += dgvOrders_CellValueChanged;
            dgvOrders.CurrentCellDirtyStateChanged += dgvOrders_CurrentCellDirtyStateChanged;
            // 
            // button2
            // 
            button2.Location = new Point(11, 73);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(108, 36);
            button2.TabIndex = 2;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBoxFilter
            // 
            textBoxFilter.Font = new Font("Segoe UI", 13F);
            textBoxFilter.Location = new Point(123, 35);
            textBoxFilter.Name = "textBoxFilter";
            textBoxFilter.Size = new Size(318, 31);
            textBoxFilter.TabIndex = 3;
            textBoxFilter.TextChanged += textBoxFilter_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 14);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 4;
            label1.Text = "POHÁRNOK";
            // 
            // button3
            // 
            button3.Location = new Point(125, 71);
            button3.Margin = new Padding(3, 2, 3, 2);
            button3.Name = "button3";
            button3.Size = new Size(108, 36);
            button3.TabIndex = 5;
            button3.Text = "Select All";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(123, 14);
            label2.Name = "label2";
            label2.Size = new Size(36, 15);
            label2.TabIndex = 6;
            label2.Text = "email";
            // 
            // textBoxPrice
            // 
            textBoxPrice.Font = new Font("Segoe UI", 13F);
            textBoxPrice.Location = new Point(239, 71);
            textBoxPrice.Name = "textBoxPrice";
            textBoxPrice.Size = new Size(318, 31);
            textBoxPrice.TabIndex = 7;
            textBoxPrice.TextChanged += textBoxPrice_TextChanged;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(447, 35);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(200, 23);
            dateTimePicker1.TabIndex = 8;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(757, 457);
            Controls.Add(dateTimePicker1);
            Controls.Add(textBoxPrice);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(label1);
            Controls.Add(textBoxFilter);
            Controls.Add(button2);
            Controls.Add(dgvOrders);
            Controls.Add(button1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
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
    }
}
