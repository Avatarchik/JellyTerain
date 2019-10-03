using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Text;

namespace System.Diagnostics
{
	/// <summary>Specifies a set of values that are used when you start a process.</summary>
	/// <filterpriority>2</filterpriority>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public sealed class ProcessStartInfo
	{
		private string arguments = string.Empty;

		private IntPtr error_dialog_parent_handle = (IntPtr)0;

		private string filename = string.Empty;

		private string verb = string.Empty;

		private string working_directory = string.Empty;

		private System.Collections.Specialized.ProcessStringDictionary envVars;

		private bool create_no_window;

		private bool error_dialog;

		private bool redirect_standard_error;

		private bool redirect_standard_input;

		private bool redirect_standard_output;

		private bool use_shell_execute = true;

		private ProcessWindowStyle window_style;

		private Encoding encoding_stderr;

		private Encoding encoding_stdout;

		private string username;

		private string domain;

		private SecureString password;

		private bool load_user_profile;

		private static readonly string[] empty = new string[0];

		/// <summary>Gets or sets the set of command-line arguments to use when starting the application.</summary>
		/// <returns>File type–specific arguments that the system can associate with the application specified in the <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property. The length of the arguments added to the length of the full path to the process must be less than 2080.</returns>
		/// <filterpriority>1</filterpriority>
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[MonitoringDescription("Command line agruments for this process.")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RecommendedAsConfigurable(true)]
		public string Arguments
		{
			get
			{
				return arguments;
			}
			set
			{
				arguments = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to start the process in a new window.</summary>
		/// <returns>true to start the process without creating a new window to contain it; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		[DefaultValue(false)]
		[MonitoringDescription("Start this process with a new window.")]
		[NotifyParentProperty(true)]
		public bool CreateNoWindow
		{
			get
			{
				return create_no_window;
			}
			set
			{
				create_no_window = value;
			}
		}

		/// <summary>Gets search paths for files, directories for temporary files, application-specific options, and other similar information.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringDictionary" /> that provides environment variables that apply to this process and child processes. The default is null.</returns>
		/// <filterpriority>1</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[Editor("System.Diagnostics.Design.StringDictionaryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("Environment variables used for this process.")]
		[NotifyParentProperty(true)]
		public StringDictionary EnvironmentVariables
		{
			get
			{
				if (envVars == null)
				{
					envVars = new System.Collections.Specialized.ProcessStringDictionary();
					foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
					{
						envVars.Add((string)environmentVariable.Key, (string)environmentVariable.Value);
					}
				}
				return envVars;
			}
		}

		internal bool HaveEnvVars => envVars != null;

		/// <summary>Gets or sets a value indicating whether an error dialog box is displayed to the user if the process cannot be started.</summary>
		/// <returns>true to display an error dialog box on the screen if the process cannot be started; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		[NotifyParentProperty(true)]
		[MonitoringDescription("Thread shows dialogboxes for errors.")]
		[DefaultValue(false)]
		public bool ErrorDialog
		{
			get
			{
				return error_dialog;
			}
			set
			{
				error_dialog = value;
			}
		}

		/// <summary>Gets or sets the window handle to use when an error dialog box is shown for a process that cannot be started.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that identifies the handle of the error dialog box that results from a process start failure.</returns>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public IntPtr ErrorDialogParentHandle
		{
			get
			{
				return error_dialog_parent_handle;
			}
			set
			{
				error_dialog_parent_handle = value;
			}
		}

		/// <summary>Gets or sets the application or document to start.</summary>
		/// <returns>The name of the application to start, or the name of a document of a file type that is associated with an application and that has a default open action available to it. The default is an empty string ("").</returns>
		/// <filterpriority>1</filterpriority>
		[RecommendedAsConfigurable(true)]
		[NotifyParentProperty(true)]
		[MonitoringDescription("The name of the resource to start this process.")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Diagnostics.Design.StartFileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		public string FileName
		{
			get
			{
				return filename;
			}
			set
			{
				filename = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the error output of an application is written to the <see cref="P:System.Diagnostics.Process.StandardError" /> stream.</summary>
		/// <returns>true to write error output to <see cref="P:System.Diagnostics.Process.StandardError" />; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[MonitoringDescription("Errors of this process are redirected.")]
		public bool RedirectStandardError
		{
			get
			{
				return redirect_standard_error;
			}
			set
			{
				redirect_standard_error = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the input for an application is read from the <see cref="P:System.Diagnostics.Process.StandardInput" /> stream.</summary>
		/// <returns>true to read input from <see cref="P:System.Diagnostics.Process.StandardInput" />; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("Standard input of this process is redirected.")]
		[DefaultValue(false)]
		[NotifyParentProperty(true)]
		public bool RedirectStandardInput
		{
			get
			{
				return redirect_standard_input;
			}
			set
			{
				redirect_standard_input = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the output of an application is written to the <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream.</summary>
		/// <returns>true to write output to <see cref="P:System.Diagnostics.Process.StandardOutput" />; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		[DefaultValue(false)]
		[NotifyParentProperty(true)]
		[MonitoringDescription("Standart output of this process is redirected.")]
		public bool RedirectStandardOutput
		{
			get
			{
				return redirect_standard_output;
			}
			set
			{
				redirect_standard_output = value;
			}
		}

		/// <summary>Gets or sets the preferred encoding for error output.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> object that represents the preferred encoding for error output. The default is null.</returns>
		public Encoding StandardErrorEncoding
		{
			get
			{
				return encoding_stderr;
			}
			set
			{
				encoding_stderr = value;
			}
		}

		/// <summary>Gets or sets the preferred encoding for standard output.</summary>
		/// <returns>An <see cref="T:System.Text.Encoding" /> object that represents the preferred encoding for standard output. The default is null.</returns>
		public Encoding StandardOutputEncoding
		{
			get
			{
				return encoding_stdout;
			}
			set
			{
				encoding_stdout = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether to use the operating system shell to start the process.</summary>
		/// <returns>true to use the shell when starting the process; otherwise, the process is created directly from the executable file. The default is true.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("Use the shell to start this process.")]
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		public bool UseShellExecute
		{
			get
			{
				return use_shell_execute;
			}
			set
			{
				use_shell_execute = value;
			}
		}

		/// <summary>Gets or sets the verb to use when opening the application or document specified by the <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property.</summary>
		/// <returns>The action to take with the file that the process opens. The default is an empty string ("").</returns>
		/// <filterpriority>2</filterpriority>
		[TypeConverter("System.Diagnostics.Design.VerbConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[MonitoringDescription("The verb to apply to a used document.")]
		public string Verb
		{
			get
			{
				return verb;
			}
			set
			{
				verb = value;
			}
		}

		/// <summary>Gets the set of verbs associated with the type of file specified by the <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property.</summary>
		/// <returns>The actions that the system can apply to the file indicated by the <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property.</returns>
		/// <filterpriority>2</filterpriority>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string[] Verbs
		{
			get
			{
				string text = (!((filename == null) | (filename.Length == 0))) ? Path.GetExtension(filename) : null;
				if (text == null)
				{
					return empty;
				}
				return empty;
			}
		}

		/// <summary>Gets or sets the window state to use when the process is started.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.ProcessWindowStyle" /> that indicates whether the process is started in a window that is maximized, minimized, normal (neither maximized nor minimized), or not visible. The default is normal.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The window style is not one of the <see cref="T:System.Diagnostics.ProcessWindowStyle" /> enumeration members. </exception>
		/// <filterpriority>2</filterpriority>
		[DefaultValue(typeof(ProcessWindowStyle), "Normal")]
		[NotifyParentProperty(true)]
		[MonitoringDescription("The window style used to start this process.")]
		public ProcessWindowStyle WindowStyle
		{
			get
			{
				return window_style;
			}
			set
			{
				window_style = value;
			}
		}

		/// <summary>Gets or sets the initial directory for the process to be started.</summary>
		/// <returns>The fully qualified name of the directory that contains the process to be started. The default is an empty string ("").</returns>
		/// <filterpriority>1</filterpriority>
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[MonitoringDescription("The initial directory for this process.")]
		[RecommendedAsConfigurable(true)]
		[Editor("System.Diagnostics.Design.WorkingDirectoryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string WorkingDirectory
		{
			get
			{
				return working_directory;
			}
			set
			{
				working_directory = ((value != null) ? value : string.Empty);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the Windows user profile is to be loaded from the registry. </summary>
		/// <returns>true to load the Windows user profile; otherwise, false. </returns>
		/// <filterpriority>1</filterpriority>
		[NotifyParentProperty(true)]
		public bool LoadUserProfile
		{
			get
			{
				return load_user_profile;
			}
			set
			{
				load_user_profile = value;
			}
		}

		/// <summary>Gets or sets the user name to be used when starting the process.</summary>
		/// <returns>The user name to use when starting the process.</returns>
		/// <filterpriority>1</filterpriority>
		[NotifyParentProperty(true)]
		public string UserName
		{
			get
			{
				return username;
			}
			set
			{
				username = value;
			}
		}

		/// <summary>Gets or sets a value that identifies the domain to use when starting the process. </summary>
		/// <returns>The Active Directory domain to use when starting the process. The domain property is primarily of interest to users within enterprise environments that use Active Directory.</returns>
		/// <filterpriority>1</filterpriority>
		[NotifyParentProperty(true)]
		public string Domain
		{
			get
			{
				return domain;
			}
			set
			{
				domain = value;
			}
		}

		/// <summary>Gets or sets a secure string that contains the user password to use when starting the process.</summary>
		/// <returns>A <see cref="T:System.Security.SecureString" /> that contains the user password to use when starting the process.</returns>
		/// <filterpriority>1</filterpriority>
		public SecureString Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.ProcessStartInfo" /> class without specifying a file name with which to start the process.</summary>
		public ProcessStartInfo()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.ProcessStartInfo" /> class and specifies a file name such as an application or document with which to start the process.</summary>
		/// <param name="fileName">An application or document with which to start a process. </param>
		public ProcessStartInfo(string filename)
		{
			this.filename = filename;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.ProcessStartInfo" /> class, specifies an application file name with which to start the process, and specifies a set of command-line arguments to pass to the application.</summary>
		/// <param name="fileName">An application with which to start a process. </param>
		/// <param name="arguments">Command-line arguments to pass to the application when the process starts. </param>
		public ProcessStartInfo(string filename, string arguments)
		{
			this.filename = filename;
			this.arguments = arguments;
		}
	}
}
