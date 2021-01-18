using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pwiz.Common.Collections;
using pwiz.Skyline.EditUI;
using pwiz.Skyline.Model;
using pwiz.Skyline.Model.Crosslinking;
using pwiz.Skyline.Model.DocSettings;
using pwiz.Skyline.SettingsUI;
using pwiz.SkylineTestUtil;

namespace pwiz.SkylineTestFunctional
{
    [TestClass]
    public class ComplexCrosslinkTest : AbstractFunctionalTest
    {
        [TestMethod]
        public void TestComplexCrosslinks()
        {
            RunFunctionalTest();
        }

        protected override void DoTest()
        {
            var peptideSettingsUi = ShowDialog<PeptideSettingsUI>(SkylineWindow.ShowPeptideSettingsUI);
            RunUI(() =>
            {
                peptideSettingsUi.SelectedTab = PeptideSettingsUI.TABS.Digest;
                peptideSettingsUi.MaxMissedCleavages = 6;
                peptideSettingsUi.SelectedTab = PeptideSettingsUI.TABS.Modifications;
            });
            var editModListDlg = ShowEditStaticModsDlg(peptideSettingsUi);
            RunDlg<EditStaticModDlg>(editModListDlg.AddItem, editStaticModDlg =>
            {
                editStaticModDlg.Modification = new StaticMod("DSS", "K", null, "C8H10O2");
                editStaticModDlg.IsCrosslinker = true;
                editStaticModDlg.OkDialog();
            });
            RunDlg<EditStaticModDlg>(editModListDlg.AddItem, editStaticModDlg =>
            {
                editStaticModDlg.Modification = new StaticMod("disulfide", "C", null, "-H2");
                editStaticModDlg.IsCrosslinker = true;
                editStaticModDlg.OkDialog();
            });
            OkDialog(editModListDlg, editModListDlg.OkDialog);
            OkDialog(peptideSettingsUi, peptideSettingsUi.OkDialog);

            RunUI(() =>
            {
                SkylineWindow.Paste("KNICKKNACK");
                SkylineWindow.SelectedPath = SkylineWindow.Document.GetPathTo((int) SrmDocument.Level.Molecules, 0);
            });
            var pepMods1 = ShowDialog<EditPepModsDlg>(SkylineWindow.ModifyPeptide);
            var editLink1 =
                ShowDialog<EditLinkedPeptideDlg>(() => pepMods1.SetModification(0, IsotopeLabelType.light, "DSS"));
            Assert.AreEqual(1, editLink1.LooplinkChoices.Count());
            Assert.AreEqual(ImmutableList.Empty<ModificationSite>(), editLink1.LooplinkChoices.First());
            RunUI(()=>editLink1.PeptideSequence = "KAFFEEKLATSCH");
            var pepMods2 = ShowNestedDlg<EditPepModsDlg>(editLink1.ShowEditModifications);
            Assert.AreEqual(1, pepMods2.RootPeptide.ExplicitMods.Crosslinks.Count);
            var editLink2 = ShowNestedDlg<EditLinkedPeptideDlg>(
                () => pepMods2.SetModification(11, IsotopeLabelType.light, "disulfide"));
            Assert.AreEqual(2, editLink2.LooplinkChoices.Count());
            CollectionAssert.Contains(editLink2.LooplinkChoices.ToList(), ImmutableList.Empty<ModificationSite>());
            CollectionAssert.Contains(editLink2.LooplinkChoices.ToList(), ImmutableList.Singleton(new ModificationSite(0, "DSS")));
            RunUI(()=>editLink2.PeptideSequence = "KINNIKINNICK");
            var pepMods3 = ShowNestedDlg<EditPepModsDlg>(editLink2.ShowEditModifications);
            var editLink3 = ShowNestedDlg<EditLinkedPeptideDlg>(() => pepMods3.SetModification(0, IsotopeLabelType.light, "DSS"));
            RunUI(()=>
            {
                editLink3.IsLooplink = true;
                editLink3.ChooseLooplinkPeptide(ModificationSitePath.Singleton(new ModificationSite(0, "DSS")));
                editLink3.AttachmentOrdinal = 7;
            });
            OkDialog(editLink3, editLink3.OkDialog);
            OkDialog(pepMods3, pepMods3.OkDialog);
            var explicitMods3 = pepMods3.ExplicitMods;
            Assert.AreEqual(1, explicitMods3.Crosslinks.Count);
            RunUI(()=>editLink2.AttachmentOrdinal = 11);
            OkDialog(editLink2, editLink2.OkDialog);
            OkDialog(pepMods2, pepMods2.OkDialog);
            RunUI(()=>editLink1.AttachmentOrdinal = 1);
            OkDialog(editLink1, editLink1.OkDialog);
            OkDialog(pepMods1, pepMods1.OkDialog);
            var peptideDocNode = SkylineWindow.Document.Peptides.First();
            var modifiedSequence = ModifiedSequence.GetModifiedSequence(SkylineWindow.Document.Settings, peptideDocNode,
                IsotopeLabelType.light);
            var strModifiedSequence = modifiedSequence.FullNames;
            Assert.AreNotEqual(string.Empty, strModifiedSequence);
            AssertEx.Serializable(SkylineWindow.Document);
            AssertEx.ValidatesAgainstSchema(SkylineWindow.Document);
        }
    }
}
