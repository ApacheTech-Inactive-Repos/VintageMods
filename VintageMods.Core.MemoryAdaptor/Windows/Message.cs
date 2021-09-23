using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using VintageMods.Core.MemoryAdaptor.Native.Types;
#if DEBUG
using System.Diagnostics;

#endif

// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable UnusedMember.Global

namespace VintageMods.Core.MemoryAdaptor.Windows
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public struct Message
    {
#if DEBUG
        private static readonly TraceSwitch AllWinMessages = new("AllWinMessages", "Output every received message");
#endif

        public IntPtr HWnd { get; set; }

        public int Msg { get; set; }

        public IntPtr WParam { get; set; }

        public IntPtr LParam { get; set; }

        public IntPtr Result { get; set; }

        public object GetLParam(Type cls)
        {
            return Marshal.PtrToStructure(LParam, cls);
        }

        public static Message Create(IntPtr hWnd, WindowsMessages msg, IntPtr wparam, IntPtr lparam)
        {
            return Create(hWnd, (int) msg, wparam, lparam);
        }

        public static Message Create(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            var m = new Message
            {
                HWnd = hWnd,
                Msg = msg,
                WParam = wparam,
                LParam = lparam,
                Result = IntPtr.Zero
            };

#if DEBUG
            if (AllWinMessages.TraceVerbose) Debug.WriteLine(m.ToString());
#endif
            return m;
        }

        public override bool Equals(object o)
        {
            if (!(o is Message)) return false;

            var m = (Message) o;
            return HWnd == m.HWnd &&
                   Msg == m.Msg &&
                   WParam == m.WParam &&
                   LParam == m.LParam &&
                   Result == m.Result;
        }

        public static bool operator !=(Message a, Message b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Message a, Message b)
        {
            return a.Equals(b);
        }


        public override int GetHashCode()
        {
            return ((int) HWnd << 4) | Msg;
        }

        private static readonly CodeAccessPermission UnmanagedCode =
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

        public override string ToString()
        {
            // ASSERT : 151574. Link Demand on System.Windows.Forms.Message 
            // fails to protect overriden methods.
            var unrestricted = false;
            try
            {
                UnmanagedCode.Demand();
                unrestricted = true;
            }
            catch (SecurityException)
            {
                // eat the exception.
            }

            if (unrestricted) return GetProperName(((WindowsMessages) Msg).ToString());
            return base.ToString();
        }

        private static string GetProperName(string name)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];

                if (i > 0 && char.IsUpper(c))
                {
                    sb.Append(' ');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}