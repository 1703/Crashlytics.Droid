using System;
using Android.Content;
using Android.Runtime;
using Java.Lang;
using JavaException = Java.Lang.Exception;

// ReSharper disable once CheckNamespace

namespace Xamarin.Android.Crashlytics
{
    public class CrashReporting
    {
        private static void AndroidEnvironmentOnUnhandledExceptionRaiser(RaiseThrowableEventArgs eventArgs,
            bool callJavaDefaultUncaughtExceptionHandler)
        {
            JavaException exception = MonoException.Create(eventArgs.Exception);

            if (callJavaDefaultUncaughtExceptionHandler && Thread.DefaultUncaughtExceptionHandler != null)
                Thread.DefaultUncaughtExceptionHandler.UncaughtException(Thread.CurrentThread(), exception);
            else
				Com.Crashlytics.Android.Crashlytics.LogException (exception);
        }

		public static void StartWithMonoHook(Context context, bool callJavaDefaultUncaughtExceptionHandler, bool debug)
        {
            if (context == null)
                throw new ArgumentNullException("context");

			var builder = new IO.Fabric.Sdk.Android.Fabric.Builder (context).Kits (new Com.Crashlytics.Android.Crashlytics ())
				.Debuggable (debug)
				.Build ();
			IO.Fabric.Sdk.Android.Fabric.With(builder);
            AndroidEnvironment.UnhandledExceptionRaiser +=
                (sender, args) =>
                {
                    AndroidEnvironmentOnUnhandledExceptionRaiser(args, callJavaDefaultUncaughtExceptionHandler);
                };
        }
    }
}