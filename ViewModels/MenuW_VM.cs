using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Linq;
using KP_OOP.Models;
using KP_OOP.Views;

namespace KP_OOP.ViewModels
{
    public class MenuW_VM : INotifyPropertyChanged
    {
        private MenuWindow thisWindow;
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
        public ObservableCollection<TASK> ThisUserTasks
        {
            get { return thisUserTasks; }
            set
            {
                thisUserTasks = value;
                OnPropertyChanged("ThisUserTasks");
            }
        }



        public MenuW_VM(MenuWindow mw)
        {
            thisWindow = mw;

            PanelElements = new ObservableCollection<PanelElement>()
            {
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/stats_guild.png", "Статистика гильдий"),
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/add_guild.png", "Создать гильдию"),
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/add_commander.png", "Все инквизиторы"),
                new PanelElement("D:/Нада/OOP_2_sem/KP/KP_OOP/Graphics/Icons/stats_task.png", "Статистика заданий"),
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
                }
                catch(Exception e)
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

        private GUILD selectedStatsGuild;
        public GUILD SelectedStatsGuild
        {
            get { return selectedStatsGuild; }
            set
            {
                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                {
                    try
                    {
                        if (value != null)
                        {
                            thisWindow.GuildMembers.Text = String.Format("{0}", (from u in inq.USERS
                                                                                 where u.US_GUILD_ID == value.GUILD_ID
                                                                                 select u).Count());

                            thisWindow.GuildTasks.Text = String.Format("{0}", (from t in inq.TASK
                                                                               where t.TASK_GUILD_ID == value.GUILD_ID
                                                                               select t).Count());
                        }
                    }
                    catch(Exception e)
                    {
                        new Warning(e.Message).Show();
                    }
                }
                selectedStatsGuild = value;
                OnPropertyChanged("SelectedStatsGuild");
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

                if(matchesJPG.Count == 0 && matchesPNG.Count == 0)
                {
                    thisWindow.ErrorFieldAddGuild.Text = "Неверный формат файла";
                    thisWindow.ErrorFieldAddGuild.Foreground = Brushes.Red;
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
                    thisWindow.ErrorFieldAddGuild.Text = "Неверный формат файла";
                    thisWindow.ErrorFieldAddGuild.Foreground = Brushes.Red;
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
                    thisWindow.ErrorFieldAddGuild.Text = "Неверный формат файла";
                    thisWindow.ErrorFieldAddGuild.Foreground = Brushes.Red;
                }
                else
                {
                    selectedWeaponImagePath = String.Format(@"{0}", value);
                    OnPropertyChanged("SelectedWeaponImagePath");
                }
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

                            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                            {
                                try
                                {
                                    switch (SelectedPanel.Text)
                                    {
                                        case "Бестиарий":

                                            MonsterLibraryElements = new ObservableCollection<CREATURE_DICTIONARY>(inq.CREATURE_DICTIONARY);
                                            ShowNewGrid(thisWindow.MonsterLibraryView);
                                            ShowGrid(thisWindow.LeftTopSortGrid);
                                            break;

                                        case "Арсенал":

                                            WeaponLibraryElements = new ObservableCollection<WEAPON_DICTIONARY>(inq.WEAPON_DICTIONARY);
                                            ShowNewGrid(thisWindow.WeaponLibraryView);
                                            ShowGrid(thisWindow.LeftTopSortGrid);
                                            break;

                                        case "Все инквизиторы":

                                            Users = new ObservableCollection<USERS>(inq.USERS);
                                            ShowNewGrid(thisWindow.UserStats);
                                            break;

                                        case "Статистика гильдий":

                                            Users = new ObservableCollection<USERS>(inq.USERS);
                                            Guilds = new ObservableCollection<GUILD>(inq.GUILD);
                                            ShowNewGrid(thisWindow.GuildStats);
                                            break;

                                        case "Статистика заданий":

                                            ShowNewGrid(thisWindow.TaskStats);
                                            break;

                                        case "Создать гильдию":

                                            ShowNewGrid(thisWindow.GuildAdding);
                                            break;

                                        default:
                                            break;

                                    }
                                }
                                catch(Exception e)
                                {
                                    new Warning(e.Message).Show();
                                }
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
                                    if (SelectedUser != null)
                                    {
                                        var tmp_tasks = from t in inq.TASK
                                                        join tm in inq.TASK_MEMBER on t.TASK_ID equals tm.TASK_ID
                                                        where tm.TASK_USER_ID == SelectedUser.US_ID
                                                        select t;

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
                                catch(Exception e)
                                {
                                    new Warning(e.Message).Show();
                                }
                            }
                        }
                    )
                );
            }
        }

