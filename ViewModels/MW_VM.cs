using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using KP_OOP;
using KP_OOP.Models;
using KP_OOP.Views;

namespace KP_OOP.ViewModels
{
    class MW_VM : INotifyPropertyChanged
    {
        private MainWindow thisWindow;
        public event PropertyChangedEventHandler PropertyChanged;

        private UserCommand callLoginFormCommand;
        private UserCommand callRegistrationFormCommand;
        private UserCommand hideSomething;
        private UserCommand exitCommand;
        private UserCommand callShortLoginFormCommand;

        private UserCommand login;
        private UserCommand register;
        private UserCommand shortLogin;

        public MW_VM(MainWindow mw)
        {
            thisWindow = mw;
            using (Inq_helperEntities2 inq = new Inq_helperEntities2())
            {
                Admin = (from u in inq.USERS
                         where u.US_TYPE_NAME == "leader"
                         select u).First(); 
            }
        }

        public USERS Admin { get; set; }


        public UserCommand Login
        {
            get
            {
                return login ??
                (login = new UserCommand
                    (
                        obj =>
                        {
                            string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                             @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

                            string patternPassword = @"\w+";

                            
                            if (thisWindow.LoginEmailTextBox.Text !="" && thisWindow.LoginPasswordPasswordBox.Password != "" && 
                                Regex.IsMatch(thisWindow.LoginEmailTextBox.Text, patternEmail, RegexOptions.IgnoreCase) && 
                                Regex.IsMatch(thisWindow.LoginPasswordPasswordBox.Password, patternPassword, RegexOptions.IgnoreCase))
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    var us = from u in inq.USERS
                                             where u.US_LOGIN == thisWindow.LoginEmailTextBox.Text && u.US_PASSWORD == thisWindow.LoginPasswordPasswordBox.Password
                                             select u;

                                    if (us.Count() < 1)
                                    {
                                        new Warning("Ошибка:\n Пользователь не найден.Повторите попытку.").Show();
                                    }
                                    else
                                    {
                                        CurrentUser.SetUser(us.First());
                                        switch (us.First().US_TYPE_NAME)
                                        {
                                            case "leader":

                                                MenuWindow mw = new MenuWindow();
                                                thisWindow.Close();
                                                mw.Show();
                                                break;

                                            case "head":

                                                HeadWindow hw = new HeadWindow();
                                                thisWindow.Close();
                                                hw.Show();
                                                break;

                                            case "member":

                                                UserWindow uw = new UserWindow();
                                                thisWindow.Close();
                                                uw.Show();
                                                break;

                                            default:

                                                MessageBox.Show("Неизвестная ошибка авторизации. Пожалуйста, повторите попытку");
                                                break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                new Warning("Ошибка:\n Введите логин и пароль.").Show();
                            }

                        }
                    )
                );
            }
        }

        public UserCommand Register
        {
            get
            {
                return register ??
                (register = new UserCommand
                    (
                        obj =>
                        {
                            string patternEmail = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                             @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";

                            string patternPassword = @"\w+";


                            if (Regex.IsMatch(thisWindow.RegisterLogin.Text, patternEmail, RegexOptions.IgnoreCase) &&
                                Regex.IsMatch(thisWindow.RegisterName.Text, patternPassword, RegexOptions.IgnoreCase) &&
                                Regex.IsMatch(thisWindow.RegisterPassword.Password, patternPassword, RegexOptions.IgnoreCase) &&
                                Regex.IsMatch(thisWindow.RegisterRepeatPassword.Password, patternPassword, RegexOptions.IgnoreCase))
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    if (    (from u in inq.USERS
                                             where u.US_PASSWORD == thisWindow.RegisterPassword.Password || u.US_NAME == thisWindow.RegisterName.Text
                                             select u).Count() < 1)
                                    {
                                        USERS tmp_user = new USERS
                                        {
                                            US_NAME = thisWindow.RegisterName.Text,
                                            US_TYPE_NAME = "member",
                                            US_LOGIN = thisWindow.RegisterLogin.Text,
                                            US_LEVEL = 1,
                                            US_PASSWORD = thisWindow.RegisterPassword.Password,
                                            US_BALANCE = 0,
                                            US_PATH = @"D:\Нада\OOP_2_sem\KP\KP_OOP\Graphics\Avatars\avatar1.png"
                                        };

                                        try
                                        {
                                            inq.USERS.Add(tmp_user);
                                            inq.SaveChanges();
                                        }
                                        catch(Exception e)
                                        {
                                            new Warning(e.Message).Show();
                                        }

                                        new Warning("Регистрация прошла успешно. Вернитесь на форму логина и войдите в свой аккаунт.").Show();
                                    }
                                    else
                                    {
                                        new Warning("Ошибка:\n Пользователь с такой почтой или паролем уже существует.").Show();
                                    }
                                    
                                }
                            }
                            else
                            {
                                new Warning("Ошибка:\n Некорректно введено одно из значений.").Show();
                            }
                        }
                    )
                );
            }
        }

