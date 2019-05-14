namespace Sequence_Data_Predict
{
    partial class Main_Form
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.Learning_Button = new System.Windows.Forms.Button();
            this.Predict_Button = new System.Windows.Forms.Button();
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MAX_Epoch_Text = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MAX_Cost_Text = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Windows_Text = new System.Windows.Forms.TextBox();
            this.Present_Data_Text = new System.Windows.Forms.TextBox();
            this.Save_Model_Button = new System.Windows.Forms.Button();
            this.Load_Model_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // Learning_Button
            // 
            this.Learning_Button.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Learning_Button.Location = new System.Drawing.Point(732, 461);
            this.Learning_Button.Name = "Learning_Button";
            this.Learning_Button.Size = new System.Drawing.Size(263, 119);
            this.Learning_Button.TabIndex = 2;
            this.Learning_Button.Text = "학습하기";
            this.Learning_Button.UseVisualStyleBackColor = true;
            this.Learning_Button.Click += new System.EventHandler(this.Learning_Button_Click);
            // 
            // Predict_Button
            // 
            this.Predict_Button.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Predict_Button.Location = new System.Drawing.Point(733, 586);
            this.Predict_Button.Name = "Predict_Button";
            this.Predict_Button.Size = new System.Drawing.Size(263, 119);
            this.Predict_Button.TabIndex = 3;
            this.Predict_Button.Text = "예측하기";
            this.Predict_Button.UseVisualStyleBackColor = true;
            this.Predict_Button.Click += new System.EventHandler(this.Predict_Button_Click);
            // 
            // ResultGrid
            // 
            this.ResultGrid.AllowUserToOrderColumns = true;
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.Location = new System.Drawing.Point(13, 63);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.ReadOnly = true;
            this.ResultGrid.RowTemplate.Height = 23;
            this.ResultGrid.Size = new System.Drawing.Size(713, 597);
            this.ResultGrid.TabIndex = 1;
            this.ResultGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultGrid_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 27);
            this.label1.TabIndex = 4;
            this.label1.Text = "예측치 이력";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(733, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "최대 학습 횟수";
            this.label2.Visible = false;
            // 
            // MAX_Epoch_Text
            // 
            this.MAX_Epoch_Text.Location = new System.Drawing.Point(889, 12);
            this.MAX_Epoch_Text.Name = "MAX_Epoch_Text";
            this.MAX_Epoch_Text.Size = new System.Drawing.Size(100, 21);
            this.MAX_Epoch_Text.TabIndex = 6;
            this.MAX_Epoch_Text.Text = "50000";
            this.MAX_Epoch_Text.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(733, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "오차 허용치 ";
            this.label3.Visible = false;
            // 
            // MAX_Cost_Text
            // 
            this.MAX_Cost_Text.Location = new System.Drawing.Point(889, 63);
            this.MAX_Cost_Text.Name = "MAX_Cost_Text";
            this.MAX_Cost_Text.Size = new System.Drawing.Size(100, 21);
            this.MAX_Cost_Text.TabIndex = 8;
            this.MAX_Cost_Text.Text = "2.6";
            this.MAX_Cost_Text.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(733, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 21);
            this.label4.TabIndex = 9;
            this.label4.Text = "과거 관측범위";
            this.label4.Visible = false;
            // 
            // Windows_Text
            // 
            this.Windows_Text.Location = new System.Drawing.Point(889, 110);
            this.Windows_Text.Name = "Windows_Text";
            this.Windows_Text.Size = new System.Drawing.Size(100, 21);
            this.Windows_Text.TabIndex = 10;
            this.Windows_Text.Text = "5";
            this.Windows_Text.Visible = false;
            // 
            // Present_Data_Text
            // 
            this.Present_Data_Text.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Present_Data_Text.Location = new System.Drawing.Point(12, 673);
            this.Present_Data_Text.Name = "Present_Data_Text";
            this.Present_Data_Text.Size = new System.Drawing.Size(714, 44);
            this.Present_Data_Text.TabIndex = 11;
            this.Present_Data_Text.Text = "다,나,나,나,나 ";
            this.Present_Data_Text.Visible = false;
            // 
            // Save_Model_Button
            // 
            this.Save_Model_Button.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Save_Model_Button.Location = new System.Drawing.Point(737, 137);
            this.Save_Model_Button.Name = "Save_Model_Button";
            this.Save_Model_Button.Size = new System.Drawing.Size(263, 119);
            this.Save_Model_Button.TabIndex = 12;
            this.Save_Model_Button.Text = "모델 저장하기";
            this.Save_Model_Button.UseVisualStyleBackColor = true;
            this.Save_Model_Button.Visible = false;
            this.Save_Model_Button.Click += new System.EventHandler(this.Save_Model_Button_Click);
            // 
            // Load_Model_Button
            // 
            this.Load_Model_Button.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Load_Model_Button.Location = new System.Drawing.Point(737, 262);
            this.Load_Model_Button.Name = "Load_Model_Button";
            this.Load_Model_Button.Size = new System.Drawing.Size(263, 119);
            this.Load_Model_Button.TabIndex = 13;
            this.Load_Model_Button.Text = "모델 불러오기";
            this.Load_Model_Button.UseVisualStyleBackColor = true;
            this.Load_Model_Button.Visible = false;
            this.Load_Model_Button.Click += new System.EventHandler(this.Load_Model_Button_Click);
            // 
            // Main_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.Load_Model_Button);
            this.Controls.Add(this.Save_Model_Button);
            this.Controls.Add(this.Present_Data_Text);
            this.Controls.Add(this.Windows_Text);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MAX_Cost_Text);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MAX_Epoch_Text);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ResultGrid);
            this.Controls.Add(this.Predict_Button);
            this.Controls.Add(this.Learning_Button);
            this.Name = "Main_Form";
            this.Text = "서지민의 시계열 데이터 예측 (Markov) v1";
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Learning_Button;
        private System.Windows.Forms.Button Predict_Button;
        private System.Windows.Forms.DataGridView ResultGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MAX_Epoch_Text;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MAX_Cost_Text;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Windows_Text;
        private System.Windows.Forms.TextBox Present_Data_Text;
        private System.Windows.Forms.Button Save_Model_Button;
        private System.Windows.Forms.Button Load_Model_Button;
    }
}

