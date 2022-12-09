using OpenQA.Selenium;
using System;

namespace DemoBrowserStackSafari
{
	class BrowserStackOptions : DriverOptions
	{
		public BrowserStackOptions(String browser_name, String browser_version)
		{
			this.BrowserName = browser_name;
			this.BrowserVersion = browser_version;
		}
		public override ICapabilities ToCapabilities()
		{
			IWritableCapabilities capabilities = this.GenerateDesiredCapabilities(true);
			return capabilities.AsReadOnly();
		}
	}
}
