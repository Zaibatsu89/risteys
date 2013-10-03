namespace KruispuntGroep4.Generator
{
	/// <summary>
	/// Class used to be the first called class.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        public static void Main(string[] args)
        {
			Generator generator = new Generator();
			generator.ShowDialog();
        }
	}
}