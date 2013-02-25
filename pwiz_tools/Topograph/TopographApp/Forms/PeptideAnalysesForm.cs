﻿/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2009 University of Washington - Seattle, WA
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using NHibernate;
using pwiz.Common.DataBinding;
using pwiz.Common.DataBinding.Attributes;
using pwiz.Topograph.Data;
using pwiz.Topograph.Model;
using pwiz.Topograph.Util;
using pwiz.Topograph.ui.Controls;
using pwiz.Topograph.ui.DataBinding;
using pwiz.Topograph.ui.Util;

namespace pwiz.Topograph.ui.Forms
{
    public partial class PeptideAnalysesForm : BasePeptideAnalysesForm
    {
        private const string Title = "Peptide Analyses";
        private readonly PeptideAnalysisRows _peptideAnalyses;

        public PeptideAnalysesForm(Workspace workspace) : base(workspace)
        {
            InitializeComponent();
            TabText = Name = Title;
            deleteMenuItem.Click += DeleteAnalysesMenuItemOnClick;
            var viewContext = new TopographViewContext(Workspace, typeof (PeptideAnalysisRow))
                                  {
                                      DeleteHandler = new PeptideAnalysisDeleteHandler(this),
                                  };
            var idPathPeptideAnalysis = new IdentifierPath(IdentifierPath.Root, "PeptideAnalysis");
            var idPathPeptide = new IdentifierPath(idPathPeptideAnalysis, "Peptide");
            var viewSpec = new ViewSpec().SetColumns(
                new[]
                    {
                        new ColumnSpec(new IdentifierPath(IdentifierPath.Root, "Peptide")),
                        new ColumnSpec(new IdentifierPath(IdentifierPath.Root, "ValidationStatus")),
                        new ColumnSpec(new IdentifierPath(idPathPeptideAnalysis, "Note")),
                        new ColumnSpec(new IdentifierPath(idPathPeptide, "ProteinName")), 
                        new ColumnSpec(new IdentifierPath(idPathPeptide, "ProteinDescription")), 
                        new ColumnSpec(new IdentifierPath(idPathPeptide, "MaxTracerCount")), 
                        new ColumnSpec(new IdentifierPath(IdentifierPath.Root, "FileAnalysisCount")), 
                        new ColumnSpec(new IdentifierPath(IdentifierPath.Root, "MinScore")), 
                        new ColumnSpec(new IdentifierPath(IdentifierPath.Root, "MaxScore")),
                    });
            viewContext.BuiltInViewSpecs = new[] {viewSpec};
            bindingListSource1.SetViewContext(viewContext);
            _peptideAnalyses = new PeptideAnalysisRows(Workspace.PeptideAnalyses);
            bindingListSource1.RowSource = _peptideAnalyses;
        }

        private void DeleteAnalysesMenuItemOnClick(object sender, EventArgs e)
        {
//            var peptideAnalysisIds = new List<long>();
//            foreach (DataGridViewRow row in dataGridView.SelectedRows)
//            {
//                if (!row.Visible)
//                {
//                    continue;
//                }
//                peptideAnalysisIds.Add((long)row.Tag);
//            }
//            if (peptideAnalysisIds.Count == 0)
//            {
//                if (dataGridView.CurrentRow != null)
//                {
//                    peptideAnalysisIds.Add((long)dataGridView.CurrentRow.Tag);
//                }
//            }
//            if (peptideAnalysisIds.Count == 0)
//            {
//                MessageBox.Show("No peptide analyses are selected", Program.AppName, MessageBoxButtons.OK,
//                                MessageBoxIcon.Error);
//                return;
//            }
//            String message;
//            if (peptideAnalysisIds.Count == 1)
//            {
//                using (var session = Workspace.OpenSession())
//                {
//                    var peptideAnalysis = session.Get<DbPeptideAnalysis>(peptideAnalysisIds[0]);
//                    message = "Are you sure you want to delete the analysis of the peptide '" +
//                              peptideAnalysis.Peptide.Sequence + "'?";
//                }
//            }
//            else
//            {
//                message = "Are you sure you want to delete these " + peptideAnalysisIds.Count + " peptide analyses?";
//            }
//            if (MessageBox.Show(message, Program.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
//            {
//                return;
//            }
//            using (var session = Workspace.OpenWriteSession())
//            {
//                session.BeginTransaction();
//                foreach (var id in peptideAnalysisIds)
//                {
//                    var peptideAnalysis = session.Get<DbPeptideAnalysis>(id);
//                    if (peptideAnalysis == null)
//                    {
//                        continue;
//                    }
//                    session.Delete(peptideAnalysis);
//                    session.Save(new DbChangeLog(Workspace, peptideAnalysis));
//                }
//                session.Transaction.Commit();
//            }
//            foreach (var id in peptideAnalysisIds)
//            {
//                var peptideAnalysis = Workspace.PeptideAnalyses[id];
//                if (peptideAnalysis != null)
//                {
//                    var frame = Program.FindOpenEntityForm<PeptideAnalysisFrame>(peptideAnalysis);
//                    if (frame != null)
//                    {
//                        frame.Close();
//                    }
//                }
//            }
//            foreach (var id in peptideAnalysisIds)
//            {
//                Workspace.PeptideAnalyses.Remove(id);
//                dataGridView.Rows.Remove(_peptideAnalysisRows[id]);
//                _peptideAnalysisRows.Remove(id);
//            }
        }

