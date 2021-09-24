using KP_OOP.Models;
using KP_OOP.Views;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Windows.Media;

namespace KP_OOP.ViewModels
{
    class UserW_VM : INotifyPropertyChanged
    {
        private UserWindow thisWindow;
        private ObservableCollection<CREATURE_DICTIONARY> monsterLibraryElements;
        private ObservableCollection<WEAPON_DICTIONARY> weaponLibraryElements;
        private ObservableCollection<GUILD> guilds;
        private ObservableCollection<GUILD> guildsPlaceHolder;
        private ObservableCollection<USERS> users;
        private ObservableCollection<TASK> tasks;
        private ObservableCollection<TASK> tasksPlaceHolder;
        private ObservableCollection<USERS> thisGuildMembers;
        private ObservableCollection<USERS> notCommanderUsers;
        private ObservableCollection<TASK> thisUserTasks;
        private ObservableCollection<USERS> myGuildMembers;
        private ObservableCollection<USERS> thisTaskMembers;

        private GUILD myGuild;
        public GUILD MyGuild
        {
            get { return myGuild; }
            set
            {
                myGuild = value;
                OnPropertyChanged("MyGuild");
            }
        }

        private TASK currentTask;
        public TASK CurrentTask
        {
            get { return currentTask; }
            set
            {
                currentTask = value;
                OnPropertyChanged("CurrentTask");
            }
        }

        private decimal myMoney;
        public decimal MyMoney
        {
            get { return myMoney; }
            set
            {
                myMoney = value;
                OnPropertyChanged("MyMoney");
            }
        }

        private decimal partOfAward;
        public decimal PartOfAward
        {
            get { return partOfAward; }
            set
            {
                partOfAward = value;
                OnPropertyChanged("PartOfAward");
            }
        }

        private decimal thisTaskAward;
        public decimal ThisTaskAward
        {
            get { return thisTaskAward; }
            set
            {
                thisTaskAward = value;
                OnPropertyChanged("ThisTaskAward");
            }
        }

        private string thisTaskStartTime;
        public string ThisTaskStartTime
        {
            get { return thisTaskStartTime; }
            set
            {
                thisTaskStartTime = value;
                OnPropertyChanged("ThisTaskStartTime");
            }
        }

        //общие коллекции
        public ObservableCollection<PanelElement> PanelElements { get; set; }

        public ObservableCollection<CREATURE_DICTIONARY> MonsterLibraryElements
        {
            get { return monsterLibraryElements; }
            set
            {
                monsterLibraryElements = value;
                OnPropertyChanged("MonsterLibraryElements");
            }
        }
        public ObservableCollection<WEAPON_DICTIONARY> WeaponLibraryElements
        {
            get { return weaponLibraryElements; }
            set
            {
                weaponLibraryElements = value;
                OnPropertyChanged("WeaponLibraryElements");
            }
        }
        public ObservableCollection<GUILD> Guilds
        {
            get { return guilds; }
            set
            {
                guilds = value;
                OnPropertyChanged("Guilds");
            }
        }
        public ObservableCollection<USERS> ThisTaskMembers
        {
            get { return thisTaskMembers; }
            set
            {
                thisTaskMembers = value;
                OnPropertyChanged("ThisTaskMembers");
            }
        }

