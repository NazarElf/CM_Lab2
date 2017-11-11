using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CM_Lab2_WPF
{
    class MyMessageBox
    {
        public static MyMessageBoxResult Show(string messageBoxText)
        {
            //show window;
            CustomMessageBox cmb = new CustomMessageBox(messageBoxText,
                                                        "",
                                                        MyMessageBoxButton.Ok,
                                                        MyMessageBoxImage.None,
                                                        MyMessageBoxResult.None);
            cmb.ShowDialog();
            return cmb.result;
        }
        public static MyMessageBoxResult Show(string messageBoxText, string caption)
        {
            CustomMessageBox cmb = new CustomMessageBox(messageBoxText,
                                                        caption,
                                                        MyMessageBoxButton.Ok,
                                                        MyMessageBoxImage.None,
                                                        MyMessageBoxResult.None);
            cmb.ShowDialog();
            return cmb.result;
        }
        public static MyMessageBoxResult Show(string messageBoxText, string caption, MyMessageBoxButton button)
        {
            CustomMessageBox cmb = new CustomMessageBox(messageBoxText,
                                                        caption,
                                                        button,
                                                        MyMessageBoxImage.None,
                                                        MyMessageBoxResult.None);
            cmb.ShowDialog();
            return cmb.result; //this must be result from window
        }
        public static MyMessageBoxResult Show(string messageBoxText, string caption, MyMessageBoxButton button, MyMessageBoxImage icon)
        {
            CustomMessageBox cmb = new CustomMessageBox(messageBoxText,
                                                        caption,
                                                        button,
                                                        icon,
                                                        MyMessageBoxResult.None);
            cmb.ShowDialog();
            return cmb.result; //here must be result from window
        }
        public static MyMessageBoxResult Show(string messageBoxText, string caption, MyMessageBoxButton button, MyMessageBoxImage icon, MyMessageBoxResult defaultResult)
        {
            CustomMessageBox cmb = new CustomMessageBox(messageBoxText,
                                                        caption,
                                                        button,
                                                        icon,
                                                        defaultResult);
            cmb.ShowDialog();
            return cmb.result; //here must be result from window
        }
    }

    public enum MyMessageBoxResult
    {
        Cancel, No, None, Ok, Yes
    }

    public enum MyMessageBoxButton
    {
        Ok, OkCancel, YesNo, YesNoCancel
    }

    public enum MyMessageBoxImage
    {
        None = 0,
        Hand = 1,
        Stop = 1,
        Error = 1,
        Question = 2,
        Exclamation = 3,
        Warning = 3,
        Asterisk = 4,
        Information = 4
    }

}
