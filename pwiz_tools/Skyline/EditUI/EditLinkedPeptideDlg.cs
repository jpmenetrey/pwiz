/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2020 University of Washington - Seattle, WA
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
using System.Linq;
using System.Windows.Forms;
using pwiz.Common.Collections;
using pwiz.Skyline.Controls;
using pwiz.Skyline.Model;
using pwiz.Skyline.Model.Crosslinking;
using pwiz.Skyline.Model.DocSettings;
using pwiz.Skyline.Properties;

namespace pwiz.Skyline.EditUI
{
    public partial class EditLinkedPeptideDlg : Form
    {
        private SrmSettings _settings;
        private ExplicitMods _explicitMods;
        private StaticMod _crosslinkMod;
        private string _rememberedPeptideSequence;
        private List<ImmutableList<ModificationSite>> _looplinkChoices;

        public EditLinkedPeptideDlg(SrmSettings settings, PeptideDocNode parentPeptide, LinkedPeptide linkedPeptide, StaticMod crosslinkMod, IEnumerable<ModificationSite> crosslinkLocation)
        {
            InitializeComponent();
            _settings = settings;
            ParentPeptide = parentPeptide;
            CrosslinkLocation = ImmutableList.ValueOf(crosslinkLocation);
            var looplinkLocation = linkedPeptide?.PeptideLocation ?? ImmutableList.ValueOf(CrosslinkLocation.Take(CrosslinkLocation.Count - 1));
            _crosslinkMod = crosslinkMod;
            _looplinkChoices = GetLinkedPeptideChoices(parentPeptide).ToList();
            foreach (var looplinkChoice in _looplinkChoices)
            {
                var peptide = parentPeptide.FindLinkedPeptide(looplinkChoice).Item1;
                comboLinkedPeptide.Items.Add(peptide.Sequence);
                if (looplinkChoice.SequenceEqual(looplinkLocation))
                {
                    comboLinkedPeptide.SelectedIndex = comboLinkedPeptide.Items.Count - 1;
                }
            }
            if (linkedPeptide == null)
            {
                radioButtonNewPeptide.Checked = true;
            }
            else
            {
                if (linkedPeptide.Peptide != null)
                {
                    radioButtonNewPeptide.Checked = true;
                    tbxPeptideSequence.Text = linkedPeptide.Peptide.Sequence;
                }
                else
                {
                    radioButtonLoopLink.Checked = true;
                }
                tbxAttachmentOrdinal.Text = (linkedPeptide.IndexAa + 1).ToString();
                _explicitMods = linkedPeptide.ExplicitMods;
            }
        }
        public PeptideDocNode ParentPeptide { get; private set; }
        public LinkedPeptide LinkedPeptide { get; private set; }
        public ImmutableList<ModificationSite> CrosslinkLocation { get; private set; }

        public void OkDialog()
        {
            LinkedPeptide linkedPeptide;
            if (!TryMakeLinkedPeptide(out linkedPeptide))
            {
                return;
            }

            LinkedPeptide = linkedPeptide;
            DialogResult = DialogResult.OK;
        }

        private bool TryMakeLinkedPeptide(out LinkedPeptide linkedPeptide)
        {
            linkedPeptide = null;
            ImmutableList<ModificationSite> looplinkLocation;
            Peptide peptide;
            if (radioButtonLoopLink.Checked)
            {
                looplinkLocation = _looplinkChoices[comboLinkedPeptide.SelectedIndex];
                peptide = ParentPeptide.FindLinkedPeptide(looplinkLocation).Item1;
            }
            else
            {
                if (!TryMakePeptide(out peptide))
                {
                    return false;
                }

                looplinkLocation = null;
            }

            string peptideSequence = peptide.Sequence;
            var messageBoxHelper = new MessageBoxHelper(this);
            int aaOrdinal;
            if (!messageBoxHelper.ValidateNumberTextBox(tbxAttachmentOrdinal, 1, peptideSequence.Length, out aaOrdinal))
            {
                return false;
            }

            string validAminoAcids = _crosslinkMod?.AAs;
            if (!string.IsNullOrEmpty(validAminoAcids))
            {
                char aa = peptideSequence[aaOrdinal - 1];
                if (!validAminoAcids.Contains(aa))
                {
                    string message = string.Format(Resources.EditLinkedPeptideDlg_TryMakeLinkedPeptide_The_crosslinker___0___cannot_attach_to_the_amino_acid___1___,
                        _crosslinkMod.Name, aa);
                    messageBoxHelper.ShowTextBoxError(tbxAttachmentOrdinal, message);
                    return false;
                }
            }

            if (looplinkLocation == null)
            {
                linkedPeptide = new LinkedPeptide(peptide, aaOrdinal - 1, MakeExplicitMods(peptide, _explicitMods));
            }
            else
            {
                linkedPeptide = new LinkedPeptide(looplinkLocation, aaOrdinal - 1);
            }
            return true;
        }

