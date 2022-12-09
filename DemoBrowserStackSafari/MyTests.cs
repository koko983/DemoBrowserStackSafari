using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace DemoBrowserStackSafari
{
	[TestClass]
	public class MyTests
	{
		const string appUrl = "https://safari-auto-demo.herokuapp.com/";
		const string browser = "safari"; // chrome - firefox
		string BROWSERSTACK_USERNAME = "<YOUR_BROWSERSTACK_USERNAME>";
		string BROWSERSTACK_ACCESS_KEY = "<YOUR_BROWSERSTACK_ACCESS_KEY>";

		private IWebDriver driver;

		[TestMethod]
		public void ClickBtnMoveToPrivacyPage()
		{
			var watch = new Stopwatch();
			watch.Start();
			var btn = driver.FindElement(By.Id("privacy"));
			btn.Click();
			watch.Stop();
			Console.WriteLine($"Continue script after {watch.Elapsed.TotalSeconds} seconds");
			CheckPrivacyPage();
		}

		[TestMethod]
		public void SelectOptionMoveToPrivacyPage()
		{
			var watch = new Stopwatch();
			watch.Start();
			var webElement = driver.FindElement(By.Id("goTo"));
			var select = new SelectElement(webElement);
			select.SelectByIndex(2);
			watch.Stop();
			Console.WriteLine($"Continue script after {watch.Elapsed.TotalSeconds} seconds");
			Thread.Sleep(4000);
			CheckPrivacyPage();
		}

		private void CheckPrivacyPage()
		{
			Assert.IsTrue(driver.Title.Contains("Privacy Policy"));
			Assert.IsTrue(driver.Url.Contains("/Privacy"));
		}

		#region setup
		[TestInitialize()]
		public void SetupTest()
		{
			SetRemoteDriver();
		}

		void SetRemoteDriver()
		{
			DriverOptions driverOptions;
			switch (browser)
			{
				case "safari":
					driverOptions = GetSafariOptions();
					break;
				default:
					driverOptions = GetChromeOptions();
					break;
			}
			driver = new RemoteWebDriver(new Uri("https://hub.browserstack.com/wd/hub/"), driverOptions);
			driver.Navigate().GoToUrl(appUrl);
		}

		DriverOptions GetSafariOptions()
		{
			Dictionary<string, object> cap3 = new Dictionary<string, object>();
			cap3.Add("browserName", "Safari");
			cap3.Add("browserVersion", "14.1");
			cap3.Add("os", "OS X");
			cap3.Add("osVersion", "Big Sur");
			return CompleteCaps(cap3);
		}

		DriverOptions GetChromeOptions()
		{
			Dictionary<string, object> cap1 = new Dictionary<string, object>();
			cap1.Add("browserName", "Chrome");
			cap1.Add("browserVersion", "103.0");
			cap1.Add("os", "Windows");
			cap1.Add("osVersion", "11");
			return CompleteCaps(cap1);
		}

		DriverOptions CompleteCaps(Dictionary<string, object> cap)
		{
			// Update your credentials
			String BUILD_NAME = "demo-heroku-app";
			cap.Add("userName", BROWSERSTACK_USERNAME);
			cap.Add("accessKey", BROWSERSTACK_ACCESS_KEY);
			cap.Add("buildName", BUILD_NAME);
			String browserVersion = cap.ContainsKey("browserVersion") == true ? (string)cap["browserVersion"] : "";
			var browserstackOptions = new BrowserStackOptions((string)cap["browserName"], browserVersion);
			browserstackOptions.AddAdditionalOption("bstack:options", cap);
			return browserstackOptions;
		}

		[TestCleanup()]
		public void MyTestCleanup()
		{
			driver.Quit();
		}
		#endregion
	}
}
