﻿namespace TF.Core.Views
{
    partial class TextView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.scintillaTranslation = new ScintillaNET.Scintilla();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(800, 450);
            this.dockPanel1.TabIndex = 0;
            // 
            // scintillaTranslation
            // 
            this.scintillaTranslation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scintillaTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaTranslation.Location = new System.Drawing.Point(0, 0);
            this.scintillaTranslation.Name = "scintillaTranslation";
            this.scintillaTranslation.Size = new System.Drawing.Size(800, 450);
            this.scintillaTranslation.TabIndex = 2;
            this.scintillaTranslation.TextChanged += new System.EventHandler(this.scintillaTranslation_TextChanged);
            // 
            // TextView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.scintillaTranslation);
            this.Controls.Add(this.dockPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TextView";
            this.Text = "Texto";
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private ScintillaNET.Scintilla scintillaTranslation;
    }
}