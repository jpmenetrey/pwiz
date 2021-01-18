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
        private List<ModificationSitePath> _looplinkChoices;

        public EditLinkedPeptideDlg(SrmSettings settings, PeptideDocNode rootPeptide, ModificationSitePath crosslinkLocation)
        {
            InitializeComponent();
            _settings = settings;
            RootPeptide = rootPeptide;
            CrosslinkLocation = crosslinkLocation;
            var crosslinkModification = rootPeptide.FindExplicitMod(CrosslinkLocation);
            var looplinkLocation = crosslinkModification.LinkedPeptide?.PeptideLocation ?? CrosslinkLocation.Parent;
            _crosslinkMod = crosslinkModification.Modification;
            _looplinkChoices = GetLinkedPeptideChoices(rootPeptide).ToList();
            foreach (var looplinkChoice in _looplinkChoices)
            {
                Peptide peptide;
                if (looplinkChoice.IsRoot)
                {
                    peptide = rootPeptide.Peptide;
                }
                else
                {
                    peptide = rootPeptide.FindExplicitMod(looplinkChoice).LinkedPeptide.Peptide;
                }
                comboLinkedPeptide.Items.Add(peptide.Sequence);
                if (looplinkChoice.Equals(looplinkLocation))
                {
                    comboLinkedPeptide.SelectedIndex = comboLinkedPeptide.Items.Count - 1;
                }
            }

            var linkedPeptide = crosslinkModification.LinkedPeptide;
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
        public PeptideDocNode RootPeptide { get; private set; }
        public LinkedPeptide LinkedPeptide { get; private set; }
        public ModificationSitePath CrosslinkLocation { get; private set; }
        public IEnumerable<ModificationSitePath> LooplinkChoices
        {
            get { return _looplinkChoices.AsEnumerable(); }
        }

        public bool IsLooplink
        {
            get
            {
                return radioButtonLoopLink.Checked;
            }
            set
            {
                if (value)
                {
                    radioButtonLoopLink.Checked = true;
                }
                else
                {
                    radioButtonNewPeptide.Checked = true;
                }
            }
        }

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
            ModificationSitePath looplinkLocation;
            Peptide peptide;
            if (radioButtonLoopLink.Checked)
            {
                looplinkLocation = _looplinkChoices[comboLinkedPeptide.SelectedIndex];
                peptide = RootPeptide.FindExplicitMod(looplinkLocation).LinkedPeptide.Peptide;
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

            var currentExplicitMods = MakeExplicitMods(peptide, _explicitMods);
            var newExplicitMod = new ExplicitMod(CrosslinkLocation.Sites.Last().IndexAa, _crosslinkMod).ChangeLinkedPeptide(new LinkedPeptide(peptide, -1, currentExplicitMods));
            var currentRootPeptide = RootPeptide.ChangeExplicitMods(
                RootPeptide.ExplicitMods.ReplaceModAt(CrosslinkLocation, newExplicitMod));

            using (var pepModsDlg = new EditPepModsDlg(_settings, currentRootPeptide, CrosslinkLocation))
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

        public IEnumerable<ModificationSitePath> GetLinkedPeptideChoices(
            PeptideDocNode peptideDocNode)
        {
            var queue = new List<Tuple<ModificationSitePath, LinkedPeptide>>();
            yield return ModificationSitePath.ROOT;
            if (peptideDocNode.ExplicitMods != null)
            {
                queue.AddRange(peptideDocNode.ExplicitMods.LinkedCrossslinks.Select(entry=>Tuple.Create(ModificationSitePath.Singleton(entry.Key), entry.Value)));
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
                        Tuple.Create(location.Append(entry.Key), entry.Value)));
                }
            }
        }

        public void ChooseLooplinkPeptide(ModificationSitePath modificationSitePath)
        {
            comboLinkedPeptide.SelectedIndex = _looplinkChoices.IndexOf(modificationSitePath);
        }
    }
}