        private bool TryMakePeptide(out Peptide peptide)
        {
            peptide = null;
            var messageBoxHelper = new MessageBoxHelper(this);
            var peptideSequence = tbxPeptideSequence.Text.Trim();
            if (string.IsNullOrEmpty(peptideSequence))
            {
                messageBoxHelper.ShowTextBoxError(tbxPeptideSequence, Resources.PasteDlg_ListPeptideSequences_The_peptide_sequence_cannot_be_blank);
                return false;
            }
            if (!FastaSequence.IsExSequence(peptideSequence))
            {
                messageBoxHelper.ShowTextBoxError(tbxPeptideSequence, Resources.PasteDlg_ListPeptideSequences_This_peptide_sequence_contains_invalid_characters);
                return false;
            }

            peptide = new Peptide(peptideSequence);
            return true;
        }

        private ExplicitMods MakeExplicitMods(Peptide peptide, ExplicitMods oldExplicitMods)
        {
            if (oldExplicitMods == null)
            {
                return null;
            }

            var newStaticMods = oldExplicitMods.StaticModifications.Where(mod => mod.IndexAA < peptide.Sequence.Length)
                .ToList();
            var newHeavyMods = new List<TypedExplicitModifications>();
            foreach (var heavyMods in oldExplicitMods.GetHeavyModifications())
            {
                var newMods = heavyMods.Modifications.Where(mod => mod.IndexAA < peptide.Sequence.Length).ToList();
                if (newMods.Count != 0)
                {
                    newHeavyMods.Add(new TypedExplicitModifications(peptide, heavyMods.LabelType, newMods));
                }
            }
            return new ExplicitMods(peptide, newStaticMods, newHeavyMods);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OkDialog();
        }

        private void btnEditModifications_Click(object sender, EventArgs e)
        {
            ShowEditModifications();
        }

        public void ShowEditModifications()
        {
            Peptide peptide;
            if (!TryMakePeptide(out peptide))
            {
                return;
            }

            var explicitMods = MakeExplicitMods(peptide, _explicitMods);
            var peptideDocNode = new PeptideDocNode(peptide, _settings, explicitMods, null, ExplicitRetentionTimeInfo.EMPTY, new TransitionGroupDocNode[0], false);
            using (var pepModsDlg = new EditPepModsDlg(_settings, peptideDocNode, CrosslinkLocation))
            {
                if (pepModsDlg.ShowDialog(this) == DialogResult.OK)
                {
                    _explicitMods = pepModsDlg.ExplicitMods;
                }
            }
        }

        public string PeptideSequence
        {
            get { return tbxPeptideSequence.Text; }
            set
            {
                tbxPeptideSequence.Text = value;
            }
        }

        public int? AttachmentOrdinal
        {
            get
            {
                if (string.IsNullOrEmpty(tbxAttachmentOrdinal.Text))
                {
                    return null;
                }
                return int.Parse(tbxAttachmentOrdinal.Text);
            }
            set { tbxAttachmentOrdinal.Text = value.HasValue ? value.ToString() : string.Empty; }
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLoopLink.Checked)
            {
                _rememberedPeptideSequence = tbxPeptideSequence.Text;
                tbxPeptideSequence.Text = string.Empty;
                comboLinkedPeptide.Enabled = true;
                tbxPeptideSequence.Enabled = false;
                btnEditModifications.Enabled = false;
            }
            else
            {
                if (string.IsNullOrEmpty(tbxPeptideSequence.Text))
                {
                    tbxPeptideSequence.Text = _rememberedPeptideSequence;
                }

                comboLinkedPeptide.Enabled = false;
                tbxPeptideSequence.Enabled = true;
                btnEditModifications.Enabled = true;
            }
        }

        public IEnumerable<ImmutableList<ModificationSite>> GetLinkedPeptideChoices(
            PeptideDocNode peptideDocNode)
        {
            var queue = new List<Tuple<ImmutableList<ModificationSite>, LinkedPeptide>>();
            yield return ImmutableList<ModificationSite>.EMPTY;
            if (peptideDocNode.ExplicitMods != null)
            {
                queue.AddRange(peptideDocNode.ExplicitMods.LinkedCrossslinks.Select(entry=>Tuple.Create(ImmutableList.Singleton(entry.Key), entry.Value)));
            }

            while (queue.Count > 0)
            {
                var location = queue[0].Item1;
                var linkedPeptide = queue[0].Item2;
                queue.RemoveAt(0);
                yield return location;
                if (linkedPeptide.ExplicitMods != null)
                {
                    queue.AddRange(linkedPeptide.ExplicitMods.LinkedCrossslinks.Select(entry =>
                        Tuple.Create(ImmutableList.ValueOf(location.Append(entry.Key)), entry.Value)));
                }
            }
        }
    }
}