        public UserCommand ShortLogin
        {
            get
            {
                return shortLogin ??
                (shortLogin = new UserCommand
                    (
                        obj =>
                        {
                            //ShortLoginPasswordBox
                            string patternPassword = @"\w+";

                            if (thisWindow.ShortLoginPasswordBox.Password != "" && Regex.IsMatch(thisWindow.ShortLoginPasswordBox.Password, patternPassword, RegexOptions.IgnoreCase))
                            {
                                using (Inq_helperEntities2 inq = new Inq_helperEntities2())
                                {
                                    var us = from u in inq.USERS
                                             where u.US_PASSWORD == thisWindow.ShortLoginPasswordBox.Password
                                             select u;

                                    if (us.Count() < 1)
                                    {
                                        new Warning("Ошибка:\n Пользователь не найден. Повторите попытку.").Show();
                                    }
                                    else
                                    {
                                        MenuWindow mw = new MenuWindow();
                                        thisWindow.Close();
                                        mw.Show();
                                    }
                                }
                            }
                            else
                            {
                                new Warning("Ошибка:\n Неверно введен пароль.").Show();
                            }
                        }
                    )
                );
            }
        }


        public UserCommand ExitCommand
        {
            get
            {
                return exitCommand ??
                (exitCommand = new UserCommand
                    (
                        obj =>
                        {
                            thisWindow.Close();
                        }
                    )
                );
            }
        }