        protected override void Requery(ISession session, ICollection<long> peptideAnalysisIds)
        {
//            String idList = null;
//            if (peptideAnalysisIds != null)
//            {
//                idList = "(" + Lists.Join(peptideAnalysisIds, ",") + ")";
//            }
//
//            var peptideAnalysisRows = new Dictionary<long, PeptideAnalysisRow>();
//                String hql = "SELECT pa.Id, pa.Peptide.Id, pa.Note, pa.FileAnalysisCount "
//                             + "\nFROM " + typeof(DbPeptideAnalysis) + " pa";
//
//                if (idList != null)
//                {
//                    hql += "\nWHERE pa.Id IN " + idList;
//                }
//                var query = session.CreateQuery(hql);
//                foreach (object[] rowData in query.List())
//                {
//                    PeptideAnalysisRow peptideAnalysisRow;
//                    var id = (long)rowData[0];
//                    if (!peptideAnalysisRows.TryGetValue(id, out peptideAnalysisRow))
//                    {
//                        peptideAnalysisRow = new PeptideAnalysisRow { Id = id };
//                        peptideAnalysisRows.Add(id, peptideAnalysisRow);
//                    }
//                    peptideAnalysisRow.PeptideId = (long)rowData[1];
//                    peptideAnalysisRow.Note = (string)rowData[2];
//                    peptideAnalysisRow.DataFileCount = (int)rowData[3];
//                }
//                BeginInvoke(new Action<Dictionary<long, PeptideAnalysisRow>>(UpdateRows), peptideAnalysisRows);
//                var hql2 = "SELECT pfa.PeptideAnalysis.Id, Min(pfa.DeconvolutionScore), Max(pfa.DeconvolutionScore), Min(pfa.ValidationStatus), Max(pfa.ValidationStatus) "
//                           + "\nfrom " + typeof(DbPeptideFileAnalysis) +
//                           " pfa ";
//                if (idList != null)
//                {
//                    hql2 += "\nWHERE pfa.PeptideAnalysis.Id IN " + idList;
//                }
//                hql2 += "\nGROUP BY pfa.PeptideAnalysis.Id";
//                var query2 = session.CreateQuery(hql2);
//                foreach (object[] rowData in query2.List())
//                {
//                    PeptideAnalysisRow peptideAnalysisRow;
//                    var id = (long)rowData[0];
//                    if (!peptideAnalysisRows.TryGetValue(id, out peptideAnalysisRow))
//                    {
//                        continue;
//                    }
//                    peptideAnalysisRow.MinScore = (double?)rowData[1];
//                    peptideAnalysisRow.MaxScore = (double?)rowData[2];
//                    peptideAnalysisRow.MinValidationStatus = (ValidationStatus?)rowData[3];
//                    peptideAnalysisRow.MaxValidationStatus = (ValidationStatus?)rowData[4];
//                }
//            if (peptideAnalysisIds != null)
//            {
//                foreach (var id in peptideAnalysisIds)
//                {
//                    if (!peptideAnalysisRows.ContainsKey(id))
//                    {
//                        peptideAnalysisRows.Add(id, null);
//                    }
//                }
//            }
//            BeginInvoke(new Action<Dictionary<long, PeptideAnalysisRow>>(UpdateRows), peptideAnalysisRows);
        }

