using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ld51.Utils
{
    /// <summary>
    /// Generic extensions
    /// </summary>
    public static class Printer
    {
        /// <summary>
        /// Print to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Print<T>(this T t) => GD.Print(t);

        /// <summary>
        /// Print to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        public static void Print<T>(this T t, String message) => GD.Print($"{message} {t}");

        /// <summary>
        /// Print to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="M"></typeparam>
        /// <param name="_"></param>
        /// <param name="obj">Object to print</param>
        public static void Print<T, M>(this T m, M obj) => GD.Print($"{obj} {m}");

        /// <summary>
        /// Print to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="format">Formatting to apply</param>
        /// <param name="provider">Format provider</param>
        public static void Printf(this IFormattable t, String format, IFormatProvider provider = null) => GD.Print(t.ToString(format, provider));

        /// <summary>
        /// Print to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        /// <param name="format">Format string</param>
        /// <param name="provider">Format provider</param>
        public static void Printf(this IFormattable t, String message, String format, IFormatProvider provider = null) => GD.Print($"{message} {t.ToString(format, provider)}");


        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void Warn<T>(this T t) => GD.PushWarning($"{t}");

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        public static void Warn<T>(this T t, String message) => GD.PushWarning($"{message} {t}");

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="O"></typeparam>
        /// <param name="t"></param>
        /// <param name="obj">Object to print</param>
        public static void Warn<T, O>(this T t, O obj) => GD.PushWarning($"{obj} {t}");

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="format">Formatting to apply</param>
        /// <param name="provider">Format provider</param>
        public static void Warnf(this IFormattable t, String format, IFormatProvider provider = null) => GD.PushWarning(t.ToString(format, provider));

        /// <summary>
        /// Prints warning to console
        /// </summary>
        /// <param name="t"></param>
        /// <param name="message">Message to prepend</param>
        /// <param name="format">Format string</param>
        /// <param name="provider">Format provider</param>
        public static void Warnf(this IFormattable t, String message, String format, IFormatProvider provider = null) => GD.PushWarning($"{message} {t.ToString(format, provider)}");
    }
}
