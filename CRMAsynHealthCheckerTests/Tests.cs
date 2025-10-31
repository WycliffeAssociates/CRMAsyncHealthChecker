using System;
using System.Collections.Generic;
using CRMAsyncHealthChecker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;

namespace CRMAsynHealthCheckerTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestUnderLimit()
        {
            // Arrange
            var mockService = new Mock<IOrganizationService>();
            var entityCollection = new EntityCollection();

            Entity recordOne = new Entity("asyncoperation", Guid.NewGuid());
            recordOne["statuscode"] = new OptionSetValue(0);
            entityCollection.Entities.Add(recordOne);

            mockService.Setup(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(entityCollection);

            // Act
            bool result = Program.CheckRecordsPastLimit(mockService.Object, 2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TestOverLimit()
        {
            // Arrange
            var mockService = new Mock<IOrganizationService>();
            var entityCollection = new EntityCollection();

            Entity recordOne = new Entity("asyncoperation", Guid.NewGuid());
            recordOne["statuscode"] = new OptionSetValue(0);
            entityCollection.Entities.Add(recordOne);

            Entity recordTwo = new Entity("asyncoperation", Guid.NewGuid());
            recordTwo["statuscode"] = new OptionSetValue(0);
            entityCollection.Entities.Add(recordTwo);

            Entity recordThree = new Entity("asyncoperation", Guid.NewGuid());
            recordThree["statuscode"] = new OptionSetValue(0);
            entityCollection.Entities.Add(recordThree);

            mockService.Setup(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(entityCollection);

            // Act
            bool result = Program.CheckRecordsPastLimit(mockService.Object, 2);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