        private void UpdateRows(Dictionary<long, PeptideAnalysisRow> rows)
        {
//            Text = TabText = Title;
//            if (rows.Count == 0)
//            {
//                return;
//            }
//            try
//            {
//                dataGridView.SuspendLayout();
//                foreach (var entry in rows)
//                {
//                    DataGridViewRow row;
//                    _peptideAnalysisRows.TryGetValue(entry.Key, out row);
//                    if (entry.Value == null)
//                    {
//                        if (row != null)
//                        {
//                            dataGridView.Rows.Remove(row);
//                            _peptideAnalysisRows.Remove(entry.Key);
//                        }
//                        continue;
//                    }
//                    if (row == null)
//                    {
//                        row = dataGridView.Rows[dataGridView.Rows.Add()];
//                        _peptideAnalysisRows.Add(entry.Key, row);
//                        row.Tag = entry.Value.Id;
//                    }
//                    Peptide peptide;
//                    Workspace.Peptides.TryGetValue(entry.Value.PeptideId, out peptide);
//                    var peptideAnalysis = Workspace.PeptideAnalyses[entry.Value.Id];
//                    if (peptide == null)
//                    {
//                        row.Cells[colProteinKey.Index].Value = null;
//                        row.Cells[colPeptide.Index].Value = null;
//                        row.Cells[colProteinDescription.Index].Value = row.Cells[colProteinDescription.Index].ToolTipText = null;
//                        row.Cells[colMaxTracers.Index].Value = 0;
//                    }
//                    else
//                    {
//                        row.Cells[colProteinKey.Index].Value = peptide.GetProteinKey();
//                        row.Cells[colPeptide.Index].Value = peptide.FullSequence;
//                        row.Cells[colProteinDescription.Index].Value = row.Cells[colProteinDescription.Index].ToolTipText = peptide.ProteinDescription;
//                        row.Cells[colMaxTracers.Index].Value = peptide.MaxTracerCount;
//                    }
//                    row.Cells[colMinScore.Index].Value = entry.Value.MinScore;
//                    row.Cells[colMaxScore.Index].Value = entry.Value.MaxScore;
//                    row.Cells[colDataFileCount.Index].Value = entry.Value.DataFileCount;
//                    if (peptideAnalysis == null)
//                    {
//                        row.Cells[colNote.Index].Value = entry.Value.Note;
//                        if (entry.Value.MinValidationStatus == entry.Value.MaxValidationStatus)
//                        {
//                            row.Cells[colStatus.Index].Value = entry.Value.MinValidationStatus;
//                        }
//                        else
//                        {
//                            row.Cells[colStatus.Index].Value = null;
//                        }
//                    }
//                    else
//                    {
//                        row.Cells[colNote.Index].Value = peptideAnalysis.Note;
//                        row.Cells[colStatus.Index].Value = peptideAnalysis.GetValidationStatus();
//                    }
//                }
//            }
//            finally
//            {
//                dataGridView.ResumeLayout();
//            }
        }

        private PeptideAnalysisFrame OpenPeptideAnalysis(PeptideAnalysis peptideAnalysis)
        {
            return PeptideAnalysisFrame.ShowPeptideAnalysis(peptideAnalysis);
        }

        private void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
//            var column = dataGridView.Columns[e.ColumnIndex];
//            if (column.ReadOnly)
//            {
//                return;
//            }
//            var row = dataGridView.Rows[e.RowIndex];
//            var cell = row.Cells[e.ColumnIndex];
//            var peptideAnalysisId = (long)row.Tag;
//            var peptideAnalysis = TurnoverForm.Instance.LoadPeptideAnalysis(peptideAnalysisId);
//            if (peptideAnalysis == null)
//            {
//                return;
//            }
//            using (Workspace.GetWriteLock())
//            {
//                if (column == colNote)
//                {
//                    peptideAnalysis.Note = Convert.ToString(cell.Value);
//                }
//                else if (column == colStatus)
//                {
//                    peptideAnalysis.SetValidationStatus((ValidationStatus?) cell.Value);
//                }
//            }
        }

