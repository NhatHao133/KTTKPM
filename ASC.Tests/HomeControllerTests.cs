using ASC.Tests.TestUtilities;
using ASC.Web.Configuration;
using ASC.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ASC.Tests
{
	public class HomeControllerTests
	{
		private readonly Mock<IOptions<ApplicationSettings>> optionMock;
		private readonly Mock<HttpContext> mockHttpContext;
		public HomeControllerTests()
		{
			optionMock = new Mock<IOptions<ApplicationSettings>>();
			mockHttpContext = new Mock<HttpContext>();
			mockHttpContext.Setup(p => p.Session).Returns(new FakeSession());

			optionMock.Setup(ap => ap.Value).Returns (new ApplicationSettings
			{
				ApplicationTitle = "ASC"
			});
		}
		[Fact]
		public void HomeController_Index_View_Test() 
		{
			var controller = new HomeController(optionMock.Object);
			controller.ControllerContext.HttpContext = mockHttpContext.Object;

			Assert.IsType(typeof(ViewResult), controller.Index());
		}
		[Fact]
		public void HomeController_Index_NoModel_Test() 
		{
			var controller = new HomeController(optionMock.Object);
			controller.ControllerContext.HttpContext = mockHttpContext.Object;
			Assert.Null((controller.Index() as ViewResult).ViewData.Model);
		}
		[Fact]
		public void HomeController_Index_Validation_Test()
		{
			var controller = new HomeController(optionMock.Object);
			controller.ControllerContext.HttpContext = mockHttpContext.Object;
			Assert.Equal(0, (controller.Index() as ViewResult).ViewData.ModelState.ErrorCount);
		}
		[Fact]
		public void HomeController_Index_Session_Test()
		{
			var controller = new HomeController(optionMock.Object);
			controller.ControllerContext.HttpContext = mockHttpContext.Object;
			controller.Index();
			Assert.Equal(0, (controller.Index() as ViewResult).ViewData.ModelState.ErrorCount);
		}
	}
}
