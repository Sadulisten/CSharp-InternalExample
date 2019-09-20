﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace MyInjectableLibrary
{
	public class PInvoke
	{

		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateFile(string lpFileName
			, [MarshalAs(UnmanagedType.U4)] DesiredAccess dwDesiredAccess
			, [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode
			, uint lpSecurityAttributes
			, [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition
			, [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes
			, uint hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetStdHandle(StdHandle nStdHandle, IntPtr hHandle);

		public enum StdHandle : int
		{
			Input = -10,
			Output = -11,
			Error = -12
		}

		[Flags]
		public enum DesiredAccess : uint
		{
			GenericRead = 0x80000000,
			GenericWrite = 0x40000000,
			GenericExecute = 0x20000000,
			GenericAll = 0x10000000
		}

		[DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
		public static extern unsafe void CopyMemory(void* dest, void* src, int count);

		[DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
		public static extern unsafe void MoveMemory(void* dest, void* src, int size);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern void SetLastError(uint dwErrorCode);

		public struct Vector3
		{
			public float x;
			public float y;
			public float z;

			public bool IsEmpty => Math.Abs(x) < 0.00001 && Math.Abs(y) < 0.00001 && Math.Abs(z) < 0.00001;

			public void Add(Vector3 vec)
			{
				x += vec.x;
				y += vec.y;
				z += vec.y;
			}
			public void Subtract(Vector3 vec)
			{
				x -= vec.x;
				y -= vec.y;
				z -= vec.z;
			}
			public void Multiply(Vector3 vec)
			{
				x *= vec.x;
				y *= vec.y;
				y *= vec.z;
			}
		}

		public struct Vector
		{
			public float x;
			public float y;

			public bool IsEmpty => Math.Abs(x) < 0.00001 && Math.Abs(y) < 0.00001;

			public void Add(Vector vec)
			{
				x += vec.x;
				y += vec.y;
			}
			public void Subtract(Vector vec)
			{
				x -= vec.x;
				y -= vec.y;
			}
			public void Multiply(Vector vec)
			{
				x *= vec.x;
				y *= vec.y;
			}
		}

		[Flags]
		public enum MemoryState : uint
		{
			MEM_COMMIT = 0x1000,
			MEM_FREE = 0x10000,
			MEM_RESERVE = 0x2000
		}

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern bool SetWindowText(IntPtr hwnd, String lpString);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		[Flags]
		public enum MemoryProtectionFlags : uint
		{
			/// <summary>
			///     Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed
			///     region results in an access violation.
			///     This value is not officially present in the Microsoft's enumeration but can occur according to the
			///     MEMORY_BASIC_INFORMATION structure documentation.
			/// </summary>
			ZeroAccess = 0x0,

			/// <summary>
			///     Enables execute access to the committed region of pages. An attempt to read from or write to the committed region
			///     results in an access violation.
			///     This flag is not supported by the CreateFileMapping function.
			/// </summary>
			Execute = 0x10,

			/// <summary>
			///     Enables execute or read-only access to the committed region of pages. An attempt to write to the committed region
			///     results in an access violation.
			/// </summary>
			ExecuteRead = 0x20,

			/// <summary>
			///     Enables execute, read-only, or read/write access to the committed region of pages.
			/// </summary>
			ExecuteReadWrite = 0x40,

			/// <summary>
			///     Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.
			///     An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the
			///     process.
			///     The private page is marked as PAGE_EXECUTE_READWRITE, and the change is written to the new page.
			///     This flag is not supported by the VirtualAlloc or <see cref="NativeMethods.VirtualAllocEx" /> functions.
			/// </summary>
			ExecuteWriteCopy = 0x80,

			/// <summary>
			///     Disables all access to the committed region of pages. An attempt to read from, write to, or execute the committed
			///     region results in an access violation.
			///     This flag is not supported by the CreateFileMapping function.
			/// </summary>
			NoAccess = 0x01,

			/// <summary>
			///     Enables read-only access to the committed region of pages. An attempt to write to the committed region results in
			///     an access violation.
			///     If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access
			///     violation.
			/// </summary>
			ReadOnly = 0x02,

			/// <summary>
			///     Enables read-only or read/write access to the committed region of pages.
			///     If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access
			///     violation.
			/// </summary>
			ReadWrite = 0x04,

			/// <summary>
			///     Enables read-only or copy-on-write access to a mapped view of a file mapping object.
			///     An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the
			///     process.
			///     The private page is marked as PAGE_READWRITE, and the change is written to the new page.
			///     If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access
			///     violation.
			///     This flag is not supported by the VirtualAlloc or <see cref="NativeMethods.VirtualAllocEx" /> functions.
			/// </summary>
			WriteCopy = 0x08,

			/// <summary>
			///     Pages in the region become guard pages.
			///     Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off
			///     the guard page status.
			///     Guard pages thus act as a one-time access alarm. For more information, see Creating Guard Pages.
			///     When an access attempt leads the system to turn off guard page status, the underlying page protection takes over.
			///     If a guard page exception occurs during a system service, the service typically returns a failure status indicator.
			///     This value cannot be used with PAGE_NOACCESS.
			///     This flag is not supported by the CreateFileMapping function.
			/// </summary>
			Guard = 0x100,

			/// <summary>
			///     Sets all pages to be non-cachable. Applications should not use this attribute except when explicitly required for a
			///     device.
			///     Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an
			///     EXCEPTION_ILLEGAL_INSTRUCTION exception.
			///     The PAGE_NOCACHE flag cannot be used with the PAGE_GUARD, PAGE_NOACCESS, or PAGE_WRITECOMBINE flags.
			///     The PAGE_NOCACHE flag can be used only when allocating private memory with the VirtualAlloc,
			///     <see cref="NativeMethods.VirtualAllocEx" />, or VirtualAllocExNuma functions.
			///     To enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the
			///     CreateFileMapping function.
			/// </summary>
			NoCache = 0x200,

			/// <summary>
			///     Sets all pages to be write-combined.
			///     Applications should not use this attribute except when explicitly required for a device.
			///     Using the interlocked functions with memory that is mapped as write-combined can result in an
			///     EXCEPTION_ILLEGAL_INSTRUCTION exception.
			///     The PAGE_WRITECOMBINE flag cannot be specified with the PAGE_NOACCESS, PAGE_GUARD, and PAGE_NOCACHE flags.
			///     The PAGE_WRITECOMBINE flag can be used only when allocating private memory with the VirtualAlloc,
			///     <see cref="NativeMethods.VirtualAllocEx" />, or VirtualAllocExNuma functions.
			///     To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the
			///     CreateFileMapping function.
			/// </summary>
			WriteCombine = 0x400
		}

		[Flags]
		public enum MemoryType : uint
		{
			MEM_IMAGE = 0x1000000,
			MEM_MAPPED = 0x40000,
			MEM_PRIVATE = 0x20000
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MEMORY_BASIC_INFORMATION
		{
			public IntPtr BaseAddress;
			public IntPtr AllocationBase;
			public AllocationTypeFlags AllocationProtect;
			public IntPtr RegionSize;
			public MemoryState State;
			public MemoryProtectionFlags Protect;
			public MemoryType Type;
		}

		public enum FreeType
		{
			Decommit = 0x4000,
			Release = 0x8000,
		}

		[Flags]
		public enum AllocationTypeFlags : uint
		{
			Commit = 0x1000,
			Reserve = 0x2000,
			Decommit = 0x4000,
			Release = 0x8000,
			Reset = 0x80000,
			Physical = 0x400000,
			TopDown = 0x100000,
			WriteWatch = 0x200000,
			LargePages = 0x20000000
		}

		public enum CERegion
		{
			NO = 0,
			DONT_CARE = 1,
			YES = 3,
		}


		[DllImport("kernel32.dll", EntryPoint = "RtlZeroMemory")]
		public static extern unsafe bool ZeroMemory(byte* destination, int length);

		[DllImport("ntdll.dll")]
		public static extern uint ZwUnmapViewOfSection(long ProcessHandle,
			long BaseAddress);

		[DllImport("kernel32.dll")]
		public static extern bool SetThreadContext(long hThread,
			IntPtr lpContext);

		[DllImport("kernel32.dll")]
		public static extern bool GetThreadContext(long hThread,
			IntPtr lpContext);

		[DllImport("kernel32.dll")]
		public static extern bool CreateProcess(string lpApplicationName,
			string lpCommandLine,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes,
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			byte[] lpStartupInfo,
			byte[] lpProcessInformation);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool VirtualProtect(IntPtr lpAddress, int dwSize,
			MemoryProtectionFlags flNewProtect, out MemoryProtectionFlags lpflOldProtect);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, AllocationTypeFlags lAllocationType, MemoryProtectionFlags flProtect);

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern bool VirtualFree(IntPtr lpAddress,
			uint dwSize, FreeType dwFreeType);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int VirtualQuery(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenMutex(uint dwDesiredAccess, bool bInheritHandle,
			string lpName);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReleaseMutex(IntPtr hMutex);

		[DllImport("kernel32.dll", SetLastError = true)]
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError= true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle,
			IntPtr hSourceHandle, IntPtr hTargetProcessHandle, out IntPtr lpTargetHandle,
			uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

		[DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateThread(
			IntPtr lpThreadAttributes,
			uint dwStackSize,
			IntPtr lpStartAddress,
			IntPtr lpParameter,
			uint dwCreationFlags,
			out IntPtr lpThreadId);

		[DllImport("kernel32.dll")]
		public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, ThreadCreationFlags dwCreationFlags, out IntPtr lpThreadId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern WaitForSingleOBjectResult WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);

		[DllImport("kernel32.dll")]
		public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int ResumeThread(IntPtr hThread);

		[DllImport("kernel32.dll")]
		public static extern int SuspendThread(IntPtr hThread);

		[DllImport("kernel32.dll")]
		public static extern int TerminateThread(IntPtr hThread, uint dwExitCode);

		public enum ThreadCreationFlags : uint
		{
			CREATE_RUN_DIRECTLY = 0x0,
			CREATE_SUSPENDED = 0x00000004,
			STACK_SIZE_PARAM_IS_A_RESERVATION = 0x00010000
		}

		public enum WaitForSingleOBjectResult : uint
		{
			WAIT_ABANDONED = 0x00000080,
			WAIT_OBJECT_O = 0x0,
			WAIT_TIMEOUT = 0x00000102
		}
	}
}