        private void btnAnalyzePeptides_Click(object sender, EventArgs e)
        {
            new AnalyzePeptidesForm(Workspace).Show(TopLevelControl);
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
//            if (e.RowIndex < 0 || e.ColumnIndex < 0)
//            {
//                return;
//            }
//            var column = dataGridView.Columns[e.ColumnIndex];
//            var row = dataGridView.Rows[e.RowIndex];
//            if (column != colPeptide && column != colMinScore && column != colMaxScore)
//            {
//                return;
//            }
//            var peptideAnalysis = TurnoverForm.Instance.LoadPeptideAnalysis((long)row.Tag);
//            if (peptideAnalysis == null)
//            {
//                return;
//            }
//            var form = OpenPeptideAnalysis(peptideAnalysis);
//            if (column == colMinScore || column == colMaxScore)
//            {
//                bool max = column == colMaxScore;
//                Peaks peaks = null;
//                foreach (var peptideFileAnalysis in peptideAnalysis.FileAnalyses.ListPeptideFileAnalyses(true))
//                {
//                    if (!peptideFileAnalysis.Peaks.DeconvolutionScore.HasValue)
//                    {
//                        continue;
//                    }
//                    if (peaks == null)
//                    {
//                        peaks = peptideFileAnalysis.Peaks;
//                    }
//                    else if (max)
//                    {
//                        if (peptideFileAnalysis.Peaks.DeconvolutionScore > peaks.DeconvolutionScore)
//                        {
//                            peaks = peptideFileAnalysis.Peaks;
//                        }
//                    }
//                    else
//                    {
//                        if (peptideFileAnalysis.Peaks.DeconvolutionScore < peaks.DeconvolutionScore)
//                        {
//                            peaks = peptideFileAnalysis.Peaks;
//                        }
//                    }
//                }
//                if (peaks == null)
//                {
//                    return;
//                }
//                PeptideFileAnalysisFrame.ActivatePeptideDataForm<TracerChromatogramForm>(form.PeptideAnalysisSummary, peaks.PeptideFileAnalysis);
//                return;
//            }
        }

        public class PeptideAnalysisRow : PropertyChangedSupport
        {
            public PeptideAnalysisRow(PeptideAnalysis peptideAnalysis)
            {
                PeptideAnalysis = peptideAnalysis;
                ListenToChanges(PeptideAnalysis);
                foreach (var peptideFileAnalysis in PeptideAnalysis.FileAnalyses)
                {
                    ListenToChanges(peptideFileAnalysis);
                }
            }

            public LinkValue<string> Peptide
            {
                get
                {
                    return new LinkValue<string>(PeptideAnalysis.Peptide.FullSequence,
                                                 (sender, args) => PeptideAnalysisFrame.ShowPeptideAnalysis(PeptideAnalysis));
                }
            }

            public PeptideAnalysis PeptideAnalysis { get; private set; }

            [DataGridViewColumnType(typeof (ValidationStatusColumn))]
            public ValidationStatus? ValidationStatus
            {
                get { return PeptideAnalysis.GetValidationStatus(); }
                set { PeptideAnalysis.SetValidationStatus(value); }
            }

            public LinkValue<double?> MinScore
            {
                get
                {
                    KeyValuePair<PeptideFileAnalysis, double>? minEntry = null;
                    foreach (var kvp in ListScores())
                    {
                        if (!minEntry.HasValue || minEntry.Value.Value > kvp.Value)
                        {
                            minEntry = kvp;
                        }
                    }
                    return new LinkValue<double?>(minEntry == null ? null : (double?) minEntry.Value.Value,
                                                  (sender, args) =>
                                                      {
                                                          if (minEntry != null)
                                                          {
                                                              PeptideFileAnalysisFrame.ShowPeptideFileAnalysis(
                                                                  PeptideAnalysis.Workspace, minEntry.Value.Key.Id);
                                                          }
                                                      });
                }
            }

            public LinkValue<double?> MaxScore
            {
                get
                {
                    KeyValuePair<PeptideFileAnalysis, double>? maxEntry = null;
                    foreach (var kvp in ListScores())
                    {
                        if (!maxEntry.HasValue || maxEntry.Value.Value <= kvp.Value)
                        {
                            maxEntry = kvp;
                        }
                    }
                    return new LinkValue<double?>(maxEntry == null ? null : (double?) maxEntry.Value.Value,
                                                  (sender, args) =>
                                                      {
                                                          if (maxEntry != null)
                                                          {
                                                              PeptideFileAnalysisFrame.ShowPeptideFileAnalysis(
                                                                  PeptideAnalysis.Workspace, maxEntry.Value.Key.Id);
                                                          }

                                                      });
                }
            }
            [DisplayName("# Data Files")]
            public int FileAnalysisCount { get { return PeptideAnalysis.FileAnalyses.Count; } }


            private IList<KeyValuePair<PeptideFileAnalysis, double>> ListScores()
            {
                return PeptideAnalysis.GetFileAnalyses(true)
                    .Select(peptideFileAnalysis => new KeyValuePair<PeptideFileAnalysis, double?>(peptideFileAnalysis, peptideFileAnalysis.PeakData.DeconvolutionScore))
                    .Where(kvp => kvp.Value.HasValue)
                    .Select(kvp =>new KeyValuePair<PeptideFileAnalysis, double>(kvp.Key, kvp.Value.GetValueOrDefault()))
                    .ToArray();
            }
        }

        class PeptideAnalysisDeleteHandler : DeleteHandler
        {
            private readonly PeptideAnalysesForm _form;
            public PeptideAnalysisDeleteHandler(PeptideAnalysesForm form)
            {
                _form = form;
            }

