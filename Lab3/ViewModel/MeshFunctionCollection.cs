using System;
using System.Windows.Input;
using Model;

namespace ViewModel
{ 
    public class MeshFunctionCollection
    {
        public ObservableModelData Collection { get; set; }

        public int selectedIndex { get; set; }

        private readonly ICommand deleteCommand;
        public ICommand DeleteCommand { get { return deleteCommand; } }

        private readonly ICommand newCommand;
        public ICommand NewCommand { get { return newCommand; } }

        private readonly ICommand openCommand;
        public ICommand OpenCommand { get { return openCommand; } }

        private readonly ICommand saveCommand;
        public ICommand SaveCommand { get { return saveCommand; } }

        private readonly ICommand addCommand;
        public ICommand AddCommand { get { return addCommand; } }

        private readonly ICommand drawCommand;
        public ICommand DrawCommand { get { return drawCommand; } }

        public bool ChangesUnsaved { get { return Collection.ChangesUnsaved; } }

        public MeshFunctionCollection(bool defaults = false)
        {
            Collection = new ObservableModelData();
            if (!defaults) Collection.Clear();
        }

        public MeshFunctionCollection(IUIServices uiServices)
        {
            Collection = new ObservableModelData();
            deleteCommand = new RelayCommand(
                _ => selectedIndex >= 0 && selectedIndex < Collection.Count,
                _ => { if (uiServices.ConfirmDelete()) Collection.RemoveAt(selectedIndex); }); ;
            
            newCommand = new RelayCommand(
                _ => true,
                _ =>
                {
                    if (ChangesUnsaved && uiServices.ConfirmSave()) {
                        string filename;
                        if ((filename = uiServices.SaveDialog()) != "")
                            if (!Save(filename))
                                throw new Exception("Error saving file");
                    }
                    Collection.Clear();
                    Collection.ChangesUnsaved = false;
                });

            openCommand = new RelayCommand(
                _ => true,
                _ =>
                {
                    string filename;
                    if (ChangesUnsaved && uiServices.ConfirmSave())
                        if ((filename = uiServices.SaveDialog()) != "")
                            if (!Save(filename))
                                throw new Exception("Error saving file");
                    if ((filename = uiServices.OpenDialog()) != "")
                        if (!Load(filename))
                            throw new Exception("Error opening file");
                    uiServices.UpdateBinding();
                });

            saveCommand = new RelayCommand(
                _ => ChangesUnsaved,
                _ =>
                {
                    string filename = uiServices.SaveDialog();
                    if (filename != "")
                        if (!Save(filename))
                            throw new Exception("Error saving file");
                });

            addCommand = new RelayCommand(
                _ => uiServices.NoErrorsAdd(),
                _ => uiServices.AddElement());

            drawCommand = new RelayCommand(
                _ => uiServices.NoErrorsDraw(),
                _ => uiServices.DrawElement());
        }

        public void RemoveAt(int index)
        {
            Collection.RemoveAt(index);
        }

        public void Add(MeshFunction mf)
        {
            Collection.Add(mf.toModelData());
        }

        public bool Save(string filename)
        {
            return SaveLoad.Save(filename, Collection);
        }

        public bool Load(string filename)
        {
            ObservableModelData loaded = new ObservableModelData();
            bool res = SaveLoad.Load(filename, ref loaded);
            Collection = loaded;
            Collection.CollectionChanged += Collection.updateFlag;
            return res;
        }
    }
}
