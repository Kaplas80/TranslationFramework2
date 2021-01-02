﻿namespace TF.GUI.Forms
{
    partial class ExportProjectForm
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
            this.components = new System.ComponentModel.Container();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbExportItems = new System.Windows.Forms.GroupBox();
            this.btnOnlyModified = new System.Windows.Forms.Button();
            this.btnInvertSelection = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lbItems = new System.Windows.Forms.CheckedListBox();
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.chkSaveTempFiles = new System.Windows.Forms.CheckBox();
            this.chkForceRebuild = new System.Windows.Forms.CheckBox();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            this.gbExportItems.SuspendLayout();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(909, 429);
            this.dockPanel1.TabIndex = 0;
            this.dockPanel1.Theme = null;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.Location = new System.Drawing.Point(741, 394);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "Aceptar";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(822, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbExportItems
            // 
            this.gbExportItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExportItems.Controls.Add(this.btnOnlyModified);
            this.gbExportItems.Controls.Add(this.btnInvertSelection);
            this.gbExportItems.Controls.Add(this.btnSelectNone);
            this.gbExportItems.Controls.Add(this.btnSelectAll);
            this.gbExportItems.Controls.Add(this.lbItems);
            this.gbExportItems.Location = new System.Drawing.Point(12, 12);
            this.gbExportItems.Name = "gbExportItems";
            this.gbExportItems.Size = new System.Drawing.Size(638, 376);
            this.gbExportItems.TabIndex = 11;
            this.gbExportItems.TabStop = false;
            this.gbExportItems.Text = "Elementos a exportar";
            // 
            // btnOnlyModified
            // 
            this.btnOnlyModified.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOnlyModified.Location = new System.Drawing.Point(312, 345);
            this.btnOnlyModified.Name = "btnOnlyModified";
            this.btnOnlyModified.Size = new System.Drawing.Size(96, 23);
            this.btnOnlyModified.TabIndex = 6;
            this.btnOnlyModified.Text = "Solo modificados";
            this.btnOnlyModified.UseVisualStyleBackColor = true;
            this.btnOnlyModified.Click += new System.EventHandler(this.btnOnlyModified_Click);
            // 
            // btnInvertSelection
            // 
            this.btnInvertSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInvertSelection.Location = new System.Drawing.Point(210, 345);
            this.btnInvertSelection.Name = "btnInvertSelection";
            this.btnInvertSelection.Size = new System.Drawing.Size(96, 23);
            this.btnInvertSelection.TabIndex = 5;
            this.btnInvertSelection.Text = "Invertir selección";
            this.btnInvertSelection.UseVisualStyleBackColor = true;
            this.btnInvertSelection.Click += new System.EventHandler(this.btnInvertSelection_Click);
            // 
            // btnSelectNone
            // 
            this.btnSelectNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectNone.Location = new System.Drawing.Point(108, 345);
            this.btnSelectNone.Name = "btnSelectNone";
            this.btnSelectNone.Size = new System.Drawing.Size(96, 23);
            this.btnSelectNone.TabIndex = 4;
            this.btnSelectNone.Text = "Nada";
            this.btnSelectNone.UseVisualStyleBackColor = true;
            this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.Location = new System.Drawing.Point(6, 345);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(96, 23);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Todo";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // lbItems
            // 
            this.lbItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbItems.CheckOnClick = true;
            this.lbItems.FormattingEnabled = true;
            this.lbItems.IntegralHeight = false;
            this.lbItems.Location = new System.Drawing.Point(6, 19);
            this.lbItems.Name = "lbItems";
            this.lbItems.ScrollAlwaysVisible = true;
            this.lbItems.Size = new System.Drawing.Size(626, 320);
            this.lbItems.TabIndex = 2;
            this.lbItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lbItems_ItemCheck);
            // 
            // gbOptions
            // 
            this.gbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOptions.Controls.Add(this.chkSaveTempFiles);
            this.gbOptions.Controls.Add(this.chkForceRebuild);
            this.gbOptions.Controls.Add(this.chkCompress);
            this.gbOptions.Location = new System.Drawing.Point(656, 12);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(241, 376);
            this.gbOptions.TabIndex = 12;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Opciones";
            // 
            // chkSaveTempFiles
            // 
            this.chkSaveTempFiles.AutoSize = true;
            this.chkSaveTempFiles.Location = new System.Drawing.Point(6, 65);
            this.chkSaveTempFiles.Name = "chkSaveTempFiles";
            this.chkSaveTempFiles.Size = new System.Drawing.Size(158, 17);
            this.chkSaveTempFiles.TabIndex = 2;
            this.chkSaveTempFiles.Text = "Guardar ficheros temporales";
            this.toolTip1.SetToolTip(this.chkSaveTempFiles, "[DEBUG] Selecciona esta opción si quieres mantener los ficheros temporales genera" +
        "dos.");
            this.chkSaveTempFiles.UseVisualStyleBackColor = true;
            // 
            // chkForceRebuild
            // 
            this.chkForceRebuild.AutoSize = true;
            this.chkForceRebuild.Location = new System.Drawing.Point(6, 42);
            this.chkForceRebuild.Name = "chkForceRebuild";
            this.chkForceRebuild.Size = new System.Drawing.Size(99, 17);
            this.chkForceRebuild.TabIndex = 1;
            this.chkForceRebuild.Text = "Forzar creación";
            this.toolTip1.SetToolTip(this.chkForceRebuild, "[DEBUG] Selecciona esta opción si quieres que la aplicación genere los ficheros d" +
        "e traducción aunque no tengan cambios respecto al original.");
            this.chkForceRebuild.UseVisualStyleBackColor = true;
            // 
            // chkCompress
            // 
            this.chkCompress.AutoSize = true;
            this.chkCompress.Checked = true;
            this.chkCompress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompress.Location = new System.Drawing.Point(6, 19);
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.Size = new System.Drawing.Size(71, 17);
            this.chkCompress.TabIndex = 0;
            this.chkCompress.Text = "Comprimir";
            this.toolTip1.SetToolTip(this.chkCompress, "Selecciona esta opción si quieres que se compriman los ficheros exportados\\nSi no" +
        " se marca, la exportación será más rápida, pero es posible que el fichero result" +
        "ante no sea compatible.");
            this.chkCompress.UseVisualStyleBackColor = true;
            // 
            // ExportProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 429);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.gbExportItems);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dockPanel1);
            this.MinimumSize = new System.Drawing.Size(757, 468);
            this.Name = "ExportProjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Exportar Traducción";
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
            this.gbExportItems.ResumeLayout(false);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbExportItems;
        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckedListBox lbItems;
        private System.Windows.Forms.Button btnInvertSelection;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnOnlyModified;
        private System.Windows.Forms.CheckBox chkForceRebuild;
        private System.Windows.Forms.CheckBox chkSaveTempFiles;
    }
}