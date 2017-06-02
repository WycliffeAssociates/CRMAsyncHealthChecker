using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CRMAsyncHealthChecker;
using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;

namespace CRMAsynHealthCheckerTests
{
    [TestClass]
    public class Tests
    {
        private XrmFakedContext context;
        private IOrganizationService service;
        private List<Entity> initialEntities;

        [TestInitialize]
        public void SetUp()
        {
            this.context = new XrmFakedContext();
            this.service = this.context.GetOrganizationService();
            this.initialEntities = new List<Entity>();
        }

        [TestMethod]
        public void TestUnderLimit()
        {
            Entity recordOne = new Entity("asyncoperation", Guid.NewGuid());
            recordOne["statuscode"] = new OptionSetValue(0);
            this.initialEntities.Add(recordOne);
            this.context.Initialize(this.initialEntities);
            Assert.IsFalse(Program.CheckRecordsPastLimit(this.service, 2));
        }

        [TestMethod]
        public void TestOverLimit()
        {
            Entity recordOne = new Entity("asyncoperation", Guid.NewGuid());
            recordOne["statuscode"] = new OptionSetValue(0);
            this.initialEntities.Add(recordOne);
            Entity recordTwo = new Entity("asyncoperation", Guid.NewGuid());
            recordTwo["statuscode"] = new OptionSetValue(0);
            this.initialEntities.Add(recordTwo);
            Entity recordThree = new Entity("asyncoperation", Guid.NewGuid());
            recordThree["statuscode"] = new OptionSetValue(0);
            this.initialEntities.Add(recordThree);
            this.context.Initialize(this.initialEntities);
            Assert.IsTrue(Program.CheckRecordsPastLimit(this.service, 2));
        }
    }
}