        public ObservableCollection<GUILD> GuildsPlaceHolder
        {
            get { return guildsPlaceHolder; }
            set
            {
                guildsPlaceHolder = value;
                OnPropertyChanged("GuildsPlaceHolder");
            }
        }
        public ObservableCollection<USERS> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }
        public ObservableCollection<TASK> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged("Tasks");
            }
        }

        public ObservableCollection<TASK> TasksPlaceHolder
        {
            get { return tasksPlaceHolder; }
            set
            {
                tasksPlaceHolder = value;
                OnPropertyChanged("TasksPlaceHolder");
            }
        }

        //локальные коллекции для вкладок
        public ObservableCollection<USERS> ThisGuildMembers
        {
            get { return thisGuildMembers; }
            set
            {
                thisGuildMembers = value;
                OnPropertyChanged("ThisGuildMembers");
            }
        }
        public ObservableCollection<USERS> NotCommanderUsers
        {
            get { return notCommanderUsers; }
            set
            {
                notCommanderUsers = value;
                OnPropertyChanged("NotCommanderUsers");
            }
        }
        public ObservableCollection<USERS> MyGuildMembers
        {
            get { return myGuildMembers; }
            set
            {
                myGuildMembers = value;
                OnPropertyChanged("MyGuildMembers");
            }
        }
        public ObservableCollection<TASK> ThisUserTasks
        {
            get { return thisUserTasks; }
            set
            {
                thisUserTasks = value;
                OnPropertyChanged("ThisUserTasks");
            }
        }


        public UserW_VM(UserWindow mw)
        {
            thisWindow = mw;

            PanelElements = new ObservableCollection<PanelElement>()
            {
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/current_task.png", "Текущее задание"),
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/users.png", "Моя гильдия"),
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/monster.png", "Бестиарий"),
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/arsenal.png", "Арсенал")
            };

            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
            {
                try
                {
                    MonsterLibraryElements = new ObservableCollection<CREATURE_DICTIONARY>(inq.CREATURE_DICTIONARY);
                    WeaponLibraryElements = new ObservableCollection<WEAPON_DICTIONARY>(inq.WEAPON_DICTIONARY);
                    Guilds = new ObservableCollection<GUILD>(inq.GUILD);
                    GuildsPlaceHolder = new ObservableCollection<GUILD>(Guilds);
                    Users = new ObservableCollection<USERS>(inq.USERS);
                    Tasks = new ObservableCollection<TASK>(inq.TASK);
                    TasksPlaceHolder = new ObservableCollection<TASK>(Tasks);



                    var ncu = from u in inq.USERS
                              where u.US_TYPE_NAME == "member"
                              select u;

                    NotCommanderUsers = new ObservableCollection<USERS>(ncu);

                    var mgm = from u in inq.USERS
                              where u.US_GUILD_ID == CurrentUser.GuildId && u.US_GUILD_ID != 0
                              select u;

                    if (mgm.Count() > 0)
                    {
                        MyGuildMembers = new ObservableCollection<USERS>(mgm);
                    }
                    else
                    {
                        MyGuildMembers = new ObservableCollection<USERS>();
                    }

                    /*var mg = from g in inq.GUILD
                             where g.GUILD_ID == CurrentUser.GuildId
                             select g;
                   
                    if (mg.Count() > 0)
                    {
                        MyGuild = mg.First();
                    }
                    else
                    {
                        MyGuild = null;
                    }*/

                    MyGuild = inq.GUILD.Find(CurrentUser.GuildId);
                    MyMoney = CurrentUser.Balance;

                    var ct = from t in inq.TASK
                             join tm in inq.TASK_MEMBER on t.TASK_ID equals tm.TASK_ID
                             where tm.TASK_USER_ID == CurrentUser.Id && t.IS_TASK_RUNNING == true
                             select t;

                    if (ct.Count() > 0)
                    {
                        CurrentTask = ct.First();
                    }
                    else
                    {
                        CurrentTask = null;
                    }

                    if (CurrentTask != null)
                    {
                        RefreshCurrentTask();
                    }

                    ThisGuildMembers = new ObservableCollection<USERS>(inq.GUILD.Find(CurrentUser.GuildId).USERS);
                }
                catch (Exception e)
                {
                    new Warning(e.Message).Show();
                }

            }
        }

        private PanelElement selectedPanel;
        public PanelElement SelectedPanel
        {
            get { return selectedPanel; }
            set
            {
                selectedPanel = value;
                OnPropertyChanged("SelectedPanel");
            }
        }

        private CREATURE_DICTIONARY selectedMLibraryElem;
        public CREATURE_DICTIONARY SelectedMLibraryElem
        {
            get { return selectedMLibraryElem; }
            set
            {
                selectedMLibraryElem = value;
                OnPropertyChanged("SelectedMLibraryElem");
            }
        }

        private WEAPON_DICTIONARY selectedWLibraryElem;
        public WEAPON_DICTIONARY SelectedWLibraryElem
        {
            get { return selectedWLibraryElem; }
            set
            {
                selectedWLibraryElem = value;
                OnPropertyChanged("SelectedWLibraryElem");
            }
        }

        private GUILD selectedGuildAC;
        public GUILD SelectedGuildAC
        {
            get { return selectedGuildAC; }
            set
            {
                selectedGuildAC = value;
                OnPropertyChanged("SelectedGuildAC");
            }
        }

        private USERS selectedMyGuildUser;
        public USERS SelectedMyGuildUser
        {
            get { return selectedMyGuildUser; }
            set
            {
                selectedMyGuildUser = value;
                OnPropertyChanged("SelectedMyGuildUser");
            }
        }

        private USERS selectedUserAC;
        public USERS SelectedUserAC
        {
            get { return selectedUserAC; }
            set
            {
                selectedUserAC = value;
                OnPropertyChanged("SelectedUserAC");
            }
        }

        private USERS selectedUser;
        public USERS SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        private ObservableCollection<USERS> selectedTaskMembers;
        public ObservableCollection<USERS> SelectedTaskMembers
        {
            get { return selectedTaskMembers; }
            set
            {
                selectedTaskMembers = value;
                OnPropertyChanged("SelectedTaskMembers");
            }

        }

        private string selectedGuildImagePath;
        public string SelectedGuildImagePath
        {
            get { return selectedGuildImagePath; }
            set
            {
                Regex regexJPG = new Regex(@"\w*jpg");
                Regex regexPNG = new Regex(@"\w*png");

                MatchCollection matchesJPG = regexJPG.Matches(value);
                MatchCollection matchesPNG = regexPNG.Matches(value);

                if (matchesJPG.Count == 0 && matchesPNG.Count == 0)
                {
                    /*thisWindow.ErrorFieldAddGuild.Text = "Неверный формат файла";
                    thisWindow.ErrorFieldAddGuild.Foreground = Brushes.Red;*/
                }
                else
                {
                    selectedGuildImagePath = String.Format(@"{0}", value);
                    OnPropertyChanged("SelectedGuildImagePath");
                }
            }
        }

        private UserCommand selectFileHandler;
        public UserCommand SelectFileHandler
        {
            get
            {
                return selectFileHandler ??
                (selectFileHandler = new UserCommand
                    (
                        obj =>
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
                            if (openFileDialog.ShowDialog() == true)
                                SelectedGuildImagePath = Path.GetFullPath(openFileDialog.FileName);
                        }
                    )
                );
            }
        }

        private string selectedMonsterImagePath;
        public string SelectedMonsterImagePath
        {
            get { return selectedMonsterImagePath; }
            set
            {
                Regex regexJPG = new Regex(@"\w*jpg");
                Regex regexPNG = new Regex(@"\w*png");

                MatchCollection matchesJPG = regexJPG.Matches(value);
                MatchCollection matchesPNG = regexPNG.Matches(value);

                if (matchesJPG.Count == 0 && matchesPNG.Count == 0)
                {
                    /*thisWindow.ErrorFieldAddGuild.Text = "Неверный формат файла";
                    thisWindow.ErrorFieldAddGuild.Foreground = Brushes.Red;*/
                }
                else
                {
                    selectedMonsterImagePath = String.Format(@"{0}", value);
                    OnPropertyChanged("SelectedMonsterImagePath");
                }
            }
        }

        private UserCommand selectFileMLHandler;
        public UserCommand SelectFileMLHandler
        {
            get
            {
                return selectFileMLHandler ??
                (selectFileMLHandler = new UserCommand
                    (
                        obj =>
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
                            if (openFileDialog.ShowDialog() == true)
                                SelectedMonsterImagePath = Path.GetFullPath(openFileDialog.FileName);
                        }
                    )
                );
            }
        }

        private string selectedWeaponImagePath;
        public string SelectedWeaponImagePath
        {
            get { return selectedWeaponImagePath; }
            set
            {
                Regex regexJPG = new Regex(@"\w*jpg");
                Regex regexPNG = new Regex(@"\w*png");

                MatchCollection matchesJPG = regexJPG.Matches(value);
                MatchCollection matchesPNG = regexPNG.Matches(value);

                if (matchesJPG.Count == 0 && matchesPNG.Count == 0)
                {
                    /*thisWindow.ErrorFieldAddGuild.Text = "Неверный формат файла";
                    thisWindow.ErrorFieldAddGuild.Foreground = Brushes.Red;*/
                }
                else
                {
                    selectedWeaponImagePath = String.Format(@"{0}", value);
                    OnPropertyChanged("SelectedWeaponImagePath");
                }
            }
        }

        private CREATURE_DICTIONARY selectedMonster;
        public CREATURE_DICTIONARY SelectedMonster
        {
            get { return selectedMonster; }
            set
            {
                selectedMonster = value;
                OnPropertyChanged("SelectedMonster");
            }
        }

        private UserCommand selectFileWPHandler;
        public UserCommand SelectFileWPHandler
        {
            get
            {
                return selectFileWPHandler ??
                (selectFileWPHandler = new UserCommand
                    (
                        obj =>
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg";
                            if (openFileDialog.ShowDialog() == true)
                                SelectedWeaponImagePath = Path.GetFullPath(openFileDialog.FileName);
                        }
                    )
                );
            }
        }



        private UserCommand selectionChangedCommand;
        public UserCommand SelectionChangedCommand
        {
            get
            {
                return selectionChangedCommand ??
                (selectionChangedCommand = new UserCommand
                    (
                        obj =>
                        {
                            string tmp = String.Format(@"{0}", SelectedPanel.Path);

                            DoubleAnimation ShowIcon = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(700), TimeSpan.FromMilliseconds(600));
                            DoubleAnimation Hide = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(700), TimeSpan.FromMilliseconds(300));

                            switch (SelectedPanel.Text)
                            {
                                case "Бестиарий":

                                    ShowNewGrid(thisWindow.MonsterLibraryView);
                                    ShowGrid(thisWindow.LeftTopSortGrid);
                                    break;

                                case "Арсенал":

                                    ShowNewGrid(thisWindow.WeaponLibraryView);
                                    ShowGrid(thisWindow.LeftTopSortGrid);
                                    break;

                                case "Все инквизиторы":

                                    ShowNewGrid(thisWindow.UserStats);
                                    break;

                                case "Создать задание":

                                    ShowNewGrid(thisWindow.TaskAdding);
                                    break;

                                case "Текущее задание":

                                    RefreshCurrentTask();
                                    ShowNewGrid(thisWindow.CurrentTask);
                                    break;


                                case "Моя гильдия":

                                    RefreshCurrentTask();
                                    ShowNewGrid(thisWindow.CurrentGuild);
                                    break;

                                default:
                                    break;

                            }

                            thisWindow.MenuPanels.IsHitTestVisible = false;
                            thisWindow.MenuPanels.BeginAnimation(Grid.OpacityProperty, Hide);
                            thisWindow.LeftTopImage.Source = new BitmapImage(new Uri(tmp));
                            thisWindow.LeftTopImageGrid.BeginAnimation(Grid.OpacityProperty, ShowIcon);
                        }
                    )
                );
            }
        }

        private UserCommand showHideSortOptions;
        public UserCommand ShowHideSortOptions
        {
            get
            {
                return showHideSortOptions ??
                (showHideSortOptions = new UserCommand
                    (
                        obj =>
                        {
                            if (thisWindow.SortOptions.Opacity > 0)
                            {
                                DoubleAnimation ShowGrid = AnimationFunctions.ReturnNewDoubleAnimationWOBT(thisWindow.SortOptions.Opacity, 0, TimeSpan.FromMilliseconds(700));
                                thisWindow.SortOptions.BeginAnimation(Grid.OpacityProperty, ShowGrid);
                            }
                            else
                            {
                                DoubleAnimation ShowGrid = AnimationFunctions.ReturnNewDoubleAnimationWOBT(0, 1, TimeSpan.FromMilliseconds(700));
                                thisWindow.SortOptions.BeginAnimation(Grid.OpacityProperty, ShowGrid);
                            }
                        }
                    )
                );
            }
        }

        private UserCommand userStatsSelectionChanged;
        public UserCommand UserStatsSelectionChanged
        {
            get
            {
                return userStatsSelectionChanged ??
                (userStatsSelectionChanged = new UserCommand
                    (
                        obj =>
                        {
                            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                            {
                                try
                                {
                                    //var tmp_tasks = from t in inq.TASK
                                    //                join tm in inq.TASK_MEMBER on t.TASK_ID equals tm.TASK_ID
                                    //                where tm.TASK_USER_ID == SelectedUser.US_ID
                                    //                select t;

                                    if (SelectedUser != null)
                                    {
                                        var tmp_tasks = from t in inq.USERS.Find(SelectedUser.US_ID).TASK_MEMBER
                                                        select t.TASK;

                                        if (tmp_tasks.Count() > 0)
                                        {
                                            ThisUserTasks = new ObservableCollection<TASK>(tmp_tasks);
                                        }
                                        else
                                        {
                                            ThisUserTasks = new ObservableCollection<TASK>();
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    new Warning(e.Message).Show();
                                }
                            }
                        }
                    )
                );
            }
        }


        private UserCommand addMonsterCommand;
        public UserCommand AddMonsterCommand
        {
            get
            {
                return addMonsterCommand ??
                (addMonsterCommand = new UserCommand
                    (
                        obj =>
                        {
                            string patternText = @"\w+";
                            string patternNumber = @"\d+";
                            string patternType = "fly|run|swim|reptile|beatle|flyfish|hybrid";

                            try
                            {
                                if (Regex.IsMatch(thisWindow.AddMonsterName.Text, patternText, RegexOptions.IgnoreCase))
                                {
                                    if (Regex.IsMatch(thisWindow.AddMonsterDescription.Text, patternText, RegexOptions.IgnoreCase))
                                    {
                                        if (Regex.IsMatch(thisWindow.AddMonsterArmour.Text, patternNumber, RegexOptions.IgnoreCase) && Convert.ToInt16(thisWindow.AddMonsterDamage.Text) < 100 && Convert.ToInt16(thisWindow.AddMonsterDamage.Text) > 0)
                                        {
                                            if (Regex.IsMatch(thisWindow.AddMonsterDamage.Text, patternNumber, RegexOptions.IgnoreCase) && Convert.ToInt32(thisWindow.AddMonsterArmour.Text) < 100 && Convert.ToInt32(thisWindow.AddMonsterArmour.Text) > 0)
                                            {
                                                if (Regex.IsMatch(thisWindow.AddMonsterType.Text, patternType, RegexOptions.IgnoreCase))
                                                {
                                                    if (thisWindow.PreviewMonsterIcon.Source.ToString() != "")
                                                    {

                                                        CREATURE_DICTIONARY m = new CREATURE_DICTIONARY
                                                        {
                                                            CR_DANGER_LVL = Convert.ToInt16(thisWindow.AddMonsterDamage.Text),
                                                            CR_ARMOUR = Convert.ToInt32(thisWindow.AddMonsterArmour.Text),
                                                            CR_NAME = thisWindow.AddMonsterName.Text,
                                                            CR_DESCRIPTION = thisWindow.AddMonsterDescription.Text,
                                                            CR_MOVEMENT_TYPE = thisWindow.AddMonsterType.Text,
                                                            CR_IMAGE = String.Format(@"{0}", thisWindow.PreviewMonsterIcon.Source)
                                                        };

                                                        using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                                        {
                                                            inq.CREATURE_DICTIONARY.Add(m);
                                                            inq.SaveChanges();
                                                            MonsterLibraryElements = new ObservableCollection<CREATURE_DICTIONARY>(inq.CREATURE_DICTIONARY);
                                                        }
                                                        new Warning("Запись успешно добавлена. Обновите бестиарий").Show();

                                                    }
                                                    else
                                                    {
                                                        new Warning("Ошбика: \nВыберите изображение").Show();
                                                    }
                                                }
                                                else
                                                {
                                                    new Warning("Ошбика: \nНекорректно введен тип. Обратите внимание на подсказку").Show();
                                                }
                                            }
                                            else
                                            {
                                                new Warning("Ошбика: \nНекорректно введен урон. Должно быть формата \'число\'").Show();
                                            }
                                        }
                                        else
                                        {
                                            new Warning("Ошбика: \nНекорректно введена броня. Должно быть формата \'число.число\'").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошбика: \nНекорректно введено описание").Show();
                                    }
                                }
                                else
                                {
                                    new Warning("Ошбика: \nНекорректно введено название").Show();
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand removeMonster;
        public UserCommand RemoveMonster
        {
            get
            {
                return removeMonster ??
                (removeMonster = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    var tmp_tasks = from t in inq.TASK_CREATURES
                                                    where t.CREATURE_ID == SelectedMLibraryElem.CR_ID
                                                    select t;

                                    if (tmp_tasks.Count() < 1)
                                    {
                                        if (inq.CREATURE_DICTIONARY.Find(SelectedMLibraryElem.CR_ID) != null)
                                        {
                                            var tmp_monster = (from m in inq.CREATURE_DICTIONARY
                                                               where m.CR_ID == SelectedMLibraryElem.CR_ID
                                                               select m).First();


                                            inq.CREATURE_DICTIONARY.Remove(tmp_monster);
                                            inq.SaveChanges();
                                            MonsterLibraryElements = new ObservableCollection<CREATURE_DICTIONARY>(inq.CREATURE_DICTIONARY);
                                            new Warning("Удалено успешно").Show();
                                        }
                                        else
                                        {
                                            new Warning("Ошибка: \nЗааись уже была удалена").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nНевозможно удалить монстра, так как он уже однажды использовался или используется").Show();
                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand addWeaponCommand;
        public UserCommand AddWeaponCommand
        {
            get
            {
                return addWeaponCommand ??
                (addWeaponCommand = new UserCommand
                    (
                        obj =>
                        {
                            string patternText = @"\w+";
                            string patternNumber = @"\d+";
                            string patternType = "bow|axe|sword|dagger|crossbow";

                            try
                            {
                                if (Regex.IsMatch(thisWindow.AddWeaponName.Text, patternText, RegexOptions.IgnoreCase))
                                {
                                    if (Regex.IsMatch(thisWindow.AddWeaponDescription.Text, patternText, RegexOptions.IgnoreCase))
                                    {
                                        if (Regex.IsMatch(thisWindow.AddWeaponWeight.Text, patternNumber, RegexOptions.IgnoreCase) && Convert.ToInt32(thisWindow.AddWeaponWeight.Text) > 0 && Convert.ToInt32(thisWindow.AddWeaponWeight.Text) < 100)
                                        {
                                            if (Regex.IsMatch(thisWindow.AddWeaponDamage.Text, patternNumber, RegexOptions.IgnoreCase) && Convert.ToInt16(thisWindow.AddWeaponDamage.Text) < 100 && Convert.ToInt16(thisWindow.AddWeaponDamage.Text) > 0)
                                            {
                                                if (Regex.IsMatch(thisWindow.AddWeaponType.Text, patternType, RegexOptions.IgnoreCase))
                                                {
                                                    if (thisWindow.PreviewWeaponIcon.Source.ToString() != "")
                                                    {

                                                        WEAPON_DICTIONARY w = new WEAPON_DICTIONARY
                                                        {
                                                            WEAPON_MORTALITY = Convert.ToInt16(thisWindow.AddWeaponDamage.Text),
                                                            WEAPON_WEIGHT = Convert.ToInt32(thisWindow.AddWeaponWeight.Text),
                                                            WEAPON_NAME = thisWindow.AddWeaponName.Text,
                                                            WEAPON_DESCRIPTION = thisWindow.AddWeaponDescription.Text,
                                                            WEAPON_TYPE_NAME = thisWindow.AddWeaponType.Text,
                                                            WEAPON_IMAGE = String.Format(@"{0}", thisWindow.PreviewWeaponIcon.Source),
                                                            WEAPON_VALUE = CountValue(Convert.ToInt16(thisWindow.AddWeaponDamage.Text), Convert.ToInt32(thisWindow.AddWeaponWeight.Text))
                                                        };

                                                        using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                                        {
                                                            inq.WEAPON_DICTIONARY.Add(w);
                                                            inq.SaveChanges();
                                                            WeaponLibraryElements = new ObservableCollection<WEAPON_DICTIONARY>(inq.WEAPON_DICTIONARY);
                                                        }
                                                        new Warning("Запись успешно добавлена. Обновите арсенал").Show();

                                                    }
                                                    else
                                                    {
                                                        new Warning("Ошбика: \nВыберите изображение").Show();
                                                    }
                                                }
                                                else
                                                {
                                                    new Warning("Ошбика: \nНекорректно введен тип. Обратите внимание на подсказку").Show();
                                                }
                                            }
                                            else
                                            {
                                                new Warning("Ошбика: \nНекорректно введен урон. Должно быть формата \'число\'").Show();
                                            }
                                        }
                                        else
                                        {
                                            new Warning("Ошбика: \nНекорректно введен вес. Должно быть формата \'число.число\'").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошбика: \nНекорректно введено описание").Show();
                                    }
                                }
                                else
                                {
                                    new Warning("Ошбика: \nНекорректно введено название").Show();
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand removeWeapon;
        public UserCommand RemoveWeapon
        {
            get
            {
                return removeWeapon ??
                (removeWeapon = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    var tmp_weapons = from w in inq.USER_ARSENAL
                                                      where w.WEAPON_ID == SelectedWLibraryElem.WEAPON_ID
                                                      select w;

                                    if (tmp_weapons.Count() < 1)
                                    {
                                        if (inq.WEAPON_DICTIONARY.Find(SelectedWLibraryElem.WEAPON_ID) != null)
                                        {
                                            var tmp_weapon = (from w in inq.WEAPON_DICTIONARY
                                                              where w.WEAPON_ID == SelectedWLibraryElem.WEAPON_ID
                                                              select w).First();


                                            inq.WEAPON_DICTIONARY.Remove(tmp_weapon);
                                            inq.SaveChanges();
                                            WeaponLibraryElements = new ObservableCollection<WEAPON_DICTIONARY>(inq.WEAPON_DICTIONARY);
                                            new Warning("Удалено успешно").Show();
                                        }
                                        else
                                        {
                                            new Warning("Ошибка: \nЗааись уже была удалена").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nНевозможно удалить оружие, так как оно уже однажды использовалось или используется").Show();
                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand addGuildMember;
        public UserCommand AddGuildMember
        {
            get
            {
                return addGuildMember ??
                (addGuildMember = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    if (SelectedUser.US_TYPE_NAME == "member")
                                    {
                                        if (SelectedUser.US_GUILD_ID == null)
                                        {
                                            var tmp_user = (from u in inq.USERS
                                                            where u.US_ID == SelectedUser.US_ID
                                                            select u).First();

                                            tmp_user.US_GUILD_ID = CurrentUser.GuildId;
                                            inq.SaveChanges();

                                            var mgm = from u in inq.USERS
                                                      where u.US_GUILD_ID == CurrentUser.GuildId && u.US_GUILD_ID != 0
                                                      select u;

                                            if (mgm.Count() > 0)
                                            {
                                                MyGuildMembers = new ObservableCollection<USERS>(mgm);
                                            }
                                            else
                                            {
                                                MyGuildMembers = new ObservableCollection<USERS>();
                                            }

                                            Users = new ObservableCollection<USERS>(inq.USERS);
                                            new Warning("Пользователь успешно добавлен").Show();
                                        }
                                        else
                                        {
                                            new Warning("Ошибка: \nНевозможно добавить пользователя, который уже состоит в гильдии").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nНевозможно добавить в гильдию главу другой гильдии или главу инквизиции").Show();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand removeGuildMember;
        public UserCommand RemoveGuildMember
        {
            get
            {
                return removeGuildMember ??
                (removeGuildMember = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    if (SelectedMyGuildUser.US_TYPE_NAME == "member")
                                    {

                                        var tmp_user = (from u in inq.USERS
                                                        where u.US_ID == SelectedMyGuildUser.US_ID
                                                        select u).First();

                                        tmp_user.US_GUILD_ID = null;
                                        inq.SaveChanges();

                                        var mgm = from u in inq.USERS
                                                  where u.US_GUILD_ID == CurrentUser.GuildId && u.US_GUILD_ID != 0
                                                  select u;

                                        if (mgm.Count() > 0)
                                        {
                                            MyGuildMembers = new ObservableCollection<USERS>(mgm);
                                        }
                                        else
                                        {
                                            MyGuildMembers = new ObservableCollection<USERS>();
                                        }

                                        Users = new ObservableCollection<USERS>(inq.USERS);
                                        new Warning("Пользователь успешно исключен").Show();
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nНевозможно исключить самого себя").Show();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand addTask;
        public UserCommand AddTask
        {
            get
            {
                return addTask ??
                (addTask = new UserCommand
                    (
                        obj =>
                        {

                            string patternText = @"\w+";
                            string patternNumber = @"\d+";

                            if (Regex.IsMatch(thisWindow.TaskAddingName.Text, patternText, RegexOptions.IgnoreCase))
                            {
                                if (Regex.IsMatch(thisWindow.TaskAddingDescription.Text, patternText, RegexOptions.IgnoreCase))
                                {
                                    if (Regex.IsMatch(thisWindow.TaskAddingMoney.Text, patternNumber, RegexOptions.IgnoreCase))
                                    {
                                        if (thisWindow.TaskAddingSelectedMembers.SelectedItem != null)
                                        {
                                            if (thisWindow.TaskAddingMonster.SelectedItem != null)
                                            {
                                                int tmp_task_id = -1;

                                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                                {

                                                    bool isAllowedToStart = true;

                                                    foreach (var u in thisWindow.TaskAddingSelectedMembers.SelectedItems)
                                                    {
                                                        USERS user = u as USERS;

                                                        if (user != null)
                                                        {
                                                            USERS tmp_user = (from us in inq.USERS
                                                                              where user.US_NAME == us.US_NAME
                                                                              select us).First();

                                                            foreach (TASK_MEMBER tskm in tmp_user.TASK_MEMBER)
                                                            {
                                                                if (tskm.TASK.IS_TASK_RUNNING == true)
                                                                {
                                                                    isAllowedToStart = false;
                                                                    break;
                                                                }
                                                            }
                                                            if (!isAllowedToStart)
                                                                break;
                                                        }
                                                        else
                                                        {
                                                            new Warning("Ошибка:\n Твой метод говно, ошибка преобразования").Show();
                                                        }
                                                    }


                                                    if (isAllowedToStart)
                                                    {
                                                        System.DateTime dtnow = System.DateTime.Now;
                                                        TASK tmp_task = new TASK
                                                        {
                                                            TASK_COLOR = Colors.Yellow,
                                                            TASK_NAME = thisWindow.TaskAddingName.Text,
                                                            TASK_DESCRIPTION = thisWindow.TaskAddingDescription.Text,
                                                            TASK_START = dtnow.ToString(),
                                                            TASK_END = new DateTime().ToString(),
                                                            TASK_PROFIT = Convert.ToDecimal(thisWindow.TaskAddingMoney.Text),
                                                            TASK_GUILD_ID = CurrentUser.GuildId,
                                                            IS_TASK_RUNNING = true
                                                        };

                                                        ThisTaskAward = Convert.ToDecimal(thisWindow.TaskAddingMoney.Text);
                                                        inq.TASK.Add(tmp_task);
                                                        inq.SaveChanges();

                                                        TASK tmp_task_1 = (from t in inq.TASK
                                                                           where t.TASK_NAME == tmp_task.TASK_NAME
                                                                           select t).First();

                                                        tmp_task_id = tmp_task_1.TASK_ID;

                                                        foreach (var u in thisWindow.TaskAddingSelectedMembers.SelectedItems)
                                                        {
                                                            USERS user = u as USERS;

                                                            if (user != null)
                                                            {
                                                                USERS tmp_user = (from us in inq.USERS
                                                                                  where user.US_NAME == us.US_NAME
                                                                                  select us).First();

                                                                TASK_MEMBER tm = new TASK_MEMBER
                                                                {
                                                                    TASK_USER_ID = tmp_user.US_ID,
                                                                    TASK_ID = tmp_task.TASK_ID
                                                                };
                                                                inq.TASK_MEMBER.Add(tm);
                                                                inq.SaveChanges();
                                                            }
                                                            else
                                                            {
                                                                new Warning("Ошибка:\n Твой метод говно, ошибка преобразования").Show();
                                                            }
                                                        }

                                                        CREATURE_DICTIONARY cr = thisWindow.TaskAddingMonster.SelectedItem as CREATURE_DICTIONARY;

                                                        if (cr != null)
                                                        {
                                                            TASK_CREATURES tc = new TASK_CREATURES
                                                            {
                                                                TASK_ID = tmp_task.TASK_ID,
                                                                CREATURE_ID = (from c in inq.CREATURE_DICTIONARY
                                                                               where cr.CR_NAME == c.CR_NAME
                                                                               select c).First().CR_ID
                                                            };

                                                            inq.TASK_CREATURES.Add(tc);
                                                            inq.SaveChanges();
                                                        }
                                                        else
                                                        {
                                                            new Warning("Ошибка:\n Твой метод говно, очередная ошибка преобразования").Show();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        new Warning("Ошибка:\n Не удалось инициализировать задание, т.к. один из выбранных пользователей в данный момент уже выполняет задание.").Show();
                                                    }
                                                };

                                                if (tmp_task_id != -1)
                                                {
                                                    RefreshCurrentTask();
                                                    new Warning("Задание успешно инициализировано").Show();
                                                }

                                            }
                                            else
                                            {
                                                new Warning("Ошибка:\n Выберите цель вашей охоты").Show();
                                            }
                                        }
                                        else
                                        {
                                            new Warning("Ошибка:\n Выберите хотя бы одного согильдейца").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка:\n Введите размер награды").Show();
                                    }
                                }
                                else
                                {
                                    new Warning("Ошибка:\n Введите описание").Show();
                                }
                            }
                            else
                            {
                                new Warning("Ошибка:\n Введите название").Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand interruptTask;
        public UserCommand InterruptTask
        {
            get
            {
                return interruptTask ??
                (interruptTask = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    if (CurrentTask != null)
                                    {
                                        TASK tmp_task = inq.TASK.Find(CurrentTask.TASK_ID);
                                        tmp_task.IS_TASK_RUNNING = false;
                                        inq.SaveChanges();
                                        new Warning("Задание успешно прервано").Show();
                                        RefreshCurrentTask();
                                    }
                                    else
                                    {
                                        new Warning("Ошибка:\nНевозможно прервать несуществующее задание").Show();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand finishTask;
        public UserCommand FinishTask
        {
            get
            {
                return finishTask ??
                (finishTask = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    if (CurrentTask != null)
                                    {
                                        ObservableCollection<USERS> tmp_col = new ObservableCollection<USERS>();

                                        foreach (var tm in inq.TASK_MEMBER)
                                        {
                                            if (tm.TASK_ID == CurrentTask.TASK_ID && tm.TASK.IS_TASK_RUNNING == true)
                                            {
                                                USERS u = inq.USERS.Find(tm.TASK_USER_ID);
                                                u.US_BALANCE = u.US_BALANCE + (decimal)tm.TASK_USER_PROFIT;
                                                inq.SaveChangesAsync();

                                            }
                                        }

                                        GUILD g = inq.GUILD.Find(CurrentUser.GuildId);
                                        g.GUILD_STAT = g.GUILD_STAT + 1;
                                        g.GUILD_BALANCE = g.GUILD_BALANCE + (decimal)CurrentTask.TASK_PROFIT;
                                        inq.SaveChanges();

                                        TASK t = inq.TASK.Find(CurrentTask.TASK_ID);
                                        t.IS_TASK_RUNNING = false;
                                        inq.SaveChanges();

                                        CurrentTask.TASK_END = DateTime.Now.ToString();

                                        new Warning("Задание успешно завершено").Show();
                                        RefreshCurrentTask();
                                    }
                                    else
                                    {
                                        new Warning("Ошибка:\nНевозможно закончить несуществующее задание").Show();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand changeUser;
        public UserCommand ChangeUser
        {
            get
            {
                return changeUser ??
                (changeUser = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {

                                MainWindow mw = new MainWindow();
                                mw.Show();
                                thisWindow.Close();
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand purchase;
        public UserCommand Purchase
        {
            get
            {
                return purchase ??
                (purchase = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    if (SelectedWLibraryElem.WEAPON_VALUE < CurrentUser.Balance)
                                    {
                                        CurrentUser.Balance -= SelectedWLibraryElem.WEAPON_VALUE;
                                        USERS u = inq.USERS.Find(CurrentUser.Id);
                                        u.US_BALANCE = CurrentUser.Balance;
                                        inq.SaveChangesAsync();
                                        USER_ARSENAL ua = new USER_ARSENAL
                                        {
                                            US_ID = CurrentUser.Id,
                                            WEAPON_ID = SelectedWLibraryElem.WEAPON_ID
                                        };
                                        inq.USER_ARSENAL.Add(ua);
                                        inq.SaveChangesAsync();

                                        new Warning("Покупка совершена успешно").Show();
                                    }
                                    else
                                    {
                                        new Warning("Ошибка:\n Недостаточно средств").Show();
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private bool ShowNewGrid(Grid grid)
        {
            if (grid != null)
            {
                Thickness to, from;

                to = new Thickness();
                to.Left = 230; to.Top = 0; to.Right = 0; to.Bottom = 0;

                from = new Thickness();
                from.Left = 1922; from.Top = 0; from.Right = -1692; from.Bottom = 0;

                ThicknessAnimation SlideGrid = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(700));
                DoubleAnimation ShowGrid = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(700));

                grid.BeginAnimation(Grid.MarginProperty, SlideGrid);
                grid.BeginAnimation(Grid.OpacityProperty, ShowGrid);

                return true;
            }
            else
            {
                return false;
            }

        }

        private bool ShowGrid(Grid grid)
        {
            if (grid != null)
            {
                DoubleAnimation ShowGrid = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(700), TimeSpan.FromMilliseconds(610));
                grid.BeginAnimation(Grid.OpacityProperty, ShowGrid);

                return true;
            }
            else
            {
                return false;
            }
        }

        private decimal CountValue(Int16 damage, Int32 weight)
        {
            return Math.Round((decimal)(100 - 0.8 * weight + 1.5 * damage), 2);
        }

        private void RecountShare(int task_id)
        {
            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
            {
                decimal GeneralPowerValue = 1;

                foreach (TASK_MEMBER tm in from tm in inq.TASK_MEMBER
                                           where tm.TASK_ID == task_id
                                           select tm)
                {
                    USERS us = inq.USERS.Find(tm.TASK_USER_ID);

                    var tmp_userarsenal = from ua in inq.USER_ARSENAL
                                          where ua.US_ID == us.US_ID
                                          select ua;

                    if (tmp_userarsenal.Count() != 0)
                    {
                        foreach (USER_ARSENAL w in tmp_userarsenal)
                        {
                            GeneralPowerValue += CountValue(Convert.ToInt16(inq.WEAPON_DICTIONARY.Find(w.WEAPON_ID).WEAPON_MORTALITY), Convert.ToInt32(inq.WEAPON_DICTIONARY.Find(w.WEAPON_ID).WEAPON_WEIGHT));
                        }
                    }
                }

                foreach (TASK_MEMBER tm in from tm in inq.TASK_MEMBER
                                           where tm.TASK_ID == task_id
                                           select tm)
                {
                    USERS us = inq.USERS.Find(tm.TASK_USER_ID);
                    decimal LocalPowerValue = 0;

                    var tmp_userarsenal = from ua in inq.USER_ARSENAL
                                          where ua.US_ID == us.US_ID
                                          select ua;

                    if (tmp_userarsenal.Count() != 0)
                    {
                        foreach (USER_ARSENAL w in from ua in inq.USER_ARSENAL
                                                   where ua.US_ID == us.US_ID
                                                   select ua)
                        {
                            LocalPowerValue += CountValue(Convert.ToInt16(inq.WEAPON_DICTIONARY.Find(w.WEAPON_ID).WEAPON_MORTALITY), Convert.ToInt32(inq.WEAPON_DICTIONARY.Find(w.WEAPON_ID).WEAPON_WEIGHT));
                        }
                    }

                    double precent = Math.Round((double)(LocalPowerValue / GeneralPowerValue) * 100, 2);

                    TASK_MEMBER tmentry = (from tme in inq.TASK_MEMBER
                                           where tme.TASK_ID == task_id && tme.TASK_USER_ID == us.US_ID
                                           select tme).First();

                    decimal profit = (decimal)(inq.TASK.Find(task_id).TASK_PROFIT * (decimal)precent / 100);
                    if (profit == 0)
                    {
                        tmentry.TASK_USER_PROFIT = 50;
                    }
                    else
                    {
                        tmentry.TASK_USER_PROFIT = profit;
                    }

                    inq.SaveChangesAsync();
                    PartOfAward = tmentry.TASK_USER_PROFIT;
                }

                MyMoney = CurrentUser.Balance;
                inq.SaveChanges();


            }
        }

        private void RecountSuccessProbability(int task_id)
        {
            //SuccessProbabilityColor
        }


        private void RefreshCurrentTask()
        {
            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
            {
                USERS u = inq.USERS.Find(CurrentUser.Id);
                bool wasUpdated = false;

                if (u != null)
                {
                    foreach (TASK_MEMBER tm in u.TASK_MEMBER)
                    {
                        if (tm.TASK.IS_TASK_RUNNING == true)
                        {
                            CurrentTask = tm.TASK;
                            wasUpdated = true;
                            break;
                        }
                    }
                }

                if (!wasUpdated)
                {
                    CurrentTask = null;
                    ThisTaskStartTime = "--";
                    ThisTaskAward = 0;
                    ThisTaskMembers = new ObservableCollection<USERS>();
                    string tmp = String.Format(@"{0}", "");
                    PartOfAward = 0;
                }

                if (CurrentTask != null)
                {
                    ThisTaskStartTime = CurrentTask.TASK_START;
                    ThisTaskAward = (decimal)CurrentTask.TASK_PROFIT;

                    var task_members = from tm in inq.TASK_MEMBER
                                       where tm.TASK_ID == CurrentTask.TASK_ID
                                       select tm;

                    ObservableCollection<USERS> tmp_col = new ObservableCollection<USERS>();

                    foreach (var tm in inq.TASK_MEMBER)
                    {
                        if (tm.TASK_ID == CurrentTask.TASK_ID && tm.TASK.IS_TASK_RUNNING == true)
                        {
                            tmp_col.Add(inq.USERS.Find(tm.TASK_USER_ID));
                        }
                    }

                    ThisTaskMembers = new ObservableCollection<USERS>(tmp_col);

                    string tmp = String.Format(@"{0}", (from c in inq.CREATURE_DICTIONARY
                                                        join tc in inq.TASK_CREATURES on c.CR_ID equals tc.CREATURE_ID
                                                        where tc.TASK_ID == CurrentTask.TASK_ID
                                                        select c).First().CR_IMAGE);

                    if (thisWindow.CurrentTaskMonsterImage != null)
                    {
                        thisWindow.CurrentTaskMonsterImage.Source = new BitmapImage(new Uri(tmp));
                    }

                    RecountShare(CurrentTask.TASK_ID);

                    MyGuild = inq.GUILD.Find(CurrentUser.GuildId);
                }
            }
        }
    } 
}
