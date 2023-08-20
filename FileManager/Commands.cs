using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace FileManager
{
    static class Commands
    {
        public static RoutedUICommand NewDir = new RoutedUICommand("Nowy folder", "NewDir", typeof(Commands));
        public static RoutedUICommand NewFile = new RoutedUICommand("Nowy plik", "NewFile", typeof(Commands));
        public static RoutedUICommand ChangeName = new RoutedUICommand("Zmień nazwę", "ChangeName", typeof(Commands));
        public static RoutedUICommand Copy = new RoutedUICommand("Kopiuj", "Copy", typeof(Commands));
        public static RoutedUICommand Move = new RoutedUICommand("Przenieś", "Move", typeof(Commands));
        public static RoutedUICommand Delete = new RoutedUICommand("Usuń", "Delete", typeof(Commands));
        public static RoutedUICommand Run = new RoutedUICommand("Uruchom", "Run", typeof(Commands));
        public static RoutedUICommand SelectDrive = new RoutedUICommand("Wybierz dysk", "SelectDrive", typeof(Commands));
        public static RoutedUICommand GoFsLevelUp = new RoutedUICommand("Wstecz", "GoFsLevelUp", typeof(Commands));
        public static RoutedUICommand GoHomeDir = new RoutedUICommand("Folder domowy", "GoHomeDir", typeof(Commands));
        public static RoutedUICommand FastSearch = new RoutedUICommand("Szybkie wyszukiwanie", "FastSearch", typeof(Commands));
        public static RoutedUICommand Console = new RoutedUICommand("Konsola", "Console", typeof(Commands));
        public static RoutedUICommand GoTo = new RoutedUICommand("Idź do", "GoTo", typeof(Commands));

        public static void Initialize()
        {
            KeyGesture kg = new KeyGesture(Key.N, ModifierKeys.Control);
            NewDir.InputGestures.Add(kg);

            kg = new KeyGesture(Key.L, ModifierKeys.Control);
            NewFile.InputGestures.Add(kg);

            kg = new KeyGesture(Key.Z, ModifierKeys.Control);
            ChangeName.InputGestures.Add(kg);

            kg = new KeyGesture(Key.C, ModifierKeys.Control);
            Copy.InputGestures.Add(kg);

            kg = new KeyGesture(Key.V, ModifierKeys.Control);
            Move.InputGestures.Add(kg);

            kg = new KeyGesture(Key.D, ModifierKeys.Control);
            Delete.InputGestures.Add(kg);

            kg = new KeyGesture(Key.Enter);
            Run.InputGestures.Add(kg);

            kg = new KeyGesture(Key.P, ModifierKeys.Control);
            SelectDrive.InputGestures.Add(kg);

            kg = new KeyGesture(Key.Back);
            GoFsLevelUp.InputGestures.Add(kg);

            kg = new KeyGesture(Key.H, ModifierKeys.Control);
            GoHomeDir.InputGestures.Add(kg);

            kg = new KeyGesture(Key.S, ModifierKeys.Control);
            FastSearch.InputGestures.Add(kg);

            kg = new KeyGesture(Key.T, ModifierKeys.Control);
            Console.InputGestures.Add(kg);

            kg = new KeyGesture(Key.G, ModifierKeys.Control);
            GoTo.InputGestures.Add(kg);
        }
    }
}