        private UserCommand guidStatsSelectionChanged;
        public UserCommand GuidStatsSelectionChanged
        {
            get
            {
                return guidStatsSelectionChanged ??
                (guidStatsSelectionChanged = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                if (SelectedStatsGuild != null)
                                {
                                    using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                    {
                                        var tmp_users = from u in inq.USERS
                                                        where u.US_GUILD_ID == SelectedStatsGuild.GUILD_ID
                                                        select u;

                                        ThisGuildMembers = new ObservableCollection<USERS>(tmp_users);
                                    }
                                }
                            }
                            catch(Exception e)
                            {
                                new Warning(e.Message).Show();
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
                            catch(Exception e)
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
                            catch(Exception e)
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
                            catch(Exception e)
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
                            catch(Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                        }
                    )
                );
            }
        }

        private UserCommand addGuildCommand;
        public UserCommand AddGuildCommand
        {
            get
            {
                return addGuildCommand ??
                (addGuildCommand = new UserCommand
                    (
                        obj =>
                        {
                            string patternText = @"\w+";

                            try
                            {
                                if (Regex.IsMatch(thisWindow.AddGuildName.Text, patternText, RegexOptions.IgnoreCase))
                                {
                                    if (thisWindow.PreviewGuildIcon.Source.ToString() != "")
                                    {
                                        if (SelectedUserAC != null)
                                        {


                                            GUILD guild = new GUILD
                                            {
                                                GUILD_BALANCE = 0,
                                                GUILD_IMAGE = String.Format(@"{0}", thisWindow.PreviewGuildIcon.Source),
                                                GUILD_NAME = thisWindow.AddGuildName.Text,
                                                GUILD_STAT = 0
                                            };

                                            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                            {
                                                var tmp_guild = from g in inq.GUILD
                                                                where g.GUILD_NAME == guild.GUILD_NAME
                                                                select g;

                                                if (tmp_guild.Count() < 1)
                                                {
                                                    inq.GUILD.Add(guild);
                                                    inq.SaveChanges();


                                                    var tmp_head = (from u in inq.USERS
                                                                    where u.US_ID == SelectedUserAC.US_ID
                                                                    select u).First();

                                                    tmp_head.US_TYPE_NAME = "head";
                                                    tmp_head.US_GUILD_ID = (from g in inq.GUILD
                                                                            where g.GUILD_NAME == guild.GUILD_NAME
                                                                            select g).First().GUILD_ID;
                                                    inq.SaveChanges();

                                                    Users = new ObservableCollection<USERS>(inq.USERS);
                                                    new Warning("Добавлено успешно").Show();
                                                }
                                                else
                                                {
                                                    new Warning("Ошибка: \nГильдия с таким именем уже существует").Show();
                                                }
                                            }

                                        }
                                        else
                                        {
                                            new Warning("Ошбика: \nВыберите пользователя, который будет назначен главой").Show();
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошбика: \nВыберите изображение").Show();
                                    }
                                }
                                else
                                {
                                    new Warning("Ошибка: \nНекорректно введено название").Show();
                                }
                            }
                            catch(Exception e)
                            {
                                new Warning(e.Message).Show();
                            }
                         }
                    )
                );
            }
        }

        private UserCommand guildNameSortTextChangedCommand;
        public UserCommand GuildNameSortTextChangedCommand
        {
            get
            {
                return guildNameSortTextChangedCommand ??
                (guildNameSortTextChangedCommand = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                if(thisWindow.GuildNameSortText.Text != "")
                                {
                                    string pattern = @"\w+";
                                    if (Regex.IsMatch(thisWindow.GuildNameSortText.Text, pattern, RegexOptions.IgnoreCase))
                                    {
                                        using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                        {
                                            var tmp_guilds = from g in inq.GUILD
                                                             where g.GUILD_NAME.Contains(thisWindow.GuildNameSortText.Text)
                                                             select g;

                                            if (tmp_guilds.Count() > 0)
                                            {
                                                GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                            }
                                            else
                                            {
                                                GuildsPlaceHolder = new ObservableCollection<GUILD>();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nВведён некорректный символ");
                                    }
                                }
                                else
                                {
                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(Guilds);
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

        private UserCommand guildTaskSortTextChangedCommand;
        public UserCommand GuildTaskSortTextChangedCommand
        {
            get
            {
                return guildTaskSortTextChangedCommand ??
                (guildTaskSortTextChangedCommand = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                if (thisWindow.GuildTaskSortText.Text != "")
                                {
                                    string pattern = @"<*|>*\d+";
                                    if (Regex.IsMatch(thisWindow.GuildTaskSortText.Text, pattern, RegexOptions.IgnoreCase))
                                    {
                                        using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                        {
                                            string tmp = thisWindow.GuildTaskSortText.Text;
                                            if (tmp.Length == 1 && (tmp[0] == '<' || tmp[0] == '>'))
                                            {

                                            }
                                            else
                                            {
                                                if (tmp[0] == '<')
                                                {
                                                    tmp = tmp.Remove(0,1);
                                                    int num = Int32.Parse(tmp);

                                                    var tmp_guilds = from g in inq.GUILD
                                                                     where g.GUILD_STAT < num
                                                                     select g;

                                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                                }
                                                else if (tmp[0] == '>')
                                                {
                                                    tmp = tmp.Remove(0,1);
                                                    int num = Int32.Parse(tmp);

                                                    var tmp_guilds = from g in inq.GUILD
                                                                     where g.GUILD_STAT > num
                                                                     select g;

                                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                                }
                                                else
                                                {
                                                    int num = Int32.Parse(tmp);

                                                    var tmp_guilds = from g in inq.GUILD
                                                                     where g.GUILD_STAT == num
                                                                     select g;

                                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nВведён некорректный символ");
                                    }
                                }
                                else
                                {
                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(Guilds);
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

        private UserCommand guildMoneySortTextChangedCommand;
        public UserCommand GuildMoneySortTextChangedCommand
        {
            get
            {
                return guildMoneySortTextChangedCommand ??
                (guildMoneySortTextChangedCommand = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                if (thisWindow.GuildMoneySortText.Text != "")
                                {
                                    string pattern = @"<*|>*\d+";
                                    if (Regex.IsMatch(thisWindow.GuildMoneySortText.Text, pattern, RegexOptions.IgnoreCase))
                                    {
                                        using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                        {
                                            string tmp = thisWindow.GuildMoneySortText.Text;
                                            if (tmp.Length == 1 && (tmp[0] == '<' || tmp[0] == '>'))
                                            {

                                            }
                                            else
                                            {
                                                if (tmp[0] == '<')
                                                {
                                                    tmp = tmp.Remove(0, 1);
                                                    int num = Int32.Parse(tmp);

                                                    var tmp_guilds = from g in inq.GUILD
                                                                     where g.GUILD_BALANCE < num
                                                                     select g;

                                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                                }
                                                else if (tmp[0] == '>')
                                                {
                                                    tmp = tmp.Remove(0, 1);
                                                    int num = Int32.Parse(tmp);

                                                    var tmp_guilds = from g in inq.GUILD
                                                                     where g.GUILD_BALANCE > num
                                                                     select g;

                                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                                }
                                                else
                                                {
                                                    int num = Int32.Parse(tmp);

                                                    var tmp_guilds = from g in inq.GUILD
                                                                     where g.GUILD_BALANCE == num
                                                                     select g;

                                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(tmp_guilds);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nВведён некорректный символ");
                                    }
                                }
                                else
                                {
                                    GuildsPlaceHolder = new ObservableCollection<GUILD>(Guilds);
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

        private UserCommand taskNameSortTextChangedCommand;
        public UserCommand TaskNameSortTextChangedCommand
        {
            get
            {
                return taskNameSortTextChangedCommand ??
                (taskNameSortTextChangedCommand = new UserCommand
                    (
                        obj =>
                        {
                            try
                            {
                                if (thisWindow.TaskNameSortText.Text != "")
                                {
                                    string pattern = @"\w+";
                                    if (Regex.IsMatch(thisWindow.TaskNameSortText.Text, pattern, RegexOptions.IgnoreCase))
                                    {
                                        using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                        {
                                            var tmp_tasks = from t in inq.TASK
                                                             where t.TASK_NAME.Contains(thisWindow.TaskNameSortText.Text)
                                                             select t;

                                            if (tmp_tasks.Count() > 0)
                                            {
                                                TasksPlaceHolder = new ObservableCollection<TASK>(tmp_tasks);
                                            }
                                            else
                                            {
                                                TasksPlaceHolder = new ObservableCollection<TASK>();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        new Warning("Ошибка: \nВведён некорректный символ");
                                    }
                                }
                                else
                                {
                                    TasksPlaceHolder = new ObservableCollection<TASK>(Tasks);
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

        public bool ShowGrid(Grid grid)
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

        public decimal CountValue(Int16 damage, Int32 weight)
        {
            return Math.Round((decimal)(100 - 0.8 * weight + 1.5 * damage), 2);
        }
    }
}