            public override void Delete()
            {
                IList<PeptideAnalysis> peptideAnalyses = GetSelectedRows<PeptideAnalysisRow>(_form.boundDataGridView1).Select(row=>row.PeptideAnalysis).ToArray();
                if (peptideAnalyses.Count == 0)
                {
                    return;
                }
                string message;
                if (peptideAnalyses.Count == 1)
                {
                    message = string.Format("Are you sure you want to delete the analysis of the peptide '{0}'?",
                                            peptideAnalyses[0].Peptide.FullSequence);
                }
                else
                {
                    message = string.Format("Are you sure you want to delete these {0} peptide analyses?",
                                            peptideAnalyses.Count);
                }
                if (MessageBox.Show(_form, message, Program.AppName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    return;
                }
                using (var longWaitDlg = new LongWaitDialog(_form, "Deleting Peptide Analyses"))
                {
                    using (var session = _form.Workspace.OpenSession())
                    {
                        new LongOperationBroker(
                            longOpBroker => DeletePeptideAnalyses(longOpBroker, session, peptideAnalyses),
                            longWaitDlg).LaunchJob();
                    }
                }
                _form.Workspace.DatabasePoller.MergeChangesNow();
            }

            private static void DeletePeptideAnalyses(LongOperationBroker broker, ISession session, IList<PeptideAnalysis> peptideAnalyses)
            {
                var changeLogs = peptideAnalyses.Select(peptideAnalysis => new DbChangeLog(peptideAnalysis)).ToArray();
                var analysisIds = peptideAnalyses.Select(peptideAnalysis => peptideAnalysis.Id).ToArray();
                var strAnalysisIds = string.Join(",", analysisIds.Select(id => id.ToString(CultureInfo.InvariantCulture)).ToArray());
                session.BeginTransaction();
                broker.UpdateStatusMessage("Deleting chromatograms");
                session.CreateSQLQuery(
                    string.Format(
                        "UPDATE DbPeptideFileAnalysis SET ChromatogramSet = NULL WHERE PeptideAnalysis IN ({0})",
                        strAnalysisIds))
                    .ExecuteUpdate();
                session.CreateSQLQuery(
                    string.Format("DELETE C FROM DbChromatogram C"
                                    + "\nJOIN DbChromatogramSet S ON C.ChromatogramSet + S.Id"
                                    + "\nJOIN DbPeptideFileAnalysis F ON S.PeptideFileAnalysis = F.Id"
                                    + "\nWHERE F.PeptideAnalysis IN ({0})",
                                    strAnalysisIds))
                    .ExecuteUpdate();
                session.CreateSQLQuery(
                    string.Format("DELETE S FROM DbChromatogramSet S"
                                    + "\nJOIN DbPeptideFileAnalysis F ON S.PeptideFileAnalysis = F.Id"
                                    + "\nWHERE F.PeptideAnalysis IN ({0})",
                                    strAnalysisIds))
                    .ExecuteUpdate();
                broker.UpdateStatusMessage("Deleting results");
                session.CreateSQLQuery(
                    string.Format("DELETE P FROM DbPeak P"
                                    + "\nJOIN DbPeptideFileAnalysis F ON P.PeptideFileAnalysis = F.Id"
                                    + "\nWHERE F.PeptideAnalysis IN ({0})",
                                    strAnalysisIds))
                    .ExecuteUpdate();
                broker.UpdateStatusMessage("Deleting analyses");
                session.CreateSQLQuery(
                    string.Format("DELETE FROM DbPeptideFileAnalysis WHERE PeptideAnalysis IN ({0})",
                                    strAnalysisIds))
                    .ExecuteUpdate();
                session.CreateSQLQuery(
                    string.Format("DELETE FROM DbPeptideAnalysis WHERE Id IN ({0})",
                                    strAnalysisIds))
                    .ExecuteUpdate();
                foreach (var changeLog in changeLogs)
                {
                    session.Save(changeLog);
                }
                session.Transaction.Commit();
            }
        }

        class PeptideAnalysisRows : ConvertedCloneableBindingList<long, PeptideAnalysis, PeptideAnalysisRow>
        {
            public PeptideAnalysisRows(PeptideAnalyses peptideAnalyses) : base(peptideAnalyses)
            {
            }

            public override long GetKey(PeptideAnalysisRow value)
            {
                return value.PeptideAnalysis.Id;
            }

            protected override PeptideAnalysisRow Convert(PeptideAnalysis source)
            {
                return new PeptideAnalysisRow(source);
            }
        }
    }
}