        public UserCommand HideSomething
        {
            get
            {
                return hideSomething ??
                (hideSomething = new UserCommand
                    (
                        obj =>
                        {
                            if (thisWindow.RegistrationForm.Opacity == 1)
                            {
                                System.Windows.Thickness to = new System.Windows.Thickness();
                                to.Left = 520; to.Top = -700; to.Right = 520; to.Bottom = 900;

                                System.Windows.Thickness from = new System.Windows.Thickness();
                                from.Left = 520; from.Top = 50; from.Right = 520; from.Bottom = 154;

                                DoubleAnimation ShowButtons = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(600));
                                DoubleAnimation HideLoginFormPart1 = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200));
                                ThicknessAnimation HideLoginFormPart2 = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200));

                                thisWindow.RoundButtonsContainer.BeginAnimation(Grid.OpacityProperty, ShowButtons);
                                thisWindow.RegistrationForm.BeginAnimation(Grid.OpacityProperty, HideLoginFormPart1);
                                thisWindow.RegistrationForm.BeginAnimation(Grid.MarginProperty, HideLoginFormPart2);
                                thisWindow.BackButtonIcon.BeginAnimation(Image.OpacityProperty, HideLoginFormPart1);
                                thisWindow.BackButton.IsEnabled = false;
                            }
                            else if(thisWindow.LoginForm.Opacity == 1)
                            {
                                System.Windows.Thickness to = new System.Windows.Thickness();
                                to.Left = 520; to.Top = 840; to.Right = 520; to.Bottom = -420;

                                System.Windows.Thickness from = new System.Windows.Thickness();
                                from.Left = 520; from.Top = 200; from.Right = 520; from.Bottom = 220;

                                DoubleAnimation ShowButtons = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(600));
                                DoubleAnimation HideLoginFormPart1 = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200));
                                ThicknessAnimation HideLoginFormPart2 = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200));

                                thisWindow.RoundButtonsContainer.BeginAnimation(Grid.OpacityProperty, ShowButtons);
                                thisWindow.LoginForm.BeginAnimation(Grid.OpacityProperty, HideLoginFormPart1);
                                thisWindow.LoginForm.BeginAnimation(Grid.MarginProperty, HideLoginFormPart2);
                                thisWindow.BackButtonIcon.BeginAnimation(Image.OpacityProperty, HideLoginFormPart1);
                                thisWindow.BackButton.IsEnabled = false;
                            }
                            else
                            {
                                //520,716,520,-296
                                System.Windows.Thickness to = new System.Windows.Thickness();
                                to.Left = 520; to.Top = 716; to.Right = 520; to.Bottom = -296;

                                System.Windows.Thickness from = new System.Windows.Thickness();
                                from.Left = 520; from.Top = 200; from.Right = 520; from.Bottom = 220;

                                DoubleAnimation ShowButtons = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(600));
                                DoubleAnimation HideLoginFormPart1 = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200));
                                ThicknessAnimation HideLoginFormPart2 = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(200));

                                thisWindow.RoundButtonsContainer.BeginAnimation(Grid.OpacityProperty, ShowButtons);
                                thisWindow.LoginFormWithoutEmail.BeginAnimation(Grid.OpacityProperty, HideLoginFormPart1);
                                thisWindow.LoginFormWithoutEmail.BeginAnimation(Grid.MarginProperty, HideLoginFormPart2);
                                thisWindow.BackButtonIcon.BeginAnimation(Image.OpacityProperty, HideLoginFormPart1);
                                thisWindow.BackButton.IsEnabled = false;
                            }
                        }
                    )
                );
            }
        }

        public UserCommand CallRegistrationFormCommand
        {
            get
            {
                return callRegistrationFormCommand ??
                (callRegistrationFormCommand = new UserCommand
                    (
                        obj =>
                        {
                            System.Windows.Thickness from = new System.Windows.Thickness();
                            from.Left = 520; from.Top = -700; from.Right = 520; from.Bottom = 900;

                            System.Windows.Thickness to = new System.Windows.Thickness();
                            to.Left = 520; to.Top = 50; to.Right = 520; to.Bottom = 154;

                            DoubleAnimation HideButtons = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(200));
                            DoubleAnimation ShowLoginFormPart1 = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(600));
                            ThicknessAnimation ShowLoginFormPart2 = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(600));

                            thisWindow.RoundButtonsContainer.BeginAnimation(Grid.OpacityProperty, HideButtons);
                            thisWindow.RegistrationForm.BeginAnimation(Grid.OpacityProperty, ShowLoginFormPart1);
                            thisWindow.RegistrationForm.BeginAnimation(Grid.MarginProperty, ShowLoginFormPart2);
                            thisWindow.BackButtonIcon.BeginAnimation(Image.OpacityProperty, ShowLoginFormPart1);
                            thisWindow.BackButton.IsEnabled = true;
                        }
                    )
                );
            }
        }
        
        public UserCommand CallLoginFormCommand
        {
            get
            {
                return callLoginFormCommand ??
                (callLoginFormCommand = new UserCommand
                    (
                        obj =>
                        {    
                            PowerEase pw = new PowerEase();
                            pw.Power = 6;

                            System.Windows.Thickness from = new System.Windows.Thickness();
                            from.Left = 520; from.Top = 840; from.Right = 520; from.Bottom = -420;

                            System.Windows.Thickness to = new System.Windows.Thickness();
                            to.Left = 520; to.Top = 200; to.Right = 520; to.Bottom = 220;

                            DoubleAnimation HideButtons = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(200));
                            DoubleAnimation ShowLoginFormPart1 = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(600));
                            ThicknessAnimation ShowLoginFormPart2 = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(600));

                            thisWindow.RoundButtonsContainer.BeginAnimation(Grid.OpacityProperty, HideButtons);
                            thisWindow.LoginForm.BeginAnimation(Grid.OpacityProperty, ShowLoginFormPart1);
                            thisWindow.LoginForm.BeginAnimation(Grid.MarginProperty, ShowLoginFormPart2);
                            thisWindow.BackButtonIcon.BeginAnimation(Image.OpacityProperty, ShowLoginFormPart1);
                            thisWindow.BackButton.IsEnabled = true;
                        }
                    )
                );
            }
        }

        public UserCommand CallShortLoginFormCommand
        {
            get
            {
                return callShortLoginFormCommand ??
                (callShortLoginFormCommand = new UserCommand
                    (
                        obj =>
                        {
                            //520,72,520,348
                            PowerEase pw = new PowerEase();
                            pw.Power = 6;

                            System.Windows.Thickness from = new System.Windows.Thickness();
                            from.Left = 520; from.Top = 840; from.Right = 520; from.Bottom = -420;

                            System.Windows.Thickness to = new System.Windows.Thickness();
                            to.Left = 520; to.Top = 72; to.Right = 520; to.Bottom = 348;

                            DoubleAnimation HideButtons = AnimationFunctions.ReturnNewDoubleAnimationWBT(1, 0, TimeSpan.FromMilliseconds(350), TimeSpan.FromMilliseconds(200));
                            DoubleAnimation ShowLoginFormPart1 = AnimationFunctions.ReturnNewDoubleAnimationWBT(0, 1, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(600));
                            ThicknessAnimation ShowLoginFormPart2 = AnimationFunctions.ReturnNewThicknessAnimationWBT(from, to, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(600));

                            thisWindow.RoundButtonsContainer.BeginAnimation(Grid.OpacityProperty, HideButtons);
                            thisWindow.LoginFormWithoutEmail.BeginAnimation(Grid.OpacityProperty, ShowLoginFormPart1);
                            thisWindow.LoginFormWithoutEmail.BeginAnimation(Grid.MarginProperty, ShowLoginFormPart2);
                            thisWindow.BackButtonIcon.BeginAnimation(Image.OpacityProperty, ShowLoginFormPart1);
                            thisWindow.BackButton.IsEnabled = true;
                        }
                    )
                );
            }
        }
    }
   
}
