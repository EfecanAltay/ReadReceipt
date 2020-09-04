using Android.Runtime;
using Java.Interop;
using System;
using static Org.Opencv.Android.CameraGLSurfaceView;

namespace Org.Opencv.Android
{
    public partial class CameraBridgeViewBase
    {
		public virtual unsafe void SetCvCameraViewListener()
		{
			const string __id = "setCvCameraViewListener.(Lorg/opencv/android/CameraBridgeViewBase$CvCameraViewListener;)V";
			try
			{
				JniArgumentValue* __args = stackalloc JniArgumentValue[1];
				__args[0] = new JniArgumentValue(IntPtr.Zero);
				_members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
			}
			finally
			{
			}
		}

		public virtual unsafe void SetCvCameraViewListener(global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListener listener)
		{
			const string __id = "setCvCameraViewListener.(Lorg/opencv/android/CameraBridgeViewBase$CvCameraViewListener2;)V";
			try
			{
				JniArgumentValue* __args = stackalloc JniArgumentValue[1];
				__args[0] = new JniArgumentValue((listener == null) ? IntPtr.Zero : ((global::Java.Lang.Object)listener).Handle);
				_members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
			}
			finally
			{
			}
		}

		WeakReference weak_implementor_CameraFrame;
		global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor ImplCameraFrame
		{
			get
			{
				if (weak_implementor_CameraFrame == null || !weak_implementor_CameraFrame.IsAlive)
					return null;
				return weak_implementor_CameraFrame.Target as global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor;
			}
			set { weak_implementor_CameraFrame = new WeakReference(value, true); }
		}

		public global::Org.Opencv.Android.CameraBridgeViewBase.CvCameraViewOnCameraFrameHandler CameraFrame
		{
			get
			{
				global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor impl = ImplCameraFrame;
				return impl == null ? null : impl.OnCameraFrameHandler;
			}
			set
			{
				global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor impl = ImplCameraFrame;
				if (impl == null)
				{
					impl = new global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor(this);
					ImplCameraFrame = impl;
				}
				else
					impl.OnCameraFrameHandler = value;
			}
		}

		public event EventHandler<global::Org.Opencv.Android.CameraBridgeViewBase.CameraViewStartedEventArgs> CameraViewStarted
		{
			add
			{
				global::Java.Interop.EventHelper.AddEventHandler<global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListener, global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor>(
						ref weak_implementor_SetCvCameraViewListener,
						__CreateICvCameraViewListenerImplementor,
						SetCvCameraViewListener,
						__h => __h.OnCameraViewStartedHandler += value);
			}
			remove
			{
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListener, global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor>(
						ref weak_implementor_SetCvCameraViewListener,
						global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor.__IsEmpty,
						__v => SetCvCameraViewListener(),
						__h => __h.OnCameraViewStartedHandler -= value);
			}
		}

		public event EventHandler CameraViewStopped
		{
			add
			{
				global::Java.Interop.EventHelper.AddEventHandler<global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListener, global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor>(
						ref weak_implementor_SetCvCameraViewListener,
						__CreateICvCameraViewListenerImplementor,
						SetCvCameraViewListener,
						__h => __h.OnCameraViewStoppedHandler += value);
			}
			remove
			{
				global::Java.Interop.EventHelper.RemoveEventHandler<global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListener, global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor>(
						ref weak_implementor_SetCvCameraViewListener,
						global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor.__IsEmpty,
						__v => SetCvCameraViewListener(),
						__h => __h.OnCameraViewStoppedHandler -= value);
			}
		}

		WeakReference weak_implementor_SetCvCameraViewListener;

		public delegate global::Org.Opencv.Core.Mat CvCameraViewOnCameraFrameHandler(global::Org.Opencv.Core.Mat p0);

		// event args for org.opencv.android.CameraBridgeViewBase.CvCameraViewListener.onCameraViewStarted
		public partial class CameraViewStartedEventArgs : global::System.EventArgs
		{
			public CameraViewStartedEventArgs(int p0, int p1)
			{
				this.p0 = p0;
				this.p1 = p1;
			}

			int p0;
			public int P0
			{
				get { return p0; }
			}

			int p1;
			public int P1
			{
				get { return p1; }
			}
		}

		internal sealed partial class ICvCameraViewListenerImplementor : global::Java.Lang.Object, ICvCameraViewListener
		{

			object sender;

			public ICvCameraViewListenerImplementor(object sender)
				: base(
					global::Android.Runtime.JNIEnv.StartCreateInstance("mono/org/opencv/android/CameraBridgeViewBase_CvCameraViewListenerImplementor", "()V"),
					JniHandleOwnership.TransferLocalRef)
			{
				global::Android.Runtime.JNIEnv.FinishCreateInstance(((global::Java.Lang.Object)this).Handle, "()V");
				this.sender = sender;
			}

#pragma warning disable 0649
			public CvCameraViewOnCameraFrameHandler OnCameraFrameHandler;
#pragma warning restore 0649

			public global::Org.Opencv.Core.Mat OnCameraFrame(global::Org.Opencv.Core.Mat p0)
			{
				var __h = OnCameraFrameHandler;
				return __h != null ? __h(p0) : default(global::Org.Opencv.Core.Mat);
			}
#pragma warning disable 0649
			public EventHandler<CameraViewStartedEventArgs> OnCameraViewStartedHandler;
#pragma warning restore 0649

			public void OnCameraViewStarted(int p0, int p1)
			{
				var __h = OnCameraViewStartedHandler;
				if (__h != null)
					__h(sender, new CameraViewStartedEventArgs(p0, p1));
			}
#pragma warning disable 0649
			public EventHandler OnCameraViewStoppedHandler;
#pragma warning restore 0649

			public void OnCameraViewStopped()
			{
				var __h = OnCameraViewStoppedHandler;
				if (__h != null)
					__h(sender, new EventArgs());
			}

			internal static bool __IsEmpty(ICvCameraViewListenerImplementor value)
			{
				return value.OnCameraFrameHandler == null && value.OnCameraViewStartedHandler == null && value.OnCameraViewStoppedHandler == null;
			}
		}

		public partial interface ICvCameraViewListener : IJavaObject, IJavaPeerable
		{
			global::Org.Opencv.Core.Mat OnCameraFrame(global::Org.Opencv.Core.Mat p0);

			void OnCameraViewStarted(int p0, int p1);

			void OnCameraViewStopped();
		}

		global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor __CreateICvCameraViewListenerImplementor()
		{
			return new global::Org.Opencv.Android.CameraBridgeViewBase.ICvCameraViewListenerImplementor(this);
		}
	}
}