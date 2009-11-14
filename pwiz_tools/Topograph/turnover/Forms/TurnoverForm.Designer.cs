﻿namespace pwiz.Topograph.ui.Forms
{
    partial class TurnoverForm
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
            this.dockPanel = new DigitalRune.Windows.Docking.DockPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeWorkspaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSearchResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.peptidesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.peptideAnalysesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mercuryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enrichmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateProteinNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.machineSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.halfLivesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.DefaultFloatingWindowSize = new System.Drawing.Size(300, 300);
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Location = new System.Drawing.Point(0, 24);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(984, 540);
            this.dockPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newWorkspaceToolStripMenuItem,
            this.openWorkspaceToolStripMenuItem,
            this.saveWorkspaceToolStripMenuItem,
            this.closeWorkspaceToolStripMenuItem,
            this.addSearchResultsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newWorkspaceToolStripMenuItem
            // 
            this.newWorkspaceToolStripMenuItem.Name = "newWorkspaceToolStripMenuItem";
            this.newWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.newWorkspaceToolStripMenuItem.Text = "New Workspace...";
            this.newWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.newWorkspaceToolStripMenuItem_Click);
            // 
            // openWorkspaceToolStripMenuItem
            // 
            this.openWorkspaceToolStripMenuItem.Name = "openWorkspaceToolStripMenuItem";
            this.openWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.openWorkspaceToolStripMenuItem.Text = "Open Workspace...";
            this.openWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.openWorkspaceToolStripMenuItem_Click);
            // 
            // saveWorkspaceToolStripMenuItem
            // 
            this.saveWorkspaceToolStripMenuItem.Enabled = false;
            this.saveWorkspaceToolStripMenuItem.Name = "saveWorkspaceToolStripMenuItem";
            this.saveWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.saveWorkspaceToolStripMenuItem.Text = "Save Workspace";
            this.saveWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.saveWorkspaceToolStripMenuItem_Click);
            // 
            // closeWorkspaceToolStripMenuItem
            // 
            this.closeWorkspaceToolStripMenuItem.Enabled = false;
            this.closeWorkspaceToolStripMenuItem.Name = "closeWorkspaceToolStripMenuItem";
            this.closeWorkspaceToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.closeWorkspaceToolStripMenuItem.Text = "Close Workspace";
            this.closeWorkspaceToolStripMenuItem.Click += new System.EventHandler(this.closeWorkspaceToolStripMenuItem_Click);
            // 
            // addSearchResultsToolStripMenuItem
            // 
            this.addSearchResultsToolStripMenuItem.Enabled = false;
            this.addSearchResultsToolStripMenuItem.Name = "addSearchResultsToolStripMenuItem";
            this.addSearchResultsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.addSearchResultsToolStripMenuItem.Text = "Add Search Results...";
            this.addSearchResultsToolStripMenuItem.Click += new System.EventHandler(this.addSearchResultsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.peptidesToolStripMenuItem,
            this.dataFilesToolStripMenuItem,
            this.peptideAnalysesToolStripMenuItem,
            this.statusToolStripMenuItem,
            this.queryToolStripMenuItem,
            this.mercuryToolStripMenuItem,
            this.halfLivesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // peptidesToolStripMenuItem
            // 
            this.peptidesToolStripMenuItem.Enabled = false;
            this.peptidesToolStripMenuItem.Name = "peptidesToolStripMenuItem";
            this.peptidesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.peptidesToolStripMenuItem.Text = "Peptides";
            this.peptidesToolStripMenuItem.Click += new System.EventHandler(this.peptidesToolStripMenuItem_Click);
            // 
            // dataFilesToolStripMenuItem
            // 
            this.dataFilesToolStripMenuItem.Enabled = false;
            this.dataFilesToolStripMenuItem.Name = "dataFilesToolStripMenuItem";
            this.dataFilesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.dataFilesToolStripMenuItem.Text = "Data Files";
            this.dataFilesToolStripMenuItem.Click += new System.EventHandler(this.dataFilesToolStripMenuItem_Click);
            // 
            // peptideAnalysesToolStripMenuItem
            // 
            this.peptideAnalysesToolStripMenuItem.Enabled = false;
            this.peptideAnalysesToolStripMenuItem.Name = "peptideAnalysesToolStripMenuItem";
            this.peptideAnalysesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.peptideAnalysesToolStripMenuItem.Text = "Peptide Analyses";
            this.peptideAnalysesToolStripMenuItem.Click += new System.EventHandler(this.peptideAnalysesToolStripMenuItem_Click);
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Enabled = false;
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.statusToolStripMenuItem.Text = "Running Jobs";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.Enabled = false;
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            this.queryToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.queryToolStripMenuItem.Text = "Queries";
            this.queryToolStripMenuItem.Click += new System.EventHandler(this.queriesToolStripMenuItem_Click);
            // 
            // mercuryToolStripMenuItem
            // 
            this.mercuryToolStripMenuItem.Enabled = false;
            this.mercuryToolStripMenuItem.Name = "mercuryToolStripMenuItem";
            this.mercuryToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.mercuryToolStripMenuItem.Text = "Isotope Distribution Graph";
            this.mercuryToolStripMenuItem.Click += new System.EventHandler(this.mercuryToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enrichmentToolStripMenuItem,
            this.modificationsToolStripMenuItem,
            this.updateProteinNamesToolStripMenuItem,
            this.machineSettingsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // enrichmentToolStripMenuItem
            // 
            this.enrichmentToolStripMenuItem.Enabled = false;
            this.enrichmentToolStripMenuItem.Name = "enrichmentToolStripMenuItem";
            this.enrichmentToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.enrichmentToolStripMenuItem.Text = "Tracers...";
            this.enrichmentToolStripMenuItem.Click += new System.EventHandler(this.enrichmentToolStripMenuItem_Click);
            // 
            // modificationsToolStripMenuItem
            // 
            this.modificationsToolStripMenuItem.Enabled = false;
            this.modificationsToolStripMenuItem.Name = "modificationsToolStripMenuItem";
            this.modificationsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.modificationsToolStripMenuItem.Text = "Modifications...";
            this.modificationsToolStripMenuItem.Click += new System.EventHandler(this.modificationsToolStripMenuItem_Click);
            // 
            // updateProteinNamesToolStripMenuItem
            // 
            this.updateProteinNamesToolStripMenuItem.Enabled = false;
            this.updateProteinNamesToolStripMenuItem.Name = "updateProteinNamesToolStripMenuItem";
            this.updateProteinNamesToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.updateProteinNamesToolStripMenuItem.Text = "Update Protein Names...";
            this.updateProteinNamesToolStripMenuItem.Click += new System.EventHandler(this.updateProteinNamesToolStripMenuItem_Click);
            // 
            // machineSettingsToolStripMenuItem
            // 
            this.machineSettingsToolStripMenuItem.Enabled = false;
            this.machineSettingsToolStripMenuItem.Name = "machineSettingsToolStripMenuItem";
            this.machineSettingsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.machineSettingsToolStripMenuItem.Text = "Miscellaneous...";
            this.machineSettingsToolStripMenuItem.Click += new System.EventHandler(this.machineSettingsToolStripMenuItem_Click);
            // 
            // halfLivesToolStripMenuItem
            // 
            this.halfLivesToolStripMenuItem.Enabled = false;
            this.halfLivesToolStripMenuItem.Name = "halfLivesToolStripMenuItem";
            this.halfLivesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.halfLivesToolStripMenuItem.Text = "Half Lives";
            this.halfLivesToolStripMenuItem.Click += new System.EventHandler(this.halfLivesToolStripMenuItem_Click);
            // 
            // TurnoverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 564);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TurnoverForm";
            this.Text = "Topograph";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSearchResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enrichmentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem peptidesToolStripMenuItem;
        private DigitalRune.Windows.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem dataFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem peptideAnalysesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveWorkspaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateProteinNamesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem machineSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mercuryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem halfLivesToolStripMenuItem;
    }
}