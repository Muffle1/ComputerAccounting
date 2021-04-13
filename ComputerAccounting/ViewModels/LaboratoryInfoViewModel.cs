using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace ComputerAccounting
{
    public class LaboratoryInfoViewModel : BaseViewModel
    {
        private List<Cabinet> _cabinets;
        public List<Cabinet> Cabinets
        {
            get => _cabinets;
            set => SetValue(ref _cabinets, value, nameof(Cabinets));
        }

        private Cabinet _selectedCabinet;
        public Cabinet SelectedCabinet
        {
            get => _selectedCabinet;
            set => SetValue(ref _selectedCabinet, value, nameof(SelectedCabinet));
        }

        public LaboratoryInfoViewModel()
        {
            using DataBaseHelper db = new DataBaseHelper();
            LoadCabinetsAsync();
        }

        private async void LoadCabinetsAsync()
        {
            await Task.Run(() =>
            {
                using DataBaseHelper db = new DataBaseHelper();
                Cabinets = db.Cabinets.ToList().OrderBy(c => c.GetCabinetNumber()).ToList();
            });
        }

        private RelayCommand _createDocumentCommand;
        public RelayCommand CreateDocumentCommand
        {
            get
            {
                return _createDocumentCommand ??= new RelayCommand(async o =>
                {
                    await Task.Run(() =>
                    {
                        if (SelectedCabinet != null)
                        {
                            using DataBaseHelper db = new DataBaseHelper();
                            List<Soft> softs = new List<Soft>();

                            foreach (var computerItem in db.Computers.Where(c => c.Cabinet == SelectedCabinet).ToList())
                            {
                                foreach (var soft in computerItem.Softs.Split(", "))
                                {
                                    if (!softs.Any(s => s.SoftName == soft))
                                        softs.Add(new Soft() { SoftName = soft });
                                }
                            }

                            object oMissing = Missing.Value;
                            object oTemplate = @$"{Environment.CurrentDirectory}\LaboratoryDocumentTemplate.dotx";

                            object oCabinetNumber1 = "CabinetNumber1";
                            object oCabinetNumber2 = "CabinetNumber2";
                            object oComputersCount = "ComputersCount";
                            object oComputerConfig = "ComputerConfig";
                            object oEndOfDoc = "\\endofdoc";

                            Word._Application oWord = new Word.Application();
                            Word._Document oDoc = new Word.Document();
                            try
                            {
                                oWord = new Word.Application();
                                oDoc = oWord.Documents.Add(ref oTemplate, ref oMissing, ref oMissing, ref oMissing);
                            }
                            catch
                            {
                                oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                                oDoc = null;
                                oWord = null;
                                return;
                            }
                            oWord.Visible = true;

                            oDoc.Bookmarks.get_Item(ref oCabinetNumber1).Range.Text = SelectedCabinet.GetCabinetNumber().ToString();
                            oDoc.Bookmarks.get_Item(ref oCabinetNumber2).Range.Text = SelectedCabinet.GetCabinetNumber().ToString();

                            oDoc.Bookmarks.get_Item(ref oComputersCount).Range.Text = db.Computers.Where(c => c.Cabinet == SelectedCabinet).Count().ToString();

                            Word.Table oTable = oDoc.Tables[2];

                            for (int i = 2; i <= softs.Count + 1; i++)
                            {
                                oTable.Rows.Add();
                                oTable.Cell(i, 1).Range.Text = (i - 1).ToString();
                                oTable.Cell(i, 2).Range.Text = softs[i - 2].SoftName;
                                oTable.Rows[i].Range.Font.Bold = 0;
                            }

                            oTable = oDoc.Tables[3];
                            Word.Range range = oTable.Range;
                            range.Copy();
                            oDoc.Tables[3].Delete();

                            for (int i = 1; i <= db.Computers.Where(c => c.Cabinet == SelectedCabinet).Count(); i++)
                            {
                                Computer computer = db.Computers.Where(c => c.Cabinet == SelectedCabinet).ToList()[i - 1];

                                if (i == 1)
                                    oDoc.Bookmarks.get_Item(ref oComputerConfig).Range.Text = $"Компьютер {computer.InventoryNumber}";
                                else
                                    oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.Text = $"Компьютер {computer.InventoryNumber}";

                                oTable = oDoc.Tables.Add(oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range, 1, 3, ref oMissing, ref oMissing);
                                oTable.Range.Paste();

                                oDoc.Tables[i + 2].Cell(2, 3).Range.Text = computer.CPUName;
                                oDoc.Tables[i + 2].Cell(3, 3).Range.Text = computer.GPUName;
                                oDoc.Tables[i + 2].Cell(4, 3).Range.Text = computer.RAMName;
                                oDoc.Tables[i + 2].Cell(5, 3).Range.Text = computer.RAMCapacity.ToString();
                                oDoc.Tables[i + 2].Cell(6, 3).Range.Text = computer.DiskName;
                                oDoc.Tables[i + 2].Cell(7, 3).Range.Text = computer.DiskCapacity.ToString();
                                oDoc.Tables[i + 2].Rows[8].Delete();
                                oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.Text = "\n";
                            }
                        }
                    });
                });
            }
        }
    }
}
